import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { AuthService } from "../../graphql/services/auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public loginForm!: FormGroup;

  constructor(private readonly _authService: AuthService) {
  }

  public ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl(null, [
        Validators.required
      ]),
      password: new FormControl(null, [
        Validators.required
      ]),
      showPassword: new FormControl(false)
    });
  }

  public login(): void {
    if (!this.loginForm.valid) {
      return;
    }

    const { username, password } = this.loginForm.value;
    this._authService
      .login({username, password})
      .subscribe({
        next: value => console.log(value),
        error: err => console.log(err),
        complete: () => console.log("complete")
      });
  }
}
