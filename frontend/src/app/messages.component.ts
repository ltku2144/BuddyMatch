import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="container py-5" style="max-width: 600px;">
      <div class="card border-0">
        <div class="card-body p-4 text-center">
          <h2 class="mb-4 fw-bold">Messages</h2>
          <p class="lead mb-0">Messaging functionality is not in the scope of this project.<br>
          This page is a placeholder.</p>
        </div>
      </div>
    </section>
  `
})
export class MessagesComponent {}
