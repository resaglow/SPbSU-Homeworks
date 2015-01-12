radd (a, b) (c, d) = (a * lcm' `div` b + c * lcm' `div` d, lcm') where lcm' = lcm b d

rsub (a, b) (c, d) = (a * lcm' `div` b - c * lcm' `div` d, lcm') where lcm' = lcm b d

rmul (a, b) (c, d) = (a * c, b * d)

rdiv (a, b) (c, d) = (a * d, b * c)

rinv (a, b) = (b, a)

rnorm (a, b) = (a `div` gcd', b `div` gcd') where gcd' = gcd a b

map' f list = [f x | x <- list]

zip' list1 [] = []
zip' (x:xs) (y:ys) = (x, y) : zip' xs ys

unzip' [] = ([], [])
unzip' ((a, b):xs) = (a : fst xsUnzip, b : xsUnzip) where ([first], [second]) = unzip' xs

flatten' [] = []
flatten' (list : otherLists) = list ++ flatten' otherLists
