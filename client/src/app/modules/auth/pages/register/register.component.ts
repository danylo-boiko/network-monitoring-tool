import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { AuthService } from "../../../graphql/services/auth.service";
import { JwtTokenService } from "../../../shared/services/jwt-token.service";
import { RegisterForm } from "./register.form";
import { MutationResult } from "apollo-angular";
import { RegisterMutation } from "../../../graphql/services/graphql.service";
import { ApolloError } from "@apollo/client/core";
import { Router } from "@angular/router";
import { isFormFieldValid } from "../../../shared/helpers/form-field-validation.helper";
import { ErrorsService } from "../../../graphql/services/errors.service";

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
      validator: this.matchingControlValidator('password', 'confirmPassword')
    });
  }

  public register(): void {
    if (!this.registerForm.valid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this._authService
      .register(this.registerForm.getRawValue())
      .subscribe({
        next: (response: MutationResult<RegisterMutation>) => {
          if (!response.loading && response.data?.register) {
            this._router.navigateByUrl('/verify-email', {
              state: {
                username: this.registerForm.value.username,
                needToSendTwoFactorCode: false
              }
            });
          }
        },
        error: (err: ApolloError) => {
          const validationErrors = this._errorsService.getValidationErrors(err);

          if (validationErrors.size == 0) {
            throw err;
          }

          for (const [property, errors] of validationErrors.entries()) {
            const control = this.registerForm.get(property);
            if (!control) {
              throw err;
            }
            control.setErrors({
              serverValidation: errors[0]
            });
          }
        }
      });
  }

  public getServerValidationErrorMessage(controlName: string): string {
    return this.registerForm.get(controlName)!.getError('serverValidation');
  }

  public matchingControlValidator(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup<RegisterForm>) => {
      const control = formGroup.get(controlName)!;
      const matchingControl = formGroup.get(matchingControlName)!;

      if (matchingControl.errors && !matchingControl.getError('matching')) {
        return;
      }

      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ matching: true });
      } else {
        matchingControl.setErrors(null);
      }
    };
  }

  public isFieldValid(controlName: string, ruleName: string): boolean {
    return isFormFieldValid(this.registerForm, controlName, ruleName);
  }
}
