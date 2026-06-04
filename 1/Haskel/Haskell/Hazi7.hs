module Hazi7 where

    data TriBool = No | Maybe | Yes
        deriving (Show, Eq)

    instance Ord TriBool where
        No <= Maybe = True
        No <= Yes = True
        Maybe <= Yes = True
        x <= y = x == y

    triOr :: TriBool -> TriBool -> TriBool
    triOr Yes No = Yes
    triOr No Yes = Yes
    triOr Maybe No = Maybe
    triOr No Maybe = Maybe
    triOr Yes Maybe = Yes
    triOr Maybe Yes = Yes

    triAnd :: TriBool -> TriBool -> TriBool
    triAnd Yes No = No
    triAnd No Yes = No
    triAnd Maybe No = No
    triAnd No Maybe = No
    triAnd Yes Maybe = Maybe
    triAnd Maybe Yes = Maybe

    incMonotonityTest :: (Integral i, Ord a) => i -> [a] -> TriBool
    incMonotonityTest 0 [] = Yes
    incMonotonityTest 0 _ = Maybe
    incMonotonityTest _ [_] = Yes
    incMonotonityTest a (x:y:zs)
        | a == 1 = Maybe
        | x <= y = incMonotonityTest (a - 1) (y:zs)
        | x > y = No

    data GolfScore = Ace | Albatross | Eagle | Birdie | Par | Bogey Integer
     deriving (Show)

    instance Eq GolfScore where
        Ace == Ace = True
        Albatross == Albatross = True
        Eagle == Eagle = True
        Birdie == Birdie = True
        Par == Par = True
        (Bogey x) == (Bogey y) = x == y
        _ == _ = False

    score :: Integer -> Integer -> GolfScore
    score _ 1 = Ace
    score a n
        | n <= a - 3 = Albatross
        | n == a - 2 = Eagle
        | n == a - 1 = Birdie
        | n == a = Par
        | n > a = Bogey (n - a)

    data Time = T Int Int
        deriving (Eq)

    t :: Int -> Int -> Time
    t h m
        | h >= 0 && h <= 23 && m >= 0 && m <= 59 = T h m
        | otherwise = error "Invalid Time"

    instance Show Time where
        show (T h m) = show h ++ ":" ++ show m
    
    instance Ord Time where
        (T h m) < (T h2 m2) = h < h2 || (h == h2 && m < m2)
        (T h m) > (T h2 m2) = h > h2 || (h == h2 && m > m2)
        (T h m) <= (T h2 m2) = not ((T h m) > (T h2 m2))
        (T h m) >= (T h2 m2) = not ((T h m) < (T h2 m2))

    isBetween :: Time -> Time -> Time -> Bool
    isBetween (T h m) (T h2 m2) (T h3 m3)
        | (T h2 m2) <= (T h m) && (T h2 m2) >= (T h3 m3) = True
        | (T h2 m2) >= (T h m) && (T h2 m2) <= (T h3 m3) = True
        | otherwise = False

    data USTime = AM Int Int | PM Int Int
        deriving (Eq)

    ustimeAM :: Int -> Int -> USTime
    ustimeAM h m
        | h >= 1 && h <= 12 && m >= 0 && m <= 59 = AM h m
        | otherwise = error "Invalid Time"
    
    ustimePM :: Int -> Int -> USTime
    ustimePM h m
        | h >= 1 && h <= 12 && m >= 0 && m <= 59 = PM h m
        | otherwise = error "Invalid Time"

    instance Show USTime where
        show (AM h m) = "AM " ++ show h ++ ":" ++ show m
        show (PM h m) = "PM " ++ show h ++ ":" ++ show m

    instance Ord USTime where
        (AM 12 m) < (AM h2 m2) = h2 /= 12 || (h2 == 12 && m < m2)
        (PM 12 m) < (PM h2 m2) = h2 /= 12 || (h2 == 12 && m < m2)
        (AM h m) < (AM h2 m2) = h < h2 || (h == h2 && m < m2)
        (PM h m) < (PM h2 m2) = h < h2 || (h == h2 && m < m2)
        (AM _ _) < (PM _ _) = True
        (PM _ _) > (AM _ _) = True

        (AM h m) <= (AM h2 m2) = (AM h m < AM h2 m2) || (h == h2 && m == m2)
        (PM h m) <= (PM h2 m2) = (PM h m < PM h2 m2) || (h == h2 && m == m2)
        (AM _ _) <= (PM _ _) = True
        (PM _ _) <= (AM _ _) = False

        (AM h m) >= (AM h2 m2) = not ((AM h m < AM h2 m2))
        (PM h m) >= (PM h2 m2) = not ((PM h m < PM h2 m2))
        (AM _ _) >= (PM _ _) = False
        (PM _ _) >= (AM _ _) = True

    ustimeToTime :: USTime -> Time
    ustimeToTime (AM 12 m) = T 0 m
    ustimeToTime (AM h m) = T h m
    ustimeToTime (PM 12 m) = T 12 m
    ustimeToTime (PM h m) = T (h + 12) m

    timeToUSTime :: Time -> USTime
    timeToUSTime (T 0 m) = AM 12 m
    timeToUSTime (T 12 m) = PM 12 m
    timeToUSTime (T h m)
        | h <= 11 && h >= 1 = AM h m
        | h <= 23 && h >= 12 = PM (h - 12) m