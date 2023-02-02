import { AbstractControl, FormGroup } from "@angular/forms";

export function isFormFieldValid(form: FormGroup, controlName: string, ruleName: string): boolean {
  if (!controlName) {
    return form.valid;
  }

  const component: AbstractControl = form.get(controlName)!;

  if (!ruleName) {
    return component.valid || component.untouched;
  }

  return !component.hasError(ruleName) || component.untouched;
}
