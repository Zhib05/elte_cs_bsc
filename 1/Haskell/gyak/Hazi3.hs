module Hazi3 where

    isSingleton :: [a] -> Bool
    isSingleton [_] = True
    isSingleton _  = False

    exactly2OrAtLeast4 :: [a] -> Bool
    exactly2OrAtLeast4 [] = False
    exactly2OrAtLeast4 [_] = False
    exactly2OrAtLeast4 [_,_,_] = False
    exactly2OrAtLeast4 _ = True

    firstTwoElements :: [a] -> [a]
    firstTwoElements (x:y:_) = [x,y]
    firstTwoElements _ = []

    withoutThird :: [a] -> [a]
    withoutThird (x:y:z:g) = (x:y:g)
    withoutThird (x) = (x)

    onlySingletons :: [[a]] -> [[a]]
    onlySingletons x = [ x | x <- x, length x == 1]