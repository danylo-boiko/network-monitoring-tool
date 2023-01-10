import { Injectable } from '@angular/core';
import { LoginCommandInput, LoginGQL, RegisterGQL } from "./graphql.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly authTokenKey: string = 'auth-token';

  constructor(private readonly _loginGQL: LoginGQL, private readonly _registerGQL: RegisterGQL) {
  }

  public login({username, password}: LoginCommandInput) {
    return this._loginGQL.mutate({
      input: {
        username,
        password
      }
    }, {
      errorPolicy: "all"
    });
  }

  private setAuthToken(token: string): void {
    localStorage.setItem(this.authTokenKey, token);
  }
}
