using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba_4444
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.DataSource = Enum.GetValues(typeof(ThreadPriority));
        }
        public int card1;
        public int sec1 = 0;
        public int min1 = 0;
        public int hr1 = 0;
        public bool check = true;
        public ThreadPriority priority;

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(redactor1);
            thread1.Start();
        }

        private void redactor1()
        {
            
            while (check)
            {
                card1++;
                this.textBox1.BeginInvoke((MethodInvoker)(() => this.textBox1.Text = $"{card1}"));
                Thread.Sleep(100);
                Thread thread2 = new Thread(Timer);
                button2.Tag = thread2;
            }                     
        }
        private void Timer()
        {
            while (check)
            {
                sec1++;
                this.label3.BeginInvoke((MethodInvoker)(() => this.label3.Text = Convert.ToString(sec1)));
                if (sec1 == 60)
                {
                    min1++;
                    sec1 = 0;
                    this.label2.BeginInvoke((MethodInvoker)(() => this.label2.Text = Convert.ToString(min1)));
                }
                if (min1 == 60)
                {
                    hr1++;
                    min1 = 0;
                    this.label1.BeginInvoke((MethodInvoker)(() => this.label1.Text = Convert.ToString(hr1)));
                }
                Thread.Sleep(1000);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            check = true;
            Thread value1 = (Thread)button2.Tag;
            value1.Start();
            button3.Tag = value1;
            button4.Tag = value1;
            button2.Enabled = false;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread value2 = (Thread)button3.Tag;
            if (value2.ThreadState == System.Threading.ThreadState.Suspended)
            {

                value2.Resume();

            }
            else
            {
                value2.Suspend();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread value3 = (Thread)button4.Tag;
            value3.Priority = priority;
            MessageBox.Show($"{value3.Priority}");
        }

        
        private void button5_Click(object sender, EventArgs e)
        {
            check = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ThreadPriority selectedState = (ThreadPriority)comboBox1.SelectedItem;
            priority = selectedState;
        }
    }
}
