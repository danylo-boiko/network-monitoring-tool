import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { IpFilterDto } from "../../../graphql/services/graphql.service";
import { intToIpString } from "../../../../core/utils/ip.util";

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
    console.log(`Delete ${this.data.ipFilter.id}`);
    this._dialogRef.close(true);
  }

  public cancel(): void {
    this._dialogRef.close();
  }

  public convertIntToIpString(ip: number): string {
    return intToIpString(ip);
  }
}
