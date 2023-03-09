using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лаба333333
{
    public partial class Form1 : Form
    {
        private bool f_open, f_save;
        public Form1()
        {
            InitializeComponent();
        }

        //Создать файл
        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label2.Text = saveFileDialog1.FileName;
                f_save = true;
            }
            else
                label2.Text = "None";
                f_save = false;
        }


        //Переименование файла
        private void button3_Click(object sender, EventArgs e) 
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                System.IO.File.Move(textBox1.Text, textBox2.Text);
                MessageBox.Show("Готово!");
            }
            

            //System.IO.File.Move(textBox1.Text, textBox2.Text);               
            //MessageBox.Show("Готово!");
        }


        //СОЗДАЁТ ПАПКУ (где создать, название папки)
        private void button4_Click(object sender, EventArgs e)
        {
            bool exists = System.IO.Directory.Exists(textBox1.Text + "\\" + textBox2.Text);
            if (!exists) System.IO.Directory.CreateDirectory(textBox1.Text + "\\" + textBox2.Text);
            else MessageBox.Show("Already Exists...!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "None";
            label2.Text = "None";
            label3.Text = "";
            f_open = false;
            f_save = false;
        }

        //СОздаёт папку и переносит туда другую(то, что перенести; куда(её сущ не должно))
        private void button5_Click(object sender, EventArgs e)
        {
            bool exists = System.IO.Directory.Exists(textBox1.Text);
            if (exists) System.IO.Directory.Move(textBox1.Text, textBox2.Text);
        }

        //Удалить папку
        private void button6_Click(object sender, EventArgs e)
        {
            System.IO.Directory.Delete(textBox1.Text, true);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string source = textBox1.Text;
            string Distination = textBox2.Text;
            bool exists = System.IO.Directory.Exists(source);
            if (exists)
            {
                System.IO.Directory.CreateDirectory(Distination);
                System.IO.Directory.Delete(source, true);
            }
        }

        //Обзор файла
        private void button8_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        //Открыть файл
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.Text = openFileDialog1.FileName;
                f_open = true;
            }
            else
                label1.Text = "None";
                f_open = false;
        }

        

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            
        }

        private void textBox3_DragDrop(object sender, DragEventArgs e)
        {
            
        }

        private void textBox3_DragEnter(object sender, DragEventArgs e)
        {
            
        }

        

        

    }
}
