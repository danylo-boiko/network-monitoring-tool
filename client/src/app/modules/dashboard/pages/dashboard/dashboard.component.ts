import { Component, OnInit } from '@angular/core';
import { UsersService } from "../../../graphql/services/users.service";
import { ApolloError, ApolloQueryResult } from '@apollo/client/core';
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { filter, map } from "rxjs";
import { GetUserInfoQuery, UserDto } from "../../../graphql/services/graphql.service";
import { ToasterService } from "../../../../core/services/toaster.service";

@UntilDestroy()
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  public dashboardLoaded = false;
  public user!: UserDto;

  constructor(private readonly _usersService: UsersService, private readonly _toasterService: ToasterService) {
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
        next: (user: UserDto) => {
          this.user = user;
          this.dashboardLoaded = true;
        },
        error: (error: ApolloError) => this._toasterService.showError(error.message)
      });
  }
}
