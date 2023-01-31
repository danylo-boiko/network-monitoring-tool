import { Component, OnInit } from '@angular/core';
import { UsersService } from "../../../graphql/services/users.service";
import { IpFiltersService } from "../../../graphql/services/ip-filters.service";
import { PacketsService } from 'src/app/modules/graphql/services/packets.service';
import { ApolloError, ApolloQueryResult } from '@apollo/client/core';
import { DateRangeService } from '../../services/date-range.service';
import { DateRangeMode } from '../../enums/date-range-mode.enum';
import { Toaster } from "ngx-toast-notifications";
import {
  GetPacketsByDeviceIdQuery,
  GetUserInfoQuery,
  PacketDto,
  UserDto
} from "../../../graphql/services/graphql.service";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  public userInfo!: UserDto;
  public packets!: Array<PacketDto>;
  public dateRangeMode!: DateRangeMode;

  constructor(
    private readonly _usersService: UsersService,
    private readonly _ipFiltersService: IpFiltersService,
    private readonly _packetsService: PacketsService,
    private readonly _dateRangeService: DateRangeService,
    private readonly _toaster: Toaster) {
  }

  public ngOnInit(): void {
    this.getUserInfo();
  }

  private getUserInfo(): void {
    this._usersService
      .getUserInfo()
      .subscribe({
        next: (response: ApolloQueryResult<GetUserInfoQuery>) => {
          if (!response.loading) {
            this.userInfo = response.data.userInfo;
          }
        },
        error: (err: ApolloError) => this.showLoadingErrorToaster("User info", err.message)
      });
  }

  private getPackets(deviceId: string): void {
    this._packetsService
      .getPacketsByDeviceId({
        deviceId
      })
      .subscribe({
        next: (response: ApolloQueryResult<GetPacketsByDeviceIdQuery>) => {
          if (!response.loading) {
            this.packets = response.data.packetsByDeviceId;
          }
        },
        error: (err: ApolloError) => this.showLoadingErrorToaster("Packets", err.message)
      });
  }

  private showLoadingErrorToaster(topic: string, text: string): void {
    this._toaster.open({
      caption: `${topic} loading failed...`,
      text,
      type: 'danger',
      position: 'top-right',
      duration: 5000
    });
  }
}
