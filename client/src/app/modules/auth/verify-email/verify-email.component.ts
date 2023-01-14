import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from "@angular/forms";
import { AuthService } from "../../graphql/services/auth.service";
import { JwtTokenService } from "../../shared/services/jwt-token.service";
import { VerifyEmailForm } from './verify-email.form';
import { MutationResult } from "apollo-angular";
import { SendTwoFactorCodeMutation, VerifyTwoFactorCodeMutation } from "../../graphql/services/graphql.service";
import { ApolloError } from "@apollo/client/core";
import { Router } from '@angular/router';

@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.scss']
})
export class VerifyEmailComponent implements OnInit {
  public verifyEmailForm!: FormGroup<VerifyEmailForm>;
  public username!: string;

  constructor(
    private readonly _authService: AuthService,
    private readonly _jwtTokenService: JwtTokenService,
    private readonly _router: Router) {
    const state = this._router.getCurrentNavigation()?.extras.state;
    if (!state || !state['username']) {
      this._router.navigateByUrl('/login');
    }

    this.username = state!['username'];
    if (state!['needToSendTwoFactorCode']) {
      this.sendTwoFactorCode();
    }
  }

  public ngOnInit(): void {
    this.verifyEmailForm = new FormGroup<VerifyEmailForm>({
      twoFactorCode: new FormControl<string>("", {
        nonNullable: true
      })
    });
  }

  public verifyEmail(): void {
    if (!this.verifyEmailForm.valid) {
      return;
    }

    this._authService
      .verifyTwoFactorCode({
        username: this.username,
        twoFactorCode: this.verifyEmailForm.value.twoFactorCode!
      })
      .subscribe({
        next: (response: MutationResult<VerifyTwoFactorCodeMutation>) => {
          if (!response.loading && response.data?.verifyTwoFactorCode) {
            this._router.navigateByUrl('/login');
          }
        },
        error: (err: ApolloError) => {
          console.log(err.networkError);
        }
      });
  }

  public sendTwoFactorCode(): void {
    this._authService
      .sendTwoFactorCode({
        username: this.username,
      })
      .subscribe({
        next: (response: MutationResult<SendTwoFactorCodeMutation>) => {
          if (!response.loading) {
            console.log(response);
          }
        },
        error: (err: ApolloError) => {
          console.log(err.networkError);
        }
      });
  }
}
