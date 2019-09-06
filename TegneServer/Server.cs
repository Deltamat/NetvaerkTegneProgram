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
        private static object key = new object();
        private static List<Color> ColorList = new List<Color>()
        {
            Color.AliceBlue, Color.Aquamarine, Color.Beige, Color.Black, Color.BlueViolet, Color.BurlyWood, Color.Chartreuse, Color.Cornsilk, Color.Crimson, Color.Cyan, Color.DarkGoldenrod,
            Color.DarkKhaki, Color.DarkMagenta, Color.DarkSalmon, Color.DarkTurquoise, Color.DeepPink, Color.Firebrick, Color.ForestGreen, Color.Plum, Color.Honeydew, Color.Peru,
            Color.HotPink
        };
        private static List<Color> AvailableColors = new List<Color>();
        Random rng = new Random();

        public Server()
        {
            InitializeComponent();
            TCPServer(13000);
            DrawBox.Image = new Bitmap(DrawBox.Width, DrawBox.Height);
            AvailableColors = ColorList;
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
            if (AvailableColors.Count == 0)
            {
                AvailableColors = ColorList;
            }
            Color clientColor = AvailableColors[rng.Next(0, AvailableColors.Count + 1)];
            AvailableColors.Remove(clientColor);
            TcpClient client = (TcpClient)obj;
            StreamWriter streamWriter = new StreamWriter(client.GetStream());
            StreamReader streamReader = new StreamReader(client.GetStream());
            IPEndPoint endPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            IPEndPoint localPoint = (IPEndPoint)client.Client.LocalEndPoint;
            Thread.CurrentThread.Name = localPoint.Address.ToString();
            bool clientConnected = true;
            string data;

            while (clientConnected)
            {
                try
                {
                    data = streamReader.ReadLine();
                    string[] stringArray = data.Split('.');

                    Delegate invoke = new Action(() => Draw(stringArray, clientColor));

                    Invoke(invoke);
                }
                catch (Exception e)
                {
                    Thread.CurrentThread.Abort();
                }
            }
        }

        private void Draw(object[] stringArray, Color clientColor)
        {
            using (Graphics graphics = Graphics.FromImage(DrawBox.Image))
            {
                graphics.DrawLine(new Pen(clientColor, 1), new Point(Convert.ToInt32(stringArray[0]), Convert.ToInt32(stringArray[1])), new Point(Convert.ToInt32(stringArray[2]), Convert.ToInt32(stringArray[3])));
            }
            DrawBox.Invalidate();
            ImageConverter converter = new ImageConverter();
            byte[] array = (byte[])converter.ConvertTo(DrawBox.Image, typeof(byte[]));
        }
    }
}
