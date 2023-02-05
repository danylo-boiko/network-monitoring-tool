import { Injectable } from '@angular/core';
import { GetPacketsChartDataByDeviceIdGQL, GetPacketsChartDataByDeviceIdQueryInput } from './graphql.service';

@Injectable({
  providedIn: 'root'
})
export class PacketsService {
  constructor(private readonly _getPacketsChartDataByDeviceId: GetPacketsChartDataByDeviceIdGQL) {
  }

  public getPacketsChartDataByDeviceId({deviceId, dateRangeMode}: GetPacketsChartDataByDeviceIdQueryInput) {
    return this._getPacketsChartDataByDeviceId.fetch({
      input: {
        deviceId,
        dateRangeMode
      }
    });
  }
}
