import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from './user.service'; // Adjust path if needed

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  user = {
    name: '',
    email: '',
    program: '',
    interests: '',
    availability: '',
    password: '',
    confirmPassword: ''
  };

  constructor(private userService: UserService, private router: Router) {}

  onSubmit(form: NgForm) {
    if (form.valid && this.user.password === this.user.confirmPassword) {
      const { confirmPassword, ...payload } = this.user;

      this.userService.create(payload).subscribe({
        next: () => {
          alert('🎉 Registration successful!');
          this.router.navigate(['/login']);
        },
        error: (err) => {
          console.error('❌ Registration failed:', err);
          alert('Something went wrong. Please try again.');
        }
      });
    }
  }
}
