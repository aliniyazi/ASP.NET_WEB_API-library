import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './registration/registration.component';
import { UserComponent } from './components/user/user.component';
import { BooksComponent } from './components/librarian/books/books.component';
import { RegisterGuard } from './shared/registeredGuard';
import { AuthGuard } from './shared/auth.guard';
import { UserGuard } from './shared/userGuard';
import { AdminGuard } from './shared/adminGuard';
import { Role } from './models/role';
import { AdminComponent } from './components/admin/admin.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { DashboardComponent } from './components/librarian/dashboard/dashboard.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'user',
    component: UserComponent,
    canActivate: [UserGuard],
    data: { role: Role.User },
  },
  { path: 'login', component: LoginComponent },
  {
    path: 'registration',
    component: RegistrationComponent,
    canActivate: [RegisterGuard],
  },
  {
    path: 'librarian',
    component: DashboardComponent,
    canActivate: [AuthGuard],
    data: { role: Role.Librarian },
  },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AdminGuard],
    data: { role: Role.Admin },
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent
  },
  {
    path: 'bookCrud',
    component: BooksComponent,
    canActivate: [AuthGuard],
    data: { role: Role.Librarian },
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
//to do add route!!! how to use angular router with metod router!!!
