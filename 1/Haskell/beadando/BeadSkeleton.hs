module NagyBead where

import Data.Either
import Data.Maybe
import Text.Read
import Data.List

basicInstances = 0 -- Mágikus tesztelőnek kell ez, NE TÖRÖLD!

data Dir = InfixL | InfixR
  deriving (Show, Eq, Ord)

data Tok a = BrckOpen | BrckClose | TokLit a | TokBinOp (a -> a -> a) Char Int Dir | TokFun (a -> a) String

instance Show a => Show (Tok a) where
  show BrckOpen = "BrckOpen"
  show BrckClose = "BrckClose"
  show (TokLit b) = "TokLit " ++ show b
  show (TokBinOp opr szimb kot dir) = "TokBinOp '" ++ [szimb] ++ "' " ++ show kot ++ " " ++ show dir
  show (TokFun fugg nev) = "TokFun " ++ nev 

instance Eq a => Eq (Tok a) where
  BrckOpen == BrckOpen = True
  BrckClose == BrckClose = True
  (TokLit a) == (TokLit b) = a == b
  (TokBinOp opr szimb kot dir) == (TokBinOp opr2 szimb2 kot2 dir2) = szimb == szimb2 && kot == kot2 && dir == dir2
  (TokFun fugg nev) == (TokFun fugg2 nev2) = nev == nev2
  _ == _ = False

type OperatorTable a = [(Char, (a -> a -> a, Int, Dir))]

tAdd, tMinus, tMul, tDiv, tPow :: (Floating a) => Tok a
tAdd = TokBinOp (+) '+' 6 InfixL
tMinus = TokBinOp (-) '-' 6 InfixL
tMul = TokBinOp (*) '*' 7 InfixL
tDiv = TokBinOp (/) '/' 7 InfixL
tPow = TokBinOp (**) '^' 8 InfixR

operatorTable :: (Floating a) => OperatorTable a
operatorTable =
    [ ('+', ((+), 6, InfixL))
    , ('-', ((-), 6, InfixL))
    , ('*', ((*), 7, InfixL))
    , ('/', ((/), 7, InfixL))
    , ('^', ((**), 8, InfixR))
    ]

operatorFromChar :: OperatorTable a -> Char -> Maybe (Tok a)
operatorFromChar [] _ = Nothing
operatorFromChar ((x, (opr, kot, dir)) : xs) szimb
  | x == szimb = Just $ TokBinOp opr szimb kot dir
  | otherwise = operatorFromChar xs szimb

getOp :: (Floating a) => Char -> Maybe (Tok a)
getOp = operatorFromChar operatorTable

parseTokens :: Read a => OperatorTable a -> String -> Maybe [Tok a]
parseTokens _ [] = Just []
parseTokens oprt s = parseTokens' oprt (helper(words s))

