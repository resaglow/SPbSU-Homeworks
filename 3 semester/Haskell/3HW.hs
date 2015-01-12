maxfold (x:xs) = foldl max x xs

minfold (x:xs) = foldl min x xs

-- Здесь 1 не считается простым числом
primes = [n | n <- [1..], divisors n == [1, n]] where
    divisors n = divisors' 1 where
        divisors' d | d * d > n = []
                    | d * d == n = [n]
                    | n `mod` d == 0 = d : (n `div` d) : ((divisors' (d + 1)))
                    | otherwise = divisors' (d + 1)

-- Здесь - считается
primes' = [n | n <- [1..], isPrime n] where
    isPrime n = isPrime' 2 where
        isPrime' d = d * d > n || (n `mod` d /= 0 && isPrime' (d + 1))

fibonacci = fibonacci' 1 1 where
    fibonacci' a b = a : fibonacci' b (a + b)
