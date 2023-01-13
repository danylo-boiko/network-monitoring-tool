import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { JwtTokenService } from "../modules/shared/services/jwt-token.service";
import { AuthService } from "../modules/graphql/services/auth.service";
import { MutationResult } from "apollo-angular";
import { RefreshTokenMutation } from "../modules/graphql/services/graphql.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private readonly _authService: AuthService, private readonly _jwtTokenService: JwtTokenService) {
  }

  public intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (this._jwtTokenService.isAccessTokenValid()) {
      const accessToken = this._jwtTokenService.getAccessToken()!;
      request = this.addAccessTokenToRequest(request, accessToken);
    } else if (this._jwtTokenService.hasAuthTokens() && this._jwtTokenService.isRefreshTokenValid()) {
      this._authService.refreshToken({
        accessToken: this._jwtTokenService.getAccessToken()!,
        refreshToken: this._jwtTokenService.getRefreshToken()!
      }).subscribe({
        next: (response: MutationResult<RefreshTokenMutation>) => {
          if (!response.loading) {
            const refreshedTokens = response.data?.refreshToken!;
            this._jwtTokenService.setAuthTokens(refreshedTokens.accessToken, refreshedTokens.refreshToken);
            request = this.addAccessTokenToRequest(request, refreshedTokens.accessToken);
          }
        },
        error: () => this._jwtTokenService.removeAuthTokens()
      })
    } else {
      this._jwtTokenService.removeAuthTokens();
    }

    return next.handle(request);
  }

  private addAccessTokenToRequest(request: HttpRequest<unknown>, accessToken: string): HttpRequest<unknown> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }
}
