(*  Artem Lobanov (c) 2014
    Basic List Functions
*)

let rec listLen list = // 1
    match list with
    | [] -> 0
    | head::tail -> 1 + listLen tail


let rec listSum list = // 2
    match list with 
    | [] -> 0
    | head::tail -> head + listSum tail


let rec concat list1 list2 = // 3 
    match list1 with
    | [] -> list2
    | head::tail -> head::(concat tail list2)


let rec addBack list value = // 4
    concat list [value]


let rec printBack list= 
    match list with
    | [] -> []
    | head::tail -> addBack (printBack tail) head

                
let rec ffilter func list = // 5
    match list with
    | [] -> []
    | head::tail -> if (func head) then
                        head::(ffilter func tail)
                    else 
                        ffilter func tail


let squares = // 6
   let rec getsquares cur n = 
       let cur_sq = cur * cur
       if cur_sq <= n then 
           (cur_sq)::(getsquares (cur + 1) n)
       else []
   getsquares 1
 

let n = 130
let testList = [1; 2; 3; 4; 100]


printfn "%A" (mapp (fun x -> x * 2) testList)
printfn "Length of list equals to %d" (listLen testList)
printfn "Sum of list elements equals to %d" (listSum testList)
printfn "Turned around list is: %A" (printBack testList)
printfn "Outfiltered list is: %A" (ffilter (fun x -> x % 2 = 0) testList)
printfn "List of squares up to n is: %A" (squares n)