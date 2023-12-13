import { FileContent } from "use-file-picker/dist/interfaces";

export interface IHttpClient{
    sendFiles(files: FileContent<string>[]) : Promise<string>
}