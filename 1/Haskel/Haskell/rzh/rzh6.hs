module Rzh6 where
    type Item = (String,Integer)

    itemsBelow100 :: [Item] -> [String]
    itemsBelow100 [] = []
    itemsBelow100 (x:xs)
        | snd x < 100 = fst x : itemsBelow100 xs
        | otherwise >= 100 = itemsBelow100 xs