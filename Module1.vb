Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Net
Imports System.Security.Principal
Imports System.Threading

Module Module1

    Function RemoveWhitespace(fullString As String) As String
        Return New String(fullString.Where(Function(x) Not Char.IsWhiteSpace(x)).ToArray())
    End Function


    Sub Main()
        Console.ForegroundColor = ConsoleColor.Magenta
        Console.Title = "Loading..."
        Console.Write("Loading... ")
        Using barradosatanas = New ProgressBar
            For i = 0 To 100
                barradosatanas.Report(i / 100)
                Thread.Sleep(50)
            Next
            Console.Clear()
        End Using

        Try
            Dim oProcess As New Process()
            Dim oStartInfo As New ProcessStartInfo("cmd.exe", "/c tasklist /v /fo csv | findstr /i Skype.exe")
            oStartInfo.UseShellExecute = False
            oStartInfo.CreateNoWindow = True
            oStartInfo.RedirectStandardOutput = True
            oProcess.StartInfo = oStartInfo
            oProcess.Start()

            Dim sOutput As String
            Using oStreamReader As System.IO.StreamReader = oProcess.StandardOutput
                sOutput = oStreamReader.ReadToEnd()
            End Using

            Dim username As String = ""
            Try
                username = sOutput.Split(",")(8).Split({""""c})(1).Split("- ")(1)
            Catch ex As Exception
                username = "Error"
            End Try
            Dim s1 As String() = sOutput.Split(",")
            Dim s2 As String() = s1(1).Split({""""c})

            Dim pid As String = sOutput.Split(",")(1).Split({""""c})(1)

            Dim oProcess2 As New Process()
            Dim oStartInfo2 As New ProcessStartInfo("cmd.exe", "/c netstat -ano | findstr UDP | findstr " + pid)
            oStartInfo2.UseShellExecute = False
            oStartInfo2.CreateNoWindow = True
            oStartInfo2.RedirectStandardOutput = True
            oProcess2.StartInfo = oStartInfo2
            oProcess2.Start()

            Dim sOutput2 As String
            Using oStreamReader2 As System.IO.StreamReader = oProcess2.StandardOutput
                sOutput2 = oStreamReader2.ReadToEnd()
            End Using

            For Each Line As String In sOutput2.Split(vbNewLine)
                If Line.Contains("0.0.0.0") = False And Line.Contains("127.0.0.1") = False And Line.Contains("[::]") = False And String.IsNullOrWhiteSpace(Line) = False And Line.Contains(":443") = False Then
                    Dim output1 As String = RemoveWhitespace(Line.Split(":")(0)).Replace("UDP", String.Empty)
                    Dim output2 As String = RemoveWhitespace(Line.Split(":")(1).Replace("*", String.Empty))
                    If username = "Error" Then
                        Console.Title = "ERRO: Você pode estar em uma chamada já!"
                        Console.ForegroundColor = ConsoleColor.White

                        Console.WriteLine("SkyFall - Skype Sniffer v1.0b")
                        Console.WriteLine(" ")
                        Console.ForegroundColor = ConsoleColor.Magenta
                        Console.WriteLine("ERRO: Você pode estar em uma chamada já!")
                        Console.Read()
                        End
                    End If


                    Console.Title = "SkyFall - Skype Sniffer v1.0b - Usuario: " + username + " - IPv4: " + output1 + " - Porta: " + output2
                    Console.ForegroundColor = ConsoleColor.White

                    Console.WriteLine("SkyFall - Skype Sniffer v1.0b")
                    Console.WriteLine(" ")
                    Console.ForegroundColor = ConsoleColor.Magenta
                    Console.WriteLine("Usuario: " + username)
                    Console.WriteLine("IPv4: " + GetIpV4())
                    Console.WriteLine("Porta: " + output2)
                    Console.WriteLine(" ")
                    Console.ForegroundColor = ConsoleColor.White


                    Console.WriteLine(" ")



                    Console.ForegroundColor = ConsoleColor.Magenta


                 
                    [SN].IPV4 = GetIpV4()
                    [SN].Porta = output2
                    [SN].STARTED = True
                    [SN].BindSocket()
                    Console.Read()
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Function GetIpV4() As String

        Dim myHost As String = Dns.GetHostName
        Dim ipEntry As IPHostEntry = Dns.GetHostEntry(myHost)
        Dim ip As String = ""

        For Each tmpIpAddress As IPAddress In ipEntry.AddressList
            If tmpIpAddress.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                Dim ipAddress As String = tmpIpAddress.ToString
                ip = ipAddress
                Exit For
            End If
        Next

        If ip = "" Then
            Throw New Exception("No IP found!")
        End If

        Return ip

    End Function

End Module
