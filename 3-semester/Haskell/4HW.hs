data Tsil a = Lin | Snoc (Tsil a) a


length' :: Tsil a -> Int
length' Lin = 0
length' (Snoc xs x) = length' xs + 1

map' :: (a -> b) -> Tsil a -> Tsil b
map' mapper Lin = Lin
map' mapper (Snoc xs x) = Snoc (map' mapper xs) (mapper x)

concat' :: Tsil a -> Tsil a -> Tsil a
concat' tsil Lin = tsil
concat' tsil1 (Snoc xs x) = Snoc (concat' tsil1 xs) x

reverse' :: Tsil a -> Tsil a
reverse' tsil = reverse'' Lin tsil where
	reverse'' acc Lin = acc
	reverse'' acc (Snoc xs x) = reverse'' (Snoc acc x) xs

-- Работает через testFun (последняя строчка)
flatten' :: Tsil([a]) -> Tsil a
flatten' Lin = Lin
flatten' (Snoc lists list) = concat' (flatten' lists) (from_list list)

-- Работает через явное сцепление Tsil-ов в Tsil(Tsil) в консоли
flatten'' :: Tsil(Tsil a) -> Tsil a
flatten'' Lin = Lin
flatten'' (Snoc lists list) = concat' (flatten'' lists) list

from_list :: [a] -> Tsil a
from_list list = from_list' $ reverse list where
	from_list' [] = Lin
	from_list' (x:xs) = Snoc (from_list' xs) x

to_list :: Tsil a -> [a]
to_list tsil = reverse $ to_list' tsil where 
	to_list' Lin = []
	to_list' (Snoc xs x) = x : (to_list' xs)

foldl' f a Lin = a
foldl' f a (Snoc xs x) = f (foldl' f a xs) x

foldr' f a Lin = a
foldr' f a (Snoc xs x) = foldr' f (f a x) xs


testList list = to_list $ from_list list
testFun f list = to_list $ flatten' $ from_list list

