import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  template: `
    <section class="container py-5" style="max-width: 420px;">
      <div class="card shadow border-0">
        <div class="card-body p-4">
          <h2 class="mb-4 text-center fw-bold">Log In to Study Buddies</h2>

          <form (ngSubmit)="onSubmit()" #loginForm="ngForm" autocomplete="on">
            <div class="mb-3">
              <label for="email" class="form-label">Email address</label>
              <input type="email" class="form-control" id="email" name="email"
                     [(ngModel)]="email" placeholder="Enter your email"
                     required autocomplete="email">
            </div>
            <div class="mb-3">
              <label for="password" class="form-label">Password</label>
              <input type="password" class="form-control" id="password" name="password"
                     [(ngModel)]="password" placeholder="Enter your password"
                     required autocomplete="current-password">
            </div>
            <div class="mb-3 form-check">
              <input type="checkbox" class="form-check-input" id="rememberMe">
              <label class="form-check-label" for="rememberMe">Remember me</label>
            </div>

            <div *ngIf="errorMessage" class="alert alert-danger">
              {{ errorMessage }}
            </div>

            <button type="submit" class="btn btn-primary w-100 mb-2" [disabled]="isLoading">
              {{ isLoading ? 'Logging in...' : 'Log In' }}
            </button>
            <a routerLink="/register" class="btn btn-outline-secondary w-100">Create an Account</a>
          </form>
          <div class="mt-3 text-center">
            <a href="#" class="link-primary">Forgot your password?</a>
          </div>
        </div>
      </div>
    </section>
  `
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    this.isLoading = true;
    this.authService.login(this.email, this.password).subscribe({
      next: (response) => {
        localStorage.setItem('userId', response.userId);
        localStorage.setItem('token', response.token);

        // 🔐 Save credentials for BasicAuthInterceptor
        localStorage.setItem('auth_email', this.email);
        localStorage.setItem('auth_password', this.password);

        this.router.navigate(['/profile']);
      },
      error: (err) => {
        console.error('Login failed:', err);
        this.errorMessage = 'Invalid email or password.';
        this.isLoading = false;
      }
    });
  }
}
