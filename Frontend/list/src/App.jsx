import { useEffect, useState } from 'react'
import './App.css'
import plusImg from "./plus.svg";
import deleteImg from "./delete.svg";
import logoutImg from "./logout.svg";

export default function App() {
  const [lists, setLists] = useState([]);  
  
  const [purchases, setPurchases] = useState([])

  const [curList, setCurList] = useState(undefined);
  const [isCreating, setIsCreating] = useState(false);
  const [newListName, setNewListName] = useState("");
  const [userName, setUserName] = useState("");

  const [isAddingPurchase, setIsAddingPurchase] = useState(false);
  const [newPurchaseName, setNewPurchaseName] = useState("");
  const [newPurchaseCount, setNewPurchaseCount] = useState("");
  const [newPurchaseType, setNewPurchaseType] = useState(0);

  const [newShare, setNewShare] = useState(null);

  const handleCreateList = () => {
    setIsCreating(true);
  };

  const handleSaveList = async () => {
    if (!newListName.trim()) return;

    const newList = { name: newListName };
    try {
      const response = await fetch("/api/list", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(newList),
      });

      if (!response.ok) {
        throw new Error("Ошибка сохранения списка");
      }

      const savedList = await response.json();
      setLists((prev) => [...prev, savedList]);
      setNewListName("");
      setIsCreating(false);
    } catch (error) {
      console.error("Ошибка:", error);
    }
  };

  const handleLogout = async () => {
    try {
      const response = await fetch("/logout");

      if (response.ok) {
        window.location.href = "/"
      } else {
        console.error("Ошибка при выходе");
      }
    } catch (error) {
      console.error("Произошла ошибка:", error);
    }
  };

  const handleDeleteList = async () => {
    const deleteList = { id: curList.id };
    try {
      const response = await fetch("/api/list", {
        method: "Delete",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(deleteList),
      });

      if (!response.ok) {
        throw new Error("Ошибка сохранения списка");
      }
      
      const deletedList = await response.json();
      setLists((prev) => prev.filter((list) => list.id !== deletedList.id));
      setCurList(undefined)
    } catch (error) {
      console.error("Ошибка:", error);
    }
  };

  const handleSavePurchase = async() => {
    if (!newPurchaseName.trim() || !curList) return;

    const newPurchase = {
      name: newPurchaseName,
      count: newPurchaseCount,
      type: newPurchaseType,
      listId: curList.id
    };

    try {
      const response = await fetch("/api/purchase", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(newPurchase),
      });

      if (!response.ok) {
        throw new Error("Ошибка сохранения списка");
      }

      const savedPurchase = await response.json();
      setPurchases((prev) => [...prev, savedPurchase]);
      setNewPurchaseName("");
      setNewPurchaseCount("");
      setNewPurchaseType(0);
      setIsAddingPurchase(false);

    } catch (error) {
      console.error("Ошибка:", error);
    }
  };

  useEffect(async () => {
    const response1 = await fetch("/api/purchases");

    if (!response1.ok) {
      throw new Error("Ошибка покупок");
    }

    const purchases = await response1.json();
    setPurchases(purchases)
  }, [])

  useEffect(async () => {
    const response2 = await fetch("/api/lists");

    if (!response2.ok) {
      throw new Error("Ошибка списков");
    }

    const lists = await response2.json();
    setLists(lists)
  }, [])

  useEffect(async () => {
    const response3 = await fetch("/username");

    if (!response3.ok) {
      throw new Error("Ошибка имени");
    }

    const user = await response3.json();
    setUserName(user.name)
  }, [])

  useEffect(() => {
    console.log(curList)
  }, [curList])

  const value = {
    0: "шт.",
    1: "гр",
    2: "мл"
  }

  const handleCopy = () => {
    const textToCopy = `${window.location.protocol}//${window.location.host}/getshare?listId=` + curList.id;
    setNewShare(textToCopy)
  };

  return (
    <div className="app">
      <aside className="sidebar">
        <div className="account">
          <h2 className="user-name">{userName}</h2>
          <img src={logoutImg} alt="выйти" onClick={handleLogout}></img>
        </div>
        <h2 className='label-lists'>Мои списки</h2>
        <ul>
          {lists.map((list) => (
              <li 
                key={list.id} 
                className="list-item" 
                onClick={() => setCurList(list)}
              >
                <p>#</p><a>{list.name}</a>
              </li>
            ))}
        </ul>

        <p className="new-list" onClick={handleCreateList}>
          <img src={plusImg} alt="плюс" /> Создать новый список
        </p>

        {isCreating && (
          <div className="new-list-form">
            <input
              type="text"
              placeholder="Введите название списка"
              name="listName"
              value={newListName}
              onChange={(e) => setNewListName(e.target.value)}
            />
            <button onClick={handleSaveList}>Сохранить</button>
          </div>
        )}
      </aside>
      <main className="main-content">
        <h2>Мои списки /</h2>
        {curList && (
          <div className="cont">
            <div className="list-name">
              <h3>{curList.name}</h3>
              <img src={deleteImg} alt="удаление" onClick={handleDeleteList}></img>
            </div>

            <p className="new-purchase" onClick={() => setIsAddingPurchase(true)}>
              <img src={plusImg} alt="плюс" /> Добавить новый товар
            </p>

            {isAddingPurchase && (
              <div className="new-purchase-form">
                <input
                  type="text"
                  placeholder="Название товара"
                  value={newPurchaseName}
                  onChange={(e) => setNewPurchaseName(e.target.value)}
                />
                <input
                  type="text"
                  placeholder="Количество"
                  value={newPurchaseCount}
                  onChange={(e) => setNewPurchaseCount(Number(e.target.value))}
                />
                <select
                  value={newPurchaseType}
                  onChange={(e) => setNewPurchaseType(Number(e.target.value))}
                >
                  <option value={0}>Количество</option>
                  <option value={1}>Вес</option>
                  <option value={1}>Объём</option>
                </select>
                <button onClick={handleSavePurchase}>Сохранить</button>
              </div>
            )}

            <ul className="purchases-list">
              {purchases
                .filter((purchase) => purchase.listId === curList.id)
                .map((purchase) => (
                  <li
                    key={purchase.id} 
                    className="purchases-list-item"
                  >
                    <p>{purchase.name}</p>
                    <p>{purchase.count} {value[purchase.type]}</p>
                  </li>
                ))}
            </ul>
          </div>
        )}
      </main>
      <p className="share-list" onClick={handleCopy}>Поделиться</p>
      <p>{newShare}</p>
    </div>
  )
}
