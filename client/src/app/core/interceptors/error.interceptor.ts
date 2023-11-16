import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, EMPTY, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { inject } from '@angular/core';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastr = inject(ToastrService);

  return next(req).pipe(
    catchError((error) => {
      if (error) {
        if (error.status === 400) {
          if (error.error.errors) {
            toastr.error(error.error.errors.join('<br>'), 'Validation error', {
              enableHtml: true,
            });
            return EMPTY;
          } else {
            throwError(() => error);
          }
        }
        if (error.status === 401) {
          toastr.error(error.error.message, error.error.statusCode);
        }
        if (error.status === 404) {
          router.navigateByUrl('/not-found');
        }
        if (error.status === 500) {
          const navigationExtras: NavigationExtras = { state: error.error };
          router.navigateByUrl('/server-error', navigationExtras);
        }
      }

      return throwError(() => error);
    }),
  );
};
