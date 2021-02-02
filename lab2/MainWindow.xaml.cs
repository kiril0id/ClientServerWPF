using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client;

namespace lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }
        


        private void StartServer(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(Server);

            if (Status.Text == "On")
            {
                thread.Abort();
                Status.Text = "Off";
                ServerButton.Content = "Запуск сервера";
            }
            else
            {
                try
                {                    
                    thread.Start();
                    ServerButton.Content = "Отключить сервер";
                    Status.Text = "On";                    
                }
                catch
                {
                    Status.Text = "error";
                }
            }

           
        }

        public void Server()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8005);

            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);
                
                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; 
                    byte[] data = new byte[256];

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    float numb = Convert.ToSingle(builder.ToString());
                    string message;

                    try
                    {
                        Dispatcher.BeginInvoke(new ThreadStart(delegate { bitcoin.Text = (Convert.ToSingle(bitcoin.Text) + numb).ToString(); }));
                        message = "операция произведена";
                    }
                    catch
                    {
                        message = "операция не произведена";
                    }

                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);                    
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        private void NewClient(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo infoStartProcess = new ProcessStartInfo();
            infoStartProcess.WorkingDirectory = @"C:\Users\Kirill\Source\Repos\AIPRP\ClientServerWPF\Client\bin\Debug";
            infoStartProcess.FileName = "Client.exe";
            Process.Start(infoStartProcess);
        }
    }
}
