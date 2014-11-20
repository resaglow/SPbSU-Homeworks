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
        if luminary.IsShining then
            if wind.Speed >= 0 && wind.Speed <= 5 then  
                if daylight.Current = Morning || daylight.Current = Day then (Stork, Puppy) 
                else (Stork, Kitten)
            else 
                if daylight.Current = Morning || daylight.Current = Evening then (Daemon, Hedgehog)
                else (Stork, Bat)
        else 
            if wind.Speed >= 0 && wind.Speed <= 3 then 
                if daylight.Current = Night then (Daemon, Balloon)
                else if daylight.Current = Day || daylight.Current = Morning then (Daemon, Bearcub)
                else (Stork, Piglet)
            else if wind.Speed >= 4 && wind.Speed <= 7 then (Stork, Balloon)

            else (Daemon, Hedgehog)
 
    member x.Create() =
        let (courierType, creatureType) = x.InternalCreate()
        let creature = Creature(creatureType)

        if courierType = Stork then 
            magic.CallStork().GiveBaby(creature)
        else 
            magic.CallDaemon().GiveBaby(creature)
        
        creature
