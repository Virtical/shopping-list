import React, { useState } from 'react';
import './LoginForm.css';

const LoginForm = () => {
  const [formData, setFormData] = useState({ username: '', password: '' });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch('/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          login: formData.username,
          password: formData.password,
        }),
      });

      setFormData({ username: '', password: '' });

      if (!response.ok) {
        throw new Error('Ошибка при выполнении запроса');
      }
      
      window.location.href = '/list';
    } catch (error) {
      console.error('Ошибка:', error);
    }
  };


  return (
    <>
    <div className="login-container">
      <h2>Войти</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          name="username"
          placeholder="Введите логин:"
          value={formData.username}
          onChange={handleChange}
          required
        />
        <input
          type="password"
          name="password"
          placeholder="Введите пароль:"
          value={formData.password}
          onChange={handleChange}
          required
        />
        <button type="submit">Войти</button>
      </form>
    </div>
    <p className="no-account">
        <a href="/registration">Зарегистрироваться</a>
      </p>
    </>
  );
};

export default LoginForm;