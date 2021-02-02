using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        static ServerObject server; // сервер
        static Thread listenThread; // потока для прослушивания
        
        public MainWindow()
        {
            InitializeComponent();
            
        } 

        private  void StartServer(object sender, RoutedEventArgs e)
        {
            if (Status.Text == "On")
            {
                server.Disconnect();
                Status.Text = "Off";
                ServerButton.Content = "Запуск сервера";
            }
            else
            {
                try
                {
                    server = new ServerObject();
                    listenThread = new Thread(new ThreadStart(server.Listen));
                    listenThread.Start(); //старт потока
                    ServerButton.Content = "Отключить сервер";
                    Status.Text = "On";
                }
                catch (Exception ex)
                {
                    server.Disconnect();
                    Console.WriteLine(ex.Message);
                }
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
