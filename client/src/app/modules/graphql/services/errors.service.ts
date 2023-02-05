import { Injectable } from '@angular/core';
import { ApolloError } from "@apollo/client/core";
import { HttpErrorResponse } from "@angular/common/http";
import { GraphQLError } from "graphql";
import { FormGroup } from "@angular/forms";
import { ToasterService } from "../../../core/services/toaster.service";

@Injectable({
  providedIn: 'root'
})
export class ErrorsService {
  constructor(private readonly _toasterService: ToasterService) {
  }

  public getGraphQLErrors(error: ApolloError): Array<GraphQLError> {
    const graphQLErrors = new Array<GraphQLError>();

    const errorResponse = error?.networkError as HttpErrorResponse;
    const networkErrors = errorResponse?.error?.errors;

    if (!networkErrors) {
      return graphQLErrors;
    }

    for (const idx in networkErrors) {
      const { message, extensions } = networkErrors[idx];

      graphQLErrors.push(new GraphQLError(message, {
        extensions: !extensions ? {} : {
          code: this.lowerizeFirstLetter(extensions.code),
          property: this.lowerizeFirstLetter(extensions.property)
        },
      }));
    }

    return graphQLErrors;
  }

  public applyGraphQLErrorsToForm(form: FormGroup, graphQLErrors: Array<GraphQLError>): void {
    for (const error of graphQLErrors) {
      const property = error.extensions['property'];
      if (!property) {
        throw "Property doesn't exist";
      }

      const control = form.get(property.toString());
      if (!control) {
        this._toasterService.showError(error.message);
        throw `Control for property ${property.toString()} doesn't exists`;
      }

      control.setErrors({
        serverValidation: error.message
      });
    }
  }

  private lowerizeFirstLetter(str?: string): string | null {
    if (!str) {
      return null;
    }
    return str.charAt(0).toLowerCase() + str.slice(1);
  }
}
