module Test where
    import Data.List.Split (splitOn)

    test :: String -> [String]
    test a = splitOn "," a