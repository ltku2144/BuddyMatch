import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="container py-5" style="max-width: 520px;">
      <div class="card shadow border-0">
        <div class="card-body p-4">
          <h2 class="mb-4 text-center fw-bold">Create Your Study Buddies Account</h2>
          <form autocomplete="on">
            <div class="mb-3">
              <label for="name" class="form-label">Full Name</label>
              <input type="text" class="form-control" id="name" name="name" placeholder="e.g. Anna Jensen" required autocomplete="name">
            </div>
            <div class="mb-3">
              <label for="email" class="form-label">Email address</label>
              <input type="email" class="form-control" id="email" name="email" placeholder="e.g. anna.j@cbs.dk" required autocomplete="email">
            </div>
            <div class="mb-3">
              <label for="program" class="form-label">Program</label>
              <input type="text" class="form-control" id="program" name="program" placeholder="e.g. Bachelor - International Business" required>
            </div>
            <div class="mb-3">
              <label for="interests" class="form-label">Interests</label>
              <input type="text" class="form-control" id="interests" name="interests" placeholder="e.g. Marketing, Case Studies" required>
              <div class="form-text">Separate multiple interests with commas.</div>
            </div>
            <div class="mb-3">
              <label for="availability" class="form-label">Availability</label>
              <input type="text" class="form-control" id="availability" name="availability" placeholder="e.g. Mon-Wed 10:00–13:00" required>
            </div>
            <div class="mb-3">
              <label for="password" class="form-label">Password</label>
              <input type="password" class="form-control" id="password" name="password" placeholder="Create a password" required autocomplete="new-password">
            </div>
            <div class="mb-3">
              <label for="confirmPassword" class="form-label">Confirm Password</label>
              <input type="password" class="form-control" id="confirmPassword" name="confirmPassword" placeholder="Repeat your password" required autocomplete="new-password">
            </div>
            <button type="submit" class="btn btn-success w-100 mb-2">Register</button>
            <a routerLink="/login" class="btn btn-outline-secondary w-100">Already have an account? Log In</a>
          </form>
        </div>
      </div>
    </section>
  `
})
export class RegisterComponent {}
