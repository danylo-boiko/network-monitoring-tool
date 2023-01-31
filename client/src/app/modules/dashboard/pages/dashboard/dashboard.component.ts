import { Component, OnInit } from '@angular/core';
import { UsersService } from "../../../graphql/services/users.service";
import { IpFiltersService } from "../../../graphql/services/ip-filters.service";
import { PacketsService } from 'src/app/modules/graphql/services/packets.service';
import { ApolloQueryResult } from '@apollo/client/core';
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

  constructor(
    private readonly _usersService: UsersService,
    private readonly _ipFiltersService: IpFiltersService,
    private readonly _packetsService: PacketsService) {
  }

  public ngOnInit(): void {
    this.getUserInfo();
    this.getPackets('34e867a2-bcb5-4cdc-827e-2534c28dd0f9');
  }

  private getUserInfo(): void {
    this._usersService
      .getUserInfo()
      .subscribe({
        next: (response: ApolloQueryResult<GetUserInfoQuery>) => {
          if (!response.loading) {
            this.userInfo = response.data.userInfo;
          }
        }
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
        }
      });
  }
}
