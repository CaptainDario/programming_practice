using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;


namespace rechtsrum_in_Rechthausen_FORMS
{
    public class Rechthausen
    {
        //the dictionary with all crossings that are in this map
        public Dictionary<String, Crossing> crossings;
        //dictonary with all crossings as keys
        //and the number they are in the lowest cost array
        public Dictionary<String, int> crossingNumbers = new Dictionary<string, int>();
             
        //array with all connections(streets) between the crossings
        public List<Street> streets;

        //
        public Pathfinding path;

        //ALL distances from every crossing to every crossing
        public float[][] distances;
        //ALL distances from every crossing to every crossing
        //measured in crossing visited on the path
        public float[][] distancesCrossingMeasured;

        //ALL distances from every crossing to every crossing
        public float[][] distancesWITH_LEFT;
        //ALL distances from every crossing to every crossing
        //measured in crossing visited on the path
        public float[][] distancesCrossingMeasuredWITH_LEFT;

        public String greatestFactorCrossing = "";
        public String greatestFactorDistances = "";

        public bool allNodesAreReachable = true;


        //the amount of crossings and streets in this map
        public int crossingsInMap = 0;
        int streetsInMap = 0;
        //the greatest numbers for the x- / y-values in this map
        float xMax = 0;
        float yMax = 0;
        //class with all function for drawing the map
        public DrawMap draw;


        //CONSTRUCTOR
        public Rechthausen()
        {
            crossings = new Dictionary<String, Crossing>();
        }


        //the routine that is the actual main routine
        public void InintializeRechtshausen(string _path, ProgressBar _prog, TextBox _time, PictureBox fullMap, PictureBox zoomedMap)
        {
            //READ AND CREATES THE MAP AND DATA
            //Reads all Crossings and creates all Crossings
            bool fileReadCorrectly = ReadInputFile(_path);
            if (!fileReadCorrectly)
                return;

            //Find all allowed Connections for all Crossings
            FindAllAllowedConnections();
            //calculate the distance to all Crossings
            CalculateAllDistances();

            //initialize the drawing object
            draw = new DrawMap(xMax, yMax);
            //Draws the map on the picturebox
            draw.DrawWholeMap(crossings, streets, new Dictionary<string, int>(), new Tuple<float, List<string>>[0], fullMap, false);


            //measure time of the algorithm
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //finds all possible paths AND shortest paths
            //WITH THE BAN OF TURNING LEFT
            path = new Pathfinding(crossings, false, false);        
            this.distances = path.findAllShortestPathes();

            path = new Pathfinding(crossings, true, false);
            this.distancesCrossingMeasured = path.findAllShortestPathes();
            

            //finds all possible paths AND shortest paths
            //WITHOUT THE BAN OF TURNING LEFT
            path = new Pathfinding(crossings, false, true);
            this.distancesWITH_LEFT = path.findAllShortestPathes();

            path = new Pathfinding(crossings, true, true);
            this.distancesCrossingMeasuredWITH_LEFT = path.findAllShortestPathes();


            //Find the greatest factors caused by the ban of turning left
            this.findGreatestFactor();


            //display the time in a textfield
            sw.Stop();
            _time.Text = "Loading time: " + sw.Elapsed.ToString();

        }


