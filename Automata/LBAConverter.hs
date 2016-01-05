import qualified Data.Text    as Text
import qualified Data.Text.IO as Text
import System.IO.Unsafe -- not a Haskell style, yet works ad hoc for a really simple purpose
import Data.List
import Data.List.Split

data MachineState = MTState String deriving(Show)
data MachineDirection = L | R | N deriving(Show)
data TuringMachine = MT [(MachineState, String, String, MachineDirection, MachineState)] deriving(Show)

data GrammarRule = Rule ([String], [String]) --deriving(Show)
data Grammar = Gr [GrammarRule] deriving(Show)

instance Show GrammarRule where
  show (Rule (list1, list2)) = (concat list1) ++ " -> " ++ (concat list2)

blank = "_"

br :: [String] -> String
br (x:xs) = "[" ++ x ++ br' xs ++ "]" where
  br' (x:xs) = "," ++ x ++ br' xs
  br' []     = ""

inputSymbols = ["0", "1", blank]
allBrackets = [br [x, y, z] | x <- inputSymbols, y <- inputSymbols, z <- inputSymbols]
tapeSymbols  = inputSymbols ++ allBrackets

left  = "L"
right = "R"
still = "*" 

startState = "q0"
goodStates = ["success"]

leftMarker  = "^"
rightMarker = "$"

s  = "S"
s' = "s"


mtString = map Text.unpack $ unsafePerformIO $ fmap Text.lines $ 
  Text.readFile "/Users/resaglow/Dev/spbsu-study/5-semester/MTConverter/machinePrimes.txt"

