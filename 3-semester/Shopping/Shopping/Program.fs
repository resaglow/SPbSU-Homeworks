// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

[<EntryPoint>]
let main argv = 
    let shop = new Shop.AlkoShop()
    let customer = new Customer.Customer(new Customer.Calendar() :> Customer.ICalendar)

    customer.GoShopping shop
    customer.GetDrunk()
    printfn "%A" (customer.IsDrunk)

    0 // return an integer exit code
