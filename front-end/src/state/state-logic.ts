import { createSlice, configureStore } from '@reduxjs/toolkit'
import { FileInstance } from '../common/FileInstance'

let filesArray: Array<FileInstance> = []
let id : string = '';
const counterSlice = createSlice({
  name: 'files_monitor',
  initialState: {
    files: filesArray,
    areFilesUploaded : false,
    sessionId: id,
    filesToUpload : []
  },
  reducers: {
    filesUploaded: (state, params) => {
      state.files.push(new FileInstance(params.payload['name']))
    },
    fileUpdated: (state, params) => {
      let file = state.files.find(x => x.fileName == params.payload['name'])
      if (file == undefined){
        throw new Error('file not found')
      }
      file.status = params.payload['status']
      file.downloadUrl = params.payload['url']
    },
    filesSelected:(state, params)=>{
      state.filesToUpload = params.payload['files']
    },
    sessionIsReadyToOpen:(state, params)=>{
      state.sessionId =params.payload['sessionId']
      state.files = state.filesToUpload.map(x => new FileInstance(x['name']))
    }
  }
})

const store = configureStore({
  reducer: counterSlice.reducer
})
const { filesUploaded, fileUpdated, sessionIsReadyToOpen, filesSelected } = counterSlice.actions
export {store, sessionIsReadyToOpen, fileUpdated, filesUploaded, filesSelected}

// Can still subscribe to the store
//store.subscribe(() => console.log(store.getState()))

// Still pass action objects to `dispatch`, but they're created for us
//store.dispatch(fileUploaded({name :'aaa'}))
// {value: 1}
//store.dispatch(incremented())
// {value: 2}
//store.dispatch(decremented())
// {value: 1}