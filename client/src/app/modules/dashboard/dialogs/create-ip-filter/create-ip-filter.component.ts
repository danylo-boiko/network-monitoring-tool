import { Component } from '@angular/core';
import { MatDialogRef } from "@angular/material/dialog";

@Component({
  selector: 'app-create-ip-filter',
  templateUrl: './create-ip-filter.component.html',
  styleUrls: ['./create-ip-filter.component.scss']
})
export class CreateIpFilterComponent {
  constructor(private readonly _dialogRef: MatDialogRef<CreateIpFilterComponent>) {
  }

  public createIpFilter(): void {
  }

  public cancel(): void {
    this._dialogRef.close();
  }
}
