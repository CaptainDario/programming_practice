using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace Rhinozelfant
{
    public partial class Form1 : Form
    {
        string path = "C:/Users/Dario/Documents/BWINF/2016/Aufgabe_2/Test_Bilder/rhinozelfant1.bmp";


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = path;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            path = textBox1.Text;
            if (File.Exists(path))
            {
                pictureBox1.Image = Image.FromFile(path);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
