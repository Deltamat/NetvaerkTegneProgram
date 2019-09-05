using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TegneServer
{
    public partial class Server : Form
    {
        private static TcpListener server;
        private static bool isRunning;

        public Server()
        {
            InitializeComponent();
            TCPServer(13000);
            
        }

        private void TCPServer(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            isRunning = true;
            LoopClients();
        }

        private void LoopClients()
        {
            while (isRunning)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start();
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            StreamWriter streamWriter = new StreamWriter(client.GetStream());
            StreamReader streamReader = new StreamReader(client.GetStream());
            IPEndPoint endPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            IPEndPoint localPoint = (IPEndPoint)client.Client.LocalEndPoint;
            bool clientConnected = true;
            string data;

            while (clientConnected)
            {
                try
                {

                    data = streamReader.ReadLine();
                    string[] stringArray = data.Split('.');

                    using (Graphics graphics = Graphics.FromImage(DrawBox.Image))
                    {
                        graphics.DrawLine(new Pen(Color.Black, 1), new Point(Convert.ToInt32(stringArray[0]), Convert.ToInt32(stringArray[1])), new Point(Convert.ToInt32(stringArray[2]), Convert.ToInt32(stringArray[3])));
                    }

                }
                catch (Exception)
                {
                    Thread.CurrentThread.Abort();
                }
            }


        }

    }
}
