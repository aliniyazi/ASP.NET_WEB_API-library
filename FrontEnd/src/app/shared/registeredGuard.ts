import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthServiceService } from '../auth-services/auth-service.service';
import { RegisterServicesService } from '../register-services/register-services.service';

@Injectable({
  providedIn: 'root'
})
export class RegisterGuard implements CanActivate {

  constructor(private registerService: RegisterServicesService, private router: Router) { }
  canActivate() {
    if (this.registerService.registered()) {
      this.router.navigate(['/'])
      return false;
    }
    return true;
  }
}