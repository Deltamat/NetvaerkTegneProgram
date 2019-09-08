using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace NetværkTegneProgram
{
    public partial class Client : Form
    {
        private bool isMouseDown;
        Point originPoint = Point.Empty;
        TcpClient client;
        StreamWriter streamWriter;
        StreamReader streamReader;
        private bool eraserOn = false;

        public Client()
        {
            InitializeComponent();
            DrawBox.Image = new Bitmap(DrawBox.Width, DrawBox.Height);
        }

        private void DrawBox_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            originPoint = e.Location;
        }

        private void DrawBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown == true && originPoint != null)
            {
                SendData(originPoint.X.ToString(), originPoint.Y.ToString(), e.Location.X.ToString(), e.Location.Y.ToString());

                DrawBox.Invalidate();
                originPoint = e.Location;
            }
        }

        private void Client_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && eraserOn == false)
            {
                eraserOn = true;
            }
            else if (e.KeyCode == Keys.Escape && eraserOn == true)
            {
                eraserOn = false;
            }
        }

        private void DrawBox_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            originPoint = Point.Empty;
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 port = 13000;
                client = new TcpClient();
                if (IPTextBox.Text.ToLower() == "l" || IPTextBox.Text.ToLower() == "local")
                {
                    IPTextBox.Text = "127.0.0.1";                    
                }
                client.Connect(IPAddress.Parse(IPTextBox.Text), port);
                NetworkStream networkStream = client.GetStream();
                streamWriter = new StreamWriter(networkStream, Encoding.ASCII);
                streamReader = new StreamReader(networkStream, Encoding.ASCII);


                // create a buffer for the incomming datastream
                byte[] imageBuffer = new byte[100000];
                int lenghtOfStream = networkStream.Read(imageBuffer, 0, imageBuffer.Length);
                // resize the array to the size of the datastream
                Array.Resize(ref imageBuffer, lenghtOfStream);
                // make the datastream into a memorystream that can be converted to a bitmap
                MemoryStream ms = new MemoryStream(imageBuffer);
                // convert to bitmap
                using (Graphics graphics = Graphics.FromImage(DrawBox.Image))
                {
                    DrawBox.Image = Image.FromStream(ms);
                }


                Thread t = new Thread(GetBitMap);
                t.IsBackground = true;
                t.Start();

                ConnectButton.Visible = false;
                ConnectButton.Enabled = false;
                EnterIPLabel.Visible = false;
                EnterIPLabel.Enabled = false;
                IPTextBox.Visible = false;
                IPTextBox.Enabled = false;
                DrawBox.Visible = true;
                DrawBox.Enabled = true;
            }
            catch (Exception)
            {
                EnterIPLabel.Text = "Could not connect to IP address";
            }
        }

        private void SendData(string originX, string originY, string locationX, string locationY)
        {
            string sendString = originX + "." + originY + "." + locationX + "." + locationY + "." + eraserOn.ToString();
            streamWriter.WriteLine(sendString);
            Thread.Sleep(1);
            streamWriter.Flush();
        }

        private void GetBitMap()
        {
            while (true)
            {
                try
                {
                    string data = streamReader.ReadLine();
                    string[] stringArray = data.Split('.');

                    Delegate invoke = new Action(() => Draw(stringArray));

                    Invoke(invoke);
                }
                catch (Exception)
                {
                    //throw;
                }
            }
        }

        private void Draw(object[] stringArray)
        {
            using (Graphics graphics = Graphics.FromImage(DrawBox.Image))
            {
                graphics.DrawLine(new Pen(Color.FromName((string)stringArray[4]), 1), new Point(Convert.ToInt32(stringArray[0]), Convert.ToInt32(stringArray[1])), new Point(Convert.ToInt32(stringArray[2]), Convert.ToInt32(stringArray[3])));
            }
            DrawBox.Invalidate();
        }
    }
}