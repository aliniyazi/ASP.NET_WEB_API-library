import { Component } from '@angular/core';
import { AuthServiceService } from './auth-services/auth-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'The curious readers';
  
  constructor(private authService: AuthServiceService) {
   if (localStorage.getItem('token')) {
     this.authService.setUserDetails();
   }
  }
}
