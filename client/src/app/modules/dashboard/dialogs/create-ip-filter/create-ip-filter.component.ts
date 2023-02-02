import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { CreateIpFilterForm } from './create-ip-filter.form';
import { IpFilterAction } from "../../../graphql/services/graphql.service";
import { ipStringToInt } from "../../../../core/utils/ip.util";
import { ipValidator } from "../../../../core/validators/ip.validator";
import { isFormFieldValid } from "../../../../core/utils/form-field-validation.util";

@Component({
  selector: 'app-create-ip-filter',
  templateUrl: './create-ip-filter.component.html',
  styleUrls: ['./create-ip-filter.component.scss']
})
export class CreateIpFilterComponent implements OnInit {
  public createIpFilterForm!: FormGroup<CreateIpFilterForm>;
  public allowedFilterActions = Object.values(IpFilterAction);

  constructor(private readonly _dialogRef: MatDialogRef<CreateIpFilterComponent>) {
  }

  public ngOnInit(): void {
    this.createIpFilterForm = new FormGroup<CreateIpFilterForm>({
      ip: new FormControl<string>("", {
        nonNullable: true,
        validators: [
          Validators.required,
          ipValidator()
        ]
      }),
      filterAction: new FormControl<IpFilterAction>(IpFilterAction.Drop, {
        nonNullable: true,
        validators: [
          Validators.required
        ]
      }),
      comment: new FormControl<string | null>(null, {
        validators: [
          Validators.maxLength(256)
        ]
      })
    });
  }

  public createIpFilter(): void {
    if (!this.createIpFilterForm.valid) {
      this.createIpFilterForm.markAllAsTouched();
      return;
    }

    const formValues = this.createIpFilterForm.getRawValue();

    const ipFilter = {
      ip: ipStringToInt(formValues.ip),
      filterAction: formValues.filterAction,
      comment: formValues.comment
    }

    console.log(ipFilter);
    this._dialogRef.close(true);
  }

  public cancel(): void {
    this._dialogRef.close();
  }

  public isFieldValid(controlName: string, ruleName: string): boolean {
    return isFormFieldValid(this.createIpFilterForm, controlName, ruleName);
  }
}
