import { Injectable } from '@angular/core';
import { ApolloError } from "@apollo/client/core";
import { HttpErrorResponse } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ErrorsService {
  public getValidationErrors(err: ApolloError): Map<string, Array<string>> {
    const validationErrors = new Map<string, Array<string>>();

    const networkError = err?.networkError as HttpErrorResponse;
    const errors = networkError?.error?.errors;

    if (!errors) {
      return validationErrors;
    }

    for (const idx in errors) {
      const error = errors[idx];
      const property = error?.extensions?.property;

      if (property) {
        const key = this.lowerizeFirstLetter(property);
        if (!validationErrors.has(key)) {
          validationErrors.set(key, new Array<string>());
        }
        validationErrors.get(key)!.push(error.message);
      }
    }

    return validationErrors;
  }

  private lowerizeFirstLetter(str: string): string {
    return str.charAt(0).toLowerCase() + str.slice(1);
  }
}
