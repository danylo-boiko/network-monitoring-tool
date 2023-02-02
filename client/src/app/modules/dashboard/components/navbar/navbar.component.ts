import { Component } from '@angular/core';
import { JwtTokenService } from "../../../../core/services/jwt-token.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  constructor(private readonly _jwtTokenService: JwtTokenService, private readonly _router: Router) {
  }

  public isUserValid(): boolean {
    return this._jwtTokenService.isAccessTokenValid() || this._jwtTokenService.isRefreshTokenValid();
  }

  public logout(): void {
    this._jwtTokenService.removeAuthTokens();
    this._router.navigateByUrl('/login');
  }
}
