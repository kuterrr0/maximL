using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace WindowsFormsApp666
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 100;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                updateText();
            }
            label2.Text = MyLibrary.MyLibrary.updateRAM();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateText();
        }

        private void updateText()
        {
            richTextBox1.Clear();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                richTextBox1.Text += String.Format("Диск {0}\nТип диска: {1}\n", d.Name, d.DriveType);
                if (d.IsReady == true)
                {
                    String[] pref = { "кило", "мега", "гига", "тера" };

                    long freeSpace = d.TotalFreeSpace;
                    int divfree = 0;
                    long totalSize = d.TotalSize;
                    int divtotal = 0;

                    while (freeSpace > 1024)
                    {
                        freeSpace /= 1024;
                        divfree++;
                    }
                    while (totalSize > 1024)
                    {
                        totalSize /= 1024;
                        divtotal++;
                    }

                    richTextBox1.Text += String.Format("\tНазвание тома: {0}\n", d.VolumeLabel);
                    richTextBox1.Text += String.Format("\tФайловая система: {0}\n", d.DriveFormat);
                    richTextBox1.Text += String.Format("\tДоступно: {0} байт", d.TotalFreeSpace);
                    if (divfree > 0)
                    {
                        richTextBox1.Text += String.Format(" ~= {0} {1}байт\n", freeSpace, pref[divfree - 1]);
                    }
                    else
                    {
                        richTextBox1.Text += "\n";
                    }

                    richTextBox1.Text += String.Format("\tВсего: {0} байт\n", d.TotalSize);
                    if (divtotal > 0)
                    {
                        richTextBox1.Text += String.Format(" ~= {0} {1}байт\n", totalSize, pref[divtotal - 1]);
                    }
                    else
                    {
                        richTextBox1.Text += "\n";
                    }

                }
            }
        }
        
        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
