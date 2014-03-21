(*  Artem Lobanov (c) 2014
    Sum of digits of N's factorial
*)

let factSum number = 
    let rec fact n =
        if n = 0 then 1I
        else bigint(n) * fact (n - 1)
        
    let rec digitSum (n: bigint) = 
        if n = 0I then 0
        else int (n % 10I) + digitSum (n / 10I)

    digitSum (fact number)

printfn "%A" (factSum 100)