import { BookInterface } from "./book";

export interface BooksDataResponse {
    data: BookInterface[];
    recordsTotal: number;
}