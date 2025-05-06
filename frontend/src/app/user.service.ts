import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// Define the user interface
export interface User {
  id?: number;
  name: string;
  email: string;
  program: string;
  interests: string;
  availability: string;
  password?: string;
  createdAt?: string;
}

@Injectable({ providedIn: 'root' })
export class UserService {
  private apiUrl = 'http://localhost:5072/api/user';

  constructor(private http: HttpClient) {}

  // ✅ Create new user
  create(user: Omit<User, 'id' | 'createdAt'>): Observable<void> {
    return this.http.post<void>(this.apiUrl, user);
  }

  // ✅ Get all users
  getAll(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  // ✅ Get one user by ID
  getById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  // ✅ Find matching users
  getMatches(id: number): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/match/${id}`);
  }

  // ✅ Update user
  update(id: number, user: Partial<User>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, user);
  }

  // ✅ Delete user
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
