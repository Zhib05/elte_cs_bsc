from pydantic import BaseModel, Field, EmailStr, field_validator
from typing import List, Optional

'''

Útmutató a fájl használatához:

Az osztályokat a schema alapján ki kell dolgozni.

A schema.py az adatok küldésére és fogadására készített osztályokat tartalmazza.
Az osztályokban az adatok legyenek validálva.
 - az int adatok nem lehetnek negatívak.
 - az email mező csak e-mail formátumot fogadhat el.
 - Hiba esetén ValuErrort kell dobni, lehetőség szerint ezt a 
   kliens oldalon is jelezni kell.

'''

ShopName='Bolt'

class Item(BaseModel):
    item_id : int = Field(..., gt=0, description="Unique id of the item")
    name : str = Field(..., min_length=3, max_length=100, description="Name of the item")
    brand : str = Field(..., min_length=2, max_length=50, description="Brand of the item")
    price : float = Field(..., gt=0, description="Price of the item")
    quantity : int = Field(..., gt=0, description="Quantity of the item")

class Basket(BaseModel):
    id : int = Field(..., gt=0, description="Unique id of the basket")
    user_id : int = Field(..., gt=0, description="Unique id of the user")
    items : List[Item] = Field(default_factory=list, description="List of items in the basket")

class User(BaseModel):
    id: int = Field(..., gt=0, description="Unique identifier for the user")
    name: str = Field(..., min_length=3, max_length=100, description="Name of the user")
    email: EmailStr = Field(..., description="Email address of the user")