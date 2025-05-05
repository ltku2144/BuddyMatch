import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

interface User {
  name: string;
  email: string;
  program: string;
  interests: string;
  availability: string;
}

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  template: `
    <section class="container py-5">
      <div class="container mt-5">
        <h2>User Management</h2>

        <form class="mt-4" (ngSubmit)="createUser()">
          <div class="mb-3">
            <label for="name">Name</label>
            <input id="name" class="form-control" type="text" placeholder="Enter name" [(ngModel)]="newUser.name" name="name" />
          </div>

          <div class="mb-3">
            <label for="email">Email</label>
            <input id="email" class="form-control" type="email" placeholder="Enter email" [(ngModel)]="newUser.email" name="email" />
          </div>

          <div class="mb-3">
            <label for="program">Program</label>
            <input id="program" class="form-control" type="text" placeholder="Enter program" [(ngModel)]="newUser.program" name="program" />
          </div>

          <div class="mb-3">
            <label for="interests">Interests</label>
            <input id="interests" class="form-control" type="text" placeholder="Enter interests" [(ngModel)]="newUser.interests" name="interests" />
          </div>

          <div class="mb-3">
            <label for="availability">Availability</label>
            <input id="availability" class="form-control" type="text" placeholder="Enter availability" [(ngModel)]="newUser.availability" name="availability" />
          </div>

          <button class="btn btn-primary" type="submit">Create User</button>
        </form>

        <hr class="my-4" />

        <h4>Existing Users</h4>
        <table class="table table-striped">
          <thead>
            <tr>
              <th>Name</th>
              <th>Email</th>
              <th>Program</th>
              <th>Interests</th>
              <th>Availability</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let user of users; let i = index">
              <td *ngIf="editIndex !== i">{{ user.name }}</td>
              <td *ngIf="editIndex === i"><input [(ngModel)]="editUser.name" name="editName{{i}}" class="form-control" /></td>
              
              <td *ngIf="editIndex !== i">{{ user.email }}</td>
              <td *ngIf="editIndex === i"><input [(ngModel)]="editUser.email" name="editEmail{{i}}" class="form-control" /></td>
              
              <td *ngIf="editIndex !== i">{{ user.program }}</td>
              <td *ngIf="editIndex === i"><input [(ngModel)]="editUser.program" name="editProgram{{i}}" class="form-control" /></td>
              
              <td *ngIf="editIndex !== i">{{ user.interests }}</td>
              <td *ngIf="editIndex === i"><input [(ngModel)]="editUser.interests" name="editInterests{{i}}" class="form-control" /></td>
              
              <td *ngIf="editIndex !== i">{{ user.availability }}</td>
              <td *ngIf="editIndex === i"><input [(ngModel)]="editUser.availability" name="editAvailability{{i}}" class="form-control" /></td>
              
              <td>
                <button *ngIf="editIndex !== i" class="btn btn-warning btn-sm me-2" (click)="startEdit(i)">Modify</button>
                <button *ngIf="editIndex === i" class="btn btn-success btn-sm me-2" (click)="saveEdit(i)">Save</button>
                <button *ngIf="editIndex === i" class="btn btn-secondary btn-sm me-2" (click)="cancelEdit()">Cancel</button>
                <button class="btn btn-danger btn-sm" (click)="deleteUser(i)">Delete</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </section>
  `
})
export class AdminComponent {
  users: User[] = [
    {
      name: 'Anna Jensen',
      email: 'anna.j@cbs.dk',
      program: 'International Business',
      interests: 'Marketing, Case Studies',
      availability: 'Mon-Wed 10:00–13:00'
    }
  ];

  newUser: User = {
    name: '',
    email: '',
    program: '',
    interests: '',
    availability: ''
  };

  editIndex: number | null = null;
  editUser: User = { name: '', email: '', program: '', interests: '', availability: '' };

  createUser() {
    if (
      this.newUser.name &&
      this.newUser.email &&
      this.newUser.program &&
      this.newUser.interests &&
      this.newUser.availability
    ) {
      this.users.push({ ...this.newUser });
      this.newUser = { name: '', email: '', program: '', interests: '', availability: '' };
    }
  }

  startEdit(index: number) {
    this.editIndex = index;
    this.editUser = { ...this.users[index] };
  }

  saveEdit(index: number) {
    this.users[index] = { ...this.editUser };
    this.editIndex = null;
    this.editUser = { name: '', email: '', program: '', interests: '', availability: '' };
  }

  cancelEdit() {
    this.editIndex = null;
    this.editUser = { name: '', email: '', program: '', interests: '', availability: '' };
  }

  deleteUser(index: number) {
    this.users.splice(index, 1);
    if (this.editIndex === index) {
      this.cancelEdit();
    }
  }
}