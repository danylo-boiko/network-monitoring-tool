import { Injectable } from '@angular/core';
import {
  LoginCommandInput,
  LoginGQL,
  RefreshTokenCommandInput,
  RefreshTokenGQL,
  RegisterCommandInput,
  RegisterGQL,
  SendTwoFactorCodeCommandInput,
  SendTwoFactorCodeGQL,
  VerifyTwoFactorCodeCommandInput,
  VerifyTwoFactorCodeGQL,
} from "./graphql.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private readonly _loginGQL: LoginGQL,
    private readonly _registerGQL: RegisterGQL,
    private readonly _refreshToken: RefreshTokenGQL,
    private readonly _sendTwoFactorCode: SendTwoFactorCodeGQL,
    private readonly _verifyTwoFactorCode: VerifyTwoFactorCodeGQL) {
  }

  public login({username, password}: LoginCommandInput) {
    return this._loginGQL.mutate({
      input: {
        username,
        password
      }
    }, {
      errorPolicy: 'all',
    });
  }

  public register({username, email, password, confirmPassword}: RegisterCommandInput) {
    return this._registerGQL.mutate({
      input: {
        username,
        email,
        password,
        confirmPassword
      }
    }, {
      errorPolicy: 'all',
    });
  }

  public refreshToken({accessToken, refreshToken}: RefreshTokenCommandInput) {
    return this._refreshToken.mutate({
      input: {
        accessToken,
        refreshToken
      }
    }, {
      errorPolicy: 'all',
    });
  }

  public sendTwoFactorCode({email}: SendTwoFactorCodeCommandInput) {
    return this._sendTwoFactorCode.mutate({
      input: {
        email
      }
    }, {
      errorPolicy: 'all',
    });
  }

  public verifyTwoFactorCode({email, twoFactorCode}: VerifyTwoFactorCodeCommandInput) {
    return this._verifyTwoFactorCode.mutate({
      input: {
        email,
        twoFactorCode
      }
    }, {
      errorPolicy: 'all',
    });
  }
}
