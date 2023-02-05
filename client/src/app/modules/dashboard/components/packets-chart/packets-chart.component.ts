import { Component, Input, NgZone } from '@angular/core';
import { DeviceDto, GetPacketsByDeviceIdQuery, PacketDto } from "../../../graphql/services/graphql.service";
import { DateRangeMode } from "../../enums/date-range-mode.enum";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { filter, map } from "rxjs";
import { ApolloError, ApolloQueryResult } from "@apollo/client/core";
import { PacketsService } from "../../../graphql/services/packets.service";
import { DateRangeService } from "../../services/date-range.service";
import { Toaster } from "ngx-toast-notifications";
import { ToasterService } from "../../../../core/services/toaster.service";

@UntilDestroy()
@Component({
  selector: 'app-packets-chart',
  templateUrl: './packets-chart.component.html',
  styleUrls: ['./packets-chart.component.scss']
})
export class PacketsChartComponent {
  @Input() devices!: DeviceDto[];

  public packets: PacketDto[] = [];
  public dateRangeMode: DateRangeMode = DateRangeMode.Day;

  constructor(
    private readonly _packetsService: PacketsService,
    private readonly _dateRangeService: DateRangeService,
    private readonly _toasterService: ToasterService,
    private readonly _ngZone: NgZone) {
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
        error: (error: ApolloError) => {
          this._toasterService.showError(error.message);
        }
      });
  }
}
