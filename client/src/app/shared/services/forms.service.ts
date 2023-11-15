import { Injectable } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { IApiError } from '../../core/models/api-error';

@Injectable({
  providedIn: 'root',
})
export class FormsService {
  public handleRemoteErrors(
    error: IApiError,
    submittedForm?: FormGroup,
  ): string[] {
    if (!submittedForm) {
      this.showErrorsAlert(error);
    }
    if (!error?.errors) {
      return error?.genericErrors;
    }

    for (const key of Object.keys(error.errors)) {
      const message = error.errors[key];
      if (submittedForm?.controls[key] != null) {
        submittedForm.controls[key].setErrors(
          { remote: message },
          { emitEvent: true },
        );
        submittedForm.controls[key].markAsDirty();
      }
    }
    return error.genericErrors;
  }

  public populateControl(
    control: FormControl,
    value: string | number | boolean,
    clearErrors = true,
    emitEvent = true,
  ): void {
    control.setValue(value, { emitEvent });
    control.markAsPristine({ onlySelf: true });
    if (clearErrors) {
      control.setErrors(null);
    }
  }

  private showErrorsAlert(error: any): void {
    let content = '';
    for (const key in error.body.validation) {
      if (error.body.validation[key]) {
        const message = error.body.validation[key].join('<br>');
        content += message;
      }
    }
    alert(content);
  }
}
