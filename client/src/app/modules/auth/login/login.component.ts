import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from "@angular/forms";
import { ApolloError } from '@apollo/client/core';
import { MutationResult } from 'apollo-angular';
import { AuthService } from "../../graphql/services/auth.service";
import { LoginMutation } from '../../graphql/services/graphql.service';
import { JwtTokenService } from '../../shared/services/jwt-token.service';
import { LoginForm } from './login.form';
import { Router } from "@angular/router";

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
    private readonly _router: Router) {
  }

  public ngOnInit(): void {
    this.loginForm = new FormGroup<LoginForm>({
      username: new FormControl<string>("", {
        nonNullable: true
      }),
      password: new FormControl<string>("", {
        nonNullable: true
      }),
      showPassword: new FormControl<boolean>(false, {
        nonNullable: true
      })
    });
  }

  public login(): void {
    if (!this.loginForm.valid) {
      return;
    }

    this._authService
      .login(this.loginForm.getRawValue())
      .subscribe({
        next: (response: MutationResult<LoginMutation>) => {
          console.log(response);
        },
        error: (err: ApolloError) => {
          this._router.navigateByUrl('/verify-email', {
            state: {
              username: this.loginForm.value.username,
              needToSendTwoFactorCode: true
            }
          });
        }
      });
  }
}
