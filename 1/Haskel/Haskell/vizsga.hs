module Vizsga where
    import Data.List

    points :: Integral a => [(String, a, a)] -> [(String, a)]
    points [] = []
    points ((nev, ido, hiba):xs)
        | hiba >= 100 = points xs
        | score <= 0 = points xs
        | otherwise = (nev, score) : points xs where
            score = 100 - (ido `div` 2) - hiba

    type Apple = (Bool, Int)
    type Tree = [Apple]
    type Garden = [Tree]

    ryuksApples :: Garden -> Int
    ryuksApples [] = 0
    ryuksApples ([]:ys) = ryuksApples ys
    ryuksApples (((x,y):xs):ys)
        | x && y <= 3 = 1 + ryuksApples (xs:ys)
        | otherwise = ryuksApples (xs:ys)

    doesContain :: String -> String -> Bool
    doesContain [] [] = True
    doesContain [] _  = True
    doesContain _ []  = False
    doesContain (x:xs) (y:ys)
        | x == y = doesContain xs ys
        | otherwise = doesContain (x:xs) ys

    barbie :: [String] -> String
    barbie [] = "farmer"
    barbie ["rozsaszin"] = "rozsaszin"
    barbie (x:xs) = helper (x:xs) 1 where
        helper [] _ = "farmer"
        helper (x:xs) i
            | x == "rozsaszin" = x
            | x /= "fekete" && (i `mod` 2) == 0 = x
            | otherwise = helper xs (i + 1)

    firstValid :: [a -> Bool] -> a -> Maybe Int
    firstValid [] _ = Nothing
    firstValid (x:xs) a = helper (x:xs) a 0 where
        helper [] _ _ = Nothing
        helper (x:xs) a i
            | x a = Just i
            | otherwise = helper xs a (i + 1)

    combineListsIf :: (a -> b -> Bool) -> (a -> b -> c) -> [a] -> [b] -> [c]
    combineListsIf _ _ [] _ = []
    combineListsIf _ _ _ [] = []
    combineListsIf pre f (x:xs) (y:ys)
        | pre x y = f x y : combineListsIf pre f xs ys
        | otherwise = combineListsIf pre f xs ys
    
    data Line = Tram Integer [String] | Bus Integer [String]
        deriving (Show, Eq)

    whichBusStop :: String -> [Line] -> [Integer]
    whichBusStop _ [] = []
    whichBusStop megallo ((Tram _ _ ):xs) = whichBusStop megallo xs
    whichBusStop megallo ((Bus a stops):ys)
        | megallo `elem` stops = a : whichBusStop megallo ys
        | otherwise = whichBusStop megallo ys

    isReservable :: Int -> String -> Bool
    isReservable 0 _ = True
    isReservable _ "" = False
    isReservable n list
        | replicate n 'x' `isPrefixOf` list = True
        | otherwise = isReservable n (tail list)

    unweirdNumber :: [(Char, Int)] -> String -> Int
    unweirdNumber mapping text = sum $ map lookupValue text
        where
            lookupValue c = maybe 0 id (lookup c mapping)

    discount :: [(Double, Double)] -> Double
    discount [] = 0
    discount ((price, disc):rest)
        | disc == 0 = (price * 0.95) + discount rest
        | otherwise = (price * ((100 - disc) / 100)) + discount rest