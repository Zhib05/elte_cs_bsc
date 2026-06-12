module Vizsga220124 where
    import Data.Char
    import Data.List
    import Data.Maybe

    whichIsEmpty :: [a] -> [b] -> Maybe Int
    whichIsEmpty [] [] = Just 1
    whichIsEmpty [] _  = Just 1
    whichIsEmpty _ []  = Just 2
    whichIsEmpty _ _   = Nothing

    match :: Eq a => (a,a) -> (a, a) -> Bool
    match (x,y) (xs,ys)
        | x == xs || x == ys || y == xs || y == ys = True
        | otherwise = False

    indicesOfEmpties :: Eq a => [[a]] -> [Int]
    indicesOfEmpties [] = []
    indicesOfEmpties list = helper list 1 where
        helper [] _ = []
        helper (x:xs) i
            | null x = i : helper xs (i + 1)
            | otherwise = helper xs (i + 1)

    applyOnWords :: (String -> String) -> String -> String
    applyOnWords _ "" = ""
    applyOnWords f s = unwords $ filter (not . null) (map f (words s))

    restUntil :: (a -> [a] -> Bool) -> [a] -> [a]
    restUntil _ [] = []
    restUntil f (x:xs)
        | f x xs = (x:xs)
        | otherwise = restUntil f xs

    replaceAll :: Eq a => a {-mit-} -> [a] -> a {-mire-} -> [a]
    replaceAll _ [] _ = []
    replaceAll a (x:xs) b
        | a == x = b : replaceAll a xs b
        | otherwise = x : replaceAll a xs b 

    listWordsWithUpper :: String -> [String]
    listWordsWithUpper "" = []
    listWordsWithUpper s = helper (words s) where
        helper [] = []
        helper ((x:xs):ys)
            | True `elem` map isUpper (x:xs) = (x:xs) : helper ys
            | otherwise = helper ys

    applyWhile :: (a -> Bool) -> (a -> a) -> [a] -> [a]
    applyWhile _ _ [] = []
    applyWhile p f (x:xs)
        | p x = f x : applyWhile p f xs
        | otherwise = (x:xs)

    replaceWithDefIfNot :: (a -> Bool) -> [a] -> [a] -> [a]
    replaceWithDefIfNot _ [] [] = []
    replaceWithDefIfNot _ [] _  = []
    replaceWithDefIfNot _ a []  = a
    replaceWithDefIfNot f (x:xs) (y:ys)
        | f x = x : replaceWithDefIfNot f xs (y:ys)
        | otherwise = y : replaceWithDefIfNot f xs ys

    applyAlternately :: (a -> b) -> (a -> b) -> [a] -> [b]
    applyAlternately _ _ [] = []
    applyAlternately f1 f2 list = helper f1 f2 list 1 where
        helper _ _ [] _ = []
        helper f1 f2 (x:xs) i
            | odd i = f1 x : helper f1 f2 xs (i + 1)
            | otherwise = f2 x : helper f1 f2 xs (i + 1)

    zipMaybe :: [a] -> [b] -> [(Maybe a, Maybe b)]
    zipMaybe [] [] = []
    zipMaybe (x:xs) [] = (Just x, Nothing) : zipMaybe xs []
    zipMaybe [] (y:ys) = (Nothing, Just y) : zipMaybe [] ys
    zipMaybe (x:xs) (y:ys) = (Just x, Just y) : zipMaybe xs ys

    data Temperature = Night Double | Daytime Double
        deriving (Show, Eq)

    isDaytime :: Temperature -> Bool
    isDaytime (Night _) = False
    isDaytime (Daytime _) = True

    daytimeAvg :: [Temperature] -> Double
    daytimeAvg [] = 0
    daytimeAvg list
        | null avgtmp = 0
        | otherwise = sum avgtmp / fromIntegral (length avgtmp) where
            avgtmp = [a | Daytime a <- list]

    lackOfLetters :: String -> [Char] -> Maybe [Char]
    lackOfLetters "" _ = Nothing
    lackOfLetters (x:xs) list
            | null (mindeneleme (nub (x:xs)) list) = Nothing
            | otherwise = Just (nub (mindeneleme (nub (x:xs)) list)) where
                mindeneleme [] _ = []
                mindeneleme (x:xs) list
                    | (toLower x) `elem` list = mindeneleme xs list
                    | otherwise = toLower x : mindeneleme xs list

    fixedPointIn :: Eq a => (a -> a) -> a -> Int -> Maybe Int
    fixedPointIn f a lepes = helper f a lepes 0 where
        helper f a lepes c
            | lepes >= c && f a == a = Just c
            | lepes >= c && not (f a == a) = helper f (f a) lepes (c + 1)
            | otherwise = Nothing