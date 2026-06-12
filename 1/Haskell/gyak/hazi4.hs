module Hazi4 where

    import Data.List

    mountain :: Integral i => i -> String
    mountain 0 = ""
    mountain a = mountain (a - 1) ++ replicate (fromIntegral a) '#' ++ "\n"

    countAChars :: Num i => String -> i
    countAChars [] = 0
    countAChars ('a':xs) = 1 + countAChars xs
    countAChars (_:xs)   = countAChars xs

    lucas :: (Integral a, Num b) => a -> b
    lucas 0 = 2
    lucas 1 = 1
    lucas n = lucas (n - 1) + lucas (n - 2)

    longerThan :: Integral i => [a] -> i -> Bool
    longerThan [] 0 = False
    longerThan [] _ = False
    longerThan _  0 = True
    longerThan (_:xs) n = longerThan xs (n - 1)

    format :: Integral i => i -> String -> String
    format 0 x = x
    format a [] = ' ' : format (a - 1) []
    format a (x:xs) = x : format (a - 1) xs

    merge :: [a] -> [a] -> [a]
    merge [] [] = []
    merge [] (y:ys) = y : merge [] ys
    merge (x:xs) [] = x : merge xs []
    merge (x:xs) (y:ys) = x : y : merge xs ys