parseTokens' :: Read a => OperatorTable a -> [String] -> Maybe [Tok a]
parseTokens' oprt s = helper' s where
  helper' [] = Just []
  helper' (x:xs)
    | x == "(" = justCsomagol [BrckOpen] (helper' xs)
    | x == ")" = justCsomagol [BrckClose] (helper' xs)
    | length x == 1 && juste (operatorFromChar oprt (head x)) = justCsomagol [justErtek (operatorFromChar oprt (head x))] (helper' xs)
    | juste val = justCsomagol [TokLit (justErtek val)] (helper' xs)
    | otherwise = Nothing where
        val = readMaybe x

helper :: [String] -> [String]
helper [] = []
helper ((z:zs):xs)
  | (z:zs) /= "(" && z == '(' = "(" : helper (zs:xs)
  | (z:zs) /= ")" && z == ')' = ")" : helper (zs:xs)
  | otherwise = (z:zs) : helper xs

justCsomagol :: [a] -> Maybe [a] -> Maybe [a]
justCsomagol _ Nothing = Nothing
justCsomagol first (Just second) = Just (first ++ second)

juste :: Maybe a -> Bool
juste (Just _) = True
juste Nothing = False

justErtek :: Maybe a -> a
justErtek (Just x) = x

parse :: String -> Maybe [Tok Double]
parse = parseTokens operatorTable

shuntingYardBasic :: [Tok a] -> ([a], [Tok a])
shuntingYardBasic [] = ([], [])
shuntingYardBasic tok = helper tok [] [] where
  helper [] litList opList = (litList, opList)
  helper ((TokLit x):xs) litList opList = helper xs (x : litList) opList
  helper (BrckOpen:xs) litList opList = helper xs litList (BrckOpen : opList)
  helper (TokBinOp f szimb kot dir : xs) litList opList = helper xs litList (TokBinOp f szimb kot dir : opList)
  helper (BrckClose:xs) litList opList = helper xs newLitList newOpList where
    (newLitList, newOpList) = kiertekeles litList opList where
      kiertekeles (a : b : xs) (TokBinOp f _ _ _ : ys) = kiertekeles ((f b a) : xs) ys
      kiertekeles lists (BrckOpen : ys) = (lists, ys)

parseAndEval :: (String -> Maybe [Tok a]) -> ([Tok a] -> ([a], [Tok a])) -> String -> Maybe ([a], [Tok a])
parseAndEval parse eval input = maybe Nothing (Just . eval) (parse input)

syNoEval :: String -> Maybe ([Double], [Tok Double])
syNoEval = parseAndEval parse shuntingYardBasic

syEvalBasic :: String -> Maybe ([Double], [Tok Double])
syEvalBasic = parseAndEval parse (\t -> shuntingYardBasic $ BrckOpen : (t ++ [BrckClose]))

shuntingYardPrecedence :: [Tok a] -> ([a], [Tok a])
shuntingYardPrecedence [] = ([], [])
shuntingYardPrecedence tok = helper tok [] [] where
  helper [] litList opList = (litList, opList)
  helper ((TokLit x): xs) litList opList = helper xs (x : litList) opList
  helper (BrckOpen: xs) litList opList = helper xs litList (BrckOpen : opList)
  helper (BrckClose: xs) litList opList = helper xs newLitList newOpList where
    (newLitList, newOpList) = kiertekeles litList opList where
      kiertekeles lits (BrckOpen : ys) = (lits, ys) 
      kiertekeles (a : b : xs) (TokBinOp f szimb kot dir : ys) = kiertekeles ((f b a) : xs) ys
  helper (TokBinOp f szimb kot dir : xs) litList opList = helper xs newLitList (TokBinOp f szimb kot dir : newOpList) where
    (newLitList, newOpList) = kiertekeles litList opList kot dir where
      kiertekeles litList [] _ _ = (litList, [])
      kiertekeles litList (TokBinOp f2 szimb2 kot2 dir2 : ys) kot dir
        | kot > kot2 || (kot == kot2 && dir == InfixR) = (litList, (TokBinOp f2 szimb2 kot2 dir2 : ys))
        | otherwise = kiertekeles newLitList newOpList kot dir where
          (newLitList, newOpList) = kiertekel litList (TokBinOp f2 szimb2 kot2 dir2) dir ys where
            kiertekel (a : b : xs) (TokBinOp f2 szimb2 kot2 dir2) dir ys = ((f2 b a) : xs, ys)
      kiertekeles litList opList _ _ = (litList, opList)

syEvalPrecedence :: String -> Maybe ([Double], [Tok Double])
syEvalPrecedence = parseAndEval parse (\t -> shuntingYardPrecedence $ BrckOpen : (t ++ [BrckClose]))



data ShuntingYardError = OperatorOrClosingParenExpected | LiteralOrOpeningParenExpected | NoClosingParen | NoOpeningParen | ParseError
  deriving (Show, Eq)

type ShuntingYardResult a = Either ShuntingYardError a

-- eqError-t vedd ki a kommentből, ha megcsináltad az 1 pontos "Hibatípus definiálása" feladatot
eqError = 0 -- Mágikus tesztelőnek szüksége van rá, NE TÖRÖLD!

-- parseAndEvalSafe ::
--     (String -> ShuntingYardResult [Tok a]) ->
--     ([Tok a] -> ShuntingYardResult ([a], [Tok a])) ->
--     String -> ShuntingYardResult ([a], [Tok a])
-- parseAndEvalSafe parse eval input = either Left eval (parse input)

-- sySafe :: String -> ShuntingYardResult ([Double], [Tok Double])
-- sySafe = parseAndEvalSafe
--   (parseSafe operatorTable)
--   (\ts -> shuntingYardSafe (BrckOpen : ts ++ [BrckClose]))

type FunctionTable a = [(String, a -> a)]

-- Ezt akkor vedd ki a kommentblokkból, ha az 1 pontos "Függvénytábla és a típus kiegészítése" feladatot megcsináltad.
tSin, tCos, tLog, tExp, tSqrt :: Floating a => Tok a
tSin = TokFun sin "sin"
tCos = TokFun cos "cos"
tLog = TokFun log "log"
tExp = TokFun exp "exp"
tSqrt = TokFun sqrt "sqrt"

functionTable :: (RealFrac a, Floating a) => FunctionTable a
functionTable =
    [ ("sin", sin)
    , ("cos", cos)
    , ("log", log)
    , ("exp", exp)
    , ("sqrt", sqrt)
    , ("round", (\x -> fromIntegral (round x :: Integer)))
    ]

functionFromString :: FunctionTable a -> String -> Maybe (Tok a)
functionFromString [] _ = Nothing
functionFromString ((nev, func) : xs) s
  | s == nev = Just $ TokFun func s
  | otherwise = functionFromString xs s 

parseWithFunctions :: Read a => OperatorTable a -> FunctionTable a -> String -> Maybe [Tok a]
parseWithFunctions oprt func [] = Just []
parseWithFunctions oprt func s = parseWithFunctions' oprt func (helper (words s))

parseWithFunctions' :: Read a => OperatorTable a -> FunctionTable a -> [String] -> Maybe [Tok a]
parseWithFunctions' oprt func s = helper' s where
  helper' [] = Just []
  helper' (x:xs)
    | x == "(" = justCsomagol [BrckOpen] (helper' xs)
    | x == ")" = justCsomagol [BrckClose] (helper' xs)
    | length x == 1 && juste (operatorFromChar oprt (head x)) = justCsomagol [justErtek (operatorFromChar oprt (head x))] (helper' xs)
    | juste (functionFromString func x) = justCsomagol [justErtek (functionFromString func x)] (helper' xs)
    | juste val = justCsomagol [TokLit (justErtek val)] (helper' xs)
    | otherwise = Nothing where
        val = readMaybe x

shuntingYardWithFunctions :: [Tok a] -> ([a], [Tok a])
shuntingYardWithFunctions [] = ([], [])
shuntingYardWithFunctions tok = helper tok [] [] where
  helper [] litList opList = (litList, opList)
  helper ((TokLit x): xs) litList opList = helper xs (x : litList) opList
  helper (BrckOpen: xs) litList opList = helper xs litList (BrckOpen : opList)
  helper (TokFun f s : xs) litList opList = helper xs litList (TokFun f s : opList)
  helper (BrckClose: xs) litList opList = helper xs newLitList newOpList where
    (newLitList, newOpList) = kiertekeles litList opList where
      kiertekeles lits (BrckOpen : ys) = (lits, ys) 
      kiertekeles (a : b : xs) (TokBinOp f szimb kot dir : ys) = kiertekeles ((f b a) : xs) ys
      kiertekeles (a : xs) (TokFun f s : ys) = kiertekeles (f a : xs) ys
  helper (TokBinOp f szimb kot dir : xs) litList opList = helper xs newLitList (TokBinOp f szimb kot dir : newOpList) where
    (newLitList, newOpList) = kiertekeles litList opList kot dir where
      kiertekeles litList [] _ _ = (litList, [])
      kiertekeles litList (TokBinOp f2 szimb2 kot2 dir2 : ys) kot dir
        | kot > kot2 || (kot == kot2 && dir == InfixR) = (litList, (TokBinOp f2 szimb2 kot2 dir2 : ys))
        | otherwise = kiertekeles newLitList newOpList kot dir where
          (newLitList, newOpList) = kiertekel litList (TokBinOp f2 szimb2 kot2 dir2) dir ys where
            kiertekel (a : b : xs) (TokBinOp f2 szimb2 kot2 dir2) dir ys = ((f2 b a) : xs, ys)
      kiertekeles (a : xs) (TokFun f s : ys) kot dir = ((f a) : xs, ys)
      kiertekeles litList opList _ _ = (litList, opList)

-- Ezt akkor vedd ki a kommentblokkból, ha a 2 pontos "Függvények parse-olása és kiértékelése" feladatot megcsináltad.
syFun :: String -> Maybe ([Double], [Tok Double])
syFun = parseAndEval
  (parseWithFunctions operatorTable functionTable)
  (\t -> shuntingYardWithFunctions $ BrckOpen : (t ++ [BrckClose]))

{-
-- Ezt akkor vedd ki a kommentblokkból, ha minden más feladatot megcsináltál ez előtt.
syComplete :: String -> ShuntingYardResult ([Double], [Tok Double])
syComplete = parseAndEvalSafe
  (parseComplete operatorTable functionTable)
  (\ts -> shuntingYardComplete (BrckOpen : ts ++ [BrckClose]))
-}
