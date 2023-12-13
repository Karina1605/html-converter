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

  constructor(){
  }

    async sendFiles(files: FileContent<string>[]): Promise<string> {
        const formData = new FormData();
        let result: string =""
        files.forEach(file=>{
            formData.append(file.name, file.content);
          });
        var url = container.get<HttpOptions>('url_info').Url

        await axios.post(url + "/convert-files", {
            data: formData,
            headers: {
              "Content-Type": "multipart/form-data"
            }
          }).then(x => result = x.data['clientId']);
        return result
    }
    
}

