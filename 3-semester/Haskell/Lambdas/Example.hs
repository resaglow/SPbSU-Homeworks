data Lambda = Var String |
			  App Lambda Lambda |
			  Lam String Lambda


show' (Var string) = string
show' (App lambda1 lambda2) = case (lambda1, lambda2) of 
	((Lam string lambda), (App lambda1' lambda2')) -> "(" ++ show' lambda1 ++ ")(" ++ show' lambda2 ++ ")"
	((Lam string lambda2'), Var string') -> "(" ++ show' lambda1 ++ ")" ++ show' lambda2
	_ -> show' lambda1 ++ show' lambda2
show' (Lam string lambda) = "\\" ++ string ++ show' lambda



fv (Var string) = [string]
fv (App a b) = fv a ++ fv b
fv (Lam x a) = foldr (\y list -> if y == x then list else (y:list)) [] (fv a)



subst lambda x b = case lambda of 
	Var string | string == x -> b
	           | otherwise -> Var string
	App lambda1 lambda2 -> App (subst lambda1 x b) (subst lambda2 x b)
	Lam param body | param == x -> Lam param body
		           | otherwise -> Lam param (subst body x b)



-- Можно было бы объявить функции vars и newSym внутри case-а в subst', не на верхнем уровне,
-- но тогда была бы слишком большая вложенность кода

vars (Var string) = [string]
vars (App a b) = vars a ++ vars b
vars (Lam x a) = [x] ++ vars a

newSym basicSym curSym body b = 
    let wrappedSym = "<" ++ curSym ++ ">" in
        if not $ elem wrappedSym (vars body) || elem wrappedSym (vars b) then wrappedSym 
        else newSym basicSym (basicSym ++ curSym) body b

subst' lambda x b = case lambda of 
	Var string | string == x -> b
	           | otherwise -> Var string
	App lambda1 lambda2 -> App (subst' lambda1 x b) (subst' lambda2 x b)
	Lam param body | param == x -> Lam param body
		           | otherwise -> 
		               if not $ elem param (fv b) then Lam param (subst' body x b)
		               else subst' (Lam curNewSym (subst' body param (Var curNewSym))) x b where
		                   curNewSym = newSym "z" "z" body b where



beta' (Var string) = Var string
beta' (Lam param body) = Lam param (beta' body)
beta' (App (Lam param body) lambda2) = beta' $ subst' body param lambda2
beta' (App lambda1 lambda2) = let lambda1' = beta' lambda1 in case lambda1' of
	Lam param body -> beta' $ subst' body param lambda2
	_ -> App lambda1' (beta' lambda2)
