import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  librarianName!:string;

  constructor(private jwtHelperService: JwtHelperService) { }

  ngOnInit(): void {
    this.librarianName = this.LibrarianUsername();
  }
  LibrarianUsername():string{

    const email = this.jwtHelperService.decodeToken(localStorage.getItem('token') || '').email;
    const username = email.split("@")[0];
    return username;
  }


}
