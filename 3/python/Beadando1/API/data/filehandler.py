import json
from typing import Dict, Any, List
from data.filereader import load_json, get_basket_by_user_id, get_all_users, get_user_by_id


'''
Útmutató a fájl függvényeinek a használatához

Új felhasználó hozzáadása:

new_user = {
    "id": 4,  # Egyedi felhasználó azonosító
    "name": "Szilvás Szabolcs",
    "email": "szabolcs@plumworld.com"
}

Felhasználó hozzáadása a JSON fájlhoz:

add_user(new_user)

Hozzáadunk egy új kosarat egy meglévő felhasználóhoz:

new_basket = {
    "id": 104,  # Egyedi kosár azonosító
    "user_id": 2,  # Az a felhasználó, akihez a kosár tartozik
    "items": []  # Kezdetben üres kosár
}

add_basket(new_basket)

Új termék hozzáadása egy felhasználó kosarához:

user_id = 2
new_item = {
    "item_id": 205,
    "name": "Szilva",
    "brand": "Stanley",
    "price": 7.99,
    "quantity": 3
}

Termék hozzáadása a kosárhoz:

add_item_to_basket(user_id, new_item)

Hogyan használd a fájlt?

Importáld a függvényeket a filehandler.py modulból:

from filehandler import (
    add_user,
    add_basket,
    add_item_to_basket,
)

 - Hiba esetén ValuErrort kell dobni, lehetőség szerint ezt a 
   kliens oldalon is jelezni kell.

'''

# A JSON fájl elérési útja
JSON_FILE_PATH = "C:\\Zhibo\\ELTE_IK\\3\\python\\Beadando1\\API\\data\\data.json"

def load_json() -> Dict[str, Any]:
    with open(JSON_FILE_PATH, "r", encoding="utf-8") as file:
        data = json.load(file)
        return data

def save_json(data: Dict[str, Any]) -> None:
    with open(JSON_FILE_PATH, "w") as file:
        json.dump(data, file, indent=4)

def add_user(user: Dict[str, Any]) -> None:
    data = load_json()
    new_id = max(u["id"] for u in get_all_users()) + 1
    new_user = {
        "id": new_id,
        "name": user["name"],
        "email": user["email"]
    }
    data["Users"].append(new_user)
    save_json(data)

def add_basket(basket: Dict[str, Any]) -> None:
    data = load_json()
    if basket["user_id"] not in [u["id"] for u in data["Users"]]:
        raise ValueError("User with given ID does not exist.")
    for b in data["Baskets"]:
        if b["user_id"] == basket["user_id"]:
            raise ValueError("Basket for this user already exists.")
    new_id = max(b["id"] for b in data["Baskets"]) + 1
    new_basket = {
        "id": new_id,
        "user_id": basket["user_id"],
        "items": []
    }
    data["Baskets"].append(new_basket)
    save_json(data)

def add_item_to_basket(user_id: int, item: Dict[str, Any]) -> None:
    data = load_json()
    user_ids = [b["user_id"] for b in data["Baskets"]]
    if user_id not in user_ids:
        raise ValueError("Basket not found for user with given ID!")
    for basket in data["Baskets"]:
        if basket["user_id"] == user_id:
            items = [i for i in basket["items"]]
            if len(items) == 0:
                new_item_id = 201
            else:
                new_item_id = max(i["item_id"] for i in basket["items"]) + 1
            new_item = {
                "item_id": new_item_id,
                "name": item["name"],
                "brand": item["brand"],
                "price": item["price"],
                "quantity": item["quantity"]
            }
            basket["items"].append(new_item)
            save_json(data)