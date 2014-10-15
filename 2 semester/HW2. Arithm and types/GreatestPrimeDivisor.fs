(*  Artem Lobanov (c) 2014
    Largest prime divisor of a number
*)

let LargestPrimeDivisor x = 
    let rec innerFunc x y = 
        if (x = y) then y
        else
            if x % y = 0L then
                innerFunc (x / y) y
            else 
                innerFunc x (y + 1L)

    innerFunc x 2L
                
printfn "%d" (LargestPrimeDivisor 4444444L)