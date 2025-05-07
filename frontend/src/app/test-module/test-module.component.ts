import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { UserService, User } from '../user.service';

@Component({
  selector: 'app-test-module',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FormsModule],
  providers: [UserService],
  templateUrl: './test-module.component.html',
  styleUrls: ['./test-module.component.css']
})
export class TestModuleComponent implements OnInit {
  users: (User & { editing?: boolean })[] = [];
  message = '🔄 Loading users from PostgreSQL...';

  newUser: Partial<User> = {
    name: '',
    email: '',
    program: '',
    interests: '',
    availability: '',
    password: 'default123'
  };

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.getAll().subscribe({
      next: (data: User[]) => {
        this.users = data.map(u => ({ ...u, editing: false }));
        this.message = `✅ Loaded ${data.length} user(s) from PostgreSQL.`;
      },
      error: (err) => {
        console.error(err);
        this.message = '❌ Failed to load users. Check backend/API.';
      }
    });
  }

  toggleEdit(user: User & { editing?: boolean }) {
    user.editing = !user.editing;
  }

  save(user: User & { editing?: boolean }) {
    if (user.id) {
      this.userService.update(user.id, user).subscribe({
        next: () => {
          user.editing = false;
          this.message = `✅ Updated ${user.name}.`;
        },
        error: (err) => {
          console.error(err);
          this.message = `❌ Failed to update ${user.name}.`;
        }
      });
    }
  }

  create(): void {
    this.userService.create(this.newUser as User).subscribe({
      next: () => {
        this.message = `✅ Added ${this.newUser.name}`;
        this.newUser = {
          name: '',
          email: '',
          program: '',
          interests: '',
          availability: '',
          password: 'default123'
        };
        this.ngOnInit();
      },
      error: (err) => {
        console.error(err);
        this.message = '❌ Failed to add user.';
      }
    });
  }

  delete(user: User): void {
    if (user.id && confirm(`Are you sure you want to delete ${user.name}?`)) {
      this.userService.delete(user.id).subscribe({
        next: () => {
          this.message = `🗑️ Deleted ${user.name}`;
          this.ngOnInit();
        },
        error: (err) => {
          console.error(err);
          this.message = `❌ Failed to delete ${user.name}`;
        }
      });
    }
  }
}
