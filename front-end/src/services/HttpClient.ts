import 'reflect-metadata';
import {IHttpClient} from '../abstractions/IHttpClient'
import { UUID, randomUUID } from 'crypto' 
import axios from "axios";
import { FileContent } from 'use-file-picker/dist/interfaces';
import { HttpOptions } from '../options/HttpOptions';
import { inject, injectable } from 'inversify';
import { container } from '../dependency-configuration';

@injectable()
export default class HttpClient implements IHttpClient{

  private readonly url : string;
  constructor(@inject('url_info') private readonly urlOps: HttpOptions){
    this.url = urlOps.Url
  }

    async sendFiles(files: FileContent<string>[]): Promise<UUID> {
        const formData = new FormData();
        let result = randomUUID()
        files.forEach(file=>{
            formData.append(file.name, file.content);
          });
          
          //!!!
        await axios.post(this.url + "/convert-files", {
            data: formData,
            headers: {
              "Content-Type": "multipart/form-data"
            }
          }).then(x => result = x.data);
        return result
    }
    
}

