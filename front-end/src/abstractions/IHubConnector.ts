import { UUID } from "crypto";

export interface IHubConnector{
    connectToHub(requestUd : UUID): IHubConnector

    disconnect(): void

    run() : IHubConnector
}