import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService, User } from './user.service'; // Import User from UserService

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <section class="container py-5">
      <h2>Find Study Buddies</h2>
      <form class="mb-4" (ngSubmit)="findMatches()">
        <div class="mb-3">
          <label for="program">Program</label>
          <input id="program" class="form-control" type="text" [(ngModel)]="searchCriteria.program" name="program" placeholder="Enter your program" />
        </div>
        <div class="mb-3">
          <label for="interests">Interests</label>
          <input id="interests" class="form-control" type="text" [(ngModel)]="searchCriteria.interests" name="interests" placeholder="Enter your interests" />
        </div>
        <div class="mb-3">
          <label for="availability">Availability</label>
          <input id="availability" class="form-control" type="text" [(ngModel)]="searchCriteria.availability" name="availability" placeholder="Enter your availability" />
        </div>
        <button class="btn btn-primary me-2" type="submit">Find Matches</button>
        <button class="btn btn-secondary" type="button" (click)="clearInputs()">Clear</button>
      </form>

      <div *ngIf="matches.length > 0">
        <h4>Matched Students</h4>
        <ul class="list-group">
          <li class="list-group-item" *ngFor="let user of matches">
            <strong>{{ user.name }}</strong> - {{ user.program }}<br>
            Interests: {{ user.interests }}<br>
            Availability: {{ user.availability }}
          </li>
        </ul>
      </div>
      <div *ngIf="matches.length === 0 && searched">
        <p>No matches found.</p>
      </div>
    </section>
  `
})
export class SearchComponent implements OnInit {
  users: User[] = [];
  searchCriteria: Partial<User> = {
    program: '',
    interests: '',
    availability: ''
  };
  matches: User[] = [];
  searched = false;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    // Fetch all users from the API on component initialization
    this.userService.getAll().subscribe({
      next: (data: User[]) => {
        this.users = data;
      },
      error: (err) => {
        console.error('Failed to fetch users:', err);
      }
    });
  }

  findMatches() {
    this.matches = [];

    const hasInput =
      (this.searchCriteria.program && this.searchCriteria.program.trim() !== '') ||
      (this.searchCriteria.interests && this.searchCriteria.interests.trim() !== '') ||
      (this.searchCriteria.availability && this.searchCriteria.availability.trim() !== '');

    if (!hasInput) {
      this.searched = true;
      return;
    }

    this.matches = this.users.filter(user => {
      const matchesProgram =
        this.searchCriteria.program &&
        user.program.toLowerCase().includes(this.searchCriteria.program!.toLowerCase());
      const matchesInterests =
        this.searchCriteria.interests &&
        user.interests.toLowerCase().includes(this.searchCriteria.interests!.toLowerCase());
      const matchesAvailability =
        this.searchCriteria.availability &&
        user.availability.toLowerCase().includes(this.searchCriteria.availability!.toLowerCase());

      return (
        (this.searchCriteria.program && matchesProgram) ||
        (this.searchCriteria.interests && matchesInterests) ||
        (this.searchCriteria.availability && matchesAvailability)
      );
    });
    this.searched = true;
  }

  clearInputs() {
    this.searchCriteria = {
      program: '',
      interests: '',
      availability: ''
    };
    this.matches = [];
    this.searched = false;
  }
}