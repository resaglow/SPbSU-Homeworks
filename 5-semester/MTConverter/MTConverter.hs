import qualified Data.Text    as Text
import qualified Data.Text.IO as Text
import System.IO.Unsafe -- real bad Haskell code, yet works ad hoc for a really simple purpose
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
f = "f" -- special "end" symbol

br x y = "[" ++ x ++ "," ++ y ++ "]"

valuableSymbols = ["0", "1"]
inputSymbols    = ["0", "1", blank]
tapeSymbols     = ["0", "1", "X", blank]

left = "L"
right = "R"
still = "*" 

allBrackets = [br x y | x <- inputSymbols, y <- tapeSymbols]

goodStates = ["success"]



mtString = map Text.unpack $ unsafePerformIO $ fmap Text.lines $ 
  Text.readFile "/Users/resaglow/Dev/spbsu-study/5-semester/MTConverter/machinePrimes.txt"

parseMT :: [String] -> TuringMachine
parseMT mtList = MT $ parseMT' mtList where
  parseMT' [] = []
  parseMT' (curString:xs) = let curList = Data.List.Split.splitOn " " curString in
    let 
      tapeInitial = curList !! 1
      tapeFinal = curList !! 2 
    in
      [(MTState $ curList !! 0, wasTape, willBeTape,
        (case curList !! 3 of 
          val | val == left  -> L
          val | val == right -> R
          val | val == still -> N
          _ -> error "Parsing error (direction is not left/right/still"), 
        MTState $ curList !! 4) | 
          wasTape    <- if tapeInitial /= "*" then [tapeInitial] else tapeSymbols, 
          willBeTape <- if tapeFinal   /= "*" then [tapeFinal]   else tapeSymbols]
      ++ (parseMT' xs)

mt  = parseMT mtString
mtt = case mt of MT list -> list



convert mt = Gr $
  -- 1st stage (hardcode here seems ok)
  [Rule (["S"],  ["[_,_]", "S"]),  Rule (["S"],  ["S", "[_,_]"]),  Rule (["S"],  ["qq"])] ++
  [Rule (["qq"], ["qq", "[0,0]"]), Rule (["qq"], ["qq", "[1,1]"]), Rule (["qq"], ["q0"])]  ++

  -- 2nd (main part)
  convert' mt ++

  -- 3rd
  [Rule ([x], [f]) | x <- goodStates] ++
  [Rule ([f, br x y], [f, x, f]) | x <- valuableSymbols, y <- tapeSymbols] ++
  [Rule ([br x y, f], [f, x, f]) | x <- valuableSymbols, y <- tapeSymbols] ++
  [Rule ([f, br blank x], [f])   | x <- tapeSymbols] ++
  [Rule ([br blank x, f], [f])   | x <- tapeSymbols] ++
  [Rule ([f], [])]
  
  where
    convert' :: TuringMachine -> [GrammarRule]
    convert' (MT []) = []
    convert' (MT ((MTState fromState, tapeInitial, tapeFinal, direction, MTState toState):xs)) = 
      (case direction of
        L -> [Rule ([x, fromState, br y tapeInitial], 
                    [toState, x, br y tapeFinal]) | x <- allBrackets, y <- inputSymbols]

        R -> [Rule ([fromState, br y tapeInitial, x], 
                    [br y tapeFinal, toState, x]) | x <- allBrackets, y <- inputSymbols]

        N -> [Rule ([fromState, br y tapeInitial], 
               [toState, br y tapeFinal]) | y <- inputSymbols]
      ) ++ (convert' (MT xs))

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
          val | val == br blank blank -> "B"
          _ -> x):(changeBrackets xs)

        -- Alternative renaming
        --(case elemIndex x allBrackets of
        --  Nothing -> x
        --  Just index -> 
        --    ([prettyBracketFsts !! (index `div` prettyBracketThreshold),
        --      prettyBracketSnds !! (index `mod` prettyBracketThreshold)])):(changeBrackets xs)

prettyGrammar     = renamePretty converted
prettyGrammarList = case prettyGrammar of Gr list -> list
-- Using "B" notation, minimal valid input to process number 7 (111) for primary numbers checking is:
-- BBBBBBBBBBBBBBBBBq0[1,1][1,1][1,1]B