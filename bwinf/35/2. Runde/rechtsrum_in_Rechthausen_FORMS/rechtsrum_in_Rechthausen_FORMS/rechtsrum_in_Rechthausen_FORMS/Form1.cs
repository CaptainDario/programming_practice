using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;




namespace rechtsrum_in_Rechthausen_FORMS
{
    public partial class Form1 : Form
    {
        Rechthausen rechts;

        int zoomFactor = 2;

        public Form1()
        {
            InitializeComponent();

            this.pictureBox1.MouseMove += new MouseEventHandler(this.pictureBox1_MouseMove);
            this.trackBar1.ValueChanged += new EventHandler(this.trackBar1_ValueChanged);


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        //Load file Button
        private void button1_Click(object sender, EventArgs e)
        {
            //the typed in path
            string path = textBox1.Text;
            //read path
            if (File.Exists(path))
            {
                this.comboBox1.Items.Clear();
                this.comboBox2.Items.Clear();
                this.textBox2.Text = "";
                this.textBox4.Text = "";

                this.comboBox1.SelectedIndexChanged -= this.comboBox_ValueChanged;
                this.comboBox2.SelectedIndexChanged -= this.comboBox_ValueChanged;
                this.radioButton1.CheckedChanged -= this.comboBox_ValueChanged;



                rechts = new Rechthausen();

                textBox1.Text = "Loading...";
                rechts.InintializeRechtshausen(path, progressBar1, textBox3, pictureBox1, pictureBox2);

                //initialize content of dropdown boxes
                foreach (KeyValuePair<String, Crossing> item in rechts.crossings)
                {
                    comboBox1.Items.Add(item.Key);
                    comboBox2.Items.Add(item.Key);
                }
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                //ADD EVENTHANDLER FOR THE COMBOBOXES
                this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox_ValueChanged);
                this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox_ValueChanged);
                this.radioButton1.CheckedChanged += new EventHandler(this.comboBox_ValueChanged);

                label5.Text = rechts.greatestFactorDistances;
                label6.Text = rechts.greatestFactorCrossing;

                if (rechts.allNodesAreReachable)
                    label7.Text = "You CAN reach every crossing from every other Crossing in this map!";
                else
                    label7.Text = "You CAN NOT reach every crossing from every other Crossing in this map!";


                textBox1.Text = "Finished Loading!";

            }
            else
                textBox1.Text = "Can not find such File!";

        }
        //Load file Inputbox
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //the image on which the map will be drawn
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }


        //EVENTS
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            // If no picture is loaded, return
            if (pictureBox1.Image == null)
                return;


            rechts.draw.UpdateZoomedImage(e, pictureBox2, pictureBox1, zoomFactor);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            zoomFactor = trackBar1.Value;
            trackBar1.Text = string.Format("x{0}", zoomFactor);
        }

        private void comboBox_ValueChanged(object sender, EventArgs e)
        {



            //DISTANCE AS MEASURMENT
            string shortestPath = "";
            if (radioButton1.Checked)
            {
                rechts.path = new Pathfinding(rechts.crossings, false, false);
                Tuple<float, List<String>>[] shortestPathNames = rechts.path.dijkstraWithPath(rechts.crossings[comboBox1.SelectedItem.ToString()], true);

                //Add the shortest path to a textbox
                for (int i = 0; i < shortestPathNames[rechts.crossingNumbers[comboBox2.SelectedItem.ToString()]].Item2.Count; i++)
                {
                    shortestPath += (shortestPathNames[rechts.crossingNumbers[comboBox2.SelectedItem.ToString()]].Item2[i].ToString() + " --> ");
                }
                //Add the destination crossing to the textbox
                shortestPath += (comboBox2.SelectedItem.ToString());

                //draw shortest path with distance as measurement
                rechts.draw.DrawWholeMap(rechts.crossings, rechts.streets, rechts.crossingNumbers, shortestPathNames, pictureBox1, true, comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString(), true);

                textBox2.Text = "From " + comboBox1.SelectedItem +
                                " to " + comboBox2.SelectedItem +
                                " in " + shortestPathNames[rechts.crossingNumbers[comboBox2.SelectedItem.ToString()]].Item1
                                + " km ";
            }

            //CROSSINGS AS MEASURMENT
            else
            {
                rechts.path = new Pathfinding(rechts.crossings, false, false);
                Tuple<float, List<String>>[] shortestPathNames_Crossings = rechts.path.dijkstraWithPath(rechts.crossings[comboBox1.SelectedItem.ToString()], false);

                //Add the shortest path to a textbox
                for (int i = 0; i < shortestPathNames_Crossings[rechts.crossingNumbers[comboBox2.SelectedItem.ToString()]].Item2.Count; i++)
                {
                    shortestPath += (shortestPathNames_Crossings[rechts.crossingNumbers[comboBox2.SelectedItem.ToString()]].Item2[i].ToString() + " --> ");
                }
                //Add the destination crossing to the textbox
                shortestPath += (comboBox2.SelectedItem.ToString());

                //draw shortest path with distance as measurement
                rechts.draw.DrawWholeMap(rechts.crossings, rechts.streets, rechts.crossingNumbers, shortestPathNames_Crossings, pictureBox1, false, comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString(), true);

                textBox2.Text = "From " + comboBox1.SelectedItem +
                                " to " + comboBox2.SelectedItem +
                                " in " + shortestPathNames_Crossings[rechts.crossingNumbers[comboBox2.SelectedItem.ToString()]].Item1
                                + " Crossings";
            }

            textBox4.Text = shortestPath;

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
