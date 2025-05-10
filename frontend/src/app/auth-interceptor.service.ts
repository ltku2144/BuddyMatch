import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const email = localStorage.getItem('auth_email');
    const password = localStorage.getItem('auth_password');

    if (email && password) {
      const token = btoa(`${email}:${password}`);
      const cloned = req.clone({
        headers: req.headers.set('Authorization', `Basic ${token}`)
      });
      return next.handle(cloned);
    }

    return next.handle(req);
  }
}
