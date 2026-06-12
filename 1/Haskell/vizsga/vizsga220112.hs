module Vizsga220112 where
    import Data.Char
    import Data.Maybe
    import Data.List

    threeDivs:: Integral a => (a,a) -> (a,a) -> (a,a) -> Maybe a
    threeDivs (x, xs) (y, ys) (z, zs)
        | xs /= 0 && ys /= 0 && zs /= 0 = Just (sum [x `div` xs, y `div` ys, z `div` zs])
        | otherwise = Nothing

    howManyDifferences:: Eq a => [(a,a)] -> Int
    howManyDifferences [] = 0
    howManyDifferences ((x,y):xs)
        | x /= y = 1 + howManyDifferences xs
        | otherwise = 0 + howManyDifferences xs

    getDigitsFromCode :: String -> String
    getDigitsFromCode list@(x:xs) = takeWhile isDigit list

    isTriangularNumber :: Integral a => a -> Bool
    isTriangularNumber 0 = False
    isTriangularNumber a = helper a 0 1 where
        helper a e i
            | e == a = True
            | a /= e && e < a = helper a (e + i) (i + 1)
            | e > a = False

    smallestInSize :: [a] -> [b] -> [c] -> Maybe Int
    smallestInSize [] [] [] = Nothing
    smallestInSize [] [] _ = Nothing
    smallestInSize [] _ [] = Nothing
    smallestInSize _ [] [] = Nothing
    smallestInSize [] _ _ = Just 1
    smallestInSize _ [] _ = Just 2
    smallestInSize _ _ [] = Just 3
    smallestInSize (_:xs) (_:ys) (_:zs) = smallestInSize xs ys zs

    reverseWords :: Integral a => String -> [a] -> String
    reverseWords "" _ = ""
    reverseWords list ilist = unwords $ helper (words list) ilist 1 where
        helper [] _ _ = []
        helper (x:xs) (y:ys) i
            | y == i = reverse x : helper xs ys (i + 1)
            | otherwise = x : helper xs (y:ys) (i + 1)

    sumMaybe :: Num a => [Maybe a] -> a
    sumMaybe [] = 0
    sumMaybe (Nothing:xs) = 0 + sumMaybe xs
    sumMaybe (x:xs) = abs (fromJust x) + sumMaybe xs

    applyIfIncreases :: Ord a => (a -> a) -> [a] -> [a]
    applyIfIncreases _ [] = []
    applyIfIncreases f (x:xs)
        | f x >= x = f x : applyIfIncreases f xs
        | otherwise = x : applyIfIncreases f xs

    elemFreqByFirstOcc :: Eq a => [a] -> [(a, Int)]
    elemFreqByFirstOcc [] = []
    elemFreqByFirstOcc (x:xs) = (x, length $ filter (== x) (x:xs)) : elemFreqByFirstOcc (filter (/= x) xs)

    type RegNum = String
    type Level = Int
    type SpotNum = Int

    data Status = Free | Occupied RegNum
        deriving (Show, Eq)

    data ParkingSpace = PS Level SpotNum Status 
        deriving (Show, Eq)

    type ParkingLot = [ParkingSpace]

    freeSpaces :: ParkingLot -> Int -> Int
    freeSpaces [] _ = 0
    freeSpaces (PS level _ Free : xs) e
        | e == level = 1 + freeSpaces xs e
        | otherwise = freeSpaces xs e
    freeSpaces (_:xs) e = freeSpaces xs e

    findCar :: ParkingLot -> RegNum -> Maybe (Level, SpotNum)
    findCar [] _ = Nothing
    findCar list regnum = helper (reverse list) regnum where
        helper [] _ = Nothing
        helper (PS level spotnum (Occupied r):xs) regnum
            | regnum == r = Just (level, spotnum)
            | otherwise = helper xs regnum
        helper (_:xs) regnum = helper xs regnum

    eval :: String -> Integer
    eval "" = 0
    eval list = sum $ map read (split '+' list) where
        split :: Char -> String -> [String]
        split _ "" = [""]
        split c (x:xs)
            | x == c    = "" : split c xs
            | otherwise = (x : y) : ys where
                (y:ys) = split c xs

    dropOrInsert :: Eq a => [a] -> [a] -> [a]
    dropOrInsert list1 list2 = helper (reverse list1) list2 where
        helper list1 [] = reverse list1
        helper [] (x:xs) = helper [x] xs
        helper (x:xs) (y:ys)
            | y `elem` (x:xs) = helper (reverse(drop1 (reverse (x:xs)) y)) ys
            | otherwise = helper (y:(x:xs)) ys where
                drop1 [] _ = []
                drop1 (x:xs) y
                    | y == x = xs
                    | otherwise = x : drop1 xs y

    movingAvg :: Int -> [Double] -> [Maybe Double]
    movingAvg _ [] = []
    movingAvg m list
        | m <= 0 = []
        | otherwise = helper m list where
            helper _ [] = []
            helper n (x:xs)
                | length (take n (x:xs)) == n = Just (sum (take n (x:xs)) / fromIntegral n) : helper n xs
                | otherwise = Nothing : helper n xs