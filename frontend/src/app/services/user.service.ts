import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  id?: number; // Optional for creation, present for existing users
  name: string;
  email: string;
  passwordHash?: string; // To be used when sending to backend
  password?: string; // To be used in the form
  program?: string;
  interests?: string;
  availability?: string;
  role?: string; // Assuming role might be part of user data
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5072/api/user'; // Adjust if your API URL is different

  constructor(private http: HttpClient) { }

  create(user: User): Observable<User> {
    // Map the frontend 'password' field to 'passwordHash' for the backend
    const userForApi: any = { ...user };
    if (userForApi.password) {
      userForApi.passwordHash = userForApi.password;
      delete userForApi.password; // Remove the original password field
    }
    console.log('Sending to API:', userForApi); // Add this line
    return this.http.post<User>(this.apiUrl, userForApi);
  }

  // You can add other methods like getUsers, getUser, updateUser, deleteUser here
}
