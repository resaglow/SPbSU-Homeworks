module Tokenizer where

import ParserCombinators
import qualified ParserCombinators

oneOf :: [Parser s a] -> Parser s a
oneOf = foldl (\ a b -> a ||| b) ParserCombinators.fail

letter = oneOf (map sym "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_")
digit  = oneOf (map sym "0123456789")
ws     = oneOf (map sym " \t\n\r")

whitespace = many ws

data Token = X String | C Int | A | S | M | D | LP | RP deriving Show

ident :: Parser String Token
ident = letter ||> (  
          \x  -> many (letter ||| digit) ||> (  
          \xs -> val (X (x:xs))
        ))

int :: Parser String Token
int = some digit ||> val . C . read

add :: Parser String Token
add = sym '+' ||> lift (val A)

sub :: Parser String Token
sub = sym '-' ||> lift (val S)

mul :: Parser String Token
mul = sym '*' ||> lift (val M)

dv :: Parser String Token
dv = sym '/' ||> lift (val D)

lp :: Parser String Token
lp = sym '(' ||> lift (val LP)

rp :: Parser String Token
rp = sym ')' ||> lift (val RP)

tokenizer :: Parser String [Token]
tokenizer = whitespace ||> lift (many token) where
  token = oneOf [ident, int, add, sub, mul, dv, lp, rp] ||> (
          \x -> whitespace ||> lift (val x)
          )

tokenize :: String -> [Token]
tokenize s = 
  case eof $ apply tokenizer s of
    [tokens] -> tokens
    []       -> error "no tokenization"
    _        -> error "ambiguous tokenization"



