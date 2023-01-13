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

  public hasAuthTokens(): boolean {
    return this.getAccessToken() != null && this.getRefreshToken() != null;
  }

  public setAuthTokens(accessToken: string, refreshToken: string): void {
    localStorage.setItem(this.accessTokenKey, accessToken);
    localStorage.setItem(this.refreshTokenKey, refreshToken);
  }

  public removeAuthTokens(): void {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
  }

  private isTokenValid(tokenKey: string): boolean {
    const token = localStorage.getItem(tokenKey);
    return token != null && !this.jwtHelper.isTokenExpired(token);
  }
}
