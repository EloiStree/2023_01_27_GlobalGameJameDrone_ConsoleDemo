using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SendDroneCommands
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UDPListenerThread t = new UDPListenerThread(System.Threading.ThreadPriority.Normal, 2508);
            t.AddListener(ManageFeedBack);
            t.m_useUnicode = true;


            Console.WriteLine("What is target ip (ex: 192.168.1.??) ?");
            string ip = Console.ReadLine();

            if (ip.Length == 0)
                ip = "127.0.0.1";

            Console.WriteLine("What is target port ?");
            string portStr = Console.ReadLine();

            if (portStr.Length == 0)
                portStr = "2509";
            int.TryParse(portStr, out int port);
        
            Console.WriteLine("Hello World!");
            while (true) { 
            Console.WriteLine("What is your commands master ?");
             string answers =  Console.ReadLine();
                answers = TryToParse(answers);

                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
ProtocolType.Udp);
                IPAddress serverAddr = IPAddress.Parse(ip);
                IPEndPoint endPoint = new IPEndPoint(serverAddr, port);
                byte[] send_buffer = Encoding.Unicode.GetBytes(answers);
                sock.SendTo(send_buffer, endPoint);
            }


        }

        private static void ManageFeedBack(in string unicodeText)
        {
           // Console.WriteLine("R:"+unicodeText);
        }

        private static string TryToParse(string answers)
        {
            if (answers == "stop")
                return "rc 0 0 0 0";
            if (answers == "center")
                return "go to 0 1.2 0 ";
            if (answers == "center down")
                return "go to 0 0.1 0 ";
            if (answers == "center top")
                return "go to 0 2 0 ";

            return answers;
        }
    }
}
