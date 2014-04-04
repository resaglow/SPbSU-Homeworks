module Huffman

type CodeTree = 
  | Fork of left: CodeTree * right: CodeTree * chars: char list * weight: int
  | Leaf of char: char * weight: int


// code tree



let baseList = ['d'; 'a'; 'b'; 'c'; 'a'; 'b'; 'c'; 'd']
let baseBits = [1; 1; 0; 0; 1; 0; 1; 1; 1; 0; 1; 0; 1; 1; 1; 1; 1; 0]
let myTree = Fork (Fork (Fork (Leaf ('c', 2), Leaf ('d', 1), ['c'; 'd'], 3), Leaf ('b', 2), ['c'; 'd'; 'b'], 5), Leaf ('a', 3), ['a'; 'b'; 'c'; 'd'], 8)


let createCodeTree (chars: char list) : CodeTree =     
    failwith "Not implemented"

// decode

type Bit = int

//let decode (tree: CodeTree)  (bits: Bit list) : char list = 
//    decodeInner tree bits acc = 
//        match bits with        
//        | [] -> acc
//        | hd :: tl decodeInner tree tl ((binSeqBit) @ [])


// encode

let encode (tree: CodeTree)  (text: char list) : Bit list = 
    let rec isSymbolInList list (symbol: char) = 
        match list with 
        | [] -> false
        | hd :: tl -> if symbol = hd then true else isSymbolInList tl symbol

    let rec symbolBinarySeq tree (symbol: char) acc = 
        match tree with
        | Fork (left, right, nodeList, nodeNum) -> if isSymbolInList nodeList symbol then 
                                                       match left with 
                                                       | Fork (leftLeft, leftRight, leftNodeList, leftNodeNum) -> if isSymbolInList leftNodeList symbol then symbolBinarySeq left symbol (1 :: acc)
                                                                                                                  else symbolBinarySeq right symbol (0 :: acc)
                                                       | Leaf (leftLeafSymbol, leftSymbolNum) -> if symbol = leftLeafSymbol then symbolBinarySeq left symbol (1 :: acc)
                                                                                                 else symbolBinarySeq right symbol (0 :: acc)
                                                   else failwith "Error"
                                                       
        | Leaf (leafSymbol, symbolNum) -> if symbol = leafSymbol then List.rev acc
                                          else failwith "Error"

    let rec encodeInner tree text acc = 
        match text with 
        | [] -> acc
        | hd :: tl -> encodeInner tree tl (acc @ (symbolBinarySeq tree hd []))

    encodeInner tree text []

let result = encode myTree baseList

printfn "%A" result
