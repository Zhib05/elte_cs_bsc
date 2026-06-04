module Vizsga211213 where
    import Data.Char

    f5 :: Integral a => a -> a
    f5 n = n `mod` 5

    matchingArgs :: Eq a => a -> a -> a -> Bool
    matchingArgs a b c
        | (a == b) || (a == c) || (b == c) || (a == b && b == c) = True
        | otherwise = False

    division :: Integral a => (a, a, a) -> Maybe a
    division (a, b, c)
        | (c == 0) || (b `mod` c == 0) = Nothing
        | otherwise = Just (a `div` (b `mod` c))

    elemOnEvenIdx :: Eq a => a -> [a] -> Bool
    elemOnEvenIdx _ [] = False
    elemOnEvenIdx a (x:xs) = (not . null) (helper a (x:xs) 1) where
        helper _ [] _ = []
        helper a (x:xs) i
            | (a == x) && (i `mod` 2 == 0) = [x]
            | otherwise = helper a xs (i + 1)

    dropEveryNth :: Int -> [a] -> [a]
    dropEveryNth a (x:xs) = helper a (x:xs) 1 where
        helper _ [] _ = []
        helper a (x:xs) i
            | i `mod` a == 0 = helper a (drop 1 (x:xs)) (i + 1)
            | otherwise = x : helper a xs (i + 1)

    simDiff :: Eq a => [a] -> [a] -> [a]
    simDiff [] [] = []
    simDiff [] a  = a
    simDiff a []  = a
    simDiff (x:xs) (y:ys)
        | x `elem` (y:ys) = simDiff xs (filter (/= x) (y:ys))
        | otherwise = x : simDiff xs (y:ys)

    parseNum :: String -> Maybe Integer
    parseNum "" = Nothing
    parseNum "+" = Nothing
    parseNum "-" = Nothing
    parseNum (x:xs)
        | (isDigit x) && (length (helper (x:xs)) == 0) = Just (read (x:xs))
        | (x == '+') && (length (helper (x:xs)) == 1) = Just (read (xs))
        | (x == '-') && (length (helper (x:xs)) == 1) = Just (read (x:xs))
        | otherwise = Nothing where
            helper "" = []
            helper (x:xs)
                | isDigit x = helper xs
                | otherwise = x : helper xs

    elevate :: Eq a => a -> [a] -> [a]
    elevate _ [] = []
    elevate a (x:xs)
        | not (a `elem` (x:xs)) = (x:xs)
        | otherwise = a : ys where
            ys = helper a (x:xs) where
                helper a [] = []
                helper a (x:xs)
                    | a == x = xs
                    | otherwise = x : helper a xs

    localMax :: Ord b => [(a -> b)]{- nem üres -} -> a -> b
    localMax [f] a = f a
    localMax (f1:f2:fs) a
        | f1 a > f2 a = f1 a
        | otherwise = localMax (f2:fs) a

    pairMap :: (a -> b) -> [(a,a)] -> [(b,b)]
    pairMap f [] = []
    pairMap f ((x,y):xs) = (f x, f y) : pairMap f xs

    applyIfReduces :: Ord a => (a -> a) -> [a] -> [a]
    applyIfReduces f [] = []
    applyIfReduces f (x:xs)
        | f x < x = f x : applyIfReduces f xs
        | otherwise = x : applyIfReduces f xs

    data Plant = Flower String Int | Tree String Int
        deriving (Show, Eq)

    survive :: [Plant] -> Int -> [String]
    survive [] _ = []
    survive (Flower nev viz:xs) csap
        | csap >= viz = nev : survive xs csap
        | otherwise = survive xs csap
    survive (Tree _ _:xs) csap = survive xs csap

    avgTreeWater :: [Plant] -> Maybe Double
    avgTreeWater [] = Nothing
    avgTreeWater list
        | null (l list) = Nothing
        | otherwise = Just ((fromIntegral (sum (l list))) / fromIntegral (length (l list))) where
            l [] = []
            l (Tree _ viz:xs) = viz : l xs
            l (Flower _ _:xs) = l xs

    reverseWordsInside :: String -> String
    reverseWordsInside "" = ""
    reverseWordsInside list = unwords (helper (words list) 1 (length (words list))) where
        helper [] _ _ = []
        helper (x:xs) i l
            | i == 1 || i == l = x : helper xs (i + 1) l
            | otherwise = reverse x : helper xs (i + 1) l

    strangePow :: [Int] -> [Int]
    strangePow [] = []
    strangePow [x] = [x]
    strangePow [x,y] = [x,y]
    strangePow (x:y:z:xs) = x ^ z : strangePow (y:z:xs)