
import { FileState } from "../../common/FileStates";
import CircularProgress from "@mui/material/CircularProgress";
import { store } from "../../state/state-logic";
import React from "react";
import { IFileStatusProps } from "./IFileStatusProps";
import { IFileStatusState } from "./IFileStatusState";
import DescriptionIcon from '@mui/icons-material/Description';
import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';
import { Button } from "@mui/material";
import DownloadIcon from '@mui/icons-material/Download';

export class FileStatus extends React.Component<IFileStatusProps, IFileStatusState> 
{
    constructor(props: IFileStatusProps){
        super(props)
        this.setState({
            fileName: props.fileName,
            status: FileState.waiting,
            downloadLink: ""
        });
        store.subscribe(()=>{
            var updated = store.getState()['files'].find(x => x['fileName'] == this.state.fileName);
            this.setState({
                status: updated!.status,
                downloadLink: updated!.downloadUrl
            })
        })
    }
    
    override render(): React.ReactNode {
        return<div>
        <DescriptionIcon/>
        <p>{this.state.fileName}</p>
        {getAvailableAction(this.state.status)}
        </div>
    }
}
function getAvailableAction(state: FileState){
    switch(state){
        case FileState.waiting:
            return <CircularProgress/>
        case FileState.processingError:
            return <ErrorOutlineIcon/>
        case FileState.successfullyProcessed:
            return <Button variant="outlined" endIcon={<DownloadIcon />}>Download</Button>
    }
}