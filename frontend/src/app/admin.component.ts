import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="container py-5">
      <div class="card border-0">
        <div class="card-body p-4 text-center">
          <h2 class="mb-4 fw-bold">Admin Page</h2>
          <p class="lead mb-0">This is a blank admin component.</p>
        </div>
      </div>
    </section>
  `
})
export class AdminComponent {}
