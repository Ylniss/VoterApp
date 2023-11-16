import { Component, Type, ViewContainerRef } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-base-dynamic-content',
  standalone: true,
  imports: [CommonModule],
  template: '',
})
export class BaseDynamicContentComponent {
  constructor(protected viewContainerRef: ViewContainerRef) {}

  protected loadComponent<T>(
    component: Type<T>,
    setInputFields?: (componentRef: T) => void,
  ) {
    this.viewContainerRef.clear();

    const componentRef = this.viewContainerRef.createComponent(component);

    if (setInputFields) {
      setInputFields(componentRef.instance);
    }
  }
}
