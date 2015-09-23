(*  Artem Lobanov (c) 2014
    Arithmetic expression simplifier
*)

type Expr =
    | Const of int
    | Var of string
    | Add of Expr * Expr
    | Sub of Expr * Expr
    | Mul of Expr * Expr
    | Div of Expr * Expr

let rec calc expr =
    let calcable operand1 operand2 exprFunc arithmFunc =
        match (calc operand1, calc operand2) with
        | (Const aResult, Const bResult) -> Const (arithmFunc aResult bResult)
        | _ -> exprFunc (calc operand1, calc operand2)

    match expr with
    | Const a -> Const a
    | Var x -> Var x
    | Add (a, b) -> match (calc a, calc b) with
                        | (Const 0, Var bResult) -> Var bResult
                        | (Var aResult, Const 0) -> Var aResult
                        | _ -> calcable a b Add (+)
    | Sub (a, b) -> match (calc a, calc b) with
                        | (Var aResult, Var bResult) -> 0
                        | _ -> calcable a b Add (-)
    | Mul (a, b) -> match (calc a, calc b) with
                        | (Const 0, Var bResult) | (Var aResult, Const 0) -> 0
                        | _ -> calcable a b Add (*)
    | Div (a, b) -> match (calc a, calc b) with
                        | (Var aResult, Const 1) -> aResult
                        | _ -> calcable a b Add (-)

printfn "%A" (calc(Add(Div(Const 18, Const 2), Add((Const 0), (Var "x")))))