Imports System.Net
Imports System.Net.Sockets
Imports System.Net.Dns
Imports System.Threading
Imports System.Text

Module Module1

    Dim Port As Integer = 9090

    Sub Main()

        ' Get the hostname of this computer
        ' and then the v4 and v6 IP address
        Dim HostName As String
        Dim IP6Address As String
        Dim IP4Address As String

        HostName = System.Net.Dns.GetHostName()
        Console.WriteLine("hostname: {0}", HostName)
        IP6Address = System.Net.Dns.GetHostEntry(HostName).AddressList(0).ToString
        Console.WriteLine("IPv6 address: {0}", IP6Address)
        IP4Address = System.Net.Dns.GetHostEntry(HostName).AddressList(1).ToString
        Console.WriteLine("IPv4 address: {0}", IP4Address)

        ' Create a new thread for receiving messages
        Dim ReceiveThread As New System.Threading.Thread(AddressOf ReceiveData) With {
            .IsBackground = True
        }
        ReceiveThread.IsBackground = True
        ReceiveThread.Start()

        Dim IP As String = "255.255.255.255"
        Dim Message As String
        Dim RemoteEndPoint As New System.Net.IPEndPoint(IPAddress.Parse(IP), Port)
        Dim Client As New System.Net.Sockets.UdpClient()

        ' Wait for the user to type in a message
        ' and send it out to everyone
        ' and repeat forever
        Do
            Message = Console.ReadLine()
            Dim Data() As Byte = System.Text.Encoding.UTF8.GetBytes(Message)
            Client.Send(Data, Data.Length, RemoteEndPoint)
        Loop

    End Sub

    ' This Sub runs in the background in its own thread
    ' whenever a message is received it writes it out in the console
    Private Sub ReceiveData()

        Dim Client As New System.Net.Sockets.UdpClient(Port)
        Dim Message As String

        Do
            Try
                Dim Data() As Byte = Client.Receive(Nothing)
                Message = System.Text.Encoding.UTF8.GetString(Data)
                Console.WriteLine(vbLf & ">> " & Message)
            Catch ex As Exception
                Console.WriteLine(Err.ToString())
            End Try
        Loop

    End Sub

End Module
