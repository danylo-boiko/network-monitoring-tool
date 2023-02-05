import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { AuthService } from "../../../graphql/services/auth.service";
import { JwtTokenService } from "../../../../core/services/jwt-token.service";
import { VerifyEmailForm } from './verify-email.form';
import { MutationResult } from "apollo-angular";
import { ApolloError } from "@apollo/client/core";
import { Router } from '@angular/router';
import { isFormFieldValid } from "../../../../core/utils/form-field-validation.util";
import { ErrorsService } from "../../../graphql/services/errors.service";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { filter, map } from "rxjs";
import { SendTwoFactorCodeMutation, VerifyTwoFactorCodeMutation } from "../../../graphql/services/graphql.service";
import { ToasterService } from "../../../../core/services/toaster.service";

@UntilDestroy()
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
    private readonly _toasterService: ToasterService,
    private readonly _router: Router) {
    const state = this._router.getCurrentNavigation()?.extras.state;

    if (!state || !state['username']) {
      this._router.navigateByUrl('/login');
    }

    this.username = state!['username'];

    if (state!['needToSendTwoFactorCode']) {
      this.sendTwoFactorCode(false);
    } else {
      this.disableSendTwoFactorCodeButton();
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
      this.verifyEmailForm.markAllAsTouched();
      return;
    }

    this._authService
      .verifyTwoFactorCode({
        username: this.username,
        twoFactorCode: this.verifyEmailForm.value.twoFactorCode!
      })
      .pipe(
        untilDestroyed(this),
        filter(response => !response.loading),
        map(response => (<MutationResult<VerifyTwoFactorCodeMutation>>response).data!.verifyTwoFactorCode)
      )
      .subscribe({
        next: (successful: boolean) => {
          if (successful) {
            this._router.navigateByUrl('/login');
          }
        },
        error: (error: ApolloError) => {
          const graphQLErrors = this._errorsService.getGraphQLErrors(error);

          if (graphQLErrors.length == 0) {
            this._toasterService.showError(error.message);
          } else {
            this._errorsService.applyGraphQLErrorsToForm(this.verifyEmailForm, graphQLErrors);
          }
        }
      });
  }

  public sendTwoFactorCode(showToaster: boolean = true): void {
    if (this.sendTwoFactorCodeButtonDisabled) {
      return;
    }

    this.disableSendTwoFactorCodeButton();

    this._authService
      .sendTwoFactorCode({
        username: this.username,
      })
      .pipe(
        untilDestroyed(this),
        filter(response => !response.loading),
        map(response => (<MutationResult<SendTwoFactorCodeMutation>>response).data!.sendTwoFactorCode)
      )
      .subscribe({
        next: (successful: boolean) => {
          if (successful && showToaster) {
            this._toasterService.showSuccess('Two factor code sent successfully');
          }
        },
        error: (error: ApolloError) => this._toasterService.showError(error.message)
      });
  }

  public getServerValidationErrorMessage(controlName: string): string {
    return this.verifyEmailForm.get(controlName)!.getError('serverValidation');
  }

  public isFieldValid(controlName: string, ruleName: string): boolean {
    return isFormFieldValid(this.verifyEmailForm, controlName, ruleName);
  }

  private disableSendTwoFactorCodeButton(): void {
    this.sendTwoFactorCodeButtonDisabled = true;

    setTimeout(() => {
      this.sendTwoFactorCodeButtonDisabled = false;
    }, 60000);
  }
}
