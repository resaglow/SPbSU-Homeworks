module Customer
 
open Shop
 
type ICalendar =
    abstract member DayOfWeek : System.DayOfWeek
 
type Calendar() =
    interface ICalendar  with
        member x.DayOfWeek = System.DateTime.Now.DayOfWeek
 
type Customer (calendar : ICalendar) = 
    let mutable goods = []
    let mutable isDrunk = false
 
    member x.GoShopping (shop : IShop) = 
        if shop.CanSell Whiskey then
            goods <- shop.Sell [Milk; Whiskey; Whiskey]
        else
            goods <- shop.Sell [Milk; Vodka; Beer "dark, Vas'ka"]
 
    member x.GetDrunk () =     
        if calendar.DayOfWeek = System.DayOfWeek.Friday then
            let (nonAlco, alco) = List.partition ((=) Good.Milk) goods
            isDrunk <- isDrunk || alco <> []
            goods <- nonAlco
        
    member x.IsDrunk = isDrunk