import { FileState } from "../../common/FileStates";

export interface IFileStatusState{
    status: FileState;
    fileName: string
    downloadLink: string;
}