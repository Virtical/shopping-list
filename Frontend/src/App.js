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
  
useEffect(() =>{getData();},[]);

async function getData() {
  const response = await fetch('https://virtical-backend-df55.twc1.net/api/purchases')
  const data = await response.json();
  console.log(data);
}


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
