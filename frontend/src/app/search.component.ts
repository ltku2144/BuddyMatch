import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="container py-5" style="max-width: 600px;">
      <div class="card border-0">
        <div class="card-body p-4">
          <h2 class="mb-4 text-center fw-bold">Find a Study Partner</h2>
          <form class="mb-4 d-flex flex-column flex-md-row gap-2 align-items-stretch justify-content-center">
            <select class="form-select" style="max-width: 180px;" required>
              <option value="program">Program</option>
              <option value="interests">Interests</option>
              <option value="availability">Availability</option>
            </select>
            <input type="text" class="form-control" placeholder="Type to search..." required style="min-width: 0;">
            <button type="submit" class="btn btn-primary">Search</button>
          </form>
          <div class="mt-4">
            <h5 class="fw-bold mb-3">Matching Results</h5>
            <div class="text-muted">(Search functionality and results will be implemented with backend integration.)</div>
            <!-- Placeholder for results -->
            <ul class="mt-3" style="list-style:none; padding:0;">
              <li class="mb-3">
                <div class="fw-bold">Anna Jensen</div>
                <div class="small">Program: Bachelor - International Business</div>
                <div class="small">Interests: Marketing, Case Studies</div>
                <div class="small">Availability: Mon-Wed 10:00–13:00</div>
              </li>
              <li class="mb-3">
                <div class="fw-bold">Markus Sørensen</div>
                <div class="small">Program: Master - Finance and Investments</div>
                <div class="small">Interests: Excel, Corporate Finance</div>
                <div class="small">Availability: Tue & Thu 14:00–17:00</div>
              </li>
              <!-- More static results can be added here -->
            </ul>
          </div>
        </div>
      </div>
    </section>
  `
})
export class SearchComponent {}
