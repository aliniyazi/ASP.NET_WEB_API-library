import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { JwPaginationModule } from 'jw-angular-pagination';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './components/login/login.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TokenInterceptorService } from './interceptors/token-interceptor.service';
import { AuthServiceService } from './auth-services/auth-service.service';
import { HomeComponent } from './components/home/home.component';
import { RegistrationComponent } from './registration/registration.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { AuthGuard } from './shared/auth.guard';
import { UserGuard } from './shared/userGuard';

import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { JwtModule } from '@auth0/angular-jwt';
import { LibrarianComponent } from './components/librarian/librarian.component';
import { UserComponent } from './components/user/user.component';

import { BookListComponent } from './components/librarian/books/book-list/book-list.component';
import { BookComponent } from './components/librarian/books/book/book.component';
import { BooksComponent } from './components/librarian/books/books.component';
import { ToastrModule } from 'ngx-toastr';
import { AdminComponent } from './components/admin/admin.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { DashboardComponent } from './components/librarian/dashboard/dashboard.component';
import { UsersComponent } from './components/librarian/dashboard/users/users.component';
import { RequestsComponent } from './components/librarian/dashboard/requests/requests.component';
import { SidebarComponent } from './components/librarian/dashboard/sidebar/sidebar.component';

export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RegistrationComponent,
    HeaderComponent,
    FooterComponent,
    LibrarianComponent,
    UserComponent,
    BooksComponent,
    BookComponent,
    BookListComponent,
    AdminComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    DashboardComponent,
    UsersComponent,
    RequestsComponent,
    SidebarComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatToolbarModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatTooltipModule,
    MatSelectModule,
    MatDialogModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
      },
    }),
    ToastrModule.forRoot()
  ],
  exports: [MatDialogModule, JwPaginationModule],
  providers: [
    AuthGuard,
    UserGuard,
    AuthServiceService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorService,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
