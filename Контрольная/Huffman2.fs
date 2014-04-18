module Huffman

open System

// For F# 3.0+ use this:
//type CodeTree = 
//  | Fork of left: CodeTree * right: CodeTree * chars: char list * weight: int
//  | Leaf of char: char * weight: int

// For F# 1.0 - 2.0 use this: 
type CodeTree = 
  | Fork of CodeTree * CodeTree * char list * int
  | Leaf of char * int


// code tree

let weight (tree : CodeTree) : int = 
    match tree with 
    | Fork (_, _, _, weight) -> weight
    | Leaf (_, _) -> 1

let chars (tree : CodeTree) : char list = 
    match tree with 
    | Fork (_, _, charList, _) -> charList
    | Leaf (symbol, _) -> [symbol]

let makeCodeTree left right = Fork(left, right, chars left @ chars right, weight left + weight right)

let string2chars str = Seq.toList str

let times (list : char list) : (char * int) list =
    let rec isKeyInDict dict key = 
        match dict with
        | [] -> false
        | (symbol, _) :: tl -> if key = symbol then true else isKeyInDict tl key

    let rec incKey dict key = 
        match dict with
        | [] -> failwith "Error"
        | (symbol, value) :: tl -> if key = symbol then 
                                       (symbol, value + 1) :: tl 
                                   else 
                                       (symbol, value) :: (incKey tl key)

    let rec times' list acc = 
        match list with
        | [] -> acc
        | hd :: tl -> if isKeyInDict acc hd then times' tl (incKey acc hd) else times' tl ((hd, 1) :: acc)

    times' list []

let makeOrderedLeafList (dict : (char * int) list) : CodeTree list = 
    let rec findMax dict letterList res =
        match dict with
        | [] -> if snd res = 0 then failwith "Empty List" else res
        | (symbol, value) :: tl -> if value > snd res && not (List.exists (fun x -> x = symbol) letterList) then findMax tl letterList (symbol, value) else findMax tl letterList res

    let rec makeOrderedLeafList' dict letterList (resList: CodeTree list) = 
        match dict with 
        | (symbol, value) :: _ -> if dict.Length = resList.Length then resList
                                  else 
                                     let maxsymbol = fst (findMax dict letterList ('a', 0))
                                     let maxvalue = snd (findMax dict letterList ('a', 0))
                                     let maxLeaf = Leaf (maxsymbol, maxvalue)
                                     makeOrderedLeafList' dict (maxsymbol :: letterList) (maxLeaf :: resList) 
        | _ -> failwith "Error" 
               
    makeOrderedLeafList' dict [] []

let myList = ['a'; 'b'; 'a']

let dict = times myList
printfn "%A" dict
let ordLeaf = makeOrderedLeafList dict
printfn "%A" ordLeaf

let singleton (treeList : CodeTree list) = 
    match treeList with 
    | [tree] -> true
    | _ -> false

let combine (treeList: CodeTree list) : CodeTree list = 
    let leastLeaf = treeList.Head
    let sndLeastLeaf = treeList.Tail.Head
    let combinedLeaf = makeCodeTree leastLeaf sndLeastLeaf
    combinedLeaf :: (treeList.Tail.Tail)
    

