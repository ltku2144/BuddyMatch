import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { UserService } from './user.service';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HttpClientModule],
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
