import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { JwtTokenService } from "./jwt-token.service";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class GuardService implements CanActivate {
  constructor(private readonly _jwtTokenService: JwtTokenService, private readonly _router: Router) {
  }

  public canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (!this._jwtTokenService.isAccessTokenValid()) {
      this._router.navigateByUrl("/login");
      return false;
    }
    return true;
  }
}
