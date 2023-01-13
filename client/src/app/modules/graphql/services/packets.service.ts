import { Injectable } from '@angular/core';
import { GetPacketsByDeviceIdGQL, GetPacketsByDeviceIdQueryInput } from "./graphql.service";

@Injectable({
  providedIn: 'root'
})
export class PacketsService {
  constructor(private readonly _getPacketsByDeviceId: GetPacketsByDeviceIdGQL) {
  }

  public getPacketsByDeviceId({deviceId, dateFrom = null, dateTo = null}: GetPacketsByDeviceIdQueryInput) {
    return this._getPacketsByDeviceId.fetch({
      input: {
        deviceId,
        dateFrom,
        dateTo
      }
    }, {
      errorPolicy: 'all',
    });
  }
}
