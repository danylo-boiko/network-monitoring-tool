import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ApolloError } from '@apollo/client/core';
import { MutationResult } from 'apollo-angular';
import { AuthService } from "../../../graphql/services/auth.service";
import { LoginMutation, TokenDto } from '../../../graphql/services/graphql.service';
import { JwtTokenService } from '../../../../core/services/jwt-token.service';
import { LoginForm } from './login.form';
import { Router } from "@angular/router";
import { ErrorsService } from "../../../graphql/services/errors.service";
import { isFormFieldValid } from "../../../../core/utils/form-field-validation.util";
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { filter, map } from 'rxjs';

@UntilDestroy()
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public loginForm!: FormGroup<LoginForm>;

  constructor(
    private readonly _authService: AuthService,
    private readonly _jwtTokenService: JwtTokenService,
    private readonly _errorsService: ErrorsService,
    private readonly _router: Router) {
  }

  public ngOnInit(): void {
    this.loginForm = new FormGroup<LoginForm>({
      username: new FormControl<string>("", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(32),
          Validators.pattern("^[A-Za-z0-9_]+$")
        ]
      }),
      password: new FormControl<string>("", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(128),
          Validators.pattern("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9\s!@#$%^&*()_+=-`~\\\]\[{}|';:/.,?><]+)$")
        ]
      }),
      showPassword: new FormControl<boolean>(false, {
        nonNullable: true
      })
    });
  }

  public login(): void {
    if (!this.loginForm.valid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this._authService
      .login(this.loginForm.getRawValue())
      .pipe(
        untilDestroyed(this),
        filter(response => !response.loading),
        map(response => (<MutationResult<LoginMutation>>response).data!.login)
      )
      .subscribe({
        next: (tokens: TokenDto) => {
          this._jwtTokenService.setAuthTokens(tokens.accessToken, tokens.refreshToken);
          this._router.navigateByUrl('/');
        },
        error: (error: ApolloError) => {
          const graphQLErrors = this._errorsService.getGraphQLErrors(error, true);

          if (graphQLErrors.length == 1 && graphQLErrors[0].extensions['code'] == 'emailConfirmation') {
            this._router.navigateByUrl('/verify-email', {
              state: {
                username: this.loginForm.value.username,
                needToSendTwoFactorCode: true
              }
            });

            return;
          }

          this._errorsService.applyGraphQLErrorsToForm(this.loginForm, graphQLErrors);
        }
      });
  }

  public getServerValidationErrorMessage(controlName: string): string {
    return this.loginForm.get(controlName)!.getError('serverValidation');
  }

  public isFieldValid(controlName: string, ruleName: string): boolean {
    return isFormFieldValid(this.loginForm, controlName, ruleName);
  }
}
