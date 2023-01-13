import { Injectable } from '@angular/core';
import { ApolloError } from "@apollo/client/core";
import { HttpErrorResponse } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ErrorsService {
  constructor() {
  }

  public getValidationErrors(err: ApolloError): Map<string, Array<string>> {
    const validationErrors = new Map<string, Array<string>>();

    const networkError = err?.networkError as HttpErrorResponse;
    const errors = networkError?.error?.errors;

    if (!errors) {
      return validationErrors;
    }

    for (const idx in errors) {
      const error = errors[idx];
      const property = error?.extensions?.property?.toLowerCase();

      if (property) {
        if (!validationErrors.has(property)) {
          validationErrors.set(property, new Array<string>());
        }
        validationErrors.get(property)!.push(error.message);
      }
    }

    return validationErrors;
  }
}
