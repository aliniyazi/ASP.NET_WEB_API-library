import { Component, OnInit } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthServiceService } from '../auth-services/auth-service.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
    form!: FormGroup;
    loading = false;
    submitted = false;
    forgotPasswordEmail!: FormData;

  constructor(private authService:AuthServiceService,private toastr:ToastrService,private router: Router) { }

  ngOnInit(): void {
  }
  onSubmit(f: NgForm) {
    this.forgotPasswordEmail = new FormData();
    this.forgotPasswordEmail.append('email',f.value.email);
    this.authService.forgotPassword(this.forgotPasswordEmail).subscribe(res=>{
      this.toastr.info('Please check your email to see your temporary password!');
      this.router.navigateByUrl('/login');
    },(err)=>this.toastr.error('Invalid Email !'));
  }
}
