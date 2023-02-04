import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { IpFilterAction, IpFilterDto } from "../../../graphql/services/graphql.service";
import { intToIpString } from "../../../../core/utils/ip.util";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { UpdateIpFilterForm } from './update-ip-filter.form';
import { isFormFieldValid } from "../../../../core/utils/form-field-validation.util";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { IpFiltersService } from "../../../graphql/services/ip-filters.service";
import { ErrorsService } from "../../../graphql/services/errors.service";
import { filter } from "rxjs";
import { ApolloError } from "@apollo/client/core";

@UntilDestroy()
@Component({
  selector: 'app-update-ip-filter',
  templateUrl: './update-ip-filter.component.html',
  styleUrls: ['./update-ip-filter.component.scss']
})
export class UpdateIpFilterComponent implements OnInit {
  public updateIpFilterForm!: FormGroup<UpdateIpFilterForm>;
  public allowedFilterActions = Object.values(IpFilterAction);

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { ipFilter: IpFilterDto },
    private readonly _dialogRef: MatDialogRef<UpdateIpFilterComponent>,
    private readonly _ipFiltersService: IpFiltersService,
    private readonly _errorsService: ErrorsService) {
  }

  public ngOnInit(): void {
    const comment = this.data.ipFilter.comment == null ? null : this.data.ipFilter!.comment;
    this.updateIpFilterForm = new FormGroup<UpdateIpFilterForm>({
      filterAction: new FormControl<IpFilterAction>(this.data.ipFilter.filterAction, {
        nonNullable: true,
        validators: [
          Validators.required
        ]
      }),
      comment: new FormControl<string | null>(comment, {
        validators: [
          Validators.maxLength(256)
        ]
      })
    });
  }

  public updateIpFilter(): void {
    if (!this.updateIpFilterForm.valid) {
      this.updateIpFilterForm.markAllAsTouched();
      return;
    }

    const formValues = this.updateIpFilterForm.getRawValue();

    const ipFilter = {
      ipFilterId: this.data.ipFilter.id,
      filterAction: formValues.filterAction,
      comment: formValues.comment
    }

    this._ipFiltersService
      .updateIpFilter(ipFilter)
      .pipe(
        untilDestroyed(this),
        filter(response => !response.loading)
      )
      .subscribe({
        next: () => {
          this._dialogRef.close({
            modified: true,
            data: {
              id: ipFilter.ipFilterId,
              ip: this.data.ipFilter.ip,
              filterAction: ipFilter.filterAction,
              comment: ipFilter.comment
            }
          });
        },
        error: (error: ApolloError) => {
          const graphQLErrors = this._errorsService.getGraphQLErrors(error, true);
          this._errorsService.applyGraphQLErrorsToForm(this.updateIpFilterForm, graphQLErrors);
        }
      });


    this._dialogRef.close(true);
  }

  public cancel(): void {
    this._dialogRef.close();
  }

  public convertIntToIpString(ip: number): string {
    return intToIpString(ip);
  }

  public getServerValidationErrorMessage(controlName: string): string {
    return this.updateIpFilterForm.get(controlName)!.getError('serverValidation');
  }

  public isFieldValid(controlName: string, ruleName: string): boolean {
    return isFormFieldValid(this.updateIpFilterForm, controlName, ruleName);
  }
}
