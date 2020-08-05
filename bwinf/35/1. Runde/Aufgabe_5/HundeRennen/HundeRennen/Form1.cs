using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;


namespace HundeRennen
{
    public partial class Form1 : Form
    {
        string path;

        //Die Startpunkte der Hunde
        Node MaxiStart;
        Node MinniStart;

        //Die geschwindigkeit der Hunde
        double minniVelocity = 20.0;
        double maxiVelocity = 30.0;

        //Dijkstra fuer Maxi
        Dijkstra dMaxi;

        List<Node> nodesMinni = new List<Node>();
        List<List<Node>> nodesMinniLL = new List<List<Node>>();

        List<Node> nodesMaxi = new List<Node>();
        List<List<Node>> nodesMaxiLL = new List<List<Node>>();

        List<Edge> edgesMaxi = new List<Edge>();
        List<Edge> edgesMinni = new List<Edge>();




        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            init();
            textBox1.Text = path;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            path = textBox1.Text;
            //ueberprueft ob die Datei existiert
            textBox2.Text = "";
            textBox3.Text = "";
            if (File.Exists(path))
            {
                Console.WriteLine("File Exists");
                //die TextBoxes zuruecksetzen
                textBox2.Text = "";
                textBox3.Text = "";
                ReadFile();
                //alle Kanten fuer Minni erstellen
                CreateEdges(nodesMinniLL, edgesMinni);

                ////MAXI
                //neuse Object der Dijkstra Klasse fuer Maxi
                dMaxi = new Dijkstra();

               
                //nach einem moeglichen fuer minni suchen
                List<Node> isAPath = searchSavePath();

                if(isAPath == null)
                {
                    textBox2.Text += ":'(" + Environment.NewLine + "Their is no escape!!!";
                    textBox3.Text += ";P" + Environment.NewLine + "Victory";
                }
                else
                {
                    foreach (Node n in isAPath)
                    {
                        textBox2.Text += n.Name;
                        textBox2.Text += Environment.NewLine;
                    }
                }
                

                //alle variablen zuruecksetzen
                //damit das Programm ohne Neustart weiter funktioniert 
                init();
            }
            //wenn keine Datei gefunden wurde
            else
                MessageBox.Show("No such file or directory", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void ReadFile()
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
                Node tmpNodeMaxi = new Node("", nodeX, nodeY);
                Node tmpNodeMinni = new Node("", nodeX, nodeY);

                switch (node)
                {
                    case "X":
                        MaxiStart = tmpNodeMaxi;
                        tmpNodeMaxi.Name = node;
                        tmpNodeMinni.Name = node;
                            //+ " - " + nodeX + "  ||  " + tmpNode.PosX + " " + tmpNode.PosY;
                        nodesMaxi.Add(tmpNodeMaxi);

                        //wenn es noch keine Liste fuer diese x-pos gibt(MAXI)
                        if (nodesMaxiLL.Count == (nodeX / 70))
                            nodesMaxiLL.Add(new List<Node>());
                        nodesMaxiLL[0].Add(tmpNodeMaxi);


                        //wenn es noch keine Liste fuer diese x-pos gibt(MINNI)
                        if (nodesMinniLL.Count == (nodeX / 70))
                            nodesMinniLL.Add(new List<Node>());
                        nodesMinniLL[nodeX / 70].Add(tmpNodeMinni);
                        break;

                    case "M":
                        MinniStart = tmpNodeMinni;
                        tmpNodeMinni.Name = node;
                            //+ " - " + nodeX + "  ||  " + tmpNodeMinni.PosX + " " + tmpNode.PosY;
                        nodesMinni.Add(tmpNodeMinni);
               
                        //wenn es noch keine Liste fuer diese x-pos gibt(MINNI)
                        if (nodesMinniLL.Count == (nodeX / 70))
                            nodesMinniLL.Add(new List<Node>());
                        nodesMinniLL[nodeX / 70].Add(tmpNodeMinni);
                        
                        break;

                    case "x":
                        tmpNodeMaxi.Name = node + " - X = "+ nodeX + "  ||   Y = " + nodeY;
                        tmpNodeMinni.Name = node + " - X = " + nodeX + "  ||   Y = " + nodeY;
                        //der 1d Liste der nodes hinzufuegen von Maxi
                        nodesMaxi.Add(tmpNodeMaxi);
                        nodesMinni.Add(tmpNodeMinni);

                        //wenn es noch keine Liste fuer diese x-pos gibt(MAXI)
                        if (nodesMaxiLL.Count == (nodeX / 70))
                            nodesMaxiLL.Add(new List<Node>());
                        nodesMaxiLL[nodeX / 70].Add(tmpNodeMaxi);

                        
                        //wenn es noch keine Liste fuer diese x-pos gibt(MINNI)
                        if (nodesMinniLL.Count == (nodeX / 70))
                            nodesMinniLL.Add(new List<Node>());
                        nodesMinniLL[nodeX / 70].Add(tmpNodeMinni);
                        
                        
                        break;

                    case "m":
                        tmpNodeMinni.Name = node + " - X = " + nodeX + "  ||   Y = " + nodeY;
                        //der 1d Liste der nodes hinzufuegen von Minni
                        nodesMinni.Add(tmpNodeMinni);
                        
                        
                        //wenn es noch keine Liste fuer diese x-pos gibt(MINNI)
                        if (nodesMinniLL.Count == (nodeX / 70))
                            nodesMinniLL.Add(new List<Node>());
                        nodesMinniLL[nodeX / 70].Add(tmpNodeMinni);
                        
                        
                        break;
                }                   
            }
        }

