import { Injectable } from '@angular/core';
import { JwtHelperService } from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class JwtTokenService {
  private readonly accessTokenKey: string = 'access-token';
  private readonly refreshTokenKey: string = 'refresh-token';

  constructor(private readonly jwtHelper: JwtHelperService) {
  }

  public getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  public getRefreshToken(): string | null {
    return localStorage.getItem(this.refreshTokenKey);
  }

  public isAccessTokenValid(): boolean {
    return this.isTokenValid(this.accessTokenKey);
  }

  public isRefreshTokenValid(): boolean {
    return this.isTokenValid(this.refreshTokenKey);
  }

  public setAuthTokens(accessToken: string, refreshToken: string) {
    localStorage.setItem(this.accessTokenKey, accessToken);
    localStorage.setItem(this.refreshTokenKey, refreshToken);
  }

  private isTokenValid(token: string | null): boolean {
    if (token) {
      return !this.jwtHelper.isTokenExpired(token);
    }

    return false;
  }
}
