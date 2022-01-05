export class Book {
    public id!: number | null;
    public title!:string;
    public description!:string;
    public quantity!:number;
    public imageName!:File | string
    public genre!:string;
    public authorFirstName!:string;
    public authorLastName!:string;
}