import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
import { LoginComponent } from './login.component';
import { ProfileComponent } from './profile.component';
import { RegisterComponent } from './register.component';
import { MessagesComponent } from './messages.component';
import { SearchComponent } from './search.component';


const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'messages', component: MessagesComponent },
  { path: 'search', component: SearchComponent },
  { path: 'test-module', loadComponent: () => import('./test-module/test-module.component').then(m => m.TestModuleComponent) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
