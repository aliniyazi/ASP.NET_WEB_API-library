import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { RequestModel, RequestResponse } from './models/request';
import { AuthServiceService } from './auth-services/auth-service.service';
import { map } from 'jquery';

@Injectable({
  providedIn: 'root'
})
export class RequestsService {
  requestModel! : RequestModel

  constructor(private http: HttpClient) { }

  postRequest(requestModel: RequestModel):Observable<any> {
     
      return this.http.post<any>(environment.baseUrl +'/Request/bookRequest' ,requestModel);
  }​​​​​
}

