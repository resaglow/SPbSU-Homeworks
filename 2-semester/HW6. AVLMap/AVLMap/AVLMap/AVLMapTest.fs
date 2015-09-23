module AVLMapTest

open System.Diagnostics

open AVLMap

let mutable myMap = new AVLMap.Map<int, string>([])

for i in [1 .. 10 .. 100] do 
    myMap <- myMap.Add(i, ("value" + i.ToString()))

for node in myMap do
    printfn "%A" node

