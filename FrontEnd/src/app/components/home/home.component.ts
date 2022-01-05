 import { BookService } from '../../services/book.service';
 import { Component, OnInit } from '@angular/core';
 import { ActivatedRoute } from '@angular/router';
 import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

 import paginate from 'jw-paginate';
 import { RequestsService } from 'src/app/requests.service';
 import { RequestModel } from 'src/app/models/request';
import { AuthServiceService } from 'src/app/auth-services/auth-service.service';
import { ToastrService } from 'ngx-toastr';
import { JwtHelperService } from '@auth0/angular-jwt';

 @Component({
   selector: 'app-home',
   templateUrl: './home.component.html',
   styleUrls: ['./home.component.scss']
 })
 export class HomeComponent implements OnInit {
    request !: RequestModel
    pager: any = {};
    pageOfItems:any = [];
    pageSize : number = 5;
    genreType!:string;
    mainItems:any = [];
    formGroup!: FormGroup;
    currentPage:number = 1;
    
    constructor(private bookService: BookService,
                private route: ActivatedRoute,
                private requestService: RequestsService,
                public authService: AuthServiceService,
                private toastr:ToastrService,
                private jwtHelperService:JwtHelperService  ){ }

    ngOnInit():void{
      this.route.queryParams.subscribe(x => this.loadPage(x.page || 1));
      this.initForm();
    };

    private loadPage(currentPage:string){

      this.bookService.getInitialBooksData(currentPage)
        .subscribe(response=>{
          const items = response.data.filter(function(x){
            return x !=null;
          });
          const recordsTotal = response.recordsTotal;
          const page = parseInt((currentPage)) || 1;
          const pager = paginate(recordsTotal, page, this.pageSize);
          items.slice(pager.startIndex, pager.endIndex + 1);
          this.pager = pager;
          this.pageOfItems = items;
          this.mainItems = items;
        });
    };
    initForm(){
      
      this.formGroup = new FormGroup({
        search: new FormControl('', [Validators.required]),
        searchType: new FormControl('', [Validators.required]),
        genreType : new FormControl('',[Validators.required])
      })
    }
    onSearch(){
      
      if(this.formGroup.value.genreType == "Search in all genres"){
        this.genreType = '';
      }
      else{
        this.genreType = this.formGroup.value.genreType;
      }
      this.bookService.searchBook(this.formGroup.value.searchType,this.formGroup.value.search,this.genreType,this.currentPage)
      .subscribe(response=>{
        const items = response.data.filter(function(x){
          return x !=null;
        });
        const recordsTotal = response.recordsTotal;
        const page = this.currentPage || 1;
        const pager = paginate(recordsTotal, page, this.pageSize);
        items.slice(pager.startIndex, pager.endIndex + 1);
        this.pager = pager;
        this.pageOfItems = items;
        this.mainItems = items;
        this.pageOfItems = response.data;
      })
      this.route.queryParams.subscribe(x=>this.bookService.searchBook(this.formGroup.value.searchType,this.formGroup.value.search,this.genreType,x.page))
    }
    exitSearch(){

      this.route.queryParams.subscribe(x => this.loadPage(x.page || 1));

    }
    requestButton(id: number){
      const decodedTokenEmail = this.jwtHelperService.decodeToken(localStorage.getItem('token') || '').email;
      this.request = {
        userEmail:decodedTokenEmail,
        bookId: id
      }
      this.requestService.postRequest(this.request).subscribe(res=>{
        this.toastr.info('Request Created successfully');
      },(err)=>this.toastr.error('Request Failed to create'));


    }
}
