// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

module Cloudy

type CreatureType = 
    | Puppy
    | Kitten
    | Hedgehog
    | Bearcub
    | Piglet
    | Bat
    | Balloon

type DaylightType = 
    | Morning
    | Day
    | Evening
    | Night

type IDaylight = 
    abstract member Current : DaylightType

type ILuminary = 
    abstract member IsShining : bool

type IWind = 
    abstract member Speed : int
   
type Creature(crType:CreatureType) = 
    member val Type = crType with get

type CourierType = 
    | Stork
    | Daemon

type ICourier = 
    abstract member GiveBaby : Creature -> unit

type IMagic = 
    abstract member CallDaemon : unit -> ICourier
    abstract member CallStork : unit -> ICourier
    

type Cloud(daylight:IDaylight, luminary:ILuminary, wind:IWind, magic:IMagic) = 
    member private x.InternalCreate() =
        match luminary.IsShining, wind.Speed, daylight.Current with
        | true, speed, time ->
            if speed >= 0 && speed <= 5 then
                match time with | Morning | Day -> (Stork, Puppy) 
                                | _ -> (Stork, Kitten)
            else if speed >= 6 && speed <= 10 then
                match time with | Morning | Evening -> (Daemon, Hedgehog)
                                | _ -> (Stork, Bat)
            else failwith "speedoverflow"

        | false, speed, time ->
            if speed >= 0 && speed <= 3 then
                match time with | Night -> (Daemon, Balloon)
                                | Day | Morning -> (Daemon, Bearcub)
                                | Evening -> (Stork, Piglet)
            else if speed >= 4 && speed <= 7 then (Stork, Balloon)
            else if speed >= 7 && speed <= 10 then (Daemon, Hedgehog)
            else failwith "speedoverflow"
 
    member x.Create() =
        let (courierType, creatureType) = x.InternalCreate()
        let creature = Creature(creatureType)

        if courierType = Stork then 
            magic.CallStork().GiveBaby(creature)
        else 
            magic.CallDaemon().GiveBaby(creature)
        
        creature
