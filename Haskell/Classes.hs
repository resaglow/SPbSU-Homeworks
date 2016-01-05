data Peano = Zero | Succ Peano deriving Show

instance Eq Peano where
    peano1 == peano2 = case (peano1, peano2) of
        (Zero, Zero) -> True
        (Succ prePeano1, Succ prePeano2) -> prePeano1 == prePeano2
        _ -> False

instance Num Peano where
    peano1 + peano2 = 
        case peano1 of 
            Zero -> peano2
            Succ prePeano1 -> prePeano1 + (Succ peano2)

    peano1 - peano2 =
        case (peano1, peano2) of 
            (Zero, _) -> Zero
            (peano1, Zero) -> peano1
            (Succ prePeano1, Succ prePeano2) -> prePeano1 - prePeano2
    
    peano1 * peano2 = 
        case (peano1, peano2) of
            (peano1, Succ Zero) -> peano1
            (Zero, _) -> Zero
            (_, Zero) -> Zero
            (peano1, peano2) -> f peano1 peano2 Zero where
                f peano1 peano2 acc = case peano2 of 
                    Zero -> acc
                    Succ (prePeano2) -> f peano1 prePeano2 (acc + peano1)

    abs peano = peano
    signum peano = peano

    fromInteger 0 = Zero
    fromInteger a = Succ (fromInteger $ abs a - 1)
