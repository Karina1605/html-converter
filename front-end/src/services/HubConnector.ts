import 'reflect-metadata';
import { inject, injectable } from "inversify";
import { IHubConnector } from "../abstractions/IHubConnector";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { UUID } from "crypto";
import { fileUpdated } from "../state/state-logic";
import { HttpOptions } from "../options/HttpOptions";

@injectable()
export default class HubConnector implements IHubConnector{

    private _hubConnection : HubConnection;
    
    constructor(@inject('url_info') private readonly options :HttpOptions){
        this._hubConnection = new HubConnectionBuilder()
	        .withUrl(options.Url +'/test')
	        .build();
    }

    connectToHub(requestUd: UUID): IHubConnector {
        this._hubConnection.start().then(a => {
            if (this._hubConnection.connectionId) {
                this._hubConnection.invoke("EstablishConnection", {
            RequestId: requestUd,
            ConnectionId: this._hubConnection.connectionId});
            }   
        }); 
        return this; 
    }

    disconnect(): void {
        this._hubConnection.stop();
    }
    run(): IHubConnector {
        this._hubConnection.on('updateInfo', message =>{
            fileUpdated({name: ''});
        })
        return this;
    }
    
}