module Vizsga220110 where
    import Data.Char
    import Data.List

    squareSum :: Num a => (a, a) -> (a, a, a)
    squareSum (x,y) = (x, y, x^2 + y^2)

    names :: [String] -> [String] -> [String]
    names vezetek kereszt = zipWith (\vez ker -> vez ++ " " ++ ker) vezetek kereszt

    triangleArea :: (Double, Double, Double) -> Maybe Double
    triangleArea (x, y, z)
        | x > 0 && y > 0 && z > 0 && x + y > z && x + z > y && y + z > x && x^2 + y^2 == z^2 = Just ((x * y) / 2)
        | otherwise = Nothing

    doubleIdxs:: Eq a  => [(a,a)] -> Maybe [Int]
    doubleIdxs [] = Nothing
    doubleIdxs zs
        | null idxs = Nothing
        | otherwise = Just idxs where
            idxs = helper zs 1
            helper [] _ = []
            helper ((x,y):zs) i
                | x == y = i : helper zs (i + 1)
                | otherwise = helper zs (i + 1)

    snakeToCamel :: String -> String
    snakeToCamel "" = ""
    snakeToCamel ('_':y:zs) = toUpper y : snakeToCamel zs
    snakeToCamel (x:zs) = x : snakeToCamel zs

    removeExtremes :: Ord a => [a] -> [a]
    removeExtremes [] = []
    removeExtremes list = filter (\x -> x /= minval && x /= maxval) list where
        minval = minimum list
        maxval = maximum list

    replaceLastOcc :: Eq a => a {-mit-} -> a {-mire-} -> [a] -> Maybe [a]
    replaceLastOcc x y list
        | reverse l == list = Nothing
        | otherwise = Just (reverse l) where
            l = helper x y (reverse list)
            helper _ _ [] = []
            helper x y (z:zs)
                | x == z = (y:zs)
                | otherwise = z : helper x y zs

    anagram :: String -> String -> Bool
    anagram s1 s2 = sort s1 == sort s2

    sumWithLenghtN :: Num a => Int -> [[a]] -> a
    sumWithLenghtN _ [] = 0
    sumWithLenghtN a ((x:xs):ys)
        | length (x:xs) == a = helper (x:xs)
        | otherwise = sumWithLenghtN a ys where
            helper [] = 0
            helper (x:xs) = x + helper xs

    isSteady :: Eq b => (a -> b) -> [a] -> Bool
    isSteady _ [] = True
    isSteady f (x:y:xs)
        | f x == f y = isSteady f (y:xs)
        | otherwise = False
    isSteady f (x:xs) = True

    data Parcell = P String Double Int
        deriving (Show, Eq)

    deliveryFee :: Parcell -> Maybe Double
    deliveryFee (P cim suly utanvet)
        | cim == "Asgard" = Just (suly * 100)
        | cim == "Midgard" = Just (suly * 10)
        | cim == "Vanaheim" = Just (suly * 80)
        | cim == "Alfheim" = Just (suly * 50)
        | otherwise = Nothing

    delivery :: [Parcell] -> Double
    delivery [] = 0
    delivery ((P cim suly utanvet) : xs) = helper ((P cim suly utanvet) : xs) 0 where
        helper [] total = total
        helper ((P cim suly utanvet) : xs) total
            | cim == "Asgard" = helper xs (total + (suly * 100 + fromIntegral utanvet))
            | cim == "Midgard" = helper xs (total + (suly * 10 + fromIntegral utanvet))
            | cim == "Vanaheim" = helper xs (total + (suly * 80 + fromIntegral utanvet))
            | cim == "Alfheim" = helper xs (total + (suly * 50 + fromIntegral utanvet))
            | otherwise = helper xs total

    -- inconsistencyInGrowing :: [[a]] -> Maybe (Int, Int)
    -- inconsistencyInGrowing [] = Nothing
    -- inconsistencyInGrowing [_] = Nothing
    -- inconsistencyInGrowing ((x:xs):(y:ys):zs) = helper ((x:xs):(y:ys):zs) 0 where
    --     helper [] _ = Nothing
    --     helper [_] _ = Nothing
    --     helper (x:y:zs) i
    --         | isShorter x y = helper (y:zs) (i + 1)
    --         | otherwise = Just (i, i+1)

    -- isShorter :: [a] -> [a] -> Bool
    -- isShorter [] _ = True
    -- isShorter _ [] = False
    -- isShorter (_:xs) (_:ys) = isShorter xs ys

    inconsistencyInGrowing :: [[a]] -> Maybe (Int, Int)
    inconsistencyInGrowing xs = helper xs 0
        where
            helper [] _ = Nothing
            helper [_] _ = Nothing
            helper (x:y:zs) i
                | isShorter x y = helper (y:zs) (i + 1)
                | otherwise = Just (i, i + 1)

    isShorter :: [a] -> [a] -> Bool
    isShorter [] [] = False           -- Ha mindkét lista üres, nem rövidebb
    isShorter [] _  = True            -- Az első lista üres, tehát rövidebb
    isShorter _  [] = False           -- A második lista üres, tehát nem hosszabb
    isShorter (_:xs) (_:ys) = isShorter xs ys -- Rekurzív összehasonlítás

    findAndReplace :: String -> String -> String -> String
    findAndReplace [] _ _ = []
    findAndReplace text@(x:xs) pattern replacement
        | take (length pattern) text == pattern = replacement ++ findAndReplace (drop (length pattern) text) pattern replacement
        | otherwise = x : findAndReplace xs pattern replacement

    shrinkText :: String -> String
    shrinkText "" = ""
    shrinkText (x:xs)
        | x == '(' && null (filter (== ')') (x:xs)) = (x:xs)
        | x == '(' = shrinkText (drop (zarojel (x:xs) 0) (x:xs))
        | otherwise = x : shrinkText xs where
            zarojel [] i = i
            zarojel (x:xs) i
                | x /= ')' = zarojel xs (i + 1)
                | otherwise = i + 1

    -- shrinkText :: String -> String
    -- shrinkText "" = ""
    -- shrinkText text = shrink text 0 where
    --     shrink [] _ = []
    --     shrink (x:xs) 0
    --         | x == '(' = shrink xs 1 -- Belépünk egy zárójelbe
    --         | otherwise = x : shrink xs 0 -- Normál karakter
    --     shrink (x:xs) depth
    --         | x == '(' = shrink xs (depth + 1) -- Újabb zárójel
    --         | x == ')' = shrink xs (depth - 1) -- Zárójel lezárása
    --         | otherwise = shrink xs depth -- Minden más karakter kihagyása
