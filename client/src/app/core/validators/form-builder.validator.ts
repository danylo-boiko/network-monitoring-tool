import { FormGroup } from "@angular/forms";
import { RegisterForm } from "../../modules/auth/pages/register/register.form";

export function matchingControlsValidator(controlName: string, matchingControlName: string) {
  return (formGroup: FormGroup<RegisterForm>) => {
    const control = formGroup.get(controlName)!;
    const matchingControl = formGroup.get(matchingControlName)!;

    if (matchingControl.errors && !matchingControl.getError('matching')) {
      return;
    }

    if (control.value !== matchingControl.value) {
      matchingControl.setErrors({ matching: true });
    } else {
      matchingControl.setErrors(null);
    }
  };
}
