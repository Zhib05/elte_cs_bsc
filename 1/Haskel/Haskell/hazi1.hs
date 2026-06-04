module Hazi1 where
    intExpr1 :: Integer
    intExpr1 = undefined

    intExpr2 :: Integer
    intExpr2 = undefined

    intExpr3 :: Integer
    intExpr3 = undefined

    charExpr1 :: Char
    charExpr1 = undefined

    charExpr2 :: Char
    charExpr2 = undefined

    charExpr3 :: Char
    charExpr3 = undefined

    boolExpr1 :: Bool
    boolExpr1 = undefined

    boolExpr2 :: Bool
    boolExpr2 = undefined

    boolExpr3 :: Bool
    boolExpr3 = undefined


    inc :: Integer -> Integer
    inc a = a + 1

    triple :: Integer -> Integer
    triple x = x * 3

    thirteen1 :: Integer
    thirteen1 = inc(inc(inc(inc(inc(inc(inc(inc(inc(inc (inc (inc (inc 0))))))))))))

    thirteen2 :: Integer
    thirteen2 = inc (triple (inc(inc(inc (inc 0)))))

    thirteen3 :: Integer
    thirteen3 = inc(inc(inc(inc(triple (triple (inc 0))))))


    cmpRem5Rem7 :: Integer -> Bool
    cmpRem5Rem7 b = (b `mod` 5) > (b `mod` 7)
    

    foo :: Int -> Bool -> Bool
    foo a b = (a > 10) || b

    bar :: Bool -> Int -> Bool
    bar b a = foo a b

