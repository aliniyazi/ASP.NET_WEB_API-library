export interface User {
    firstName: string,
    lastName: string,
    email:string,
    password: string,
    confirmpassword: string,
    phonenumber:string,
    address: Address
}

export interface Address {
    country: string
    city: string
    street: string
    streetnumber: string
    buildingnumber: string
    apartmentnumber: string
    aditionalinfo:string
}