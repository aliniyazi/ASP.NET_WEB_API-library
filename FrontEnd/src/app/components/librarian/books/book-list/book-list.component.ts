import { CursorError } from '@angular/compiler/src/ml_parser/lexer';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import paginate from 'jw-paginate';
import { ToastrService } from 'ngx-toastr';
import { HeaderComponent } from 'src/app/components/header/header.component';
import { Book } from 'src/app/services/book.model';
import { BookService } from 'src/app/services/book.service';



@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss']
})
export class BookListComponent implements OnInit {
    currentPage:number = 1;
    countAllBooks: number = 0;
    countAllGenres: number = 0;
    countAllUsers: number = 0;
    booksPerPage:number = 8;

  constructor(public service: BookService, private toastr: ToastrService,private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.service.refreshList(1);
    this.service.getInitialBookInfo()
        .subscribe((response)=>{
          this.countAllBooks = response.countAllBooks;
          this.countAllGenres = response.countAllGenres;
          this.countAllUsers = response.countAllUsers;
        })
  }
  nextButton(){
    if(this.currentPage < Math.ceil(this.countAllBooks/this.booksPerPage))
    {
    ++this.currentPage;
    document.getElementById("currentPage")!.textContent=this.currentPage.toString();
    this.service.refreshList(this.currentPage);
    this.service.currentPage = this.currentPage;
    }
  }
  previousButton(){
    if(this.currentPage > 1){
      --this.currentPage;
      document.getElementById("currentPage")!.textContent=this.currentPage.toString();
      this.service.refreshList(this.currentPage);
      this.service.currentPage = this.currentPage;
    }
  }
  lastButton(){
    this.currentPage =  Math.ceil(this.countAllBooks/this.booksPerPage);
    document.getElementById("currentPage")!.textContent=this.currentPage.toString();
    this.service.refreshList(this.currentPage);
    this.service.currentPage = this.currentPage;
  }
  firstButton(){
    this.currentPage = 1;
    document.getElementById("currentPage")!.textContent=this.currentPage.toString();
    this.service.refreshList(this.currentPage);
    this.service.currentPage = this.currentPage;
  }
  public ReturnNextPage(){
    return this.currentPage;
  }
  populateForm(emp: Book) {
    document.getElementById("sub")!.style.background = "blue";
    document.getElementById("sub")!.textContent = "UPDATE";
    document.getElementById("sub")!.style.color = "white";
    document.getElementById("img")!.style.visibility='visible';
    this.service.formData = Object.assign({}, emp);
    this.service.refreshList(this.currentPage);
  }
  onDelete(id: number | null) {
    if (confirm("Are you sure to delete this book ?")) {
      this.service.deleteBook(id!).subscribe(res => {
        this.toastr.warning('Deleted succesfully');
        this.service.refreshList(this.currentPage);
      })
    }
  }
}
