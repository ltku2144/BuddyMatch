import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService, User } from './user.service'; 
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <section class="container py-5" style="max-width: 420px;">
      <div class="card shadow border-0">
        <div class="card-body p-4">
          <h2 class="mb-4 text-center fw-bold">Create an Account</h2>
          
          <form (ngSubmit)="onSubmit()" #registerForm="ngForm" autocomplete="on" novalidate>
            <!-- Full Name -->
            <div class="mb-3">
              <label for="name" class="form-label">Full Name</label>
              <input type="text" class="form-control" id="name" name="name" 
                     [(ngModel)]="name" placeholder="Enter your full name" 
                     required minlength="3" #nameInput="ngModel">
              <div class="text-danger" *ngIf="nameInput.invalid && nameInput.touched">
                <small *ngIf="nameInput.errors?.['required']">Name is required.</small>
                <small *ngIf="nameInput.errors?.['minlength']">Name must be at least 3 characters long.</small>
              </div>
            </div>

            <!-- Email -->
            <div class="mb-3">
              <label for="email" class="form-label">Email address</label>
              <input type="email" class="form-control" id="email" name="email" 
                     [(ngModel)]="email" placeholder="Enter your email" 
                     required email #emailInput="ngModel">
              <div class="text-danger" *ngIf="emailInput.invalid && emailInput.touched">
                <small *ngIf="emailInput.errors?.['required']">Email is required.</small>
                <small *ngIf="emailInput.errors?.['email']">Invalid email format.</small>
              </div>
            </div>

            <!-- Password -->
            <div class="mb-3">
              <label for="password" class="form-label">Password</label>
              <input type="password" class="form-control" id="password" name="password" 
                     [(ngModel)]="password" placeholder="Enter your password" 
                     required minlength="8" #passwordInput="ngModel">
              <div class="text-danger" *ngIf="passwordInput.invalid && passwordInput.touched">
                <small *ngIf="passwordInput.errors?.['required']">Password is required.</small>
                <small *ngIf="passwordInput.errors?.['minlength']">Password must be at least 8 characters long.</small>
              </div>
            </div>

            <!-- Program -->
            <div class="mb-3">
              <label for="program" class="form-label">Program</label>
              <input type="text" class="form-control" id="program" name="program" 
                     [(ngModel)]="program" placeholder="Enter your program">
            </div>

            <!-- Interests -->
            <div class="mb-3">
              <label for="interests" class="form-label">Interests</label>
              <input type="text" class="form-control" id="interests" name="interests" 
                     [(ngModel)]="interests" placeholder="Enter your interests">
            </div>

            <!-- Availability -->
            <div class="mb-3">
              <label for="availability" class="form-label">Availability</label>
              <input type="text" class="form-control" id="availability" name="availability" 
                     [(ngModel)]="availability" placeholder="Enter your availability">
            </div>

            <!-- Error Message -->
            <div *ngIf="errorMessage" class="alert alert-danger">
              {{ errorMessage }}
            </div>

            <!-- Submit Button -->
            <button type="submit" class="btn btn-primary w-100 mb-2" [disabled]="isLoading || registerForm.invalid">
              {{ isLoading ? 'Creating Account...' : 'Create Account' }}
            </button>
            <a class="btn btn-outline-secondary w-100" (click)="redirectToLogin()">Already have an account? Log In</a>
          </form>
        </div>
      </div>
    </section>
  `
})
export class RegisterComponent {
  name: string = '';
  email: string = '';
  password: string = '';
  program: string = '';
  interests: string = '';
  availability: string = '';
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(private userService: UserService, private router: Router) {}

  onSubmit() {
    if (!this.name || !this.email || !this.password) {
      this.errorMessage = 'Please fill in all required fields';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const newUser = {
      name: this.name,
      email: this.email,
      password: this.password,
      program: this.program,
      interests: this.interests,
      availability: this.availability
    };

    this.userService.create(newUser).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Registration failed:', err);
        this.errorMessage = 'Failed to create account. Please try again.';
        this.isLoading = false;
      }
    });
  }

  // Add this method to handle redirection to the login page
  redirectToLogin() {
    this.router.navigate(['/login']);
  }
}
