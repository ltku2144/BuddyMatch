import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  name: string;
  email: string;
  program: string;
  interests: string;
  availability: string;
  password?: string;
  // Add other fields as needed
}

@Injectable({ providedIn: 'root' })
export class UserService {
  private apiUrl = 'https://your-api-url/api/users'; // Replace with your actual API URL

  constructor(private http: HttpClient) {}

  // Register a new user
  register(user: User): Observable<any> {
    return this.http.post(this.apiUrl, user);
  }

  // Get all users
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  // Get a user by ID
  getUserById(id: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  // Update a user
  updateUser(id: string, user: User): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, user);
  }

  // Delete a user
  deleteUser(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}