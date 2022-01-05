import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { InitialBookInfoResponse } from '../models/bookInitialInfoResponce'
import { BooksDataResponse } from '../models/booksDataResponse'
import { Book } from './book.model';

@Injectable({
  providedIn: 'root'
})

export class BookService {
  formData!: Book;
  list!: Book[];
  book!: Book;
  currentPage!: number;

  constructor(private http: HttpClient) {
  }

  getInitialBookInfo() : Observable<InitialBookInfoResponse>{
    return this.http.get<InitialBookInfoResponse>(environment.baseUrl+'/Books');
  }

  getInitialBooksData(pageNo: string) : Observable<BooksDataResponse>{

    const pageSize = 5;
    const params = new HttpParams()
       .set('page', pageNo)
       .set('booksPerPage', pageSize);
    return this.http.get<BooksDataResponse>(environment.baseUrl + '/Books/details', {params});
  }

  postBook(formData: FormData): Observable<any> {
    return this.http.post<Book>(environment.baseUrl + '/books',formData);
  }
  refreshList(pageNO:number) {
    const pageSize = 8;
    const params = new HttpParams()
       .set('page',pageNO)
       .set('booksPerPage', pageSize);
       this.currentPage = pageNO;
    return this.http.get<Book[]>(environment.baseUrl + '/Books/all',{params}).toPromise().then(res => this.list = res);
  }
  putBook(formData: FormData) {
    return this.http.put(environment.baseUrl + '/books/' + formData.get('Id'), formData);
  }
  deleteBook(id: number) {
    return this.http.delete(environment.baseUrl + '/books/' + id);
  }
  searchBook(searchType:string,search:string,genre:string,pageNO:number):Observable<BooksDataResponse>{
    const pageSize = 8;
    const params = new HttpParams()
       .set('page',pageNO)
       .set('booksPerPage',5)
       .set('searchType',searchType)
       .set('search', search)
       .set('genres',genre);
    return this.http.get<BooksDataResponse>(environment.baseUrl + '/Books/search',{params});
  }
}
