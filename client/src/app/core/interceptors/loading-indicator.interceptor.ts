import { HttpInterceptorFn } from '@angular/common/http';
import { finalize } from 'rxjs';
import { LoadingIndicatorService } from '../../shared/services/loading-indicator.service';
import { inject } from '@angular/core';

export const loadingIndicatorInterceptor: HttpInterceptorFn = (req, next) => {
  const loader = inject(LoadingIndicatorService);

  loader.increment();
  return next(req).pipe(
    finalize(() => {
      loader.decrement();
    }),
  );
};
