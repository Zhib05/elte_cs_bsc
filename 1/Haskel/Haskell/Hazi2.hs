module Hazi2 where

    addV :: (Double,Double) -> (Double,Double) -> (Double,Double)
    addV (a,b) (x,y) = (x + a, y + b)

    subV :: (Double,Double) -> (Double,Double) -> (Double,Double)
    subV (a,b) (x,y) = (a - x, b - y)

    scaleV :: Double -> (Double,Double) -> (Double,Double)
    scaleV a (x,y) = (a * x, a * y)

    scalar :: (Double,Double) -> (Double,Double) -> Double
    scalar (a,b) (x,y) = (a * x) + (b * y)

    divides :: Integral a => a -> a -> Bool
    divides 0 0 = True
    divides 0 _ = False
    divides a b = b `mod` a == 0

    add :: (Integral a, Integral b, Num c) => a -> b -> c
    add a b = fromIntegral a + fromIntegral b