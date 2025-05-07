import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <section class="container py-5" style="max-width: 600px;">
      <div class="card border-0">
        <div class="card-body p-4">
          <h2 class="mb-4 fw-bold text-center">Messages</h2>

          <!-- Messages List -->
          <div class="mb-4">
            <h5>Conversation</h5>
            <ul class="list-group">
              <li class="list-group-item" *ngFor="let message of messages">
                <strong>{{ message.sender }}:</strong> {{ message.text }}
              </li>
            </ul>
          </div>

          <!-- Message Input -->
          <form (ngSubmit)="sendMessage()" class="d-flex">
            <input
              type="text"
              class="form-control me-2"
              [(ngModel)]="newMessage"
              name="message"
              placeholder="Type your message..."
              required
            />
            <button type="submit" class="btn btn-primary">Send</button>
          </form>
        </div>
      </div>

      <!-- Disclaimer -->
      <div class="text-center mt-4">
        <p class="lead mb-0">Messaging functionality is not in the scope of this project.<br>
        This page is a placeholder.</p>
      </div>
    </section>
  `
})
export class MessagesComponent {
  messages = [
    { sender: 'Alice', text: 'Hi there!' },
    { sender: 'Bob', text: 'Hello! How are you?' }
  ];
  newMessage: string = '';

  sendMessage() {
    if (this.newMessage.trim()) {
      this.messages.push({ sender: 'You', text: this.newMessage });
      this.newMessage = '';
    }
  }
}
