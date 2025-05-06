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
import { TestModuleComponent } from './test-module/test-module.component';

@NgModule({
  declarations: [
    AppComponent,
    TestModuleComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HomeComponent,
    LoginComponent,
    ProfileComponent,
    RegisterComponent,
    MessagesComponent,
    SearchComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
