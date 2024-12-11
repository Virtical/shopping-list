import { useState, useEffect } from 'react';
import './App.css'

export default function App() {

  const[purchases, setPurchases] = useState([])

  useEffect(() => {
    async function fetchPurchases() {
      const currentQuery = window.location.search;
      try {
        const response = await fetch("api/getshare" + currentQuery);
        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const result = await response.json();
        setPurchases(result);
      } catch (err) {
        console.log(err.message);
      }
    }

    fetchPurchases();
  }, []);

  const typeLabels = {
    0: "шт.",
    1: "гр",
    2: "мл"
  }

  return (
    <ul className="purchases-list">
          {purchases.map((purchase, index) => (
            <li key={index} className="purchase-item">
              <span className="purchase-name">{purchase.name}</span>
              <span className="purchase-count">{purchase.count}</span>
              <span className="purchase-type">{typeLabels[purchase.type]}</span>
            </li>
          ))}
    </ul>
  )
}