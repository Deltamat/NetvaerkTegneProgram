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
using System.Drawing.Imaging;

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
        public HashSet<StreamWriter> streamWriters = new HashSet<StreamWriter>();

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
            NetworkStream networkStream = client.GetStream();
            IPEndPoint endPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            IPEndPoint localPoint = (IPEndPoint)client.Client.LocalEndPoint;
            Thread.CurrentThread.Name = localPoint.Address.ToString();
            bool clientConnected = true;
            string data;
            streamWriters.Add(streamWriter);

            // send server-side bitmap to clients that join
            using (MemoryStream imageStream = new MemoryStream())
            {
                // Save bitmap in a format.
                DrawBox.Image.Save(imageStream, ImageFormat.Png);
                imageStream.Position = 0;

                // convert imagestream to array
                byte[] imageBytes = imageStream.ToArray();

                // send the array to the client
                networkStream.Write(imageBytes, 0, imageBytes.Length);
                networkStream.Flush();
            }

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
                    streamWriters.Remove(streamWriter);
                    Thread.CurrentThread.Abort();
                }
            }
        }

        private void Draw(object[] stringArray, Color clientColor)
        {
            using (Graphics graphics = Graphics.FromImage(DrawBox.Image))
            {
                if (stringArray[4].ToString() == "True")
                {
                    graphics.DrawLine(new Pen(Color.White, 1), new Point(Convert.ToInt32(stringArray[0]), Convert.ToInt32(stringArray[1])), new Point(Convert.ToInt32(stringArray[2]), Convert.ToInt32(stringArray[3])));
                }
                else
                {
                    graphics.DrawLine(new Pen(clientColor, 1), new Point(Convert.ToInt32(stringArray[0]), Convert.ToInt32(stringArray[1])), new Point(Convert.ToInt32(stringArray[2]), Convert.ToInt32(stringArray[3])));
                }
            }

            foreach (StreamWriter streamWriter in streamWriters)
            {
                if (stringArray[4].ToString() == "True")
                {
                    string dataString = stringArray[0] + "." + stringArray[1] + "." + stringArray[2] + "." + stringArray[3] + "." + Color.White.Name;
                    streamWriter.WriteLine(dataString);
                }
                else
                {
                    string dataString = stringArray[0] + "." + stringArray[1] + "." + stringArray[2] + "." + stringArray[3] + "." + clientColor.Name;
                    streamWriter.WriteLine(dataString);
                }
                Thread.Sleep(1);
                streamWriter.Flush();

            }
            DrawBox.Invalidate();
        }
    }
}
