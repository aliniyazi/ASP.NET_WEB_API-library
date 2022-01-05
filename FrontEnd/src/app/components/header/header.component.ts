import { Component, Input ,OnInit } from '@angular/core';
import { AuthServiceService } from 'src/app/auth-services/auth-service.service';
import { BookService } from '../../services/book.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Input() appTitle: string | undefined;
  countAllBooks: number = 0;
  countAllGenres: number = 0;
  countAllUsers: number = 0;
  
  constructor(private bookService: BookService, private authService:AuthServiceService) { }

  ngOnInit(): void {
      this.bookService.getInitialBookInfo()
        .subscribe((response)=>{
          this.countAllBooks = response.countAllBooks;
          this.countAllGenres = response.countAllGenres;
          this.countAllUsers = response.countAllUsers;
        })
  }

  Logout(){
    this.authService.logout();
  }

  isLoggedIn(): boolean {
    return localStorage.getItem('token') !== null;
  }
}
