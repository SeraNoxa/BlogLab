export class ApplicationUser {

    constructor(
        public applicationUserId: number,
        public username: string,
        public fullname: string,
        public password: string,
        public token: string
    ){}
    
}