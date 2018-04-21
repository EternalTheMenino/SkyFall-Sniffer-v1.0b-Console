Imports System.Net
Imports System.Net.Sockets
Public Class [SN]

    Public Shared SOCKETS As New Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP)
    Public Shared BYTE_DATA(4096) As Byte
    Public Shared STARTED As Boolean
    Public Shared IP_FROM As IPAddress
    Public Shared IP_TO As IPAddress
    Public Shared DES_PORT As UInteger
    Public Shared SRC_PORT As UInteger
    Public Shared IPV4 As String
    Private Shared SK As Threading.Thread
    Public Shared Porta As Integer
    Private Shared aTimer As New System.Timers.Timer


    Public Shared Sub OnReceive(ByVal asyncresult As IAsyncResult)
        If STARTED = True Then
            Dim READ_LENGTH As UInteger = BitConverter.ToUInt16(Byteswap(BYTE_DATA, 0), 0)
            SRC_PORT = BitConverter.ToUInt16(Byteswap(BYTE_DATA, 22), 0)
            DES_PORT = BitConverter.ToUInt16(Byteswap(BYTE_DATA, 24), 0)

            IP_FROM = New IPAddress(BitConverter.ToUInt32(BYTE_DATA, 12))
            IP_TO = New IPAddress(BitConverter.ToUInt32(BYTE_DATA, 16))

            ' SK = New Threading.Thread(AddressOf RESULT_UPDATE)
            '  SK.IsBackground = True
            ' SK.Start()
            Try

                '
                Dim threadStart As New Threading.ThreadStart(AddressOf RESULT_UPDATE)
                '
                Dim thread As New Threading.Thread(threadStart)
                thread.IsBackground = True

                thread.Priority = Threading.ThreadPriority.Highest
                thread.Start()
            Catch ex As Exception


            End Try


        End If
        SOCKETS.BeginReceive(BYTE_DATA, 0, BYTE_DATA.Length, SocketFlags.None, New AsyncCallback(AddressOf OnReceive), Nothing)
    End Sub
    Private Shared IPL As String
    Private Shared IPList As List(Of String)


    Public Shared Sub RESULT_UPDATE()

        Do

            Try


                If BYTE_DATA(9) = 17 And IP_FROM.ToString = IPV4 And DES_PORT = 80 Or DES_PORT = 443 Or DES_PORT = 22 Or DES_PORT = 21 And IP_TO.ToString.StartsWith("104") = False Then


                    If Not IPL = IP_TO.ToString Then
                        If Not IP_TO.ToString = IPV4 Then


                            IPL = IP_TO.ToString
                            Console.WriteLine(GetIpV4() + ":" + SRC_PORT.ToString + " ~> " + (String.Format("{0}{1}", IP_TO.ToString, ":" & DES_PORT, vbCrLf)))

                        End If


                    End If
                End If


                '  SK = New Threading.Thread(AddressOf RESULT_UPDATE)
                '   SK.IsBackground = True
                '  SK.Start()
                Threading.Thread.Sleep(7000)
            Catch ex As Exception

            End Try

        Loop
    End Sub

    Public Shared Function Byteswap(ByVal bytez() As Byte, ByVal index As UInteger)
        Dim result(1) As Byte
        result(0) = bytez(index + 1)
        result(1) = bytez(index)
        Return result
    End Function

    Public Shared Sub BindSocket()

        Try
            SOCKETS.Bind(New IPEndPoint(System.Net.IPAddress.Parse(IPV4), Porta))
            SOCKETS.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, True)
            Dim bytrue() As Byte = {1, 0, 0, 0}
            Dim byout() As Byte = {1, 0, 0, 0}
            SOCKETS.IOControl(IOControlCode.ReceiveAll, bytrue, byout)
            SOCKETS.Blocking = False

            ReDim BYTE_DATA(SOCKETS.ReceiveBufferSize)
            SOCKETS.BeginReceive(BYTE_DATA, 0, BYTE_DATA.Length, SocketFlags.None, New AsyncCallback(AddressOf OnReceive), Nothing)
        Catch ex As Exception
        End Try
    End Sub
End Class