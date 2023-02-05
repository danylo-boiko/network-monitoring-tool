import { Injectable } from '@angular/core';
import { GetUserInfoGQL } from "./graphql.service";

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  constructor(private readonly _getUserInfo: GetUserInfoGQL) {
  }

  public getUserInfo() {
    return this._getUserInfo.fetch();
  }
}
