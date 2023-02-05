import { Component, Input, ViewChild } from '@angular/core';
import { IpFilterDto } from 'src/app/modules/graphql/services/graphql.service';
import { CreateIpFilterComponent } from "../../dialogs/create-ip-filter/create-ip-filter.component";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { BehaviorSubject, filter, map } from "rxjs";
import { UpdateIpFilterComponent } from "../../dialogs/update-ip-filter/update-ip-filter.component";
import { DeleteIpFilterComponent } from "../../dialogs/delete-ip-filter/delete-ip-filter.component";
import { IpFiltersService } from "../../../graphql/services/ip-filters.service";
import { MatDialog } from "@angular/material/dialog";
import { intToIpString } from "../../../../core/utils/ip.util";
import { MatPaginator } from '@angular/material/paginator';
import { IpFiltersDataSource } from "./ip-filters-data-source";

@UntilDestroy()
@Component({
  selector: 'app-ip-filters-table',
  templateUrl: './ip-filters-table.component.html',
  styleUrls: ['./ip-filters-table.component.scss']
})
export class IpFiltersTableComponent {
  @Input() ipFilters!: IpFilterDto[];
  @ViewChild(MatPaginator, {static: true}) paginator!: MatPaginator;

  public ipFiltersDataSource!: IpFiltersDataSource;
  public ipFiltersTableColumns = ['ip', 'filterAction', 'comment', 'actions'];
  private ipFiltersSubject = new BehaviorSubject<IpFilterDto[]>([]);

  constructor(
    private readonly _ipFiltersService: IpFiltersService,
    private readonly _dialog: MatDialog) {
  }

  public ngOnInit(): void {
    this.ipFiltersSubject.next(this.ipFilters);
    this.ipFiltersDataSource = new IpFiltersDataSource(this.ipFiltersSubject, this.paginator);
  }

  public createIpFilter(): void {
    const dialogRef = this._dialog.open(CreateIpFilterComponent);

    dialogRef
      .afterClosed()
      .pipe(
        untilDestroyed(this),
        filter(response => response?.modified),
        map(response => response.data)
      )
      .subscribe((ipFilter: IpFilterDto) => {
        const ipFilters = [...this.ipFiltersSubject.value];
        ipFilters.unshift(ipFilter);

        this.ipFiltersSubject.next(ipFilters);
        this.refreshTable();
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
        filter(response => response?.modified),
        map(response => response.data)
      )
      .subscribe((ipFilter: IpFilterDto) => {
        const ipFilters = [...this.ipFiltersSubject.value];

        const idx = ipFilters.findIndex(x => x.id === ipFilter.id);
        ipFilters[idx] = ipFilter;

        this.ipFiltersSubject.next(ipFilters);
        this.refreshTable();
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
        filter(response => response?.modified),
        map(response => response.data.id)
      )
      .subscribe((ipFilterId: string) => {
        this.ipFiltersSubject.next(this.ipFiltersSubject.value.filter(i => i.id != ipFilterId));
        this.refreshTable();
      });
  }

  public convertIntToIpString(ip: number): string {
    return intToIpString(ip);
  }

  private refreshTable(): void {
    this.paginator._changePageSize(this.paginator.pageSize);
  }
}
