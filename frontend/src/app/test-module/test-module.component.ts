import { Component } from '@angular/core';

@Component({
  selector: 'app-test-module',
  standalone: false,
  templateUrl: './test-module.component.html',
  styleUrl: './test-module.component.css'
})
export class TestModuleComponent {
  message = 'Test Module is working!';
}
