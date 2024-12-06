import React, { useEffect, useState } from "react";
import LoginPage from "./components/LoginPage";
import ShoppingList from "./components/ShoppingList";
import "./App.css";

export default function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [currentUser, setCurrentUser] = useState(null);
  const [lists, setLists] = useState([]);

  const handleLogin = (username) => {
    setIsLoggedIn(true);
    setCurrentUser(username);
  };

  const handleLogout = () => {
    setIsLoggedIn(false);
    setCurrentUser(null);
  };

  const addList = (name) => {
    setLists([...lists, { id: Date.now(), name, items: [] }]);
  };

  const updateList = (updatedList) => {
    setLists(
      lists.map((list) => (list.id === updatedList.id ? updatedList : list))
    );
  };

  //---------------> Получение данных с бекенда <---------------
  const [newLists, setNewLists] = useState([]); // Создаём пустой массив
  
  useEffect(() =>{  // useEffect без параметров => вызывается при загрузки страницы
    getData();      // Вызываем функцию getData
  },[]);

  async function getData() {
    try {
      const response = await fetch("https://virtical-backend-df55.twc1.net/api/purchases"); // Делаем запрос по url
      const data = await response.json();                                                   // Преобразуем ответ в json
      setNewLists(data);                                                                    // Задаём значение переменной newLists, через useState
    } catch (error) {
      console.error("Ошибка при загрузке данных:", error);
    }
  }
  //------------------------------------------------------------

  // Выводим значние в консоль, если newLists изменился.
  useEffect(() =>{
    console.log(newLists);
  },[newLists]);
  //------------------------------------------------------------

  return (
    <div className="app">
      {isLoggedIn ? (
        <div>
          <header className="header">
            <div className="user-info">
              <span>👤 {currentUser}</span>
              <button onClick={handleLogout}>Выйти</button>
            </div>
          </header>
          <main className="main-content">
            <h1 className="main-title">Список покупок</h1>
            <ShoppingList lists={lists} onAddList={addList} onUpdateList={updateList} />
          </main>
        </div>
      ) : (
        <LoginPage onLogin={handleLogin} />
      )}
    </div>
  );
}
