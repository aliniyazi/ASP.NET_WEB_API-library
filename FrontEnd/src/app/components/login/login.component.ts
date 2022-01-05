import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthServiceService } from '../../auth-services/auth-service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  formGroup!: FormGroup;


  constructor(private authService: AuthServiceService,
              private router: Router,
              private fb: FormBuilder,
              private route: ActivatedRoute,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.initForm();

  }

  initForm() {
    this.formGroup = new FormGroup({
      email: new FormControl('', [Validators.pattern(/[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/)]),
      password: new FormControl('', [Validators.required])
    })
  }

  LoginProces() {

    if (this.formGroup.invalid) {
      this.toastr.warning('Invalid Email or Password');
      return;
    } else {
      const val = this.formGroup.value;
      this.authService.login(val)
        .pipe(first())
        .subscribe(result => {
          // redirect to home page
          //document.getElementById("logout")!.style.visibility='visible';
          this.router.navigateByUrl('/admin');
        },(err)=>this.toastr.error('Wrong Email or Password'));
    }
  }


}
