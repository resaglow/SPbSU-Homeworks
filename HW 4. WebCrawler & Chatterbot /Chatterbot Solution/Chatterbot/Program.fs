(*  Artem Lobanov (c) 2014
    Chatterbot WinForms App 
*)

module FormExample

open System
open System.Windows.Forms
open ChatterBotAPI

let myBotFactory = new ChatterBotFactory()
let bot1 = myBotFactory.Create(ChatterBotType.PANDORABOTS, "b0dafd24ee35a477");
let bot1Session = bot1.CreateSession()

let myForm = new Form(Text = "Chatterbot (My message)", Height = 300, Width = 300)
let sendButton = new Button(Text = "Send", Top = 200, Left = 100)
let messageBox = new TextBox(Text = "Type your text", Top = 150, Left = 50, Width = 200)
let waitLabel = new Label(Top = 50, Left = 50, Height = 100, Width = 200)

let botForm = new Form(Text = "Chatterbot (Bot's message)", Height = 300, Width = 300)
let answerButton = new Button(Text = "Answer", Top = 200, Left = 100)
let botAnswerLabel = new Label(Top = 50, Left = 50, Height = 100, Width = 200)

sendButton.Click.Add (fun _ -> let myMessage = messageBox.Text

                               messageBox.Text <- "";
                               waitLabel.Text <- "Waiting for answer"   
                               myForm.Update()                   
                                                
                               let botAnswer = bot1Session.Think(myMessage)

                               myForm.Hide()

                               botAnswerLabel.Text <- botAnswer
                               botForm.Show()
                               )

answerButton.Click.Add (fun _ -> botForm.Hide()

                                 waitLabel.Text <- "" 
                                 myForm.Show()          
                                 )

let StartChat () = 
    myForm.Controls.AddRange [|messageBox; sendButton; waitLabel|]
    botForm.Controls.AddRange [|answerButton; botAnswerLabel|]

    myForm.Show()

    Application.Run()

StartChat ()