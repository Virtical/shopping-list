import React, { useState } from 'react';
import './RegistrationForm.css';

const RegistrationForm = () => {
  const [formData, setFormData] = useState({
    username: '',
    password: '',
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch('/registration', {
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

      alert("Успешная регистрация!")
      
      window.location.href = '/login';
    } catch (error) {
      console.error('Ошибка:', error);
    }
  };

  return (
    <>
      <div className="registration-container">
        <h2>Регистрация</h2>
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
          <button type="submit">Зарегистрироваться</button>
        </form>
      </div>
      <p className="already-registered">
        Уже зарегистрировались? <a href="/login"> Войти</a>
      </p>
    </>
  );
};

export default RegistrationForm;
