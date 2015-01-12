-- Quiz 1
-- Artem Lobanov

sprod x y = sum $ zipWith (*) x y

normalize l = foldr (\x soFar -> if elem x soFar then soFar else x:soFar) [] l

sort (x:xs) = sort [a | a <- xs, a <= x] ++ [x] ++ sort [b | b <- xs, b > x]

isProgression (x1:x2:xs) = isProgression' (x2:xs) where
	delta = x2 - x1
	isProgression' (x1:x2:xs) = if x2 - x1 == delta then isProgression' (x2:xs) else False
	isProgression' _ = True
isProgression _ = True

-- По признаку
isProgression2 (x1:x2:x3:xs) = (x2 == (x1 + x3) `div` 2) && isProgression (x2:x3:xs)
isProgression2 _ = True

isFunction r = length r == (length $ normalize xs) where xs = map fst r

isSymmetric r = foldr (&&) True $ map (\(x, y) -> elem (y, x) r) r

-- Для каких иксов нужно, чтобы работало?
isReflexive r = foldr (&&) True $ map (\(x, y) -> elem (x, x) r) r
-- Работает странно, но для всех целых
isReflexive2 r = foldr (\x reflFlag -> if elem (x, x) r && elem (-x, -x) r then reflFlag else False) True [0, 1..]

closure r = normalize [(x, z) | (x, ym) <- r, (yn, z) <- r, ym == yn, not $ elem (x, z) r] ++ r

isTransitive r = length r == (length $ closure r)