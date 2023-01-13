import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from "@angular/forms";
import { AuthService } from "../../graphql/services/auth.service";
import { JwtTokenService } from "../../shared/services/jwt-token.service";
import { RegisterForm } from "./register.form";
import { MutationResult } from "apollo-angular";
import { RegisterMutation } from "../../graphql/services/graphql.service";
import { ApolloError } from "@apollo/client/core";
import { Router } from "@angular/router";

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
    private readonly _router: Router) {
  }

  public ngOnInit(): void {
    this.registerForm = new FormGroup<RegisterForm>({
      username: new FormControl<string>("", {
        nonNullable: true
      }),
      email: new FormControl<string>("", {
        nonNullable: true
      }),
      password: new FormControl<string>("", {
        nonNullable: true
      }),
      confirmPassword: new FormControl<string>("", {
        nonNullable: true
      })
    });
  }

  public register(): void {
    if (!this.registerForm.valid) {
      return;
    }

    this._authService
      .register(this.registerForm.getRawValue())
      .subscribe({
        next: (response: MutationResult<RegisterMutation>) => {
          if (!response.loading && response.data?.register) {
            this._router.navigateByUrl('/verify-email', {
              state: {
                email: this.registerForm.value.email,
                sendTwoFactorCode: false
              }
            });
          }
        },
        error: (err: ApolloError) => {
          console.log(err.networkError);
        }
      });
  }
}
