module ShoppingTests.Tests 
 
open NUnit.Framework
open NSubstitute
open Shop
open Customer


 
type EmptyShop() = 
    interface IShop with
        member x.CanSell (good : Good) = false
        member x.Sell (goods:Good list) = []
 
type AnytimeFullShop() = 
    interface IShop with
        member x.CanSell (good : Good) = true
        member x.Sell (goods:Good list) = goods
 
type TestCalendar (day : System.DayOfWeek) =
    interface Customer.ICalendar  with
        member x.DayOfWeek = day
 
let calendar day = new TestCalendar(day) :> Customer.ICalendar
let calendarFriday = calendar System.DayOfWeek.Friday 
   
[<Test>]
let test1 () = 
    let customer = new Customer.Customer(calendarFriday)
    Assert.IsFalse(customer.IsDrunk)
 
[<Test>]
let test2 () = 
    let customer = new Customer.Customer(calendarFriday)
    let emptyShop = new EmptyShop()
 
    customer.GoShopping emptyShop
    customer.GetDrunk ()
    Assert.IsFalse(customer.IsDrunk)
 
[<Test>]
let test3 () = 
    let customer = new Customer.Customer(calendarFriday)
    let allInclusiveShop = new AnytimeFullShop()
 
    customer.GoShopping allInclusiveShop
    customer.GetDrunk ()
    Assert.IsTrue(customer.IsDrunk)
 
[<Test>]
let ``test not drunk on Thursday`` () = 
    let customer = new Customer.Customer(calendar System.DayOfWeek.Thursday)
    let allInclusiveShop = new AnytimeFullShop()
 
    customer.GoShopping allInclusiveShop
    customer.GetDrunk ()
    Assert.IsFalse(customer.IsDrunk)
 

let shouldDrunk (day : System.DayOfWeek) = 
    let customer = new Customer.Customer(calendar day)
    let allInclusiveShop = new AnytimeFullShop()
 
    customer.GoShopping allInclusiveShop
    customer.GetDrunk ()
    not (customer.IsDrunk) || day = System.DayOfWeek.Friday
 
[<Test>]
let testQuick () = FsCheck.Check.Quick shouldDrunk

[<Test>] 
let methodOrderTest () = 
    let methodOrder (day:System.DayOfWeek) = 
        let mockShop = Substitute.For<IShop>()
        let customer = new Customer(calendar day)
  
        customer.GoShopping(mockShop)

        Received.InOrder(fun () -> 
            mockShop.CanSell(Arg.Any<Good>()) |> ignore
            mockShop.Sell(Arg.Any<list<Good>>()) |> ignore
            )

    FsCheck.Check.Quick methodOrder


[<Test>]
let canSellTest () =
    // Behaviour doesn't depend on a specific day, for example taking monday
    let mockShop = Substitute.For<IShop>()
    let customer = new Customer.Customer(calendar System.DayOfWeek.Monday)
    let newGoods:ref<Good list> = ref []
    
    mockShop.CanSell(Arg.Any<Good>()).Returns(false) |> ignore

    mockShop.Sell(Arg.Any<list<Good>>()).
        Returns(fun parameters -> 
            let gotGoods = List.filter (mockShop.CanSell) (parameters.[0] :?> list<Good>)
            newGoods := gotGoods
            gotGoods) |> ignore

    customer.GoShopping mockShop

    Assert.IsTrue(newGoods.Value = [])


