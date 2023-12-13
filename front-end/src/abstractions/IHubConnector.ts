export interface IHubConnector{
    connectToHub(): IHubConnector

    disconnect(): void

    run() : IHubConnector
}