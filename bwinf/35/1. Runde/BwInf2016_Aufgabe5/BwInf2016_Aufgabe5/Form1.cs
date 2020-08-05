using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;


namespace BwInf2016_Aufgabe5
{
    public partial class Form1 : Form
    {
        //der Datei Pfad
        string path = "";
        //alle Loecher fuer MAXI || MINNI
        List<List<Node>> holesMAX = new List<List<Node>>();
        List<List<Node>> holesMIN = new List<List<Node>>();

        //die laenge des kuerzesten weges von MAXI || MINNI
        float shortestWayMAX = 0;
        float shortestWayMIN = 0;
        //der jetztige pfad
        List<Node> currentWayMIN = new List<Node>();
        List<Node> currentWayMAX = new List<Node>();

        Graphics g;
        //die verschiedenen stifte zum zeichnen
        Pen black;
        Pen blue;
        Pen white;


        private void Form1_Load(object sender, EventArgs e)
        {
            black = new Pen(Color.Black, 2F);          
            blue = new Pen(Color.Blue, 2f);
            white = new Pen(Color.White, 2f);

            this.DoubleBuffered = true;

            pictureBox1.Size = new Size(this.Width, this.Height - 110);
            g = pictureBox1.CreateGraphics();

        }

        public Form1()
        {
            InitializeComponent();
        }
        //CHECK FILE
        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            path = textBox1.Text;
            if (File.Exists(path))
            {
                //alle werte zurueksetzen um das programm nicht immer schliessen zu muessen
                holesMAX = new List<List<Node>>();
                holesMIN = new List<List<Node>>();
                currentWayMAX = new List<Node>();
                currentWayMIN = new List<Node>();
                g.Clear(Color.Yellow);
                //alle loecher aus der Datei einlesen
                createAllHoles();
                //alle loecher zeichnen
                drawHoles();
                //alle moeglichen Pfade fuer maxi finden
                shortestWayMIN = findShortestWay(holesMIN, currentWayMIN, 0f, 0f, 0);
                //alle 50 Einheiten eine linie zeichnen
                drawLines();

            }          
            else
                textBox2.Text = "No such File or directory";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        //Application beenden
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        //aus der Input datei die Holes creieren
        void createAllHoles()
        {
            var lines = File.ReadLines(path);
            foreach (var line in lines)
            {
                //Die Art des NODES
                int index1 = line.IndexOf(' ');
                string node = line.Substring(0, index1);

                //Die x-pos des NODES
                int index2 = line.IndexOf(' ', index1 + 1);
                int nodeX = int.Parse(line.Substring(index1, index2));
                //wenn der knoten auf x-null ist und nicht Maxis oder Minnies Start ist  -->  ueberspringen
                if (node != "X" && node != "M" && nodeX == 0)
                    continue;
                //die y-pos des NODES
                float nodeY = float.Parse(line.Substring(index2 + 1), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);

                //neuen NODE erstellen
                Node tmpNode = new Node();
                //erberpruefen ob es schon eine liste fuer dieses x-position gibt
                if (holesMAX.Count <= (nodeX / 70))
                    holesMAX.Add(new List<Node>());
                if (holesMIN.Count <= (nodeX / 70))
                    holesMIN.Add(new List<Node>());
                switch (node)
                {
                    case "X":                    
                        tmpNode.init(new PointF(nodeX, nodeY), 2, 0);
                        holesMAX[0].Add(tmpNode);
                        currentWayMAX.Add(tmpNode.copy());
                        break;
                    case "M":
                        tmpNode.init(new PointF(nodeX, nodeY), 2, 1);
                        holesMIN[0].Add(tmpNode);
                        currentWayMIN.Add(tmpNode.copy());
                        break;
                    case "x":
                        tmpNode.init(new PointF(nodeX, nodeY), 2, 2);
                        holesMAX[nodeX / 70].Add(tmpNode);
                        holesMIN[nodeX / 70].Add(tmpNode);
                        break;
                    case "m":
                        tmpNode.init(new PointF(nodeX, nodeY), 1, 2);
                        holesMIN[nodeX / 70].Add(tmpNode);
                        break;
                }                      
            }       
        }

