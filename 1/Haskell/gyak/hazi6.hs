module Hazi6 where
    splitOn :: Eq a => a -> [a] -> [[a]]
    splitOn _ [] = [[]]
    splitOn e (x:xs)
        | x == e = [] : splitOn e xs
        | otherwise = (x : head list) : tail list where
            list = splitOn e xs

    emptyLines :: Num a => String -> [a]
    emptyLines [] = [1]
    emptyLines (x:xs) = helper 1 (x:xs) where
        helper _ [] = []
        helper n ('\n':'\n':xs) = n + 1 : helper (n + 1) ('\n':xs)
        helper n ('\n':[]) = n + 1 : helper (n + 1) []
        helper n ('\n':xs) = helper (n + 1) xs
        helper n (_:xs) = helper n xs