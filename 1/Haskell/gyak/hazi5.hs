module Hazi5 where

    import Data.List
    import Data.Char (toUpper)

    toUpperThird :: String -> String
    toUpperThird (x:y:z:zs) = x : y : toUpper z : zs
    toUpperThird x = x

    isSorted :: Ord a => [a] -> Bool
    isSorted (x:y:xs) = x <= y && isSorted (y:xs)
    isSorted _ = True

    (!!!) :: Integral b => [a] -> b -> a
    (!!!) (x:xs) k
        | k == 0 = x
        | k > 0 = (!!!) xs (k-1)
        | otherwise = reverse (x:xs) !!! ((k+1) * (-1))
    
    format :: Integral b => b -> String -> String
    format i x
        | i == 0 = x
        | i < 0 = x
    format a [] = ' ' : format (a - 1) []
    format a (x:xs) = x : format (a - 1) xs

    mightyGale :: (Num a, Ord b, Num b, Integral c) => [(String, a, b, c)] -> String
    mightyGale [] = ""
    mightyGale ((x,_,z,_):xs)
        | z > 110 = x
        | z <= 110 = mightyGale xs

    doubleElements :: [a] -> [a]
    doubleElements [] = []
    doubleElements (x:xs) = x : x : doubleElements xs