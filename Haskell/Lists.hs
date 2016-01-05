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

map' f list = [f x | x <- list]

zip' list1 [] = []
zip' (x:xs) (y:ys) = (x, y) : zip' xs ys

unzip' [] = ([], [])
unzip' ((a, b):xs) = (a : fst xsUnzip, b : xsUnzip) where ([first], [second]) = unzip' xs

flatten' [] = []
flatten' (list : otherLists) = list ++ flatten' otherLists

primes = [n | n <- [1..], divisors n == [1, n]] where
    divisors n = divisors' 1 where
        divisors' d | d * d > n = []
                    | d * d == n = [n]
                    | n `mod` d == 0 = d : (n `div` d) : ((divisors' (d + 1)))
                    | otherwise = divisors' (d + 1)

primes' = [n | n <- [1..], isPrime n] where
    isPrime n = isPrime' 2 where
        isPrime' d = d * d > n || (n `mod` d /= 0 && isPrime' (d + 1))

fibonacci = fibonacci' 1 1 where
    fibonacci' a b = a : fibonacci' b (a + b)
