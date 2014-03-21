(*  Artem Lobanov (c) 2014
    Sum of N filtered fibonacci numbers
*)

let fiboFunc x = 
    x % 2 = 0

let finalFibo fiboFunc lastFibo = 
    let rec fiboEvenSum a b n =
          if (a + b) < n then
            if fiboFunc (a + b) then
                (a + b) + fiboEvenSum b (a + b) n
            else fiboEvenSum b (a + b) n            
          else 0

    fiboEvenSum 0 1 lastFibo

printfn "%d" (finalFibo fiboFunc 4000000)