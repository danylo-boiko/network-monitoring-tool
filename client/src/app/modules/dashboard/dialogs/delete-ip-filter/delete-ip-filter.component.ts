import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { IpFilterDto } from "../../../graphql/services/graphql.service";
import { intToIpString } from "../../../../core/utils/ip.util";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { IpFiltersService } from "../../../graphql/services/ip-filters.service";
import { filter } from "rxjs";
import { ToasterService } from "../../../../core/services/toaster.service";
import { ApolloError } from "@apollo/client/core";

@UntilDestroy()
@Component({
  selector: 'app-delete-ip-filter',
  templateUrl: './delete-ip-filter.component.html',
  styleUrls: ['./delete-ip-filter.component.scss']
})
export class DeleteIpFilterComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { ipFilter: IpFilterDto },
    private readonly _dialogRef: MatDialogRef<DeleteIpFilterComponent>,
    private readonly _ipFiltersService: IpFiltersService,
    private readonly _toasterService: ToasterService) {
  }

  public deleteIpFilter(): void {
    this._ipFiltersService
      .deleteIpFilter({
        ipFilterId: this.data.ipFilter.id
      })
      .pipe(
        untilDestroyed(this),
        filter(response => !response.loading)
      )
      .subscribe({
        next: () => {
          this._dialogRef.close({
            modified: true,
            data: {
              id: this.data.ipFilter.id,
            }
          });
        },
        error: (error: ApolloError) => this._toasterService.showError(error.message)
      });
  }

  public cancel(): void {
    this._dialogRef.close();
  }

  public convertIntToIpString(ip: number): string {
    return intToIpString(ip);
  }
}
