Imports System.Text
Imports System.IO
Imports System.Security.Cryptography
Public Class ToolBox
    Public Shared Function StringGen(ByVal Lenght As Integer) 'Generates some random text
        Dim Allowed As String = "dsbEaTKBehJGiVxjIPHSglunZLmqXMfIDprWYkNtUwCzAoQivcyFRO"
        Dim sTngHm As String = ""
        Dim gRndNbr As New Random
        For i = 1 To Lenght
            sTngHm &= Allowed.Chars(gRndNbr.Next(0, Allowed.Length - 1))
        Next
        Threading.Thread.Sleep(500)
        Return sTngHm
    End Function
    Public Shared Function EncryptText(ByVal input As String, ByVal key As String) As String
        'This function Polymorphically encrypts the entered Text, utilising a Key and Salt.
        'All credits to Ethereal of HackForums.net
        'Please leave the credits.
        Dim R As New Random()
        Dim kc As Integer = 0
        Dim text As Char() = input.ToCharArray()
        Dim keyarr As Char() = key.ToCharArray()
        Dim FinVal As Char() = New Char(input.Length) {}
        Dim Rnd As Integer = R.[Next](100, 220)
        For index As Integer = 0 To input.Length - 1
            If kc >= keyarr.Length Then
                kc = 0
            End If
            Dim ptVal As Integer = AscW(text(index))
            Dim kVal As Integer = AscW(keyarr(kc))
            Dim ciVal As Integer = ptVal + kVal + Rnd
            FinVal(index) = Convert.ToChar(ciVal)
            kc += 1
        Next
        FinVal(input.Length) = ChrW(Rnd)
        Dim RetStr As New String(FinVal)
        Return RetStr
    End Function
    Public Shared Function DecryptText(ByVal input As String, ByVal key As String) As String
        'This function decrypts text that was encrypted with the Polymorphic encrypter in this class.
        'All credits to Ethereal of HackForums.net
        'Please leave the credits.
        Dim text As Char() = input.ToCharArray()
        Dim keyarr As Char() = key.ToCharArray()
        Dim FinVal As Char() = New Char(input.Length - 2) {}
        Dim RndKVal As Integer = AscW(text(input.Length - 1))
        text(input.Length - 1) = ControlChars.NullChar
        Dim kc As Integer = 0
        For index As Integer = 0 To input.Length - 1
            If index >= input.Length - 1 Then
                Continue For
            End If
            If kc >= keyarr.Length Then
                kc = 0
            End If
            Dim ciVal As Integer = AscW(text(index))
            Dim kVal As Integer = AscW(keyarr(kc))
            Dim ptVal As Integer = ciVal - RndKVal - kVal
            FinVal(index) = Convert.ToChar(ptVal)
            kc += 1
        Next
        Dim RetStr As New String(FinVal)
        Return RetStr
    End Function
    Public Shared Function format(ByVal input As String) As String ' Codedom has maximum of possible chars per line so we are storing the string in multiple strings
        Dim out As New System.Text.StringBuilder ' Declaring a new StringBuilder to store the output string
        'Dim base64data As String = Convert.ToBase64String(input) ' Get a readable String from the Byte Array
        Dim arr As String() = SplitString(input, 1500) ' Split the string into parts to fit in the Codedom-lines
        For i As Integer = 0 To arr.Length - 1 ' Looping thought each string in the array
            If i = arr.Length - 1 Then  ' If i equals the highest number
                out.Append(Chr(34) & arr(i) & Chr(34))
            Else 'I is smaller than arr.Length - 1 (i < arr.Length - 1)
                out.Append(Chr(34) & arr(i) & Chr(34) & " & _" & vbNewLine)
            End If
        Next
        Return out.ToString
    End Function
    Private Shared Function SplitString(ByVal input As String, ByVal partsize As Long) As String()
        Dim amount As Long = Math.Ceiling(input.Length / partsize) 'Get Long value of the amount of parts using the formular (Length of Input / Length of Parts)
        Dim out(amount - 1) As String 'Declaring the Array to Return using the amount of Parts to define the size
        Dim currentpos As Long = 0 ' Declaring the Currentposition in the String
        For I As Integer = 0 To amount - 1 ' Looping thought each string in the array
            If I = amount - 1 Then ' If i equals the highest number
                Dim temp((input.Length - currentpos) - 1) As Char ' Declaring a temporary Array of Chars for storing the current Part of the String
                input.CopyTo(currentpos, temp, 0, (input.Length - currentpos)) ' Current part is everything left from the string
                out(I) = Convert.ToString(temp) ' Current part is appended to the output string
            Else 'I is smaller than amount - 1 (i < amount - 1)
                Dim temp(partsize - 1) As Char ' Declaring a temporary Array of Chars for storing the current Part if the String using the Size of a part (partsize)
                input.CopyTo(currentpos, temp, 0, partsize) ' Copying the current Part to the temp array
                out(I) = Convert.ToString(temp) ' Current part is appended to the output string
                currentpos += partsize ' Currentposition is increase to catch the next part in the next "round" of the loop
            End If
        Next
        Return out ' Return the Output String
    End Function
End Class