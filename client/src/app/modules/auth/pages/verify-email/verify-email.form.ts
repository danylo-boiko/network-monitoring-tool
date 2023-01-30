import { FormControl } from "@angular/forms";

export interface VerifyEmailForm {
  twoFactorCode: FormControl<string>;
}
