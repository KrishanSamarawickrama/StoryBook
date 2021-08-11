import { JwtInterceptorService } from './Security/jwt-interceptor.service';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { PostListComponent } from './Posts/post-list/post-list.component';
import { PostCreateComponent } from './Posts/post-create/post-create.component';
import { MenuComponent } from './menu/menu.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './Security/login/login.component';
import { RegisterComponent } from './Security/register/register.component';
import { AuthorizedViewComponent } from './Security/authorized-view/authorized-view.component';
import { UserListComponent } from './Security/user-list/user-list.component';
import { EditorListComponent } from './Editors/editor-list/editor-list.component';
import { PostStatComponent } from './Posts/post-stat/post-stat.component';


@NgModule({
  declarations: [
    AppComponent,
    PostListComponent,
    PostCreateComponent,
    MenuComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    AuthorizedViewComponent,
    UserListComponent,
    EditorListComponent,
    PostStatComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    SweetAlert2Module.forRoot()
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: JwtInterceptorService,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
