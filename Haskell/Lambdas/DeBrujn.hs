import Data.List

data Lambda = Var String |
			  App Lambda Lambda |
			  Lam String Lambda
	deriving(Show)

data LambdaDeBruijn = Num Int |
                      Appl LambdaDeBruijn LambdaDeBruijn |
                      LamDeBruijn LambdaDeBruijn
	deriving(Show)  

show'' (Num int) = show int
show'' (Appl lambda1 lambda2) = case (lambda1, lambda2) of 
	((LamDeBruijn lambda), (Appl lambda1' lambda2')) -> "(" ++ show'' lambda1 ++ ")(" ++ show'' lambda2 ++ ")"
	((LamDeBruijn lambda2'), Num string') -> "(" ++ show'' lambda1 ++ ")" ++ show'' lambda2
	_ -> show'' lambda1 ++ show'' lambda2
show'' (LamDeBruijn lambda) = "\\" ++ show'' lambda                   

convertTo lambdaDeBruijn = convertTo' lambdaDeBruijn [] where
	convertTo' (Var string) varList = case elemIndex string varList of
		Just value -> Num value
		Nothing -> Num (-1)
	convertTo' (App lambda1 lambda2) varList = Appl (convertTo' lambda1 varList) (convertTo' lambda2 varList)
	convertTo' (Lam param lambda) varList = LamDeBruijn (convertTo' lambda (param:varList))

convertFrom lambdaDeBruijn = convertFrom' lambdaDeBruijn (-1) [0] where
	convertFrom' (Num num) nestLevel scopeVarList =	if diff < 0 then Var [['x'..] !! ((maximum scopeVarList) + 1)] else Var [['x'..] !! diff] where diff = nestLevel - num
	convertFrom' (Appl lambda1 lambda2) nestLevel scopeVarList = App (convertFrom' lambda1 nestLevel scopeVarList) (convertFrom' lambda2 nestLevel scopeVarList)
	convertFrom' (LamDeBruijn lambda) nestLevel scopeVarList = Lam [['x'..] !! (nestLevel + 1)] (convertFrom' lambda (nestLevel + 1) [0])
