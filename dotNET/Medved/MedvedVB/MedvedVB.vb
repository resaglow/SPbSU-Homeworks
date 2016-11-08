Public Class MedvedVB : Inherits MedvedCSharp
    Public Overrides Sub MeetMedved()
        Console.WriteLine("Preved from VB.NET")
        MyBase.MeetMedved()
    End Sub
End Class
