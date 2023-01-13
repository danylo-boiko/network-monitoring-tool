import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ApolloError } from '@apollo/client/core';
import { MutationResult } from 'apollo-angular';
import { AuthService } from "../../graphql/services/auth.service";
import { LoginMutation } from '../../graphql/services/graphql.service';
import { ErrorsService } from "../../graphql/services/errors.service";
import { JwtTokenService } from '../../shared/services/jwt-token.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public loginForm!: FormGroup;

  constructor(
    private readonly _authService: AuthService,
    private readonly _jwtTokenService: JwtTokenService,
    private readonly _errorsService: ErrorsService) {
  }

  public ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl(null),
      password: new FormControl(null),
      showPassword: new FormControl(false)
    });
  }

  public login(): void {
    if (!this.loginForm.valid) {
      return;
    }

    const { username, password } = this.loginForm.value;

    this._authService
      .login({username, password})
      .subscribe({
        next: (response: MutationResult<LoginMutation>) => {
          if (!response.loading) {
            const tokens = response.data?.login!;
            this._jwtTokenService.setAuthTokens(tokens.accessToken, tokens.refreshToken);
          }
        },
        error: (err: ApolloError) => {
          const validationErrors = this._errorsService.getValidationErrors(err);
          console.log(validationErrors);
          if (validationErrors.size == 0) {
            throw err;
          }
        }
      });
  }
}
