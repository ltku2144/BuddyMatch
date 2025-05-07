import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService, User } from './user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  providers: [UserService],
  template: `
    <section class="container py-5" style="max-width: 600px;">
      <div class="card border-0">
        <div class="card-body p-4">
          <h2 class="mb-4 text-center fw-bold">My Profile</h2>
          <form (ngSubmit)="updateProfile()" *ngIf="user">
            <div class="mb-3">
              <label for="name" class="form-label">Full Name</label>
              <input
                type="text"
                id="name"
                class="form-control"
                [(ngModel)]="user.name"
                name="name"
                required
              />
            </div>
            <div class="mb-3">
              <label for="email" class="form-label">Email</label>
              <input
                type="email"
                id="email"
                class="form-control"
                [(ngModel)]="user.email"
                name="email"
                required
              />
            </div>
            <div class="mb-3">
              <label for="program" class="form-label">Program</label>
              <input
                type="text"
                id="program"
                class="form-control"
                [(ngModel)]="user.program"
                name="program"
              />
            </div>
            <div class="mb-3">
              <label for="interests" class="form-label">Interests</label>
              <input
                type="text"
                id="interests"
                class="form-control"
                [(ngModel)]="user.interests"
                name="interests"
              />
            </div>
            <div class="mb-3">
              <label for="availability" class="form-label">Availability</label>
              <input
                type="text"
                id="availability"
                class="form-control"
                [(ngModel)]="user.availability"
                name="availability"
              />
            </div>
            <div class="mt-4 text-center">
              <button type="submit" class="btn btn-primary me-2">Save Changes</button>
              <button type="button" class="btn btn-outline-secondary" (click)="logout()">Log Out</button>
            </div>
          </form>
        </div>
      </div>
    </section>
  `
})
export class ProfileComponent implements OnInit {
  user: User | null = null;

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {
    try {
      const userId = this.getLoggedInUserId();
      this.userService.getProfile(userId).subscribe({
        next: (data: User) => {
          this.user = data;
        },
        error: (err: any) => {
          console.error('Failed to fetch user profile:', err);
        }
      });
    } catch (error) {
      console.error(error);
      this.router.navigate(['/login']); // Redirect to login if user ID is not found
    }
  }

  updateProfile(): void {
    if (this.user && this.user.id !== undefined) {
      this.userService.updateProfile(this.user.id, this.user).subscribe({
        next: () => {
          alert('Profile updated successfully!');
        },
        error: (err: any) => {
          console.error('Failed to update profile:', err);
          alert('Failed to update profile. Please try again.');
        }
      });
    } else {
      console.error('User or User ID is undefined.');
      alert('Unable to update profile. Please try again.');
    }
  }

  logout(): void {
    localStorage.removeItem('token'); // Clear session
    localStorage.removeItem('userId'); // Clear user ID
    this.router.navigate(['/login']);
  }

  private getLoggedInUserId(): number {
    const userId = localStorage.getItem('userId');
    if (!userId) {
      throw new Error('User ID not found in local storage');
    }
    return parseInt(userId, 10);
  }
}


