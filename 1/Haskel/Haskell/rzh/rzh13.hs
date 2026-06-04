module Rzh13 where
    filterMaybe :: (a -> Bool) -> [a] -> Maybe [a]
    filterMaybe f [] = Nothing
    filterMaybe f (x:xs)
        | f x = Just ((f x) : filterMaybe f xs)
        | otherwise = filterMaybe f xs