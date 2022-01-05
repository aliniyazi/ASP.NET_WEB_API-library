import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from '../validators/custom-validators';
import { FormControl } from '@angular/forms';
import { TooltipPosition } from '@angular/material/tooltip';
import { HttpClient } from "@angular/common/http"
import { countries } from '../shared/country-data-store';
import { RegisterServicesService } from '../register-services/register-services.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
// import { RegisterModel } from '../models/RegisterModel';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
})

export class RegistrationComponent implements OnInit {
  public frmSignup: FormGroup;

  public countries: any = countries;
  // user: RegisterModel | undefined;
  address: object | undefined;
  public showErrors:boolean = false;

  constructor(private registerService: RegisterServicesService,
              private fb: FormBuilder,
              private router: Router, 
              private http: HttpClient,
              private toastr: ToastrService) {
    this.frmSignup = this.createSignupForm();
  }

  ngOnInit(): void {

    this.createSignupForm();
  }
  createSignupForm(): FormGroup {
    return this.fb.group(
      {
        email: [
          null,
          Validators.compose([
            Validators.required,
            CustomValidators.patternValidator(/[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/, {
              IsValid: true
            })
          ])
        ],
        password: [
          null,
          Validators.compose([
            Validators.required,
            // check whether the entered password has a number
            CustomValidators.patternValidator(/\d/, {
              hasNumber: true
            }),
            // check whether the entered password has upper case letter
            CustomValidators.patternValidator(/[A-ZА-Я]/, {
              hasCapitalCase: true
            }),
            // check whether the entered password has a lower case letter
            CustomValidators.patternValidator(/[a-zа-я]/, {
              hasSmallCase: true
            }),
            // check whether the entered password has a special character
            CustomValidators.patternValidator(
              /[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/,
              {
                hasSpecialCharacters: true
              }
            ),
            Validators.minLength(10),
            Validators.maxLength(45)
          ])
        ],
        firstname: [
          null,
          Validators.compose([
            Validators.required,
            CustomValidators.patternValidator(/^.{1,50}$/, {
              IsValid: true
            })
          ])
        ],
        lastname: [
          null,
          Validators.compose([
            Validators.required,
            CustomValidators.patternValidator(/^.{1,50}$/, {
              IsValid: true
            })
          ])
        ],
        phonenumber: [
          null,
          Validators.compose([
            Validators.required,
            CustomValidators.patternValidator(/[0-9.|+-]{3,}/, {
              IsValid: true
            })
          ])

        ],
        country: [
          null,
          Validators.compose([
            Validators.required,
            CustomValidators.patternValidator(/[a-zA-Z-]+/, {
              IsValid: true
            }),
            Validators.minLength(4)
          ])
        ],
        city: [
          null,
          Validators.compose([
            Validators.required,
            CustomValidators.patternValidator(/[a-zA-Z-]+/, {
              IsValid: true
            }),
            Validators.minLength(2)
          ])
        ],
        street: [
          null,
          Validators.compose([
            Validators.required,
            CustomValidators.patternValidator(/[a-zA-Z0-9-]+/, {
              IsValid: true
            }),
            Validators.minLength(1)
          ])
        ],
        streetnumber: [
          null,
          Validators.compose([Validators.required, Validators.maxLength(10)])
        ],
        buildingnumber: [
          null,
          Validators.compose([Validators.maxLength(20)])
        ],
        apartmentnumber: [
          null,
          Validators.compose([Validators.maxLength(20)])
        ],
        aditionalinfo: [
          null,
          Validators.compose([Validators.maxLength(200)])
        ],
        confirmPassword: [null, Validators.compose([Validators.required])]
      },
      {
        // check whether our password and confirm password match
        validator: CustomValidators.passwordMatchValidator
      }
    );
  }

  submit() {
    if (this.frmSignup.invalid) {
    } else {
      const val = this.frmSignup.value;
      this.address = {
        country: val.country,
        city: val.city,
        streetname: val.street,
        streetnumber: val.streetnumber,
        buildingnumber: val.buildingnumber,
        appartmentnumber: val.apartmentnumber,
        additionalinfo: val.aditionalinfo
      };
      this.registerService.register(val.firstname, val.lastname, val.email, val.password, val.confirmPassword, val.phonenumber, this.address).subscribe(result => {
        // redirect to home page
        this.router.navigateByUrl('/');
        this.toastr.success('Successful registration.');
      },
      (err)=>this.toastr.warning('The email is already in use.'));
    }
  }
  focusFunction(){
    this.showErrors = true;
  }
  focusOutFunction(){
    this.showErrors = false;
  }

}