parseMT :: [String] -> TuringMachine
parseMT mtList = MT $ parseMT' mtList where
  parseMT' [] = []
  parseMT' (curString:xs) = let curList = Data.List.Split.splitOn " " curString in
    let 
      x = curList !! 1
      y = curList !! 2 
    in
      [(MTState $ curList !! 0, wasTape, willBeTape,
        (case curList !! 3 of 
          val | val == left  -> L
          val | val == right -> R
          val | val == still -> N
          _ -> error "Parsing error (dir is not left/right/still"), 
        MTState $ curList !! 4) | 
          wasTape    <- if x /= "*" then [x] else tapeSymbols, 
          willBeTape <- if y   /= "*" then [y]   else tapeSymbols]
      ++ (parseMT' xs)

mt  = parseMT mtString
mtt = case mt of MT list -> list



convert mt = Gr $ 
  simpleRules ++ dirSensitiveRules mt where
    simpleRules = concat [[
        -- 1 Model short input configs
        Rule ([s], [br [startState, leftMarker, a, a, rightMarker]]), 

        -- 3.* Short input symbol recovery
        Rule ([br [q, leftMarker, x, a, rightMarker]], [a]), 
        Rule ([br [leftMarker, q, x, a, rightMarker]], [a]),
        Rule ([br [leftMarker, x, a, q, rightMarker]], [a]),

        -- 4.* Model long input configs
        Rule ([s],  [br [startState, leftMarker, a, a], s']),
        Rule ([s'], [br [a, a], s']),
        Rule ([s'], [br [a, a, rightMarker]]),

        -- 8.* Long input symbol recovery
        Rule ([br [p, leftMarker, x, a]], [a]),
        Rule ([br [leftMarker, q, x, a]], [a]),
        Rule ([br [q, x, a]], [a]),
        Rule ([br [q, x, a, rightMarker]], [a]),
        Rule ([br [x, a, q, rightMarker]], [a]),

        -- 9.* Input word recovery
        Rule ([a, br [x, b]], [a, b]),
        Rule ([a, br [x, b, rightMarker]], [a, b]),
        Rule ([br [x, a], b], [a, b]),
        Rule ([br [leftMarker, x, a], b], [a, b])

      ] | p <- goodStates, q <- goodStates,
          a <- inputSymbols, b <- inputSymbols, 
          x <- tapeSymbols, y <- tapeSymbols, z <- tapeSymbols]

    dirSensitiveRules (MT []) = []
    dirSensitiveRules (MT ((MTState q, x, y, dir, MTState p):xs)) =
      (case elemIndex x goodStates of
        Just _ -> []
        Nothing -> case dir of
          L -> 
            (if x == rightMarker && y == rightMarker then concat [[

                Rule ([br [leftMarker, x, a, q, rightMarker]], [br [leftMarker, p, x, a, rightMarker]]),
                Rule ([br [x, a, q, rightMarker]], [br [p, x, a, rightMarker]])

              ] | a <- inputSymbols, b <- inputSymbols, z <- tapeSymbols]
            else []) ++ concat [[

              Rule ([br [leftMarker, q, x, a, rightMarker]], [br [p, leftMarker, y, a, rightMarker]]),
              Rule ([br [leftMarker, q, x, a]], [br [p, leftMarker, y, a]]),
              Rule ([br [z, b], br [q, x, a]], [br [p, z, b], br [y, a]]),
              Rule ([br [leftMarker, z, b], br [q, x, a]], [br [leftMarker, p, z, b], br [y, a]]),
              Rule ([br [z, b], br [q, x, a, rightMarker]], [br [p, z, b], br [y, a, rightMarker]])

            ] | a <- inputSymbols, b <- inputSymbols, z <- tapeSymbols]

          R -> 
            (if x == leftMarker && y == leftMarker then concat [[

                Rule ([br [q, leftMarker, x, a, rightMarker]], [br [leftMarker, p, x, a, rightMarker]]),
                Rule ([br [q, leftMarker, x, a]], [br [leftMarker, p, x, a]])

              ] | a <- inputSymbols, b <- inputSymbols, z <- tapeSymbols]
            else []) ++ concat [[

              Rule ([br [leftMarker, q, x, a, rightMarker]], [br [leftMarker, y, a, p, rightMarker]]),
              Rule ([br [leftMarker, q, x, a], br [z, b]], [br [leftMarker, y, a], br [p, z, b]]),
              Rule ([br [q, x, a], br [z, b]], [br [y, a], br [p, z, b]]),
              Rule ([br [q, x, a], br [z, b, rightMarker]], [br [y, a], br [p, z, b, rightMarker]]),
              Rule ([br [q, x, a, rightMarker]], [br [y, a, p, rightMarker]])

            ] | a <- inputSymbols, b <- inputSymbols, z <- tapeSymbols]

      ) ++ (dirSensitiveRules (MT xs))

  
converted     = convert mt
convertedList = case converted of Gr list -> list

-- Total num == 16, 10 is fairly reasonable
prettyBracketThreshold = 10
prettyBracketFsts = ['m', 'n', 'k']
prettyBracketSnds = ['0'..'9']

renamePretty :: Grammar -> Grammar
renamePretty (Gr grammar) = Gr $ renamePretty' grammar where
  renamePretty' [] = []
  renamePretty' ((Rule (list1, list2)):rules) = 
    (Rule (changeBrackets list1, changeBrackets list2)):(renamePretty' rules) where
      changeBrackets :: [String] -> [String]
      changeBrackets [] = []
      changeBrackets (x:xs) = (case x of 
        val | val == br [blank, blank, blank] -> "B"
        _ -> (case elemIndex x allBrackets of
          Nothing -> x
          Just index -> 
            ['[', 
             prettyBracketFsts !! (index `div` prettyBracketThreshold),
             prettyBracketSnds !! (index `mod` prettyBracketThreshold),
             ']'])):(changeBrackets xs)

prettyGrammar     = renamePretty converted
prettyGrammarList = case prettyGrammar of Gr list -> list
-- Using "B" notation, minimal valid input to process number 7 (111) for primary numbers checking is:
-- BBBBBBBBBBBBBBBBBq0[1,1][1,1][1,1]B