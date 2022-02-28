
namespace View
{
    partial class Form1
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
            this.connectButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.serverLabel = new System.Windows.Forms.Label();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(345, 14);
            this.connectButton.Margin = new System.Windows.Forms.Padding(2);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(56, 19);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(188, 15);
            this.nameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(36, 13);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "name:";
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(10, 15);
            this.serverLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(39, 13);
            this.serverLabel.TabIndex = 2;
            this.serverLabel.Text = "server:";
            this.serverLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // serverTextBox
            // 
            this.serverTextBox.Location = new System.Drawing.Point(58, 15);
            this.serverTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(76, 20);
            this.serverTextBox.TabIndex = 3;
            this.serverTextBox.Text = "localhost";
            this.serverTextBox.TextChanged += new System.EventHandler(this.serverTextBox_TextChanged);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(228, 14);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(76, 20);
            this.nameTextBox.TabIndex = 4;
            this.nameTextBox.Text = "player";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(904, 911);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.serverTextBox);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.connectButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(9999, 9999);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TankWars";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleKeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HandleKeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control1_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.TextBox nameTextBox;
    }
}

