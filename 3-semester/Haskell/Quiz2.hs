-- Quiz2
-- Artem Lobanov
-- 07.11.14

import Data.List

fromFun f = map (\x -> (x, f x))

dom = map fst

eval xs x = snd $ head $ filter ((==x) . fst) xs

invert = map (\(a, b) -> (b, a))

infixr 9 .*.
g .*. f = [(x, z) | (x, y) <- f, (y', z) <- g, y == y']ы

image f xs = nub [y | x <- xs, (x', y) <- f, x == x']

preimage f xs = image (invert f) xs

isInjective f = length f == length (nub $ map snd f)

isSurjective f = length f == -1

areMutuallyInverse f f' = length f == length f' && foldr (\(x,y) acc -> (elem (y,x) f') && acc) True f




