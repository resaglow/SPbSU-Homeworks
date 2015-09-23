data Tsil a = Lin | Snoc (Tsil a) a

length' :: Tsil a -> Int
length' Lin = 0
length' (Snoc xs x) = length' xs + 1

map' :: (a -> b) -> Tsil a -> Tsil b
map' mapper Lin = Lin
map' mapper (Snoc xs x) = Snoc (map' mapper xs) (mapper x)

concat' :: Tsil a -> Tsil a -> Tsil a
concat' list Lin = list
concat' list1 (Snoc xs x) = Snoc (concat' list1 xs) x

reverse' :: Tsil a -> Tsil a
reverse' Lin = Lin
reverse' (Snoc xs x) = concat' (Snoc Lin x) (reverse' xs)

flatten' :: Tsil(Tsil a) -> Tsil a
flatten' Lin = Lin
flatten' (Snoc lists list) = concat' (flatten' lists) list

from_list :: [a] -> Tsil a
from_list [] = Lin
from_list 

--to_list :: Tsil a -> [a]