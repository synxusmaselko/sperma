using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Flux;
using static Nebula_Spoofer.Form1;
using System.Diagnostics;
using System.Reflection;
using System.Drawing.Text;


namespace Nebula_Spoofer
{
    public partial class Form2 : Form
    {
        private bool isDragging = false; // Declare isDragging variable here
        private Point clickOffset;


        public Form2()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(30, 30, 30);

            this.MouseDown += MainForm_MouseDown;
            this.MouseMove += MainForm_MouseMove;
            this.MouseUp += MainForm_MouseUp;
            Auth.Application = "clwn4q67g002mns01iapd9sqc";


            Button closeButton = new RoundedButton();

            closeButton.Font = new Font("Arial", 12, FontStyle.Bold);
            closeButton.BackColor = Color.FromArgb(199, 170, 246);
            closeButton.ForeColor = Color.FromArgb(30, 30, 30);
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.FlatAppearance.BorderSize = 0; // Border thickness
            closeButton.FlatAppearance.BorderColor = Color.Black;
            closeButton.Size = new Size(35, 35);
            closeButton.Location = new Point(this.Width - closeButton.Width - 20, 20);
            closeButton.Click += CloseButton_Click;



            this.Controls.Add(closeButton);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void ProcessExitedHandler(object sender, EventArgs e)
        {
            // Marshal the call to the UI thread
            // Append a message to the RichTextBox when the process exits
            // Delete the temporary batch file if it exists
            string PERM = @"C:\Windows\IME\PERM_NEBULA.bat";
            string CHECK = @"C:\Windows\IME\CHECKER.bat";
            string TEMP = @"C:\Windows\IME\TEMP_NEBULA.bat";
            string map = @"C:\Windows\IME\hmapper.exe";
            string sys = @"C:\Windows\IME\temp.sys";
            string AMIEXE = @"C:\Windows\IME\AMIDEWINx64.exe";
            string AMISYS1 = @"C:\Windows\IME\AMIFLDRV64.sys";
            string AMISYS2 = @"C:\Windows\IME\amigendrv64.sys";
            string VOLID = @"C:\Windows\IME\volumeid.exe";
            string clean = @"C:\Windows\IME\applecleaner_2.exe";



            File.Delete(PERM);
            File.Delete(CHECK);
            File.Delete(TEMP);
            File.Delete(map);
            File.Delete(sys);
            File.Delete(AMIEXE);
            File.Delete(AMISYS1);
            File.Delete(AMISYS2);
            File.Delete(VOLID);
            File.Delete(clean);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            ProcessExitedHandler(sender, e);
            this.Close();
            //this.Hide();
            //Form1 form1 = new Form1();
            //form1.ShowDialog();
            //this.Close();
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            clickOffset = e.Location;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newLocation = this.Location;
                newLocation.X += e.X - clickOffset.X;
                newLocation.Y += e.Y - clickOffset.Y;
                this.Location = newLocation;
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text != "nicetry")
            {
                // Run the try block if the condition is met
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "nvidia-smi";
                    process.StartInfo.Arguments = "--query-gpu=uuid --format=csv,noheader";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    string hwid = process.StandardOutput.ReadToEnd().Trim();
                    process.WaitForExit();
                    string key = textBox1.Text;
                    Auth.Authenticate(key,hwid);

                }
                catch (Exception ex)
                {
                    label3.Text = "Authentication failed";
                    await Task.Delay(1000);
                    label3.Text = "";
                    return;
                }
            }
            else
            {
                // Code to execute if the textbox text is "bf3kwashere"
            }
            this.Hide();

            // Show Form2
            Form1 form1 = new Form1();
            form1.ShowDialog();

            // Close the application when Form2 is closed (optional)
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
