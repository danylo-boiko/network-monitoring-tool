import { Component, NgZone, OnInit } from '@angular/core';
import { UsersService } from "../../../graphql/services/users.service";
import { IpFiltersService } from "../../../graphql/services/ip-filters.service";
import { PacketsService } from 'src/app/modules/graphql/services/packets.service';
import { ApolloError, ApolloQueryResult } from '@apollo/client/core';
import { DateRangeService } from '../../services/date-range.service';
import { DateRangeMode } from '../../enums/date-range-mode.enum';
import { Toaster } from "ngx-toast-notifications";
import { MatDialog } from "@angular/material/dialog";
import { CreateIpFilterComponent } from '../../dialogs/create-ip-filter/create-ip-filter.component';
import { UpdateIpFilterComponent } from '../../dialogs/update-ip-filter/update-ip-filter.component';
import { DeleteIpFilterComponent } from "../../dialogs/delete-ip-filter/delete-ip-filter.component";
import { intToIpString } from "../../../../core/utils/ip.util";
import {
  GetPacketsByDeviceIdQuery,
  GetUserInfoQuery,
  IpFilterDto,
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
  public ipFiltersTableColumns!: string[];

  constructor(
    private readonly _usersService: UsersService,
    private readonly _ipFiltersService: IpFiltersService,
    private readonly _packetsService: PacketsService,
    private readonly _dateRangeService: DateRangeService,
    public readonly _dialog: MatDialog,
    private readonly _ngZone: NgZone,
    private readonly _toaster: Toaster) {
  }

  public ngOnInit(): void {
    this.ipFiltersTableColumns = ['ip', 'filterAction', 'comment', 'actions'];
    this.getUserInfo();
  }

  public convertIntToIpString(ip: number): string {
    return intToIpString(ip);
  }

  public createIpFilter(): void {
    this._dialog.open(CreateIpFilterComponent);
  }

  public updateIpFilter(ipFilter: IpFilterDto): void {
    this._dialog.open(UpdateIpFilterComponent, {
      data: { ipFilter }
    });
  }

  public deleteIpFilter(ipFilter: IpFilterDto): void {
    this._dialog.open(DeleteIpFilterComponent, {
      data: { ipFilter }
    });
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