        //reads the file and creates all crossings
        public bool ReadInputFile(string _userPath)
        {
            string[] textData = File.ReadAllLines(_userPath);

            //counts the created crossings
            int crossingCnt = 0;
            foreach (string line in textData)
            {
                //if this line is a comment
                if (line[0] == '#') continue;
                //create tmp vars
                float x = 0;
                float y = 0;
                string n = "";
                //counts the detected elementes from one line
                int cnt = 0;

                //the current value to remember from the file
                string tmp = "";
                //iterate over one line of the file
                for (int i = 0; i < line.Length; i++)
                {
                    try
                    {
                        //READ THE FIRST LINE WITH THE NUMBER OF CROSSINGS AND STREETS
                        if (crossingsInMap == 0 && streetsInMap == 0)
                        {
                            while (true)
                            {
                                if (line.Length == i)
                                {
                                    streetsInMap = Int32.Parse(tmp);
                                    streets = new List<Street>();
                                    streets.Capacity = streetsInMap;
                                    break;
                                }
                                if (line[i] == ' ')
                                {
                                    crossingsInMap = Int32.Parse(tmp);
                                    i++;
                                    tmp = "";
                                }

                                tmp += line[i];
                                i++;

                            }
                        }
                        //READS ALL CROSSINGS
                        else if (crossingCnt < crossingsInMap)
                        {
                            while (line[i] != ' ')
                            {
                                tmp += line[i];
                                i++;
                                if (line.Length == i)
                                    break;
                            }
                            switch (cnt)
                            {
                                case 0:
                                    n = tmp;
                                    tmp = "";
                                    cnt++;
                                    break;
                                case 1:
                                    x = float.Parse(tmp, System.Globalization.CultureInfo.InvariantCulture);

                                    //if a new maximum value for the x coordinate was found
                                    if (x > xMax)
                                        xMax = x;

                                    tmp = "";
                                    cnt++;
                                    break;
                                case 2:
                                    y = float.Parse(tmp, System.Globalization.CultureInfo.InvariantCulture);

                                    //if a new maximum value for the x coordinate was found
                                    if (y > yMax)
                                        yMax = y;

                                    tmp = "";
                                    crossingCnt++;
                                    cnt = 0;
                                    crossings.Add(n, new Crossing(n, x, y, crossingCnt));
                                    crossingNumbers.Add(n, crossingCnt - 1);
                                    break;
                            }
                        }
                        //IGNORE THE LINE BETWEEN THE CROSSINGS AND STREETS
                        else if (crossingCnt == crossingsInMap)
                        {
                            crossingCnt++;
                            break;
                        }
                        //READS ALL STREETS
                        else
                        {
                            while (line[i] != ' ')
                            {
                                tmp += line[i];
                                i++;
                            }
                            i++;
                            //name of the crossing at the end of the street
                            string tmp2 = "";
                            while (i < line.Length)
                            {
                                tmp2 += line[i];
                                i++;
                            }
                            //adds the connection to all connections
                            streets.Add(new Street(crossings[tmp], crossings[tmp2]));

                            //creates the connection between the crossings
                            crossings[tmp].connections.Add(crossings[tmp2]);
                            crossings[tmp2].connections.Add(crossings[tmp]);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("The file format is not corret!" + '\n' + "Maybe the given number of crossings or streets is not correct!", 
                                        "Can't read file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                }
            }
            return true;
        }


        // Finds ALL allowed connections for EVERY crossing(turning right)
        void FindAllAllowedConnections()
        {
            foreach (KeyValuePair<String, Crossing> entry in crossings)
            {
                entry.Value.FindAllowedConnectionsForThisCrossing();
            }
        }
        //Calculates the distances for ALL connections
        void CalculateAllDistances()
        {
            foreach (KeyValuePair<String, Crossing> item in crossings)
            {
                item.Value.CalculateLengthToAllConnectedCrossings();
            }
        }

        //Finds The Greatest Factor caused by the ban of turning left
        void findGreatestFactor()
        {
            float factorMeasurementCrossing = 0;
            String factorMeasurementCrossingNames = "";

            float factorMeasurementDistance = 0;
            String factorMeasurementDistanceNames = "";

            int i = 0;
            int j = 0;

            foreach (KeyValuePair<String, Crossing> item in this.crossings)
            {
                foreach (KeyValuePair<String, Crossing> item2 in this.crossings)
                {
                    //if a pair of crossings doesnt have a shortest path
                    //--> Not all crossings are reachable from all crossings
                    if (this.distances[i][j] >= float.MaxValue / 2)
                    {
                        this.allNodesAreReachable = false;

                        this.greatestFactorDistances = "Factor = INFINITY";
                        this.greatestFactorCrossing = "Because you can not reach all crossings!!!";

                        return;
                    }
                        

                    //check if a new greatest factor was found
                    //measurement -- DISTANCE
                    if (this.distances[i][j] / this.distancesWITH_LEFT[i][j] > factorMeasurementDistance)
                    {
                        factorMeasurementDistance = this.distances[i][j] / this.distancesWITH_LEFT[i][j];
                        factorMeasurementDistanceNames = "From " + item.Key + " To " + item2.Key + " Was Increased by the factor of ";
                    }

                    //check if a new greatest factor was found
                    //measurement -- CROSSINGS
                    if (this.distancesCrossingMeasured[i][j] / this.distancesCrossingMeasuredWITH_LEFT[i][j] > factorMeasurementCrossing)
                    {
                        factorMeasurementCrossing = this.distancesCrossingMeasured[i][j] / this.distancesCrossingMeasuredWITH_LEFT[i][j];
                        factorMeasurementCrossingNames = "From " + item.Key + " To " + item2.Key + " Was Increased by the factor of ";
                    }

                    j++;
                }
                j = 0;
                i++;
            }

            this.greatestFactorDistances = "Distances " + factorMeasurementDistanceNames + factorMeasurementDistance.ToString();
            this.greatestFactorCrossing = "Crossings " + factorMeasurementCrossingNames + factorMeasurementCrossing.ToString();
        }


        //Restarts the Application...
        public void RestartApplication()
        {
            //RESTART PROGRAM
            crossings = new Dictionary<String, Crossing>();
            crossingsInMap = 0;
            streetsInMap = 0;            
        }
        
    }
    
}
