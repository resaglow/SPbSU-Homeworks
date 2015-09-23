module CloudyTests

open Cloudy
open NUnit.Framework
open NSubstitute
open FsCheck

type Generator = 
    static member int() = Arb.fromGen <| Gen.choose(0, 10)

[<Test>]
let cloudTest() =         
    let mockTestProperty(time:DaylightType, isShining:bool, windSpeed:int) =
        let lightType = Substitute.For<IDaylight>()
        let luminary = Substitute.For<ILuminary>()
        let wind = Substitute.For<IWind>() 
        let magic = Substitute.For<IMagic>()  
        let courier = Substitute.For<ICourier>()

        lightType.Current.Returns(time) |> ignore 
        luminary.IsShining.Returns(isShining) |> ignore
        wind.Speed.Returns(windSpeed) |> ignore 
        magic.CallStork().Returns(courier) |> ignore
        magic.CallDaemon().Returns(courier) |> ignore

        let checkOrder (courierType:CourierType) (creatureType:CreatureType) = 
            if courierType = Stork then 
                Received.InOrder(fun () ->
                    magic.CallStork() |> ignore
                    courier.Received().GiveBaby(Arg.Is<Creature>(fun (creature:Creature) -> creature.Type = creatureType))
                    )
            else 
                Received.InOrder(fun () ->
                    magic.CallDaemon() |> ignore
                    courier.Received().GiveBaby(Arg.Is<Creature>(fun (creature:Creature) -> creature.Type = creatureType))
                    )

        let mockCloud = new Cloud(lightType, luminary, wind, magic)

        mockCloud.Create() |> ignore

        if isShining then
            if windSpeed >= 0 && windSpeed <= 5 then
                if time = Morning || time = Day then checkOrder Stork Puppy
                else checkOrder Stork Kitten
            else 
                if time = Morning || time = Evening then checkOrder Daemon Hedgehog
                else checkOrder Stork Bat
        else 
            if windSpeed >= 0 && windSpeed <= 3 then 
                if time = Night then checkOrder Daemon Balloon
                else if time = Day || time = Morning then checkOrder Daemon Bearcub
                else checkOrder Stork Piglet
            else if windSpeed >= 4 && windSpeed <= 7 then checkOrder Stork Balloon

            else checkOrder Daemon Hedgehog
    
    Arb.register<Generator>() |> ignore
    FsCheck.Check.Verbose(mockTestProperty)