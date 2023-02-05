import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { AuthService } from "../../../graphql/services/auth.service";
import { JwtTokenService } from "../../../../core/services/jwt-token.service";
import { RegisterForm } from "./register.form";
import { MutationResult } from "apollo-angular";
import { RegisterMutation } from "../../../graphql/services/graphql.service";
import { ApolloError } from "@apollo/client/core";
import { Router } from "@angular/router";
import { isFormFieldValid } from "../../../../core/utils/form-field-validation.util";
import { ErrorsService } from "../../../graphql/services/errors.service";
import { matchingControlsValidator } from "../../../../core/validators/form-builder.validator";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { filter, map } from "rxjs";
import { ToasterService } from "../../../../core/services/toaster.service";

@UntilDestroy()
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  public registerForm!: FormGroup<RegisterForm>;

  constructor(
    private readonly _authService: AuthService,
    private readonly _jwtTokenService: JwtTokenService,
    private readonly _errorsService: ErrorsService,
    private readonly _toasterService: ToasterService,
    private readonly _formBuilder: FormBuilder,
    private readonly _router: Router) {
  }

  public ngOnInit(): void {
    this.registerForm = this._formBuilder.group({
      username: new FormControl<string>("", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(32),
          Validators.pattern("^[A-Za-z0-9_]+$")
        ]
      }),
      email: new FormControl<string>("", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.email
        ]
      }),
      password: new FormControl<string>("", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(128),
          Validators.pattern("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9\s!@#$%^&*()_+=-`~\\\]\[{}|';:/.,?><]+)$")
        ],
      }),
      confirmPassword: new FormControl<string>("", {
        nonNullable: true
      }),
      showPassword: new FormControl<boolean>(false, {
        nonNullable: true
      })
    }, {
      validator: matchingControlsValidator('password', 'confirmPassword')
    });
  }

  public register(): void {
    if (!this.registerForm.valid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this._authService
      .register(this.registerForm.getRawValue())
      .pipe(
        untilDestroyed(this),
        filter(response => !response.loading),
        map(response => (<MutationResult<RegisterMutation>>response).data!.register)
      )
      .subscribe({
        next: (successful: boolean) => {
          if (successful) {
            this._router.navigateByUrl('/verify-email', {
              state: {
                username: this.registerForm.value.username,
                needToSendTwoFactorCode: false
              }
            });
          }
        },
        error: (error: ApolloError) => {
          const graphQLErrors = this._errorsService.getGraphQLErrors(error);

          if (graphQLErrors.length == 0) {
            this._toasterService.showError(error.message);
          } else {
            this._errorsService.applyGraphQLErrorsToForm(this.registerForm, graphQLErrors);
          }
        }
      });
  }

  public getServerValidationErrorMessage(controlName: string): string {
    return this.registerForm.get(controlName)!.getError('serverValidation');
  }

  public isFieldValid(controlName: string, ruleName: string): boolean {
    return isFormFieldValid(this.registerForm, controlName, ruleName);
  }
}
