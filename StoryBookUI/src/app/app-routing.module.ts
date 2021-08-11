import { UserLoggedInGuard } from './Security/user-loged-in.guard';
import { RegisterComponent } from './Security/register/register.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './Security/login/login.component';
import { UserListComponent } from './Security/user-list/user-list.component';
import { IsModeratorGuard } from './Security/is-modarator.guard';
import { EditorListComponent } from './Editors/editor-list/editor-list.component';
import { PostStatComponent } from './Posts/post-stat/post-stat.component';

const routes: Routes = [

  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: '', component: HomeComponent, canActivate: [UserLoggedInGuard] },
  { path: 'writers', component: EditorListComponent, canActivate: [UserLoggedInGuard] },
  { path: 'users', component: UserListComponent, canActivate: [UserLoggedInGuard,IsModeratorGuard] },
  { path: 'stat', component: PostStatComponent, canActivate: [UserLoggedInGuard,IsModeratorGuard] },
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
