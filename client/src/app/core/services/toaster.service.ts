import { Injectable } from '@angular/core';
import { Toaster } from 'ngx-toast-notifications';

@Injectable({
  providedIn: 'root'
})
export class ToasterService {
  constructor(private readonly _toaster: Toaster) {
  }

  public showError(message: string): void {
    this._toaster.open({
      text: message,
      type: 'danger',
      position: 'top-right',
      duration: 5000
    });
  }

  public showSuccess(message: string): void {
    this._toaster.open({
      text: message,
      type: 'success',
      position: 'top-right',
      duration: 5000
    });
  }
}
