import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="container py-5" style="max-width: 600px;">
      <div class="card border-0">
        <div class="card-body p-4">
          <h2 class="mb-4 text-center fw-bold">My Profile</h2>
          <div class="mb-4 text-center">
            <img src="assets/logo.png" alt="Profile Picture" width="96" height="96" class="rounded-circle mb-2" style="background:#35373c;">
            <h4 class="fw-bold mt-2">Anna Jensen</h4>
            <div class="text-muted" style="font-size:1.1em;">anna.j&#64;cbs.dk</div>
          </div>
          <dl class="row mb-0">
            <dt class="col-5">Program</dt>
            <dd class="col-7 mb-2">Bachelor - International Business</dd>
            <dt class="col-5">Interests</dt>
            <dd class="col-7 mb-2">Marketing, Case Studies</dd>
            <dt class="col-5">Availability</dt>
            <dd class="col-7 mb-2">Mon-Wed 10:00–13:00</dd>
          </dl>
          <div class="mt-4 text-center">
            <button class="btn btn-primary me-2">Edit Profile</button>
            <button class="btn btn-outline-secondary">Log Out</button>
          </div>
        </div>
      </div>
    </section>
  `
})
export class ProfileComponent {}
