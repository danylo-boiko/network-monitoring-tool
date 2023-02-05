import { DataSource } from "@angular/cdk/collections";
import { MatPaginator } from "@angular/material/paginator";
import { BehaviorSubject, map, merge, Observable } from "rxjs";
import { IpFilterDto } from "src/app/modules/graphql/services/graphql.service";

export class IpFiltersDataSource extends DataSource<IpFilterDto> {
  public renderedData: IpFilterDto[] = [];

  constructor(
    private readonly _ipFiltersSubject: BehaviorSubject<IpFilterDto[]>,
    private readonly _paginator: MatPaginator) {
    super();
  }

  public connect(): Observable<IpFilterDto[]> {
    const dataChanges = [
      this._ipFiltersSubject,
      this._paginator.page
    ];

    return merge(...dataChanges).pipe(map(() => {
      const startIndex = this._paginator.pageIndex * this._paginator.pageSize;
      const ipFilters = [...this._ipFiltersSubject.value];

      this.renderedData = ipFilters.splice(startIndex, this._paginator.pageSize);
      return this.renderedData;
    }));
  }

  public disconnect(): void {
  }

  public dataLength(): number {
    return this._ipFiltersSubject.value.length;
  }
}
