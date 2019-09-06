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

        public Client()
        {
            InitializeComponent();
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
                TcpClient client = new TcpClient();
                if (IPTextBox.Text.ToLower() == "l" || IPTextBox.Text.ToLower() == "local")
                {
                    IPTextBox.Text = "127.0.0.1";                    
                }
                client.Connect(IPAddress.Parse(IPTextBox.Text), port);
                NetworkStream networkStream = client.GetStream();
                streamWriter = new StreamWriter(networkStream, Encoding.ASCII);
                streamReader = new StreamReader(networkStream, Encoding.ASCII);
                
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
            string sendString = originX + "." + originY + "." + locationX + "." + locationY;
            streamWriter.WriteLine(sendString);
            Thread.Sleep(10);
            streamWriter.Flush();
        }
    }
}
