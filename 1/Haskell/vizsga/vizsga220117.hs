module Vizsga220117 where
    import Data.Char
    import Data.Maybe

    concatTripleString :: ([Char], [Char], [Char]) -> [Char]
    concatTripleString ("", "", "") = ""
    concatTripleString (a, b, c) = a ++ b ++ c

    mods :: Integral a => a -> a -> Maybe (a, a)
    mods 0 _ = Nothing
    mods _ 0 = Nothing
    mods a b = Just (a `mod` b, b `mod` a)

    dropEmpties :: Eq a => [[a]] -> [[a]]
    dropEmpties [] = []
    dropEmpties (x:xs)
        | null x = dropEmpties xs
        | otherwise = x : dropEmpties xs

    createChain :: Integer -> String
    createChain a 
        | a <= 0 = ""
        | otherwise = helper a 1 where
            helper a i
                | i <= a = "(" ++ show i ++ ")" ++ helper a (i + 1)
                | otherwise = []

    aLtErNaTiNgCaPs :: String -> String
    aLtErNaTiNgCaPs "" = ""
    aLtErNaTiNgCaPs list = helper list 1 where
        helper "" _ = ""
        helper (x:xs) i
            | odd i = (toLower x) : helper xs (i + 1)
            | otherwise = (toUpper x) : helper xs (i + 1)

    result :: [Maybe Bool] -> Int -> Bool
    result list a = a <= helper list a 0 where
        helper [] _ s = s
        helper (Just True:xs) a s = helper xs a (s + 1)
        helper (Just False:xs) a s = helper xs a (s - 1)
        helper (Nothing:xs) a s = helper xs a s

    maximumIF :: Ord a => (a -> Bool) -> [a] -> Maybe a
    maximumIF _ [] = Nothing
    maximumIF f list 
        | not (null (filter f list)) = Just (maximum $ filter f list)
        | otherwise = Nothing

    fillBlanks :: String -> String -> String
    fillBlanks "" "" = ""
    fillBlanks "" _ = ""
    fillBlanks a "" = a
    fillBlanks ('_':xs) (y:ys) = y : fillBlanks xs ys
    fillBlanks (x:xs) (y:ys) = x : fillBlanks xs (y:ys)

    riffleShuffle :: [a] -> [a]
    riffleShuffle [] = []
    riffleShuffle list = helper (take ((length list) `div` 2) list) (drop ((length list) - (fromIntegral (ceiling (realToFrac (length list) / 2)))) list) where
        helper [] [] = []
        helper [] [y] = [y]
        helper (x:xs) (y:ys) = x : y : helper xs ys

    getPositions :: Eq a => a -> [a] -> Maybe [Int]
    getPositions _ [] = Nothing
    getPositions a (x:xs)
        | not (null (helper a (x:xs) 1)) = Just (helper a (x:xs) 1)
        | otherwise = Nothing where
            helper _ [] _ = []
            helper a (x:xs) i
                | a == x = i : helper a xs (i + 1)
                | otherwise = helper a xs (i + 1)

    applies :: [a -> b] -> [a] -> [b]
    applies _ [] = []
    applies [] _ = []
    applies (f:fs) list = map f list ++ applies fs list

    data FiniteList = Empty | NonEmpty Int [Integer]
        deriving (Show, Eq)

    toFinite :: Int -> [Integer] -> FiniteList
    toFinite _ [] = Empty
    toFinite a list
        | a <= 0 = Empty
        | otherwise = NonEmpty (length (hossz a list)) (take a list) where
            hossz _ [] = []
            hossz a list = take a list

    concatFL :: [FiniteList] -> FiniteList
    concatFL [] = Empty
    concatFL l
        | null (list l) = Empty
        | otherwise = conc (list l) where
            list [] = []
            list (Empty:xs) = list xs
            list (NonEmpty a b:xs) = (NonEmpty a b) : list xs
            conc [NonEmpty a b] = NonEmpty a b
            conc (NonEmpty a b : NonEmpty c d : xs) = conc ((NonEmpty (a + c) (b ++ d)):xs)
    
    difference :: String -> String -> Maybe String
    difference [] [] = Nothing
    difference [] _ = Nothing
    difference a [] = Just a
    difference (x:xs) (y:ys)
        | null (helper (x:xs) (y:ys)) = Nothing
        | otherwise = Just (helper (x:xs) (y:ys)) where
            helper [] _ = []
            helper a [] = a
            helper (x:xs) (y:ys)
                | x == y = helper xs ys
                | otherwise = x : helper xs ys

    filterByMajority :: [(a -> Bool)] -> [a] -> [a]
    filterByMajority [] [] = []
    filterByMajority _ []  = []
    filterByMajority [] a  = a
    filterByMajority (f:fs) (x:xs) 
        | helper (f:fs) x > ((length (f:fs)) `div` 2) = x : filterByMajority (f:fs) xs
        | otherwise = filterByMajority (f:fs) xs where
            helper [] _ = 0
            helper (f:fs) x
                | f x = 1 + helper fs x
                | otherwise = helper fs x