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

  create(user: Omit<User, 'id' | 'createdAt'>): Observable<void> {
    return this.http.post<void>(this.apiUrl, user);
  }

  // Fetch user profile by ID
  getProfile(userId: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/profile/${userId}`);
  }

  // Update user profile
  updateProfile(userId: number, user: User): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/profile/${userId}`, user);
  }

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  getById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  getMatches(id: number): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/match/${id}`);
  }

  update(id: number, user: Partial<User>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, user);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
