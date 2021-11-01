export class Photo {

    constructor(
        public photoId: number,
        public applicationUserId: number,
        public imageUrl: string,
        public publicId: string,
        public description: string,
        public publishDate: Date,
        public updateData: Date,
        public deleteConfirm: boolean = false

    ){}
    
}