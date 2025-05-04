import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="container py-5">
    <div class="container mt-5">
  <h2>User Management</h2>

  <form class="mt-4">
    <div class="mb-3">
      <label for="name">Name</label>
      <input id="name" class="form-control" type="text" placeholder="Enter name" />
    </div>

    <div class="mb-3">
      <label for="email">Email</label>
      <input id="email" class="form-control" type="email" placeholder="Enter email" />
    </div>

    <div class="mb-3">
      <label for="program">Program</label>
      <input id="program" class="form-control" type="text" placeholder="Enter program" />
    </div>

    <div class="mb-3">
      <label for="interests">Interests</label>
      <input id="interests" class="form-control" type="text" placeholder="Enter interests" />
    </div>

    <div class="mb-3">
      <label for="availability">Availability</label>
      <input id="availability" class="form-control" type="text" placeholder="Enter availability" />
    </div>

    <button class="btn btn-primary" type="submit">Create User</button>
  </form>

  <hr class="my-4" />

  <h4>Existing Users</h4>
  <table class="table table-striped">
    <thead>
      <tr>
        <th>Name</th>
        <th>Email</th>
        <th>Program</th>
        <th>Interests</th>
        <th>Availability</th>
      </tr>
    </thead>
    <tbody>
      <!-- Sample row -->
      <tr>
        <td>Anna Jensen</td>
        <td>anna.j&#64;cbs.dk</td>
        <td>International Business</td>
        <td>Marketing, Case Studies</td>
        <td>Mon-Wed 10:00–13:00</td>
      </tr>
    </tbody>
  </table>
</div>
    </section>
  `
})
export class AdminComponent {}
