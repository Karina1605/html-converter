# Html files formatter


## Description

The project consists of 2 rest api services, 2 background workers and  one signalR hub.
Ui based on React.
As a file storage minio is used, sessions information is stored in postgre database. 
Communication between services goes through kafka and http requests.
The infrastructure description is presented in svg in this repo.

### Api for getting files from client

- Accepts set of files, generates Guid for the current session
- Saves files to minio storage
- Adds information about session to database.
- Put to kafka tasks for converting.
- Returns sessionId to user. Then user connect to hub with this Id.
In case of restart this service user will proceed getting updates on his files. 

### File converting processor

Service is Listening Kafka messages. When the new one arrives:
- Gets url for html file from LinksService
- With Puppeteer Sharp open html and converts it to pdf
- In case of success saves new pdf file with the same name in minio
- Put to kafka operation results
As we commit reseiving message only after processing, the service restart will not affect converting pipeline

### Converting result processor

Service is Listening Kafka messages. When the new one arrives:
- In case of success gets link from Link service and send info with download url to hub
- In case of error sends to hub error message
The same mechanism with Kafka, restart is not crucial

### Api for generating temporary urls for files

Gets requests with session id and file to download and generates link to minio. 
Weak point: services appealing to this do not have retry policy, we can catch errors in case of restarting
Solution : add retry policy, is not implemented with a week

### Hub Runner
Listens kafka and accepts client connections. After getting update send the suitable client notification about file
React signalR supports reconnecting, so service restart is not crucial.

### React client
Has 2 windows with routing. The first one uploads files, then Monitoring window is opened.
Monitoring window opens hub connection and updates state by server notification

## Durability
All the services can be replicated and distributed by kafka partitions and nginx.

## Improvements
- Writing tests
