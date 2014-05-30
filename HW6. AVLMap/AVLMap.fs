(*  Artem Lobanov (c) 2014
    AVLTree-based Map      
*)


type AVLTree<'K, 'V> = 
        | Empty
        | Node of left: AVLTree<'K, 'V> * right: AVLTree<'K, 'V> * key: 'K * value: 'V * leftHeight: int * rightHeight: int    

module AVLTreeMap = 
    open System.Collections.Generic
    open System.Collections

    type Path = | Left | Right

    let getHeight = function
        | Empty -> 0 
        | Node(_, _, _, _, lh, rh) -> max lh rh + 1    
    
    let getBalance = function
        | Empty -> 0
        | Node(_, _, _, _, lh, rh) -> lh - rh   
    
    let getLeft = function
        | Empty -> failwith "trying to get a left subtree of an empty tree"
        | Node(lt, _, _, _, _, _) -> lt  
        
    let getRight = function
        | Empty -> failwith "trying to get a right subtree of an empty tree"  
        | Node(_, rt, _, _, _, _) -> rt 
        
    let isEmpty = function
        | Empty -> true
        | _ -> false                                      

    let balance node = 
        if getBalance node > 1 && getBalance (getLeft node) >= 0 then
            // LL rotation
            match node with
            | Node(Node(blt, brt, bk, bv, blh, brh), art, ak, av, _, arh) ->
                let newA = Node(brt, art, ak, av, brh, arh)
                Node(blt, newA, bk, bv, blh, getHeight newA) 
            | _ -> failwith "Error"

        elif getBalance node > 1 && getBalance (getLeft node) < 0 then
            // LR rotation    
            match node with
            | Node(Node(blt, Node(clt, crt, ck, cv, clh, crh), bk, bv, blh, _), art, ak, av, _, arh) ->
                let newA = Node(crt, art, ak, av, crh, arh)
                let newB = Node(blt, clt, bk, bv, blh, clh)
                Node(newB, newA, ck, cv, getHeight newB, getHeight newA)
            | _ -> failwith "Error"

        else if getBalance node < -1 && getBalance (getRight node) <= 0 then 
            // RR rotation
            match node with
            | Node(alt, Node(blt, brt, bk, bv, blh, brh), ak, av, alh, _) ->
                let newA = Node(alt, blt, ak, av, alh, blh)
                Node(newA, brt, bk, bv, getHeight newA, brh)
            | _ -> failwith "Error"

        else if getBalance node < -1 && getBalance (getRight node) <= 0 then 
            // RL rotation
            match node with 
            | Node(alt, Node(Node(clt, crt, ck, cv, clh, crh), brt, bk, bv, _, brh), ak, av, alh, _) ->
                let newA = Node(alt, clt, ak, av, alh, clh)
                let newB = Node(crt, brt, bk, bv, crh, crh)
                Node(newA, newB, ck, cv, getHeight newA, getHeight newB)
            | _ -> failwith "Error"

        else node

    let rec add key value = function
        | Empty -> Node(Empty, Empty, key, value, 0, 0)
        | Node(lt, rt, k, v, lh, rh) -> 
            if key = k then Node(lt, rt, k, value, lh, rh)
            else 
                if key > k then
                    let unbalancedRT = add key value rt
                    if abs (lh - getHeight unbalancedRT) >= 2 then balance (Node(lt, unbalancedRT, k, v, lh, getHeight unbalancedRT))
                    else Node(lt, unbalancedRT, k, v, lh, getHeight unbalancedRT)                    
                else
                    let unbalancedLT = add key value lt
                    if abs (getHeight unbalancedLT - rh) >= 2 then balance (Node(unbalancedLT, rt, k, v, lh, getHeight unbalancedLT))
                    else Node(unbalancedLT, rt, k, v, getHeight unbalancedLT, rh)
   
            
    let rec remove key tree = 
        let rec getNext = function
            | Empty -> failwith "Looking for the next key in an Empty tree"
            | Node(Empty, rt, nextK, nextV, lh, rh) as nextElem -> (rt, (nextK, nextV)) // rt is now either Empty or a single node
            | Node(lt, rt, k, v, lh, rh) as curNode -> 
                let (newLT, (nextK, nextV)) = getNext lt
                (Node(newLT, rt, k, v, getHeight newLT, rh), (nextK, nextV))

        match tree with 
        | Empty -> 
            printfn "No such element to remove in the tree"
            Empty
        | Node(lt, rt, k, v, lh, rh) ->
            if key = k then 
                if lt = Empty then rt
                elif rt = Empty then lt
                else 
                    match rt with
                    | Empty -> lt
                    | _ -> 
                        let (newRT, (nextK, nextV)) = getNext rt
                        if abs (lh - getHeight newRT) >= 2 then balance (Node(lt, newRT, nextK, nextV, lh, getHeight newRT))
                        else Node(lt, newRT, nextK, nextV, lh, getHeight newRT)
                        
            elif key > k then
                let newRT = remove key rt
                balance (Node(lt, newRT, k, v, lh, getHeight newRT))
            else 
                let newLT = remove key lt
                balance (Node(newLT, rt, k, v, getHeight newLT, rh))

    let rec tryFind key = function
        | Empty -> None
        | Node(lt, rt, k, v, _, _) ->
            if key = k then Some(v)
            else if key > k then tryFind key rt
            else tryFind key lt    

    type Map<'K, 'V when 'K: comparison and 'V: comparison>(givenTree: AVLTree<'K, 'V>) =

        member this.Empty = isEmpty givenTree
        member this.Add k v = new Map<_, _>(add k v givenTree)
        member this.Remove k = new Map<_, _>(remove k givenTree)
        member this.TryFind k = tryFind k givenTree
        member this.ContainsKey key = this.TryFind key |> Option.isSome
        member this.Item key = (this.TryFind key).Value

        member this.Count() = 
            let rec count = function
                | Empty -> 0
                | Node(lt, rt, _, _, _, _) -> count lt + 1 + count rt
            count givenTree

        override this.ToString() = 
            let rec mapToString= function
                | Empty -> ""
                | Node(lt, rt, k, v, _, _) -> mapToString lt + "key: " + k.ToString() + " - value: " + v.ToString() + "\n" + mapToString rt
            mapToString givenTree

        override this.Equals anotherMap =
            match anotherMap with
            | :? Map<'K, 'V> as anotherMap -> (anotherMap.Count() = this.Count()) && (Seq.forall2 (=) this anotherMap)
            | _ -> false

        member private this.getEnumerator() = 
            let rec toList = function
                | Empty -> [] 
                | Node(lt, rt, _, _, _, _) as curNode -> toList lt @ curNode :: toList rt

            let startList = Empty :: toList givenTree
            let remainedList = ref startList

            let current() = 
                match (!remainedList).Head with 
                | Empty -> failwith "Trying to get current at the starting position"
                | Node(_, _, k, v, _, _) -> (k, v)

            { new IEnumerator<'K * 'V> with
                  member enum.Current = current()
              interface IEnumerator with
                  member enum.Current = current() :> obj
                  member enum.MoveNext() = 
                      remainedList := (!remainedList).Tail
                      not (!remainedList).IsEmpty
                    
                  member enum.Reset() = remainedList := startList
              interface System.IDisposable with 
                  member enum.Dispose() = () 
            }       

        interface IEnumerable<'K * 'V> with
            member this.GetEnumerator() = this.getEnumerator()
        interface IEnumerable with
            member this.GetEnumerator() = this.getEnumerator() :> IEnumerator
            