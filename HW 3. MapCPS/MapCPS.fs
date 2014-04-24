(* Artem Lobanov (c) 2014
   Map function via CPS
*)


let rec factCPS n cont = 
    match n with
    | 0 -> cont 1
    | _ -> factCPS (n - 1) (fun prevFact -> cont (prevFact * n))


let rec mapCPS list func cont = 
    match list with
    | [] -> cont []
    | hd :: tl -> func hd (fun hdMapped -> mapCPS tl func (fun tlMapped -> cont (hdMapped :: tlMapped)))
    

mapCPS [ 1; 2; 3; 4; 5; ] factCPS (printfn "%A")