        //alle pfade erstellen
        float findShortestWay(List<List<Node>> holes, List<Node> currentWay, float wayLength, float currentDist, int shortestWayIndex)
        {
            for (int x = 1; x < holes.Count; x++)
            {
                //die KUERZESTE distanz von dem letzten sicheren punkt zu dem naechsten Punkt der ueberprueft werden soll
                currentDist = 0;
                for (int y = 0; y < (holes[x].Count); y++)
                {
                    //letzter sicher Punkt zu naechstem moeglichen punkt messen
                    float tmpDist = holes[x][y].distance(currentWay[x - 1], holes[x][y]);
                    //wenn die kuerzeste gemessenen strecke laenger als die jetztige ist
                    //oder noch keine bisherige gemessen wurde
                    if(currentDist > tmpDist || currentDist == 0)
                    {
                        //den Index des Knotens merken um ihndem Pfad hinzuzufuegen
                        //falls kein kuerzerer mehr gefunden wird
                        shortestWayIndex = y;
                        //jetztige distanz zu dem knoten merken
                        currentDist = tmpDist;
                    }
                }
                //weg Laenge um die distanz des letzten weges verlaengern
                wayLength += currentDist;
                //das Hole dem jetztigen Pfad hinzufuegen
                currentWayMIN.Add(holes[x][shortestWayIndex]);
            }
            
            for (int i = 0; i < currentWayMIN.Count; i++)
            {
                Console.Write(currentWayMIN[i].pos.Y);
                if (i == currentWayMIN.Count - 1)
                {
                    Console.WriteLine("  ||  SHORTEST = " + wayLength);
                    Console.WriteLine("\r\n");
                }
                else
                {
                    Console.Write(" --> ");
                    g.DrawLine(black, currentWayMIN[i].pos, currentWay[i + 1].pos);
                }
                    
            }
            return wayLength;
        }
        //die holes zeichnen
        void drawHoles()
        {
            //MAXI
            g.DrawEllipse(blue, holesMAX[0][0].pos.X - 5, (int)holesMAX[0][0].pos.Y - 5, 10, 10);
            g.DrawRectangle(new Pen(Color.Red, 2f), holesMAX[0][0].pos.X, (int)holesMAX[0][0].pos.Y, 1, 1);
            //MINNI
            g.DrawEllipse(white, holesMIN[0][0].pos.X - 5, (int)holesMIN[0][0].pos.Y - 5, 10, 10);
            g.DrawRectangle(new Pen(Color.Red, 2f), holesMIN[0][0].pos.X, (int)holesMIN[0][0].pos.Y, 1, 1);

            for (int x = 0; x < holesMIN.Count; x++)
            {
                for (int y = 0; y < holesMIN[x].Count; y++)
                {                   
                    //MAXI HOLES
                    if (holesMIN[x][y].width == 2)
                    {
                        g.DrawEllipse(black, holesMIN[x][y].pos.X - 5, holesMIN[x][y].pos.Y - 10, 10, 20);
                        g.DrawRectangle(new Pen(Color.Red, 2f), holesMIN[x][y].pos.X, ((int)holesMIN[x][y].pos.Y), 1,1);
                    }
                    //MINI HOLES             
                    else
                    {
                        g.DrawEllipse(black, holesMIN[x][y].pos.X - 5, holesMIN[x][y].pos.Y - 5, 10, 10);
                        g.DrawRectangle(new Pen(Color.Red, 2f), holesMIN[x][y].pos.X, ((int)holesMIN[x][y].pos.Y), 1,1);
                    }
                }
            }
        }
        //horizontale linie zeichnen alle 50 Einheiten
        void drawLines()
        {
            for (int x = 0; x < pictureBox1.Height; x += 70)
            {          
                g.DrawLine(new Pen (Color.Gray, 1f), new Point(x, 0), new Point(x, pictureBox1.Height));
                g.DrawLine(new Pen(Color.Gray, 1f), new Point(0, x), new Point(pictureBox1.Width, x));
            }       
        }
    }

    public class Node
    {
        //0  -->  Maxisstart  ||  1  -->  Minnisstart  ||  2  --  hole
        public int type;
        public PointF pos;
        public int width;

        public void init(PointF coor, int w, int typ)
        {
            pos = coor;
            width = w;
            type = typ;
        }
        public float distance(Node one, Node two)
        {
            float dist = (float)Math.Sqrt(((two.pos.X - one.pos.X) * (two.pos.X - one.pos.X)) + ((two.pos.Y - one.pos.Y) * (two.pos.Y - one.pos.Y)));
            return dist;
        }
        public Node copy()
        {
            return (Node)this.MemberwiseClone();
        }
    }
}


