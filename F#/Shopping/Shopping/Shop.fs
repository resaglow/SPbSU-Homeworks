module Shop
 
type Good =
    | Vodka
    | Beer of string
    | Whiskey
    | Wine
    | Milk
 
type IShop =
    abstract member CanSell : Good -> bool
    abstract member Sell : Good list -> Good list
 
type AlkoShop() =
    let rnd = new System.Random()
    
    interface IShop with
        member x.CanSell (good : Good) = 
            let now = System.DateTime.Now
            let isAlcoAllowed = now.Hour > 9 && now.Hour < 22
 
            isAlcoAllowed || good = Milk || rnd.Next(10) < 1
 
        member x.Sell (goods:Good list) = 
            List.filter (x :> IShop).CanSell goods