import { Injectable } from '@angular/core';
import { GetUserByIdGQL, GetUserWithDevicesAndIpFiltersByIdQueryInput } from "./graphql.service";

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  constructor(private readonly _getUserById: GetUserByIdGQL) {
  }

  public getUserById({userId}: GetUserWithDevicesAndIpFiltersByIdQueryInput) {
    return this._getUserById.fetch({
      input: {
        userId
      }
    }, {
      errorPolicy: 'all',
    });
  }
}
