import React from "react";
import type { IHttpClient } from "../../abstractions/IHttpClient";
import type { IHubConnector } from "../../abstractions/IHubConnector";
import { sessionIsReadyToOpen, store } from "../../state/state-logic";
import { FileStatus } from "../FileStatus/FileStatus";
import { container } from "../../dependency-configuration";

export class FilesStatusWindow extends React.Component<{}, {}>{

    override componentDidMount(): void {
        let client = container.get<IHttpClient>('http_client')
       
        console.log(client)
        client.sendFiles(store.getState()['filesToUpload'])
        .then(x => {
            store.dispatch(sessionIsReadyToOpen({sessionId: x}))
            let hub = container.get<IHubConnector>('hub_connector')
            hub.connectToHub().run();
        })
    }

    override render(): React.ReactNode {
        let files = store.getState()['files'].map(x => <FileStatus fileName={x.fileName}/>);
        return(
            <div>
                {files}
            </div>
        )
    }
}