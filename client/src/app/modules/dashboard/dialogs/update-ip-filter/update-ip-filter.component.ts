import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { IpFilterDto } from "../../../graphql/services/graphql.service";

@Component({
  selector: 'app-update-ip-filter',
  templateUrl: './update-ip-filter.component.html',
  styleUrls: ['./update-ip-filter.component.scss']
})
export class UpdateIpFilterComponent {
  constructor(
    private readonly _dialogRef: MatDialogRef<UpdateIpFilterComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { ipFilter: IpFilterDto }) {
  }

  public updateIpFilter(): void {
  }

  public cancel(): void {
    this._dialogRef.close();
  }
}
