import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Request } from './request.model';
import { Observable } from 'rxjs';
import { RequestUpdateModel } from './requestUpdateModel';
@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  list!:Request[];
  formData!:Request;

  constructor(private http: HttpClient) { }
  getAllRequests(){
    return this.http.get<Request[]>(environment.baseUrl + '/request/all').toPromise().then(res => this.list = res);
  }
  modifireRequest(model:RequestUpdateModel){
    return this.http.put(environment.baseUrl + '/request/update',model);
  }
}
