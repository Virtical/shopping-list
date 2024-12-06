import React, { useEffect, useState } from "react";
import "./ShoppingList.css";

export default function ShoppingList({ lists, onAddList, onUpdateList }) {
  const [newListName, setNewListName] = useState("");
  const [searchQuery, setSearchQuery] = useState("");
  const [editingItem, setEditingItem] = useState(null);
  const [editingValue, setEditingValue] = useState("");
  const [newItems, setNewItems] = useState({}); // Хранение новых товаров для каждого списка
  const [newQuantities, setNewQuantities] = useState({}); // Хранение количества товаров

  const handleAddList = () => {
    if (!newListName.trim()) return;
    onAddList(newListName);
    setNewListName(""); // Очистка поля после добавления
  };



useEffect(() =>{getData();},[]);

async function getData() {
  const response = await fetch('https://virtical-backend-df55.twc1.net/api/purchases')
  const data = await response.json();
  console.log(data);
}

  const handleAddItem = (listId) => {
    const newItem = newItems[listId];
    const newQuantity = newQuantities[listId] || 1; // По умолчанию количество = 1

    if (!newItem || !newItem.trim()) return;

    const updatedList = lists.find((list) => list.id === listId);
    updatedList.items.push({
      id: Date.now(),
      name: newItem,
      completed: false,
      quantity: newQuantity,
    });
    onUpdateList(updatedList);

    // Очистка полей для добавленного товара
    setNewItems((prevState) => ({
      ...prevState,
      [listId]: "",
    }));
    setNewQuantities((prevState) => ({
      ...prevState,
      [listId]: "",
    }));
  };

  const toggleCompleted = (listId, itemId) => {
    const updatedList = lists.find((list) => list.id === listId);
    updatedList.items = updatedList.items.map((item) =>
      item.id === itemId ? { ...item, completed: !item.completed } : item
    );
    onUpdateList(updatedList);
  };

  const handleDeleteItem = (listId, itemId) => {
    const updatedList = lists.find((list) => list.id === listId);
    updatedList.items = updatedList.items.filter((item) => item.id !== itemId);
    onUpdateList(updatedList);
  };

  const handleEditItem = (listId, itemId) => {
    const updatedList = lists.find((list) => list.id === listId);
    updatedList.items = updatedList.items.map((item) =>
      item.id === itemId ? { ...item, name: editingValue } : item
    );
    onUpdateList(updatedList);
    setEditingItem(null);
    setEditingValue("");
  };

  const filteredLists = lists.map((list) => ({
    ...list,
    items: list.items.filter((item) =>
      item.name.toLowerCase().includes(searchQuery.toLowerCase())
    ),
  }));

  return (
    <div className="list-wrapper">
      {/* Поле для добавления списка */}
      <div className="add-list-wrapper">
        <input
          type="text"
          placeholder="Введите название списка"
          value={newListName}
          onChange={(e) => setNewListName(e.target.value)}
        />
        <button onClick={handleAddList}>Добавить список</button>
      </div>

      {/* Поле для поиска */}
      <div className="search-wrapper">
        <input
          type="text"
          placeholder="Поиск товаров"
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
        />
      </div>

      {/* Отображение списков */}
      {filteredLists.map((list) => (
        <div className="list-item" key={list.id}>
          <h3>{list.name}</h3>

          {/* Поле для добавления товара */}
          <div>
            <input
              type="text"
              placeholder="Добавить товар"
              value={newItems[list.id] || ""}
              onChange={(e) =>
                setNewItems((prevState) => ({
                  ...prevState,
                  [list.id]: e.target.value,
                }))
              }
            />
            <input
              type="number"
              placeholder="Кол-во"
              value={newQuantities[list.id] || ""}
              min="1"
              onChange={(e) =>
                setNewQuantities((prevState) => ({
                  ...prevState,
                  [list.id]: e.target.value,
                }))
              }
            />
            <button onClick={() => handleAddItem(list.id)}>Добавить</button>
          </div>

          {/* Список товаров */}
          <ul>
            {list.items.map((item) => (
              <li key={item.id} className={item.completed ? "completed" : ""}>
                <div className="item-content">
                  <div className="item-details">
                    <input
                      type="checkbox"
                      checked={item.completed}
                      onChange={() => toggleCompleted(list.id, item.id)}
                    />
                    {editingItem === item.id ? (
                      <input
                        type="text"
                        value={editingValue}
                        onChange={(e) => setEditingValue(e.target.value)}
                      />
                    ) : (
                      <span>
                        {item.name} (x{item.quantity})
                      </span>
                    )}
                  </div>
                  <div className="item-actions">
                    {editingItem === item.id ? (
                      <button onClick={() => handleEditItem(list.id, item.id)}>
                        Сохранить
                      </button>
                    ) : (
                      <>
                        <button onClick={() => setEditingItem(item.id)}>
                          Редактировать
                        </button>
                        <button onClick={() => handleDeleteItem(list.id, item.id)}>
                          Удалить
                        </button>
                      </>
                    )}
                  </div>
                </div>
              </li>
            ))}
          </ul>
        </div>
      ))}
    </div>
  );
}
