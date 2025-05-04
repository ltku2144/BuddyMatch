import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  template: `
  <section class="container py-5">
  <div class="row align-items-center mb-5">
    <div class="col-md-6 text-center text-md-start mb-4 mb-md-0">
      <h1 class="display-4 fw-bold mb-2">Welcome to Study Buddies!</h1>
      <p class="lead mb-4">Connect, collaborate, and succeed together. Study Buddies helps you find partners, join groups, and stay motivated on your academic journey.</p>
    </div>
    <div class="col-md-6 text-center">
      <img src="assets/logo.png" alt="Study Group" class="img-fluid rounded shadow" style="max-width: 320px;">
    </div>
  </div>

  <div class="row g-4">
    <div class="col-md-4">
      <div class="card h-100 border-0 shadow-sm">
        <div class="card-body text-center">
          <i class="bi bi-people-fill display-5 text-primary mb-3"></i>
          <h5 class="card-title fw-bold">Find Study Partners</h5>
          <p class="card-text">Connect with students who share your courses, interests, or goals. Build your perfect study group!</p>
        </div>
      </div>
    </div>
    <div class="col-md-4">
      <div class="card h-100 border-0 shadow-sm">
        <div class="card-body text-center">
          <i class="bi bi-chat-dots-fill display-5 text-success mb-3"></i>
          <h5 class="card-title fw-bold">Chat & Collaborate</h5>
          <p class="card-text">Message your buddies, share resources, and keep each other motivated with real-time chat and notifications.</p>
        </div>
      </div>
    </div>
    <div class="col-md-4">
      <div class="card h-100 border-0 shadow-sm">
        <div class="card-body text-center">
          <i class="bi bi-person-badge-fill display-5 text-warning mb-3"></i>
          <h5 class="card-title fw-bold">Personalize Your Profile</h5>
          <p class="card-text">Showcase your strengths, set your study goals, and let others know what you’re looking for in a study buddy.</p>
        </div>
      </div>
    </div>
  </div>
</section>
  `
})
export class HomeComponent {}
