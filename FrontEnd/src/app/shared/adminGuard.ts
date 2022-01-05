import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { AuthServiceService } from '../auth-services/auth-service.service';
import { Role } from "../models/role";

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private authService: AuthServiceService, private router: Router) { }
  
  canActivate() {
    
    const user = this.authService.userValue;
      if (!this.authService.isLoggedIn()) {
        this.router.navigate(['login'])
        return false;
      }
      if (this.authService.checkTokenIsExpired(user.token)){
        this.authService.logout();
        this.router.navigate([''])
        return false;
      }
      if(!user.role[0].includes(Role.Admin)){
        this.router.navigate(['librarian']);
      }
    return true;
  }
}