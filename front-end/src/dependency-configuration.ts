import "reflect-metadata";
import { Container } from 'inversify'
import  HttpClient  from './services/HttpClient'
import HubConnector  from './services/HubConnector';
import { HttpOptions } from './options/HttpOptions';
import conf from './config.json'
import { IHttpClient } from "./abstractions/IHttpClient";
import { IHubConnector } from "./abstractions/IHubConnector";


const container = new Container();
container.bind<HttpOptions>('url_info').toConstantValue({Url: conf.ServerUrl});
container.bind<IHttpClient>('http_client').to(HttpClient).inSingletonScope();
container.bind<IHubConnector>('hub_connector').to(HubConnector).inSingletonScope();
container.load();
export { container };