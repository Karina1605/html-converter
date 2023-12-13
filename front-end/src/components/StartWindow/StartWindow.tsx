import { Button } from "@mui/material";
import FileUploadIcon from '@mui/icons-material/FileUpload';
import { useFilePicker } from 'use-file-picker';
import { filesSelected, store } from "../../state/state-logic";
import { connect } from "react-redux"
import './StartWindow.css'
import { redirect } from "react-router-dom";

export function StartWindow(){
    connect(
        null,
        { filesSelected }
      )(StartWindow);
    function saveFiles(){
        store.dispatch(filesSelected({files: filesContent}))
        console.log('redirecting')
        return redirect(`/status`)
    }
    const { openFilePicker, filesContent, loading, errors, plainFiles, clear } = useFilePicker({
        accept: '.html',
        multiple: true,
        onFilesSelected: ({ plainFiles, filesContent, errors }) => {
            saveFiles()
          }
      });
    return (<div className="center">
        <Button startIcon={<FileUploadIcon/>} onClick={() => openFilePicker()}>Select files</Button>
      <br />
      
    </div>)
}
