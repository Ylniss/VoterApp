import {
  Directive,
  ElementRef,
  HostListener,
  Input,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { NgControl } from '@angular/forms';

export interface IValidationMessageInput {
  errorCode: string;
  message: string;
}

interface IValidationMessage {
  message: string;
  id: string;
}

@Directive({
  selector: '[appValidationMessages]',
  standalone: true,
})
export class ValidationMessagesDirective implements OnInit, OnDestroy {
  private _inputValidationMessages: IValidationMessageInput[] = [];
  private _validationMessages = new Map<string, IValidationMessage>();
  private _currentErrorCodes = [''];
  private readonly _remoteErrorCode = 'remote';
  private _statusChangeSubscription: Subscription = new Subscription();

  constructor(
    private el: ElementRef,
    private control: NgControl,
  ) {}

  @Input()
  public set appValidationMessages(
    validationMessages: IValidationMessageInput[],
  ) {
    this._inputValidationMessages = validationMessages;
  }

  ngOnInit(): void {
    this._inputValidationMessages.forEach((vm) => {
      this._validationMessages.set(vm.errorCode, {
        message: vm.message,
        id: vm.message + new Date(),
      });
    });

    this._validationMessages.set(this._remoteErrorCode, {
      message: '',
      id: this._remoteErrorCode,
    });

    if (this.control.statusChanges) {
      this._statusChangeSubscription = this.control.statusChanges.subscribe(
        (status: any) => {
          if (status !== 'INVALID') this.removeErrors();
        },
      );
    }
  }

  ngOnDestroy(): void {
    this._statusChangeSubscription?.unsubscribe();
  }

  @HostListener('blur')
  onLoseFocus() {
    this.control.errors ? this.showErrors() : this.removeErrors();
  }

  private showErrors() {
    this.removeErrors();

    if (this.control.errors) {
      this._currentErrorCodes = Object.keys(this.control.errors);

      this._currentErrorCodes.forEach((errorCode) => {
        this.insertErrorMessageToDom(errorCode);
      });

      this.el.nativeElement.classList.add('is-invalid');
    }
  }

  private removeErrors(): void {
    const errorsToRemove = new Set(this._validationMessages.keys());

    errorsToRemove.forEach((errorCode) => {
      const validationMessage = this._validationMessages.get(errorCode);
      if (validationMessage) {
        const errorElement = document.getElementById(validationMessage.id);
        if (errorElement) errorElement.remove();
      }
    });

    this.el.nativeElement.classList.remove('is-invalid');
  }

  private insertErrorMessageToDom(errorCode: string): void {
    const validationMsg = this.getErrorMessage(errorCode);
    if (!validationMsg) throw new Error(`Unhandled errorCode: ${errorCode}`);

    const errorMsgHtml = this.getErrorMessageHtml(validationMsg);
    this.el.nativeElement.parentElement.insertAdjacentHTML(
      'beforeend',
      errorMsgHtml,
    );
  }

  private getErrorMessage(errorCode: string): IValidationMessage | undefined {
    if (errorCode === this._remoteErrorCode) {
      const message = this.control.getError(errorCode);
      return {
        message: message,
        id: this._remoteErrorCode,
      } as IValidationMessage;
    } else {
      return this._validationMessages.get(errorCode);
    }
  }

  private getErrorMessageHtml(validationMsg: IValidationMessage): string {
    return `<div class="invalid-feedback" id="${validationMsg.id}">${validationMsg.message}</div>`;
  }
}
