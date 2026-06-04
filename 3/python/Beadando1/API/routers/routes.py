from schemas.schema import User, Basket, Item
from fastapi.responses import JSONResponse, RedirectResponse
from fastapi import FastAPI, HTTPException, Request, Response, Cookie
from fastapi import APIRouter
from typing import Dict, Any, List

from data.filehandler import (
    add_user,
    add_basket,
    add_item_to_basket,
    load_json,
    save_json,
)

from data.filereader import (
    get_user_by_id,
    get_basket_by_user_id,
    get_all_users,
    get_total_price_of_basket,
)

'''

Útmutató a fájl használatához:

- Minden route esetén adjuk meg a response_modell értékét (típus)
- Ügyeljünk a típusok megadására
- A függvények visszatérési értéke JSONResponse() legyen
- Minden függvény tartalmazzon hibakezelést, hiba esetén dobjon egy HTTPException-t
- Az adatokat a data.json fájlba kell menteni.
- A HTTP válaszok minden esetben tartalmazzák a 
  megfelelő Státus Code-ot, pl 404 - Not found, vagy 200 - OK

'''

routers = APIRouter()

@routers.post('/adduser', response_model=User)
def adduser(user: User) -> User:
    try:
        user_dict = user.model_dump()
        data = load_json()
        user_ids = [u["id"] for u in data["Users"]]
        new_id = max(user_ids, default=0) + 1
        user_dict["id"] = new_id
        add_user(user_dict)
        return JSONResponse(content=user_dict)
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))

@routers.post('/addshoppingbag', response_model=str)
def addshoppingbag(userid: int) -> str:
    try:
        data = load_json()
        new_id = max((b["id"] for b in data["Baskets"]), default=100) + 1
        new_basket = {"id": new_id, "user_id": userid, "items": []}
        add_basket(new_basket)
        return JSONResponse(content="Kosár sikeresen létrehozva.", status_code=201)
    except Exception as e:
        raise HTTPException(status_code=409, detail=str(e))

@routers.post('/additem', response_model=Basket)
def additem(userid: int, item: Item) -> Basket:
    try:
        add_item_to_basket(userid, item.model_dump())
        data = load_json()
        for basket in data["Baskets"]:
            if basket["user_id"] == userid:
                return JSONResponse(content=basket)
        raise ValueError("Basket not found after adding item.")
    except ValueError as e:
        raise HTTPException(status_code=400, detail=str(e))

@routers.put('/updateitem', response_model=Basket)
def updateitem(userid: int, itemid: int, updateItem: Item) -> Basket:
    try:
        data = load_json()
        for basket in data["Baskets"]:
            if basket["user_id"] == userid:
                for item in basket["items"]:
                    if item["item_id"] == itemid:
                        item["name"] = updateItem.name
                        item["brand"] = updateItem.brand
                        item["price"] = updateItem.price
                        item["quantity"] = updateItem.quantity
                        save_json(data)
                        return JSONResponse(content=basket)
                raise ValueError("Item not found.")
        raise ValueError("Basket not found.")
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

@routers.delete('/deleteitem', response_model=Basket)
def deleteitem(userid: int, itemid: int) -> Basket:
    try:
        data = load_json()
        for basket in data["Baskets"]:
            if basket["user_id"] == userid:
                basket["items"] = [i for i in basket["items"] if i["item_id"] != itemid]
                save_json(data)
                return JSONResponse(content=basket)
        raise ValueError("Basket not found.")
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

@routers.delete('/deleteuser', response_model=str)
def deleteuser(userid: int) -> str:
    try:
        data = load_json()
        data["Users"] = [u for u in data["Users"] if u["id"] != userid]
        data["Baskets"] = [b for b in data["Baskets"] if b["user_id"] != userid]
        save_json(data)
        return JSONResponse(content="User and associated basket deleted successfully.")
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

@routers.get('/user', response_model=User)
def user(userid: int) -> User:
    try:
        user_data = get_user_by_id(userid)
        return JSONResponse(content=user_data)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))

@routers.get('/users', response_model=User)
def users() -> list[User]:
    all_users = get_all_users()
    return JSONResponse(content=all_users)

@routers.get('/shoppingbag', response_model=Item)
def shoppingbag(userid: int) -> list[Item]:
    try:
        items = get_basket_by_user_id(userid)
        return JSONResponse(content=items)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))

@routers.get('/getusertotal', response_model=float)
def getusertotal(userid: int) -> float:
    try:
        total = get_total_price_of_basket(userid)
        return JSONResponse(content=total)
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))