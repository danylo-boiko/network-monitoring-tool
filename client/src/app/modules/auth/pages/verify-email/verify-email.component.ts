import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { AuthService } from "../../../graphql/services/auth.service";
import { JwtTokenService } from "../../../shared/services/jwt-token.service";
import { VerifyEmailForm } from './verify-email.form';
import { MutationResult } from "apollo-angular";
import { SendTwoFactorCodeMutation, VerifyTwoFactorCodeMutation } from "../../../graphql/services/graphql.service";
import { ApolloError } from "@apollo/client/core";
import { Router } from '@angular/router';
import { isFormFieldValid } from "../../../shared/helpers/form-field-validation.helper";
import { ErrorsService } from "../../../graphql/services/errors.service";
import { Toaster } from 'ngx-toast-notifications';

@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.scss']
})
export class VerifyEmailComponent implements OnInit {
  public verifyEmailForm!: FormGroup<VerifyEmailForm>;
  public username!: string;
  public sendTwoFactorCodeButtonDisabled: boolean = false;

  constructor(
    private readonly _authService: AuthService,
    private readonly _jwtTokenService: JwtTokenService,
    private readonly _errorsService: ErrorsService,
    private readonly _toaster: Toaster,
    private readonly _router: Router) {
    const state = this._router.getCurrentNavigation()?.extras.state;
    if (!state || !state['username']) {
      this._router.navigateByUrl('/login');
    }

    this.username = state!['username'];
    if (state!['needToSendTwoFactorCode']) {
      this.sendTwoFactorCode(false);
    }
  }

  public ngOnInit(): void {
    this.verifyEmailForm = new FormGroup<VerifyEmailForm>({
      twoFactorCode: new FormControl<string>("", {
        nonNullable: true,
        validators: [
          Validators.required,
          Validators.pattern("^[0-9]{6}$")
        ]
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
          const validationErrors = this._errorsService.getValidationErrors(err);

          if (validationErrors.size == 0) {
            throw err;
          }

          for (const [property, errors] of validationErrors.entries()) {
            const control = this.verifyEmailForm.get(property);
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

  public sendTwoFactorCode(showToaster: boolean = true): void {
    if (this.sendTwoFactorCodeButtonDisabled) {
      return;
    }

    this.sendTwoFactorCodeButtonDisabled = true;

    setTimeout(() => {
      this.sendTwoFactorCodeButtonDisabled = false;
    }, 60000);

    this._authService
      .sendTwoFactorCode({
        username: this.username,
      })
      .subscribe({
        next: (response: MutationResult<SendTwoFactorCodeMutation>) => {
          if (!response.loading && showToaster) {
            this._toaster.open({
              text: 'Two factor code sent successfully',
              type: 'success',
              position: 'top-right',
              duration: 4000
            });
          }
        }
      });
  }

  public getServerValidationErrorMessage(controlName: string): string {
    return this.verifyEmailForm.get(controlName)!.getError('serverValidation');
  }

  public isFieldValid(controlName: string, ruleName: string): boolean {
    return isFormFieldValid(this.verifyEmailForm, controlName, ruleName);
  }
}
