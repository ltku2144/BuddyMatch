import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home.component';
import { LoginComponent } from './login.component';
import { ProfileComponent } from './profile.component';
import { RegisterComponent } from './register.component';
import { MessagesComponent } from './messages.component';
import { SearchComponent } from './search.component';

@NgModule({
  declarations: [
    AppComponent
    // 🚫 REMOVE TestModuleComponent from here
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HomeComponent,
    LoginComponent,
    ProfileComponent,
    MessagesComponent,
    SearchComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
