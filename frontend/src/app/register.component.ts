import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <section class="container py-5" style="max-width: 520px;">
      <div class="card shadow border-0">
        <div class="card-body p-4">
          <h2 class="mb-4 text-center fw-bold">Create Your Study Buddies Account</h2>
          <form #registerForm="ngForm" (ngSubmit)="onSubmit(registerForm)" autocomplete="on" novalidate>
            <div class="mb-3">
              <label for="name" class="form-label">Full Name</label>
              <input
                type="text"
                class="form-control"
                id="name"
                name="name"
                placeholder="e.g. Anna Jensen"
                required
                minlength="2"
                autocomplete="name"
                [(ngModel)]="user.name"
                #name="ngModel"
              >
              <div class="text-danger" *ngIf="name.invalid && name.touched">
                Name is required (min 2 characters).
              </div>
            </div>
            <div class="mb-3">
              <label for="email" class="form-label">Email address</label>
              <input
                type="email"
                class="form-control"
                id="email"
                name="email"
                placeholder="e.g. anna.j@cbs.dk"
                required
                email
                autocomplete="email"
                [(ngModel)]="user.email"
                #email="ngModel"
              >
              <div class="text-danger" *ngIf="email.invalid && email.touched">
                Enter a valid email address.
              </div>
            </div>
            <div class="mb-3">
              <label for="program" class="form-label">Program</label>
              <input
                type="text"
                class="form-control"
                id="program"
                name="program"
                placeholder="e.g. Bachelor - International Business"
                required
                [(ngModel)]="user.program"
                #program="ngModel"
              >
              <div class="text-danger" *ngIf="program.invalid && program.touched">
                Program is required.
              </div>
            </div>
            <div class="mb-3">
              <label for="interests" class="form-label">Interests</label>
              <input
                type="text"
                class="form-control"
                id="interests"
                name="interests"
                placeholder="e.g. Marketing, Case Studies"
                required
                [(ngModel)]="user.interests"
                #interests="ngModel"
              >
              <div class="form-text">Separate multiple interests with commas.</div>
              <div class="text-danger" *ngIf="interests.invalid && interests.touched">
                Interests are required.
              </div>
            </div>
            <div class="mb-3">
              <label for="availability" class="form-label">Availability</label>
              <input
                type="text"
                class="form-control"
                id="availability"
                name="availability"
                placeholder="e.g. Mon-Wed 10:00–13:00"
                required
                [(ngModel)]="user.availability"
                #availability="ngModel"
              >
              <div class="text-danger" *ngIf="availability.invalid && availability.touched">
                Availability is required.
              </div>
            </div>
            <div class="mb-3">
              <label for="password" class="form-label">Password</label>
              <input
                type="password"
                class="form-control"
                id="password"
                name="password"
                placeholder="Create a password"
                required
                minlength="6"
                autocomplete="new-password"
                [(ngModel)]="user.password"
                #password="ngModel"
              >
              <div class="text-danger" *ngIf="password.invalid && password.touched">
                Password is required (min 6 characters).
              </div>
            </div>
            <div class="mb-3">
              <label for="confirmPassword" class="form-label">Confirm Password</label>
              <input
                type="password"
                class="form-control"
                id="confirmPassword"
                name="confirmPassword"
                placeholder="Repeat your password"
                required
                autocomplete="new-password"
                [(ngModel)]="user.confirmPassword"
                #confirmPassword="ngModel"
              >
              <div class="text-danger" *ngIf="confirmPassword.invalid && confirmPassword.touched">
                Please confirm your password.
              </div>
              <div class="text-danger" *ngIf="user.password && user.confirmPassword && user.password !== user.confirmPassword && confirmPassword.touched">
                Passwords do not match.
              </div>
            </div>
            <button type="submit" class="btn btn-success w-100 mb-2" [disabled]="registerForm.invalid || (user.password !== user.confirmPassword)">
              Register
            </button>
            <a routerLink="/login" class="btn btn-outline-secondary w-100">Already have an account? Log In</a>
          </form>
        </div>
      </div>
    </section>
  `
})
export class RegisterComponent {
  user = {
    name: '',
    email: '',
    program: '',
    interests: '',
    availability: '',
    password: '',
    confirmPassword: ''
  };

  onSubmit(form: NgForm) {
    if (form.valid && this.user.password === this.user.confirmPassword) {
      // Registration logic here
      alert('Registration successful!');
      // Reset form or redirect as needed
    }
  }
}