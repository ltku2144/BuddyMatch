import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'study-buddies';
  showTestModule = true;

  toggleTestModule() {
    this.showTestModule = !this.showTestModule;
  }
  
  constructor(public authService: AuthService) {}
  
  logout(event: Event): void {
    event.preventDefault();
    this.authService.logout();
  }
}
