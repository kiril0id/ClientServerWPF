using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static int port = 8005;
        static string address = "127.0.0.1"; 
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
               
                socket.Connect(ipPoint);
                Console.WriteLine("Терминал банка\nЖелаете произвести операции с биткойнами.\nВведите величину которую хотите списать или положить(положительное число положить, отрицательное соответственно снять.)");
                string message;
                byte[] data;
                while (true)
                {
                    try
                    {
                        message = Console.ReadLine();
                        Convert.ToSingle(message.ToString());
                        data = Encoding.Unicode.GetBytes(message);
                        socket.Send(data);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Unknown формат попрубуйте ещё раз! \n Например: +20 , -13,5 \n");
                    }
                }

                data = new byte[256]; 
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);

                Console.WriteLine("ответ сервера: " + builder.ToString());
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
