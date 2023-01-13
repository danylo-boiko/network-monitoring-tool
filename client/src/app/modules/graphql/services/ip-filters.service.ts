import { Injectable } from '@angular/core';
import {
  CreateIpFilterCommandInput,
  CreateIpFilterGQL,
  DeleteIpFilterCommandInput,
  DeleteIpFilterGQL,
} from "./graphql.service";

@Injectable({
  providedIn: 'root'
})
export class IpFiltersService {
  constructor(
    private readonly _createIpFilter: CreateIpFilterGQL,
    private readonly _deleteIpFilter: DeleteIpFilterGQL) {
  }

  public createIpFilter({userId, ip, filterAction, comment = null}: CreateIpFilterCommandInput) {
    return this._createIpFilter.mutate({
      input: {
        userId,
        ip,
        filterAction,
        comment
      }
    }, {
      errorPolicy: 'all',
    });
  }

  public deleteIpFilter({ipFilterId}: DeleteIpFilterCommandInput) {
    return this._deleteIpFilter.mutate({
      input: {
        ipFilterId
      }
    }, {
      errorPolicy: 'all',
    });
  }
}