        void CreateEdges(List<List<Node>> listWithNodes, List<Edge> _edges)
        {
            //x-Positionen
            for (int i = 0; i < listWithNodes.Count; i++)
            {
                //die nodes dieser x-Position
                for (int j = 0; j < listWithNodes[i].Count; j++)
                {
                    //wenn es keine nodes mehr gibt die nach dieser x-position kommen
                    if (i + 1 == listWithNodes.Count)
                        continue;

                    //die nodes von der nachfolgenden x-Position
                    for (int l = 0; l < listWithNodes[i + 1].Count; l++)
                    {
                        //die Distanz berechnen
                        double x = Convert.ToDouble(Math.Pow (listWithNodes[i + 1][l].PosX - listWithNodes[i][j].PosX, 2));
                        double y = Convert.ToDouble(Math.Pow (listWithNodes[i + 1][l].PosY - listWithNodes[i][j].PosY, 2));
                        
                        double dist = Math.Sqrt(x + y);

                        //die Edge erstellen
                        Edge tmpEdge = new Edge(listWithNodes[i][j], listWithNodes[i + 1][l], dist);

                        //die Edge allen edges hinzufuegen
                        _edges.Add(tmpEdge);
                    }
                }
                
            }
        }

        List<Node> searchSavePath()
        {
            //alle moeglichkeiten die noch ausprobiert werden koennen
            List<List<Node>> possiblePathes = new List<List<Node>>();
            //der momentane Pfad
            List<Node> currentPath = new List<Node>();
            currentPath.Add(MinniStart);
            double minniTime = 0;
            //
            List<List<Node>> maxiLL = new List<List<Node>>();
            maxiLL.Add(nodesMaxiLL[0]);
            List<Node> maxi = new List<Node>();


            

            //Den Weg zu jedem Loch fuer den grossen Hund berechnen
            for (int buhnen = 1; buhnen < nodesMinniLL.Count; buhnen++)
            {
                maxiLL[buhnen - 1] = nodesMaxiLL[buhnen - 1];  
                    
                maxiLL.Add(nodesMinniLL[buhnen]);

                

                //die einfache Liste von Maxi 
                maxi = new List<Node>();
                //alle loecher von der doppelteverketteten liste in eine einfache verschieben
                for (int x = 0; x < maxiLL.Count; x++)
                {
                    for (int y = 0; y < maxiLL[x].Count; y++)
                    {
                        maxi.Add(maxiLL[x][y]);
                    }
                }

                CreateEdges(maxiLL, edgesMaxi);
                //den kuerzesten Weg zu jedem Node berechnen
                dMaxi.CalculateMinDistForNodes(maxi, maxiLL, edgesMaxi, MaxiStart);

                for (int loecher = 0; loecher < nodesMinniLL[buhnen].Count; loecher++)
                {


                    for (int names = 0; names < nodesMinniLL[buhnen].Count; names++)
                    {
                        if (maxiLL[buhnen][loecher].Name == nodesMinniLL[buhnen][loecher].Name)
                        {
                            nodesMinniLL[buhnen][loecher].maxiTime = dMaxi.TimeForRun(maxiLL[buhnen][loecher].cost, maxiVelocity);
                            break;
                        }
                    }          
                }
            }

            int bNow = 1;
            while (bNow < nodesMinniLL.Count) { 
                //Console.WriteLine(bNow);


                minniTime = 0;

                for (int i = 0; i < currentPath.Count - 1; i++)
                {
                    //die Zeit fuer minni berechnen
                    double r = Convert.ToDouble(Math.Pow(currentPath[i].PosX - currentPath[i + 1].PosX, 2));
                    double s = Convert.ToDouble(Math.Pow(currentPath[i].PosY - currentPath[i + 1].PosY, 2));

                    double dist = Math.Sqrt(r + s);

                    //die Zeit von der letzten buhne  zu der jetztigen
                    double tmpTime = (dist) / (minniVelocity * 1000 / 3600);

                    minniTime += tmpTime;
                }
                
                //alle Loecher von dieser Buhne
                for (int l = 0; l < nodesMinniLL[bNow].Count; l++)
                {
                    if (nodesMinniLL[bNow][l].active)
                    {
                        //Console.WriteLine(nodesMinniLL[bNow][l].Name);
                        //die Zeit fuer minni berechnen
                        double r = Convert.ToDouble(Math.Pow(nodesMinniLL[bNow][l].PosX - currentPath[currentPath.Count - 1].PosX, 2));
                        double s = Convert.ToDouble(Math.Pow(nodesMinniLL[bNow][l].PosY - currentPath[currentPath.Count - 1].PosY, 2));

                        double dist = Math.Sqrt(r + s);

                        //die Zeit von der letzten buhne  zu der jetztigen
                        double tmpTime = (dist) / (minniVelocity * 1000 / 3600);

                        minniTime += tmpTime;

                     

                        //Minni ist schneller
                        if (nodesMinniLL[bNow][l].maxiTime > minniTime)
                        {
                            currentPath.Add(nodesMinniLL[bNow][l]);
                            nodesMinniLL[bNow][l].active = false;
                            bNow++;
                            break;
                        }

                        //Maxi ist schneller                
                        else if (nodesMinniLL[bNow][l].maxiTime <= minniTime)
                            nodesMinniLL[bNow][l].active = false;

                        //von der gesamten Zeit die Zeit von der letzten Buhne abziehen
                        //da maxi schneller war
                        minniTime -= tmpTime;
                    }

                    //wenn es auf dieser Buhne keinen Weg gibt bei dem Minni schneller als Maxi ist
                    if (l == nodesMinniLL[bNow].Count - 1)
                    {           
                        for (int i = bNow; i < nodesMinniLL.Count; i++)
                        {
                            for (int j = 0; j < nodesMinniLL[i].Count; j++)
                            {
                                nodesMinniLL[i][j].active = true;
                            }
                        }
                        bNow--;
                        currentPath.RemoveAt(currentPath.Count - 1);

                        if (bNow == 0)
                            return null;

                        break;
                    }

                    if (bNow == 0)
                        return null;  
                }

                //wenn Minni an einem Loch der letzten Buhne vor Maxi ankommen kann
                if (bNow >= nodesMinniLL.Count)
                    return currentPath;
            }
            return null;
        }

        void init()
        {
            path = "";

            dMaxi = null;

            nodesMinni = new List<Node>();
            nodesMinniLL = new List<List<Node>>();

            nodesMaxi = new List<Node>();
            nodesMaxiLL = new List<List<Node>>();

            edgesMaxi = new List<Edge>();
            edgesMinni = new List<Edge>();
        }
    }
}
