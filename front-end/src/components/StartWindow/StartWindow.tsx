import { Button } from "@mui/material";
import FileUploadIcon from '@mui/icons-material/FileUpload';
import { useFilePicker } from 'use-file-picker';
import { filesSelected, store } from "../../state/state-logic";
import { connect } from "react-redux"
import './StartWindow.css'

export function StartWindow(){
    connect(
        null,
        { filesSelected }
      )(StartWindow);
    const { openFilePicker, filesContent, loading, errors, plainFiles, clear } = useFilePicker({
        accept: '.html',
        multiple: true,
        onFilesSelected: ({ plainFiles, filesContent, errors }) => {
            store.dispatch(filesSelected({files: filesContent}))
          }
      });
    return (<div className="center">
        <Button startIcon={<FileUploadIcon/>} onClick={() => openFilePicker()}>Select files</Button>
      <br />
      
    </div>)
}
