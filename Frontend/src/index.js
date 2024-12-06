import React from "react";
import ReactDOM from "react-dom";
import "./index.css"; // Необязательно: создайте файл index.css для глобальных стилей
import App from "./App";

ReactDOM.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
  document.getElementById("root")
);
