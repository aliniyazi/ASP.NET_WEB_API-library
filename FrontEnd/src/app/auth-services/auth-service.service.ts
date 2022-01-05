import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { catchError, map, retry } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginModel, LoginResponse } from '../models/LoginModel';
import { Router } from '@angular/router';
import { User } from '../models/user';
import { ControlContainer } from '@angular/forms';

@Injectable({ providedIn: 'root' })
export class AuthServiceService {
  userData = new BehaviorSubject<User>(new User());
  user = this.userData.asObservable();
  token: any;
  userEmail!: string;

  constructor(
    private router: Router,
    private http: HttpClient,
    private jwtHelperService: JwtHelperService
  ) {}

  public get userValue(): User {
    return this.userData.value;
  }

  login(model: LoginModel) {
    return this.http
      .post<LoginResponse>(environment.baseUrl + '/user/login', model)
      .pipe(
        map((response) => {
          localStorage.setItem('token', response.token);
          this.setUserDetails();
          // document.getElementById("resetPassword")!.style.visibility="visible";
        })
      );
  }

  logoutBackend(){
    return this.http
      .post(environment.baseUrl + '/user/logout',{})
      .subscribe((x)=>{
      });
  }

  setUserDetails() {
      const currentToken = localStorage.getItem('token') || '';
      const userDetail = new User();
      const decodedToken = this.jwtHelperService.decodeToken(currentToken);
      userDetail.role.push(decodedToken.role);
      userDetail.token = currentToken;
      this.userData.next(userDetail);
      this.userEmail = decodedToken.email;
  }

  logout() {
    localStorage.removeItem('token');
    this.userData.next(new User());
    this.logoutBackend();
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }

  checkTokenIsExpired(token: string) {
    return this.jwtHelperService.isTokenExpired(token);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  handleError(error: any) {
    let errorMessage = '';
    var element = document.getElementById('warning');
    element!.style.display = 'block';
    return throwError(errorMessage);
  }
  forgotPassword(formData: FormData){
    return this.http.post<string>(environment.baseUrl + '/user/forgotPassword',formData);
  }
  resetPassword(formData: FormData){
    return this.http.post<string>(environment.baseUrl + '/user/resetPassword',formData);
  }
}
