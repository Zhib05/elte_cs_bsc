module Gyak3 where


    fst' :: (a, b) -> a
    fst' (x,y) = x

    snd' :: (a,b) -> b
    snd' (x,y) = y

    -- Parametrikusan polimorf függvény: Olyan függvény, amely működése független a bemeneti paraméter tipusátol.

    -- fromIntegral :: (Integral a, Num b) => a -> b

    add :: Num a => a -> a -> a
    add x y = x + y

    -- (==) :: Eq a => a -> a -> Bool

    isLarger :: (Ord a) => a -> a -> Bool
    isLarger x y = x > y

    -- Listák 
    -- homogén adatszerkezet
    -- pl. [1,2,3,4,5] == 1 : 2 : 3 : 4 : 5 : []
    -- ['a', 'b', 'c']
    -- [True,True,Bool]
    -- "Str" == 'S' : 't' : 'r' : []

    -- [True, 5, 'a']

    null' :: [a] -> Bool
    null' [] = True
    null' _  = False

    head' :: [a] -> a
    head' (x:xs) = x
    --     a bele van füzve valamilyen a listába

    --  tail' :: [a] -> [a]
    --  tail' (x:xs) = xs

    remove2nd :: [a] -> [a]
    remove2nd (z:_:x:xs) = (z:x:xs)
    remove2nd list@(z:y:xs) = list      -- @ névadás

    -- {x^2 | x eleme [1,10] }

    squares :: [Integer]
    squares = [x^2 | x <- [1..10] ]

    evens :: [Integer]
    evens = [ x | x <- [4,8,7,2], even x ]