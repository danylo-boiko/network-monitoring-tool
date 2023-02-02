import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";
import { isIpStringValid } from "../utils/ip.util";

export function ipValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    return !isIpStringValid(control.value) ? { invalidIpFormat: true } : null;
  };
}
