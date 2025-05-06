import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { UserService, User } from '../user.service';

@Component({
  selector: 'app-test-module',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  providers: [UserService],
  templateUrl: './test-module.component.html',
  styleUrls: ['./test-module.component.css']
})
export class TestModuleComponent implements OnInit {
  users: User[] = [];
  message = '🔄 Loading users from PostgreSQL...';

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.getAll().subscribe({
      next: (data: User[]) => {
        this.users = data;
        this.message = `✅ Loaded ${data.length} user(s) from PostgreSQL.`;
      },
      error: (err) => {
        console.error(err);
        this.message = '❌ Failed to load users. Check backend/API.';
      }
    });
  }
}
