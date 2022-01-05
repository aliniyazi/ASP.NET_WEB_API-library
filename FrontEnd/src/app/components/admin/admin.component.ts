import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from "@angular/common/http"
import { Router } from '@angular/router';
import { CustomValidators } from 'src/app/validators/custom-validators';
import { countries } from '../../shared/country-data-store';
import { RegisterServicesService } from 'src/app/register-services/register-services.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})

export class AdminComponent implements OnInit {
  public frmSignup: FormGroup;

  public countries: any = countries;
  address: object | undefined;

  constructor(private registerService:RegisterServicesService ,private fb: FormBuilder, private router: Router, private http: HttpClient,private toastr:ToastrService) {
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
            CustomValidators.patternValidator(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/, {
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
            CustomValidators.patternValidator(/^[A-Z][a-z]+/, {
              IsValid: true
            }),
            Validators.minLength(4)
          ])
        ],
        city: [
          null,
          Validators.compose([
            Validators.required,
            CustomValidators.patternValidator(/^[A-Z][a-z]+/, {
              IsValid: true
            }),
            Validators.minLength(4)
          ])
        ],
        street: [
          null,
          Validators.compose([
            Validators.required,
            CustomValidators.patternValidator(/^[A-Z][a-z]+/, {
              IsValid: true
            }),
            Validators.minLength(4)
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
      this.registerService.registerLibrarian(val.firstname, val.lastname, val.email, val.password, val.confirmPassword, val.phonenumber, this.address)
      .subscribe((result) => {
          // redirect to home page
        this.toastr.success('Librarian created succesfully');
        this.router.navigate(['']);
        }, (error) =>{
        }
      );
    }
  }

}
