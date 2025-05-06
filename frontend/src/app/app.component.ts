import { Component } from '@angular/core';

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
  
}
