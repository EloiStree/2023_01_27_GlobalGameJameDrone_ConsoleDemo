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


            Console.WriteLine("Hello World!");
            Console.WriteLine("");
            Console.WriteLine("Documentation:");
            Console.WriteLine("https://github.com/EloiStree/2023_01_27_GlobalGameJameDroneDoc");
            Console.WriteLine("");
            Console.WriteLine("Manual");
            Console.WriteLine("> rc joystickLeftX joystickLeftY joystickRightX joystickRightY  (value between -1 & 1)");
            Console.WriteLine("> rc 0 0 0 0   neutral     rc -1 0 1 0  turn left move forward");
            Console.WriteLine("> rcs 1 0.5 1 1  max speed and slow down vertical move ");
            Console.WriteLine("> ks1 || ks0   set the kill switch to on, off");
            Console.WriteLine("> as1 || as0   set the armed switch to on, off");
            Console.WriteLine("");

            Console.WriteLine("What is target ip (ex: 192.168.1.??) ?");
            string ip = Console.ReadLine();

            if (ip.Length == 0)
                ip = "127.0.0.1";

            Console.WriteLine("What is target port ?");
            string portStr = Console.ReadLine();

            if (portStr.Length == 0)
                portStr = "2509";
            int.TryParse(portStr, out int port);

         

            while (true)
            {
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
