// components/Registration.js
import React, { useState } from "react";
import "./Registration.css";

export default function Registration({ setUser }) {
  const [username, setUsername] = useState("");

  const handleRegister = () => {
    if (username.trim()) {
      setUser(username);
    } else {
      alert("Введите имя!");
    }
  };

  return (
    <div className="registration-wrapper">
      <h2>Регистрация</h2>
      <input
        type="text"
        placeholder="Введите ваше имя"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
      <button onClick={handleRegister}>Войти</button>
    </div>
  );
}
