import { FormControl } from "@angular/forms";

export interface RegisterForm {
  username: FormControl<string>;
  email: FormControl<string>;
  password: FormControl<string>;
  confirmPassword: FormControl<string>;
  showPassword: FormControl<boolean>;
}
