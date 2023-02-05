import { Injectable } from '@angular/core';
import { catchError, filter, map, Observable, switchMap, throwError } from 'rxjs';
import { JwtTokenService } from "../services/jwt-token.service";
import { AuthService } from "../../modules/graphql/services/auth.service";
import { MutationResult } from 'apollo-angular';
import { RefreshTokenMutation, TokenDto } from 'src/app/modules/graphql/services/graphql.service';
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';

@UntilDestroy()
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isTokenRefreshing = false;

  constructor(private readonly _authService: AuthService, private readonly _jwtTokenService: JwtTokenService) {
  }

  public intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (this._jwtTokenService.isAccessTokenValid()) {
      return next.handle(this.addAuthToken(request));
    }

    if (this._jwtTokenService.hasAuthTokens() && this._jwtTokenService.isRefreshTokenValid()) {
      if (!this.isTokenRefreshing) {
        this.isTokenRefreshing = true;

        return this._authService.refreshToken({
          accessToken: this._jwtTokenService.getAccessToken()!,
          refreshToken: this._jwtTokenService.getRefreshToken()!
        }).pipe(
          untilDestroyed(this),
          filter(response => !response.loading),
          map(response => (<MutationResult<RefreshTokenMutation>>response).data!.refreshToken),
          switchMap((tokens: TokenDto) => {
            this.isTokenRefreshing = false;

            this._jwtTokenService.setAuthTokens(tokens.accessToken, tokens.refreshToken);

            return next.handle(this.addAuthToken(request));
          }),
          catchError(error => {
            this.isTokenRefreshing = false;
            this._jwtTokenService.removeAuthTokens();

            return throwError(() => error);
          })
        );
      }
    }

    return next.handle(request);
  }

  private addAuthToken(request: HttpRequest<unknown>): HttpRequest<unknown> {
    const accessToken = this._jwtTokenService.getAccessToken();

    if (accessToken == null) {
      return request;
    }

    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }
}
