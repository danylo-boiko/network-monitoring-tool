import { Component, Input, NgZone } from '@angular/core';
import { DeviceDto, GetPacketsByDeviceIdQuery, PacketDto } from "../../../graphql/services/graphql.service";
import { DateRangeMode } from "../../enums/date-range-mode.enum";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { filter, map } from "rxjs";
import { ApolloError, ApolloQueryResult } from "@apollo/client/core";
import { PacketsService } from "../../../graphql/services/packets.service";
import { DateRangeService } from "../../services/date-range.service";
import { Toaster } from "ngx-toast-notifications";

@UntilDestroy()
@Component({
  selector: 'app-packets-chart',
  templateUrl: './packets-chart.component.html',
  styleUrls: ['./packets-chart.component.scss']
})
export class PacketsChartComponent {
  @Input() devices: DeviceDto[] | undefined;

  public packets!: PacketDto[];
  public dateRangeMode!: DateRangeMode;

  constructor(
    private readonly _packetsService: PacketsService,
    private readonly _dateRangeService: DateRangeService,
    private readonly _ngZone: NgZone,
    private readonly _toaster: Toaster) {
  }

  private getPackets(deviceId: string): void {
    this._packetsService
      .getPacketsByDeviceId({
        deviceId
      })
      .pipe(
        untilDestroyed(this),
        filter(response => !response.loading),
        map(response => (<ApolloQueryResult<GetPacketsByDeviceIdQuery>>response).data!.packetsByDeviceId)
      )
      .subscribe({
        next: (packets: Array<PacketDto>) => {
          this.packets = packets;
        },
        error: (err: ApolloError) => {
          this._toaster.open({
            caption: 'Packets loading failed...',
            text: err.message,
            type: 'danger',
            position: 'top-right',
            duration: 5000
          });
        }
      });
  }
}
