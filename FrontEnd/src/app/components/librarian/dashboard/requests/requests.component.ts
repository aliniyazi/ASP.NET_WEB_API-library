import { Component, OnInit } from '@angular/core';
import { DashboardService } from 'src/app/services/dashboard.service';
import { Request } from 'src/app/services/request.model';
import { RequestUpdateModel } from 'src/app/services/requestUpdateModel';

@Component({
  selector: 'app-requests',
  templateUrl: './requests.component.html',
  styleUrls: ['./requests.component.scss']
})
export class RequestsComponent implements OnInit {
  requestApprove!: boolean;
  model!: RequestUpdateModel;

  constructor(public service: DashboardService) { }

  ngOnInit(): void {
    this.ShowAllElements();
  }


  ShowAllElements() {
    this.service.getAllRequests();
  }
  onApprove(userId: string, bookId: number) {
    this.model = new RequestUpdateModel;
    this.model.userId = userId;
    this.model.bookId = bookId
    this.model.requestApproved = true;
    this.service.modifireRequest(this.model).subscribe(res => {

      this.ShowAllElements();
    });
  }
  onReject(userId: string, bookId: number) {
    this.model = new RequestUpdateModel;
    this.model.userId = userId;
    this.model.bookId = bookId
    this.model.requestApproved = false;
    this.service.modifireRequest(this.model).subscribe(res => {

      this.ShowAllElements();
    });
  }

}
