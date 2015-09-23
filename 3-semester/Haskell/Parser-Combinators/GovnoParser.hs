import Data.List
import Data.Char

newtype Parser s a = P (s -> [(a, s)])

apply :: Parser s a -> s -> [(a, s)]
apply (P p) = p 

data Token = X String | C Int | A | S | M | D | LP | RP | E deriving Show
data WordType = Id | Number | Other deriving Eq

tokenize :: Parser String [Token]
tokenize = P p where
    p s = p' s [] [] False Other where
        getSimpleToken lexem = case lexem of 
            '+' -> A; '-' -> S; '*' -> M; '/' -> D; '(' -> LP; ')' -> RP

        addSimpleLexem (c1:cs) wordAcc tokenAcc started wordType tokenType = 
            let wordAcc' = reverse wordAcc in case wordType of
                Id -> p' cs [] (tokenType:(X wordAcc'):tokenAcc) False Other
                Number -> p' cs [] (tokenType:(C (read wordAcc' :: Int)):tokenAcc) False Other
                Other -> p' cs [] (tokenType:tokenAcc) False Other

        pLast input wordAcc tokenAcc started wordType = let wordAcc' = reverse wordAcc in case wordType of -- EOF
            Id -> [(reverse ((X wordAcc'):tokenAcc), input)] 
            Number -> [(reverse (C (read wordAcc' :: Int):tokenAcc), input)] 
            Other -> [(reverse tokenAcc, input)]

        p' input@(c1:cs) wordAcc tokenAcc started wordType = case c1 of
            symbol | elem symbol ['+', '-', '*', '/', '(', ')'] -> 
                addSimpleLexem input wordAcc tokenAcc started wordType (getSimpleToken c1)
            symbol | elem symbol [' ', '\r', '\n', '\t'] -> 
                if not started then p' cs wordAcc tokenAcc started wordType
                else let wordAcc' = reverse wordAcc in case wordType of
                    Id -> p' cs [] ((X wordAcc'):tokenAcc) False Other
                    Number -> p' cs [] ((C (read wordAcc' :: Int)):tokenAcc) False Other
                    Other -> [] -- Error
            _ -> if isDigit c1 || isAlpha c1 then case wordType of
                     Id -> p' cs (c1:wordAcc) tokenAcc True Id
                     Number ->
                         if isDigit c1 then
                             p' cs (c1:wordAcc) tokenAcc True Number
                         else 
                             p' cs [c1] ((C (read (reverse wordAcc) :: Int)):tokenAcc) True Id
                     Other -> 
                         if isDigit c1 then p' cs (c1:wordAcc) tokenAcc True Number 
                         else p' cs (c1:wordAcc) tokenAcc True Id
                 else pLast input wordAcc tokenAcc started wordType
        
        p' [] wordAcc tokenAcc started wordType = pLast [] wordAcc tokenAcc started wordType


tokenizer string = if length tokenized /= 0 then fst $ head $ tokenized else [] where 
    tokenized = apply tokenize string
