import { FileState } from "./FileStates"

export class FileInstance{
    public fileName : string
    public lastUpdated: Date
    public status : FileState
    public downloadUrl : string

    constructor(fileName: string){
        this.fileName = fileName
        this.lastUpdated = new Date()
        this.status = FileState.waiting
        this.downloadUrl = ''
    }

    public setDownLoadUrl(url: string){
        this.downloadUrl = url
        this.status = FileState.successfullyProcessed
        this.lastUpdated = new Date()
    }

    public setStatus(status: FileState){
        if(status == FileState.successfullyProcessed && this.downloadUrl == ''){
            throw new Error('Url is not defined')
        }

        this.status = status;
        this.lastUpdated = new Date();
    }
}