import { HttpClient } from '@angular/common/http';
import { DefaultIterableDiffer, Injectable } from '@angular/core';
import { Observable, throwError} from 'rxjs';
import { catchError, map, retry } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { JwtHelperService } from "@auth0/angular-jwt";
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class RegisterServicesService {
  token: any;
  frmSignup: any;

  constructor(private http: HttpClient,private jwtHelperService: JwtHelperService,private toastr:ToastrService) { }



  getToken(){
    return localStorage.getItem('token');
  }
  register(firstname:string,lastname:string,email:string,password:string,confirmPassword:string,phonenumber:string,address:any): Observable<any> {
    return this.http.post<string>(environment.baseUrl+'/User/register',{firstname,lastname,email,password,confirmPassword,phonenumber,address}).
      pipe(retry(1),catchError(this.handleError), map(res => {
        if(res){

          this.token = res;
          localStorage.setItem('token', this.token.token);
        }


      }));
  }

  registerLibrarian(firstname:string,lastname:string,email:string,password:string,confirmPassword:string,phonenumber:string,address:any): Observable<any> {
    return this.http.post<string>(environment.baseUrl+'/User/register/librarian',{firstname,lastname,email,password,confirmPassword,phonenumber,address});
  }

  registered(){
    let token=this.token || this.getToken();
    return !this.jwtHelperService.isTokenExpired(token);
  }
  handleError(error:any) {
    let errorMessage = '';
    var element = document.getElementById('warning');
    element!.style.display='block'
    return throwError(errorMessage);
  }

}
