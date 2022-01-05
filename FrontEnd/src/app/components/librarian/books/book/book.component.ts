import { NULL_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, Input, OnInit } from '@angular/core';
import { Form, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BookService } from 'src/app/services/book.service';


@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.scss']
})
export class BookComponent implements OnInit {
  uploadedImage!: string | Blob;
  shownImage!: string | ArrayBuffer | null;

  constructor(public service: BookService,private toastr:ToastrService) { }
  bookFormData!:FormData;


  ngOnInit(): void {
    document.getElementById("clearButton")!.style.background="darkred";
    document.getElementById("clearButton")!.style.color="white";
    this.showCreateButton();
    this.resetForm()
  }

  resetForm(form?: NgForm) {
    if (form != null)
      form.resetForm();
    this.service.formData = {
      id: null,
      title: '',
      description: '',
      quantity:0,
      imageName:'',
      authorFirstName: '',
      authorLastName: '',
      genre: ''
    }
  }
  Clear(){
    this.showCreateButton();
    this.service.formData = {
      id: null,
      title: '',
      description: '',
      quantity:0,
      imageName:'',
      authorFirstName: '',
      authorLastName: '',
      genre: ''
    }
    document.getElementById("img")!.style.visibility='hidden';
  }

  onSubmit(ngForm: NgForm) {

    this.bookFormData = new FormData();
    this.bookFormData.append('Title' , ngForm.value.title);
    this.bookFormData.append('Description' ,  ngForm.value.description);
    this.bookFormData.append('Quantity' ,  ngForm.value.quantity);
    this.bookFormData.append('ImageFile' , this.uploadedImage);
    this.bookFormData.append('Genre' ,  ngForm.value.genre);
    this.bookFormData.append('AuthorFirstName' ,  ngForm.value.authorFirstName);
    this.bookFormData.append('AuthorLastName' ,  ngForm.value.authorLastName);
    console.log( ngForm);

    if (ngForm.value.id == null){
    this.insertRecord(this.bookFormData,ngForm)
  }
    else{
    this.updateRecord(this.bookFormData,ngForm);
  }}
  insertRecord(form: FormData,ngForm:NgForm) {
    this.service.postBook(form).subscribe(res => {
      this.toastr.success('Created succesfully');
      this.resetForm(ngForm);
      this.service.currentPage = 1;
      this.service.refreshList(this.service.currentPage);
      document.getElementById("img")!.style.visibility='hidden';
    });
  }
  updateRecord(form: FormData,form2:NgForm) {
    form.append('Id',form2.value.id);
    this.service.putBook(form).subscribe(res => {
      this.toastr.info('Updated succesfully');
      this.resetForm(form2);
      this.service.refreshList(this.service.currentPage);
      document.getElementById("img")!.style.visibility='hidden';
    })
    this.showCreateButton();
  }
  showCreateButton(){
    document.getElementById("sub")!.style.background="green";
    document.getElementById("sub")!.textContent="CREATE";
    document.getElementById("sub")!.style.color="white";
  }
  onSelectFile(event:any) { // called each time file input changes
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();
      this.uploadedImage = <File>event.target.files[0];

      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event) => { // called once readAsDataURL is completed
        this.shownImage = event.target!.result;
      }
      document.getElementById("img")!.style.visibility='visible';
    }
    
    
}
  
}
