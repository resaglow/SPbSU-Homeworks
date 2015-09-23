inverse' list = inverse'' list [] where
	inverse'' [] acc = acc
	inverse'' (x:xs) acc = inverse'' xs (x : acc) 

foldl' f a [] = a
foldl' f a (x:xs) = foldl' f (f a x) xs

foldr' f a [] = a
foldr' f a (x:xs) = f x (foldr' f xs a)

lenfold list1 = foldl' (\r -> r + 1) 0 list1