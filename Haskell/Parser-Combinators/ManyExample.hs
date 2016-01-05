module ManyExample where

newtype Parser s a = P (s -> [(a, s)])

apply :: Parser s a -> s -> [(a, s)]
apply (P p) = p 

sym :: Eq s => s -> Parser [s] s
sym c = P p where p (x:xs) | x == c = [(x, xs)]
                  p _               = []

lift :: Parser s a -> b -> Parser s a
lift p _ = p

val :: a -> Parser s a
val a = P (\s -> [(a, s)])

infixl 1 |||
 (|||) :: Parser s a -> Parser s a -> Parser s a
(P p) ||| (P q) = P (\ s -> p s ++ q s)

infixl 3 ||>
 (||>) :: Parser s a -> (a -> Parser s b) -> Parser s b
(P p) ||> q = P (\s -> concat [apply (q a) s | (a, s) <- p s])




many :: Parser s a -> Parser s [a]
many p = p ||> (\x -> many p ||> val . (x:)) ||| 
         val []

-- [Start] "many" parser example; code below doesn't compile, just clarifies what's going on behind the scenes

symC = sym 'c'

many' p = P (\s -> concat [apply ((\x -> P (\s2 -> concat [apply ((\list -> val (x:list)) a2) s3 | (a2, s3) <- apply (many' p) s2])) a) s1 | (a, s1) <- apply p s] ++ [([], s)])
manySymC = P (\s -> concat [apply ((\x -> P (\s2 -> concat [apply ((\list -> val (x:list)) a2) s3 | (a2, s3) <- apply (many symC) s2])) a) s1 | (a, s1) <- apply symC s] ++ [([], s)])

appliedManySymC s = 
	concat [apply ((\x -> P (\s2 -> concat [apply ((\list -> val (x:list)) a2) s3 | (a2, s3) <- appliedManySymC s2])) a) s1 | (a, s1) <- apply symC s] ++ [([], s)]


appliedManySymC "cclol" = 
	concat [apply ((\x -> P (\s2 -> concat [apply ((\list -> val (x:list)) a2) s3 | (a2, s3) <- apply (many symC) s2])) a) s1 | (a, s1) <- apply symC "cclol"] ++ [([], "cclol")]
	concat [apply ((\x -> P (\s2 -> concat [apply ((\list -> val (x:list)) a2) s3 | (a2, s3) <- apply (many symC) s2])) 'c') "clol"] ++ [([], "cclol")]
	concat [apply (P (\s2 -> concat [apply ((\list -> val ('c':list)) a2) s3 | (a2, s3) <- apply (many symC) s2])) "clol"] ++ [([], "cclol")]
	concat [concat [apply ((\list -> val ('c':list)) a2) s3 | (a2, s3) <- apply (many symC) "clol"]] ++ [([], "cclol")]

appliedManySymC "clol" = 
	concat [apply ((\x -> P (\s2 -> concat [apply ((\list -> val (x:list)) a2) s3 | (a2, s3) <- apply (many symC) s2])) a) s1 | (a, s1) <- apply symC "clol"] ++ [([], "clol")]
	concat [apply ((\x -> P (\s2 -> concat [apply ((\list -> val (x:list)) a2) s3 | (a2, s3) <- apply (many symC) s2])) 'c') "lol"] ++ [([], "clol")]
	concat [apply (P (\s2 -> concat [apply ((\list -> val ('c':list)) a2) s3 | (a2, s3) <- apply (many symC) s2])) "lol"] ++ [([], "clol")]
	concat [concat [apply ((\list -> val ('c':list)) a2) s3 | (a2, s3) <- apply (many symC) "lol"]] ++ [([], "clol")]

appliedManySymC "lol" = 
	concat [apply ((\x -> P (\s2 -> concat [apply ((\list -> val (x:list)) a2) s3 | (a2, s3) <- apply (many symC) s2])) a) s1 | (a, s1) <- apply symC "lol"] ++ [([], "lol")]
	[([], "lol")]

appliedManySymC "clol" = 
	concat [concat [apply ((\list -> val ('c':list)) []) "lol"]] ++ [([], "clol")]
	concat [concat [apply (val "c") "lol"]] ++ [([], "clol")]
	concat [concat [[("c", "lol")]]] ++ [([], "clol")]
	[("c", "lol")] ++ [([], "clol")]
	[("c", "lol"), ([], "clol")]

appliedManySymC "cclol" = 
	concat [concat [apply ((\list -> val ('c':list)) a2) s3 | (a2, s3) <- [("c", "lol"), ([], "clol")]]] ++ [([], "cclol")]
	concat [concat [apply ((\list -> val ('c':list)) "c") "lol", apply ((\list -> val ('c':list)) []) "clol"]] ++ [([], "cclol")]
	concat [concat [apply (val "cc") "lol", apply (val "c") "clol"]] ++ [([], "cclol")]
	concat [concat [[("cc", "lol")], [("c", "clol")]]] ++ [([], "cclol")]
	[("cc", "lol"), ("c", "clol")] ++ [([], "cclol")]
	[("cc", "lol"), ("c", "clol"), ([], "cclol")]

-- [End] "many" parser example
