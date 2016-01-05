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

let myForm = new Form(Text = "Chatterbot", Height = 400, Width = 800)
myForm.Icon <- new System.Drawing.Icon("icon.ico")

let sendButton = new Button(Text = "Send", Top = 320, Left = 700)
let messageBox = new TextBox(Text = "Type your text", Top = 322, Left = 8, Width = 680)
let waitLabel = new Label(Top = 300, Left = 350, Height = 50, Width = 200)
let historyTextBox = new RichTextBox(Top = 0, Left = 0, Height = 300, Width = 785)

historyTextBox.ReadOnly <- true
let mutable history = ""

let moveCursor (textBox: RichTextBox) = 
    textBox.SelectionStart <- textBox.Text.Length;
    textBox.ScrollToCaret();

let answer () = 
    let myMessage = messageBox.Text
    if myMessage <> "" then
        history <- history + "Me: " + myMessage + "\n\n"
        historyTextBox.Text <- history

        moveCursor historyTextBox
        myForm.Update()

        messageBox.Text <- "";
        waitLabel.Text <- "Waiting for answer"
        myForm.Update()
        let botAnswer = bot1Session.Think(myMessage)

        waitLabel.Text <- ""
        history <- history + "Bot: " + botAnswer + "\n\n"
        historyTextBox.Text <- history
        moveCursor historyTextBox

messageBox.KeyDown.Add (fun x -> 
    match x.KeyCode with
    | Keys.Enter -> answer ()
    | Keys.Escape -> Application.Exit()
    | _ -> ()
    )

sendButton.Click.Add (fun _ -> answer ())

let StartChat () = 
    myForm.Controls.AddRange [|messageBox; sendButton; waitLabel; historyTextBox|]
    myForm.Show()
    Application.Run()

StartChat ()