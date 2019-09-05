using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetværkTegneProgram
{
    public partial class Client : Form
    {
        private bool isMouseDown;
        Point originPoint = Point.Empty;

        public Client()
        {
            InitializeComponent();
        }

        private void DrawBox_Click(object sender, EventArgs e)
        {

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
                if (DrawBox.Image == null)
                {
                    Bitmap bmp = new Bitmap(DrawBox.Width, DrawBox.Height);
                    DrawBox.Image = bmp;
                }

                using (Graphics graphics = Graphics.FromImage(DrawBox.Image))
                {
                    graphics.DrawLine(new Pen(Color.Black, 1), originPoint, e.Location);
                }

                DrawBox.Invalidate();
                originPoint = e.Location;
            }
        }

        private void DrawBox_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            originPoint = Point.Empty;
        }
    }
}
