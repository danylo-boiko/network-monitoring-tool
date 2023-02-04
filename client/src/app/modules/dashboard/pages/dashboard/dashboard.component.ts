import { Component, NgZone, OnInit } from '@angular/core';
import { UsersService } from "../../../graphql/services/users.service";
import { PacketsService } from 'src/app/modules/graphql/services/packets.service';
import { ApolloError, ApolloQueryResult } from '@apollo/client/core';
import { DateRangeService } from '../../services/date-range.service';
import { Toaster } from "ngx-toast-notifications";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { filter, map } from "rxjs";
import { DeviceDto, GetUserInfoQuery, IpFilterDto, UserDto } from "../../../graphql/services/graphql.service";

@UntilDestroy()
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  public ipFilters!: IpFilterDto[];
  public devices!: DeviceDto[]

  constructor(
    private readonly _usersService: UsersService,
    private readonly _packetsService: PacketsService,
    private readonly _dateRangeService: DateRangeService,
    private readonly _ngZone: NgZone,
    private readonly _toaster: Toaster) {
  }

  public ngOnInit(): void {
    this.getUserInfo();
  }

  private getUserInfo(): void {
    this._usersService
      .getUserInfo()
      .pipe(
        untilDestroyed(this),
        filter(response => !response.loading),
        map(response => (<ApolloQueryResult<GetUserInfoQuery>>response).data!.userInfo)
      )
      .subscribe({
        next: (userInfo: UserDto) => {
          this.ipFilters = userInfo.ipFilters;
          this.devices = userInfo.devices;
        },
        error: (err: ApolloError) => {
          this._toaster.open({
            caption: 'User info loading failed...',
            text: err.message,
            type: 'danger',
            position: 'top-right',
            duration: 5000
          });
        }
      });
  }
}
