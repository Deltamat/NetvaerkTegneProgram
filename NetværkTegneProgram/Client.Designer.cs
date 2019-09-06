namespace NetværkTegneProgram
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DrawBox = new System.Windows.Forms.PictureBox();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.EnterIPLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).BeginInit();
            this.SuspendLayout();
            // 
            // DrawBox
            // 
            this.DrawBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.DrawBox.Enabled = false;
            this.DrawBox.Location = new System.Drawing.Point(5, 5);
            this.DrawBox.Margin = new System.Windows.Forms.Padding(2);
            this.DrawBox.Name = "DrawBox";
            this.DrawBox.Size = new System.Drawing.Size(750, 750);
            this.DrawBox.TabIndex = 0;
            this.DrawBox.TabStop = false;
            this.DrawBox.Visible = false;
            this.DrawBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseDown);
            this.DrawBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseMove);
            this.DrawBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseUp);
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(322, 169);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(100, 20);
            this.IPTextBox.TabIndex = 1;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(428, 168);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 20);
            this.ConnectButton.TabIndex = 2;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // EnterIPLabel
            // 
            this.EnterIPLabel.AutoSize = true;
            this.EnterIPLabel.BackColor = System.Drawing.Color.Transparent;
            this.EnterIPLabel.Location = new System.Drawing.Point(319, 153);
            this.EnterIPLabel.Name = "EnterIPLabel";
            this.EnterIPLabel.Size = new System.Drawing.Size(85, 13);
            this.EnterIPLabel.TabIndex = 3;
            this.EnterIPLabel.Text = "Enter IP address";
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 761);
            this.Controls.Add(this.EnterIPLabel);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.IPTextBox);
            this.Controls.Add(this.DrawBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Client";
            this.Text = "Client";
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox DrawBox;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label EnterIPLabel;
    }
}

