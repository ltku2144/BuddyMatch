import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5072/api/user'; // Adjust to your actual API URL
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasValidToken());
  
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { email, password })
      .pipe(
        tap(response => {
          // Store the auth token in localStorage
          localStorage.setItem('auth_token', response.token);
          localStorage.setItem('user_id', response.userId);
          this.isAuthenticatedSubject.next(true);
        })
      );
  }

  logout(): void {
    // Clear auth data
    localStorage.removeItem('auth_token');
    localStorage.removeItem('user_id');
    this.isAuthenticatedSubject.next(false);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  getUserId(): string | null {
    return localStorage.getItem('user_id');
  }

  isAuthenticated(): boolean {
    return this.hasValidToken();
  }

  private hasValidToken(): boolean {
    // Check if token exists - in a real app, you would also check if it's expired
    return !!localStorage.getItem('auth_token');
  }
}
