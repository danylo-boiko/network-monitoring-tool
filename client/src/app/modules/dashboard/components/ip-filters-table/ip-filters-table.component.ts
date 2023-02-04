import { Component, Input } from '@angular/core';
import { IpFilterDto } from 'src/app/modules/graphql/services/graphql.service';
import { CreateIpFilterComponent } from "../../dialogs/create-ip-filter/create-ip-filter.component";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { filter } from "rxjs";
import { UpdateIpFilterComponent } from "../../dialogs/update-ip-filter/update-ip-filter.component";
import { DeleteIpFilterComponent } from "../../dialogs/delete-ip-filter/delete-ip-filter.component";
import { IpFiltersService } from "../../../graphql/services/ip-filters.service";
import { MatDialog } from "@angular/material/dialog";
import { Toaster } from "ngx-toast-notifications";
import { intToIpString } from "../../../../core/utils/ip.util";

@UntilDestroy()
@Component({
  selector: 'app-ip-filters-table',
  templateUrl: './ip-filters-table.component.html',
  styleUrls: ['./ip-filters-table.component.scss']
})
export class IpFiltersTableComponent {
  @Input() ipFilters: IpFilterDto[] | undefined;

  public ipFiltersTableColumns!: string[];

  constructor(
    private readonly _ipFiltersService: IpFiltersService,
    public readonly _dialog: MatDialog,
    private readonly _toaster: Toaster) {
  }

  public ngOnInit(): void {
    this.ipFiltersTableColumns = ['ip', 'filterAction', 'comment', 'actions'];
  }

  public createIpFilter(): void {
    const dialogRef = this._dialog.open(CreateIpFilterComponent);

    dialogRef
      .afterClosed()
      .pipe(
        untilDestroyed(this),
        filter(response => response?.modified)
      )
      .subscribe(dialogResponse => {
        console.log(dialogResponse.data);
      });
  }

  public updateIpFilter(ipFilter: IpFilterDto): void {
    const dialogRef = this._dialog.open(UpdateIpFilterComponent, {
      data: { ipFilter }
    });

    dialogRef
      .afterClosed()
      .pipe(
        untilDestroyed(this),
        filter(response => response?.modified)
      )
      .subscribe(dialogResponse => {
        console.log(dialogResponse.data);
      });
  }

  public deleteIpFilter(ipFilter: IpFilterDto): void {
    const dialogRef = this._dialog.open(DeleteIpFilterComponent, {
      data: { ipFilter }
    });

    dialogRef
      .afterClosed()
      .pipe(
        untilDestroyed(this),
        filter(response => response?.modified)
      )
      .subscribe(dialogResponse => {
        console.log(dialogResponse.data);
      });
  }

  public convertIntToIpString(ip: number): string {
    return intToIpString(ip);
  }
}
