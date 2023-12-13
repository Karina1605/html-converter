import 'reflect-metadata';
import { inject, injectable } from "inversify";
import { IHubConnector } from "../abstractions/IHubConnector";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { UUID } from "crypto";
import { fileUpdated, store } from "../state/state-logic";
import { HttpOptions } from "../options/HttpOptions";

@injectable()
export default class HubConnector implements IHubConnector{

    private _hubConnection : HubConnection;
    private readonly _sessionId : string;
    
    constructor(@inject('url_info') private readonly options :HttpOptions){
        this._sessionId = store.getState()['sessionId'] 
        this._hubConnection = new HubConnectionBuilder()
	        .withUrl(options.Url +'/test')
	        .build();
    }

    connectToHub(): IHubConnector {
        this._hubConnection.start().then(a => {
            this.establishConnection(this._sessionId);
        }); 
        this._hubConnection.onreconnecting(() => this.establishConnection(this._sessionId))
        return this; 
    }

    disconnect(): void {
        this._hubConnection.stop();
    }
    run(): IHubConnector {
        this._hubConnection.on('updateInfo', message =>{
            store.dispatch(fileUpdated({
                name: message['OriginalFileName'],
                status: message['Status'],
                downloadUrl: message['DownloadLink']
            }));
        })
        return this;
    }

    private establishConnection(requestId: string) : void{
        if (this._hubConnection.connectionId) {
            this._hubConnection.invoke("EstablishConnection", {
        RequestId: requestId,
        ConnectionId: this._hubConnection.connectionId});
        }   
    }
    
}