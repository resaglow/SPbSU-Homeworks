prime n = prime' 2 where
	prime' d = d * d > n || (n `mod` d /= 0 && prime' (d + 1))

boolOr x y = if x then True else y

sum' [] = 0
sum' (x:xs) = x + sum' xs

prod' [] = 0
prod' (x:xs) = x * (prod' xs)

length' [] = 0
length' (x:xs) = 1 + length' xs

concat' [] x = x
concat' (x:xs) y = x : (concat' xs y)

inverse' [] = []
inverse' (x:xs) = concat' (inverse' xs) [x]

divisors n = divisors' 1 where
	divisors' d | d * d > n = []
	            | d * d == n = [n]
	            | n `mod` d == 0 = d : (concat' (divisors' (d + 1)), [n `div` d])
	            | otherwise = divisors' (d + 1)

-- Важно всегда думать о том, нужно ли применять аккумуляторы (довольно часто можно обойтись и без них)
-- Через несколько уравнений делать лучше, чем через if-ы
-- $ это труъ




