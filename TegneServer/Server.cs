﻿using System;
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
        public HashSet<StreamWriter> streamWriters = new HashSet<StreamWriter>();

        public Server()
        {
            InitializeComponent();
            TCPServer(13000);
            DrawBox.Image = new Bitmap(DrawBox.Width, DrawBox.Height);
        }

        private void TCPServer(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            isRunning = true;
            Thread T = new Thread(LoopClients);
            T.IsBackground = true;
            T.Start();
        }

        private void LoopClients()
        {
            while (isRunning)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.IsBackground = true;
                clientThread.Start(client);
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
            streamWriters.Add(streamWriter);

            while (clientConnected)
            {
                try
                {
                    data = streamReader.ReadLine();
                    string[] stringArray = data.Split('.');

                    Delegate invoke = new Action(() => Draw(stringArray));

                    Invoke(invoke);
                        
                    
                    DrawBox.Invalidate();
                }
                catch (Exception e)
                {
                    Thread.CurrentThread.Abort();
                }
            }
        }

        private void Draw(object[] stringArray)
        {
            using (Graphics graphics = Graphics.FromImage(DrawBox.Image))
            {
                graphics.DrawLine(new Pen(Color.Black, 1), new Point(Convert.ToInt32(stringArray[0]), Convert.ToInt32(stringArray[1])), new Point(Convert.ToInt32(stringArray[2]), Convert.ToInt32(stringArray[3])));
            }
            foreach (StreamWriter streamWriter in streamWriters)
            {
                string dataString = stringArray[0] + "." + stringArray[1] + "." + stringArray[2] + "." + stringArray[3];
                streamWriter.WriteLine(dataString);
                streamWriter.Flush();
            }
        }


    }
}
