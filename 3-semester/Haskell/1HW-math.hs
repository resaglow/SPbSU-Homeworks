-- Не работает для 2-ки
prim' n k = if n `mod` k /= 0 then
                if k * k < n then prim' n (k + 1)
                else True
            else False
prim n = prim' n 2

--Короткая версия, абсолютно эквивалентна
--prim n = prim' n 2 where
--    prim' d = n `mod` k && (k * k >= 0 || prim' (k + 1))

rprim' m n k = if m `mod` k == 0 && n `mod` k == 0 then False
               else
               		if k < m || k < n then rprim' m n (k + 1)
               		else True
rprim m n = rprim' m n 2


lcm' m n k | k `mod` m == 0 && k `mod` n == 0 = k
           | otherwise = lcm' m n (k + 1)
lcm m n = lcm' m n 1


ind' n curDiv soFar = 
    if curDiv * 2 <= n then 
        if n `mod` curDiv == 0 then ind' n (curDiv + 1) (soFar + 1)
        else ind' n (curDiv + 1) soFar
    else (soFar + 1)
ind n = ind' n 1 0


isd' n curDiv soFar = 
    if curDiv * 2 <= n then 
        if n `mod` curDiv == 0 then isd' n (curDiv + 1) (soFar + curDiv)
        else isd' n (curDiv + 1) soFar
    else (soFar + n)
isd n = isd' n 1 0


euler' n k soFar = 
    if k < n then 
        if rprim n k then euler' n (k + 1) (soFar + 1)
        else euler' n (k + 1) soFar
    else soFar
euler 1 = 1
euler n = euler' n 1 0
