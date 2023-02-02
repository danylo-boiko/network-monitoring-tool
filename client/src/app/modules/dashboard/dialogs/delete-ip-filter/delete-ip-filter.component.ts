import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { IpFilterDto } from "../../../graphql/services/graphql.service";

@Component({
  selector: 'app-delete-ip-filter',
  templateUrl: './delete-ip-filter.component.html',
  styleUrls: ['./delete-ip-filter.component.scss']
})
export class DeleteIpFilterComponent {
  constructor(
    private readonly _dialogRef: MatDialogRef<DeleteIpFilterComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { ipFilter: IpFilterDto }) {
  }

  public deleteIpFilter(): void {
  }

  public cancel(): void {
    this._dialogRef.close();
  }
}
