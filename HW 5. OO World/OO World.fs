(*  Artem Lobanov (c) 2014
    OO World
*)

module MascotWorld

open System

[<AbstractClass>]
type Character(name : string) = 
    let mutable mood = 0  
    let mutable fightSkill = 0

    member this.Name = name

    abstract member Mood : int with get, set
    default this.Mood
        with get () = mood
        and set (value) = mood <- value
        
    abstract member FightSkill : int with get, set
    default this.FightSkill
        with get () = fightSkill
        and set (value) = fightSkill <- value


type Mascot(name : string, orgName : string) = 
    inherit Character(name)
        
    member this.OrgName = orgName
    
    override this.ToString () = this.OrgName + " mascot " + this.Name

    member this.Train (time : int) = this.FightSkill <- this.FightSkill + (5 * time)
                                     this.Mood <- this.Mood - (2 * time)

    member this.TrainWithPartner (partner : Mascot) (time : int) = this.Train(time)
                                                                   partner.Train(time)

    member this.Fight (enemy : Mascot) = printfn "%A fights vs %A" (this.ToString ()) (enemy.ToString ())                                       
                                         let judge = new MascotReferee()

                                         Threading.Thread.Sleep(2000)

                                         this.FightSkill <- this.FightSkill + 1000
                                         enemy.FightSkill <- enemy.FightSkill + 1000
                                                              
                                         let selfStrength = this.Mood + this.FightSkill
                                         let enemyStrength = enemy.Mood + enemy.FightSkill
                                         let diffStrength = selfStrength - enemyStrength

                                         if abs diffStrength > 100 then
                                             if diffStrength > 100 then 
                                                 printfn "%A wins by knockout!" (this.ToString ())
                                                 this.Mood <- this.Mood + 2000    
                                                 enemy.Mood <- enemy.Mood - 2000
                                                 1
                                             else
                                                 printfn "%A wins by knockout!" (enemy.ToString ())
                                                 this.Mood <- this.Mood - 2000    
                                                 enemy.Mood <- enemy.Mood + 2000
                                                 -1
                                         else 
                                             let result = judge.MakeDecision(this, enemy, diffStrength) 
                                             
                                             if result = 1 then 
                                                 this.Mood <- this.Mood + 1000
                                                 enemy.Mood <- enemy.Mood - 1000
                                             elif result = -1 then
                                                 this.Mood <- this.Mood - 1000
                                                 enemy.Mood <- enemy.Mood + 1000

                                             result     


and MascotReferee() = 
    inherit Mascot ("Referee Mascot", "None")

    member this.MakeDecision (fighter1 : Mascot, fighter2 : Mascot, diffStrength : int) = 
        printfn "Referee decides..."
        Threading.Thread.Sleep(2000)

        let result = if diffStrength > 0 then 1 elif diffStrength < 0 then -1 else 0

        if result <> 0 then 
            let winnerByDecision = if result = 1 then fighter1 else fighter2
            printfn "%A wins by decisiion!" (winnerByDecision.ToString ())
        else printfn "Dew!"      

        result

type SuperMascot() as this = 
    inherit Mascot ("SuperMascot", "None")

    do 
        this.Mood <- 1000
        this.FightSkill <- 1000

    override this.ToString () = this.Name


type Tux() =
    inherit Mascot("Tux", "Linux")

    override this.ToString () = this.Name

    member this.GetAngry () = this.FightSkill <- 1000
    member this.TuxFight (enemy : Mascot) = let mutable result = this.Fight enemy
    
                                            if result <> 1 then 
                                                this.GetAngry ()                                              
                                                result <- this.Fight enemy
                                            result


type Beastie() = 
    inherit Mascot("Beastie", "BSD")

    override this.ToString () = this.Name
        
    member this.GetHelp (helper : Mascot) = this.Mood <- (this.Mood + helper.Mood)
                                            this.FightSkill <- this.FightSkill + helper.FightSkill

    member this.LeaveHelp (helper : Mascot) = this.Mood <- this.Mood - helper.Mood
                                              this.FightSkill <- this.FightSkill - helper.FightSkill

    member this.BeastieFight (enemy : Mascot) = let mutable result = this.Fight enemy 

                                               if result <> 1 then
                                                   let helper = new SuperMascot ()                 
                                                   this.GetHelp helper
                                                   result <- this.BeastieFight enemy
                                                   this.LeaveHelp helper
                                               result


type Programmer(name : string) =
    inherit Character(name) 

    member this.MascotTournament (mascots : Mascot list) = 
        if mascots.Length = 1 then mascots.Head
        else 
            let rec mascotRound (mascots : Mascot list) (newMascots : Mascot list) =                 
                match mascots with
                | [] -> newMascots
                | [mascot] -> newMascots @ [mascot]
                | mascot1 :: mascot2 :: tl -> let trainTimeGenerator = new Random()
                                              [mascot1; mascot2] |> List.iter (fun (mascot : Mascot) -> mascot.Train (trainTimeGenerator.Next(1, 1000)))
                                              
                                              let result = (match mascot1 with
                                                            | :? Tux as mascotTux -> mascotTux.TuxFight mascot2
                                                            | :? Beastie as mascotBeastie -> mascotBeastie.BeastieFight mascot2
                                                            | _ -> mascot1.Fight mascot2
                                                           )

                                              Threading.Thread.Sleep(2000)

                                              let passedMascots = if result = 1 then [mascot1] elif result = -1 then [mascot2] else [mascot1; mascot2]
                                              mascotRound tl (newMascots @ passedMascots)

            let newMascots = mascotRound mascots []
            printfn "Now left if the tournament: %A" newMascots
            Threading.Thread.Sleep(2000)
            this.MascotTournament newMascots
         
    member this.PlanProject (mascots : Mascot list) = 
        printfn "Initializing Mascot Tournament...."
        Threading.Thread.Sleep(2000)
        let bestMascot = this.MascotTournament mascots

        printfn "And the best mascot is..."
        Threading.Thread.Sleep(2000)
        printfn "%A! And it's mood is %A" bestMascot.Name bestMascot.Mood

        if bestMascot.Mood > 1000 then 
            printfn "It's gonna be a great project!"
        elif bestMascot.Mood > 200 then 
            printfn "It's gonna be rather good project." 
        else 
            printfn "Probably it will be not that good project..."


let tux1 = new Tux()
tux1.Mood <- 50

let beastie1 = new Beastie()
beastie1.Mood <- 1060

let superMan1 = new Mascot("superMan", "Earth")
superMan1.Train 10

let superMan2 = new Mascot("superMan2", "Earth")
superMan2.Train 20

let myMascotList = 
    [superMan1
     superMan2
     tux1 :> Mascot
     beastie1 :> Mascot
    ]
    
let myVasya = new Programmer("Vasya")

myVasya.PlanProject myMascotList
