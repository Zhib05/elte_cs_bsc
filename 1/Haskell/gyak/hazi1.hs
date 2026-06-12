module Hazi1 where
    intExpr1 :: Integer
    intExpr1 = 30

    intExpr2 :: Integer
    intExpr2 = 20

    intExpr3 :: Integer
    intExpr3 = 10

    charExpr1 :: Char
    charExpr1 = 'a'

    charExpr2 :: Char
    charExpr2 = 'b'

    charExpr3 :: Char
    charExpr3 = 'c'

    boolExpr1 :: Bool
    boolExpr1 = True

    boolExpr2 :: Bool
    boolExpr2 = False

    boolExpr3 :: Bool
    boolExpr3 = False


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

