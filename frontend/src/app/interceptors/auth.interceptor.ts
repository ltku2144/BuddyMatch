import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const email = localStorage.getItem('auth_email');
    const password = localStorage.getItem('auth_password');

    if (email && password) {
      const encoded = btoa(`${email}:${password}`);
      const authReq = req.clone({
        setHeaders: {
          Authorization: `Basic ${encoded}`
        }
      });
      return next.handle(authReq);
    }

    return next.handle(req);
  }
}
