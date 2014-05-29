(* Artem Lobanov (c) 2014
   Square area paths
*)


let rec fact n = if n < 2 then 1I else bigint(n) * fact (n - 1)

let squarePaths sideLength = fact (2 * sideLength) / (fact sideLength * fact sideLength)

printfn "%A" (squarePaths 20)