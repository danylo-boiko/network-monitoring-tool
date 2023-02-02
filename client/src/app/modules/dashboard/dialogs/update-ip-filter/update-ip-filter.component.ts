import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { IpFilterAction, IpFilterDto } from "../../../graphql/services/graphql.service";
import { intToIpString } from "../../../../core/utils/ip.util";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { UpdateIpFilterForm } from './update-ip-filter.form';
import { isFormFieldValid } from "../../../../core/utils/form-field-validation.util";

@Component({
  selector: 'app-update-ip-filter',
  templateUrl: './update-ip-filter.component.html',
  styleUrls: ['./update-ip-filter.component.scss']
})
export class UpdateIpFilterComponent implements OnInit {
  public updateIpFilterForm!: FormGroup<UpdateIpFilterForm>;
  public allowedFilterActions = Object.values(IpFilterAction);

  constructor(
    private readonly _dialogRef: MatDialogRef<UpdateIpFilterComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { ipFilter: IpFilterDto }) {
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
      filterId: this.data.ipFilter.id,
      filterAction: formValues.filterAction,
      comment: formValues.comment
    }

    console.log(ipFilter);
    this._dialogRef.close(true);
  }

  public cancel(): void {
    this._dialogRef.close();
  }

  public convertIntToIpString(ip: number): string {
    return intToIpString(ip);
  }

  public isFieldValid(controlName: string, ruleName: string): boolean {
    return isFormFieldValid(this.updateIpFilterForm, controlName, ruleName);
  }
}
