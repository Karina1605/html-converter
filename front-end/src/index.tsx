import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { Provider } from "react-redux";
import {store} from './state/state-logic';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import { StartWindow } from './components/StartWindow/StartWindow';
import { FilesStatusWindow } from './components/FilesStatusWindow/FilesStatusWindow';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

const router = createBrowserRouter([
  {
    path: "/",
    element: <StartWindow/>,
  },
  {
    path:"/status",
    element: <FilesStatusWindow/>
  }
]);

root.render(
  <Provider store={store}>
     <RouterProvider router={router} />
  </Provider>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
