import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ToastrService } from 'ngx-toastr';
import { AuthServiceService } from '../auth-services/auth-service.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
    resetPassword!: FormData;

  constructor(private authService:AuthServiceService,private toastr:ToastrService,private router: Router,private jwtHelperService: JwtHelperService) { }

  ngOnInit(): void {
    const decodedToken = this.jwtHelperService.decodeToken();
    localStorage.getItem('token')
  }
  onSubmit(f: NgForm) {
    if(localStorage.getItem('token')!==null){
      const decodedTokenEmail = this.jwtHelperService.decodeToken(localStorage.getItem('token') || '').email;
      this.resetPassword = new FormData();
      this.resetPassword.append('email',decodedTokenEmail);
      this.resetPassword.append('password',f.value.password);
      this.authService.resetPassword(this.resetPassword).subscribe(res=>{
        this.toastr.info('Your password has been successfully changed');
        this.router.navigateByUrl('/login');
      });
    }
    else{
      this.toastr.error('you are not logged in!');
    }
  }

}
