(*  Hexic
    Greedy algorithm
    Artem Lobanov (c) 2014
*)

module Hexic 

let N, M = 5, 6
let colorsCount = 5
let empty = 0
type RotateType = CW | CCW
let countPoints = function 
    | 1 -> 0
    | 2 -> 0
    | 3 -> 3
    | 4 -> 6
    | 5 -> 10
    | x -> x * (x + 1) / 2

let random = new System.Random(121)
let mutable field = Array2D.init N M (fun _ _ -> random.Next(10) % 5 + 1) 

let neighbours i j = 
    let possibleNeighbours invalidList = [for p in -1..1 do 
                                              for q in -1..1 do 
                                                  if not (List.exists ((=) (p, q)) invalidList) 
                                                      then yield (i + p, j + q)]

    let pNeighbours = if j % 2 = 0 then possibleNeighbours [(0, 0); (1, -1); (1, 1)]
                      else possibleNeighbours [(0, 0); (-1, -1); (-1, 1)]

    List.filter (fun (x, y) -> x >= 0 && x < N && y >= 0 && y < M && field.[x, y] = field.[i, j]) pNeighbours

let rec bfs gone front = 
    let newFront = Seq.toList <| (Seq.distinct <| [for (i, j) in front do
                                                       for (p, q) in neighbours i j do
                                                           if not (List.exists ((=) (p, q)) gone) && 
                                                              not (List.exists ((=) (p, q)) front) && 
                                                              field.[p, q] = field.[i, j] 
                                                                  then yield (p, q)])
    if newFront <> [] then bfs (front @ gone) newFront
    else front @ gone

// Returns triples clockwise
let allTriples () = 
    [for j in 0..M - 1 do 
         for i in 0..N - 1 do       
             if i <> N - 1 && field.[i, j] <> empty then 
                 if j % 2 = 0 then
                     if j <> 0 then yield [(i, j); (i + 1, j); (i, j - 1)]                
                     if j <> M - 1 then yield [(i, j); (i, j + 1); (i + 1, j)]                
                 else 
                     yield [(i, j); (i + 1, j); (i + 1, j - 1)]
                     if j <> M - 1 then yield [(i, j); (i + 1, j + 1); (i + 1, j)]]


let sameColoredTriples () = 
    List.filter (fun triple -> 
        match triple with
        | (i1, j1) :: (i2, j2) :: (i3, j3) :: [] ->
            field.[i1, j1] = field.[i2, j2] && field.[i1, j1] = field.[i3, j3] 
        | _ -> failwith "Triple is not actually a triple"
        ) <| allTriples ()

let shiftEmpties () = 
    for j in 0..M - 1 do
        let filteredColumn = (Array.filter <| ((<>) empty) <| field.[*, j])
        let columnLen = Array.length filteredColumn
        field.[*, j] <- [| for i in 0..N - columnLen - 1 do yield random.Next(10) % colorsCount + 1
                           for i in N - columnLen..N - 1 do yield filteredColumn.[i - (N - columnLen)] |]

// Presumably 1 2 3 are clockwise
let rotate rotateType = function
    | [(i1, j1); (i2, j2); (i3, j3)] ->
        let cell1, cell2, cell3 = field.[i1, j1], field.[i2, j2], field.[i3, j3]
        match rotateType with
        | CW ->  field.[i1, j1] <- cell3, field.[i2, j2] <- cell1, field.[i3, j3] <- cell2
        | CCW -> field.[i1, j1] <- cell2, field.[i2, j2] <- cell3, field.[i3, j3] <- cell1
    | _ -> failwith "Triple is not actually a triple"

let hexic () =
    let started = ref false
    let points = ref 0

    let removeAndShift (sameColored:(int * int) list list) = 
        let toRemove = bfs <| [] <| sameColored.[0]
        if !started then points := !points + countPoints <| toRemove.Length else ()
        for (i, j) in toRemove do field.[i, j] <- empty
        shiftEmpties ()

    let rec initGame () = 
        let triples = sameColoredTriples ()

        if triples = [] then
            started := true
            printfn "INIT END."
        else 
            removeAndShift triples
            initGame ()

    let rec play all =      
        match all with
        | triple :: rest -> 
            rotate CW triple
            let sameColored = sameColoredTriples ()
            if sameColored = [] then
                rotate CW triple // CW == CCW CCW
                let sameColored2 = sameColoredTriples ()
                if sameColored2 = [] then
                    rotate CW triple
                    play rest
                else removeAndShift (sameColored2)
                     play <| allTriples ()  
            else removeAndShift (sameColored)
                 play <| allTriples ()  
            
        | [] -> printfn "TAAAAAA DAAAAAAAAAA, THEEEE EEEEEEENDDD!!!"
        | _ -> failwith "Stange triple"

    initGame ()
    play <| allTriples ()
    
    printfn "%A points overall" !points


hexic ()