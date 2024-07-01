// import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
// import { inject } from '@angular/core';
// import { Router } from '@angular/router';
// import { catchError, finalize, switchMap, tap } from 'rxjs';
// import { SpinnerService } from '../services/SpinnerServices/spinner.service';
// import { AuthService } from '../services/authServices/auth.service';

// export const loggerInterceptor: HttpInterceptorFn = (req, next) => {
//   const router = inject(Router);
//   const spinnerServices = inject(SpinnerService);
//   const authService = inject(AuthService);
//   spinnerServices.showSpinner.next(true);
//   const authreq = req.clone({
//     headers: req.headers.set(
//       'Authorization',
//       'Bearer ' + localStorage.getItem('accessToken')
//     ),
//   });

//   return next(authreq).pipe(
//     catchError((err) => {
//       if (err instanceof HttpErrorResponse) {
//         if (err.status === 401) {
//           let refreshToken: string = localStorage.getItem('refreshToken')!;
//           if (refreshToken === null || refreshToken === undefined) {
//             router.navigateByUrl('/home');
//             localStorage.clear();
//           } else {
//             authService.getTokenByrefreshToken(refreshToken).subscribe({
//               next: (res) => {
//                 localStorage.setItem('accessToken', res.jwttoken);
//                 localStorage.setItem('refreshToken', res.refreshToken);

//                 const newAuthReq = req.clone({
//                   headers: req.headers.set(
//                     'Authorization',
//                     'Bearer ' + res.jwttoken
//                   ),
//                 });
//                 return next(newAuthReq);

//               },
//               error: (refreshErr) => {
//                 localStorage.clear();
//                 router.navigateByUrl('/home');
//                 throw refreshErr;
//               },
//             });
//           }
//         }
//       }
//       throw err;
//     }),
//     finalize(() => {
//       spinnerServices.showSpinner.next(false);
//     })
//   );
// };

import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, finalize, switchMap, tap } from 'rxjs';
import { SpinnerService } from '../services/SpinnerServices/spinner.service';
import { AuthService } from '../services/authServices/auth.service';

export const loggerInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const spinnerServices = inject(SpinnerService);
  const authService = inject(AuthService);
  spinnerServices.showSpinner.next(true);
  const authreq = req.clone({
    headers: req.headers.set(
      'Authorization',
      'Bearer ' + localStorage.getItem('accessToken')
    ),
  });

  return next(authreq).pipe(
    catchError((err) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401) {
          let refreshToken: string = localStorage.getItem('refreshToken')!;
          if (refreshToken === null || refreshToken === undefined) {
            router.navigateByUrl('/home');
            localStorage.clear();
          } else {
            return authService.getTokenByrefreshToken(refreshToken).pipe(
              switchMap((res) => {
                localStorage.setItem('accessToken', res.jwttoken);
                localStorage.setItem('refreshToken', res.refreshToken);

                const newAuthReq = req.clone({
                  headers: req.headers.set(
                    'Authorization',
                    'Bearer ' + res.jwttoken
                  ),
                });
                return next(newAuthReq).pipe(
                  catchError((refreshErr) => {
                    localStorage.clear();
                    router.navigateByUrl('/home');
                    throw refreshErr;
                  })
                );
              })
            );
          }
        }
      }
      throw err;
    }),
    finalize(() => {
      spinnerServices.showSpinner.next(false);
    })
  );
};
