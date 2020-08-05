using System;
using System.Collections.Generic;
using System.IO;

namespace rechtsrum_in_Rechthausen
{
    public class Rechthausen
    {
        //the dictionary with all crossings that are in this map
        public Dictionary<String, Crossing> crossings;
        //ALL distances from every crossing to every crossing
        float[,] distances;
        //the amount of crossings and streets in this map
        int crossingsInMap = 0;
        int streetsInMap = 0;

        //if program is running in debugmode
        bool debugMode;

        //constructor
        public Rechthausen()
        {
            crossings = new Dictionary<String, Crossing>();
        }

        //the routine that is the actual main routine
        public void MainProgram()
        {
            //ask the user if program should run in debugmode
            DebugMode();

            Console.WriteLine("#####################################################################");
            Console.WriteLine("#                Aufgabe 2: Rechtsrum in Rechthausen                #");
            Console.WriteLine("#                                                                   #");
            Console.WriteLine("#                Calculates shortest path from S to D               #");
            Console.WriteLine("#         Checks if every node is reachable from every other        #");
            Console.WriteLine("# Determines the greatest Factor caused by the ban of turning right #");
            Console.WriteLine("#                                                                   #");
            Console.WriteLine("#                    Developed by Dario Klepoch                     #");
            Console.WriteLine("#                                                                   #");
            Console.WriteLine("#####################################################################");

            //Outputs the user the current possibilities for the user
            GiveAdvise();

            //Get the path to the file
            string userPath;
            int cnt = 0;
            do
            {
                if (cnt > 0) Console.WriteLine("Can not find file or directory!" + "\n" + "Please enter a valid path!");
                //Gets the path to the File
                userPath = GetPath();
                cnt++;
            } while (userPath == null);
            
            //READ AND CREATES THE MAP AND DATA
            //Reads the and creates all Crossings
            ReadInputFile(userPath);
            //outputs all Crossings
            if(debugMode)
                DebugCrossings();
            //Find all allowed Connections for all Crossings
            FindAllAllowedConnections();
            //calculate the distance to all Crossings
            CalculateAllDistances();

            //??

            //the table for all distances between the crossings
            distances = new float[crossings.Count, crossings.Count];

            //finds all possible pathes AND shortest pathes
            Pathfinding path = new Pathfinding(debugMode, crossings);
            //distances = 
            path.findAllShortestPathes();

            //??

            //---------------------------------------------------
            //ENDLESS LOOP OF THE MAIN MENU UNTIL USER DECIDES TO CLOSE OR RESTART
            //
            ProgramMainmenu();
            //---------------------------------------------------
        }

        //Asks the user weather or not the program should run in debugmode
        public void DebugMode()
        {
            //Ask if Program Should run in debugmode
            Console.WriteLine("Run Program in debug mode?");
            Console.WriteLine("Debug Mode has an impact on the performance!!!");
            Console.WriteLine("");

            string tmp = Console.ReadLine().ToUpper();
            Console.Clear();
            if (tmp == "YES" || tmp == "Y" || tmp == "JO" || tmp == "J")
            {
                debugMode = true;
                
                Console.WriteLine("Program will run in Debugmode...");
            }

            else
            {
                debugMode = false;

                Console.WriteLine("Program will not run in Debugmode...");
            }
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        //Outputs the Hint for the User
        public void GiveAdvise()
        {
            Console.WriteLine("Please input a path to a support file...");
        }

        //gets the path to the file
        public string GetPath()
        {
            string path = "";
            path = Console.ReadLine();

            if (path == "") path = "D:/BWINF/2016/2. Runde/rechtsrum_in_Rechthausen/Beispiele/ganz_einfach_02.txt";
            //"D:/BWINF/2016/2. Runde/rechtsrum_in_Rechthausen/Samples/001.txt";

            if (File.Exists(path)) return path;

            else return null;
        }

        //reads the file and creates all crossings
        public void ReadInputFile(string _userPath)
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
                    //READ THE FIRST LINE WITH THE NUMBER OF CROSSINGS AND STREETS
                    if (crossingsInMap == 0 && streetsInMap == 0)
                    {
                        while (true)
                        {
                            if (line.Length == i)
                            {
                                streetsInMap = Int32.Parse(tmp);
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
                                tmp = "";
                                cnt++;
                                break;
                            case 2:
                                y = float.Parse(tmp, System.Globalization.CultureInfo.InvariantCulture);
                                tmp = "";
                                crossingCnt++;
                                cnt = 0;
                                crossings.Add(n, new Crossing(n, x, y, crossingCnt));
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
                        

                        //creates the connection between the crossings
                        crossings[tmp].connections.Add(crossings[tmp2]);
                        crossings[tmp2].connections.Add(crossings[tmp]);
                    }            
                }             
            }
            Console.WriteLine("This map consists of " + crossingsInMap + " Crossing(s) and " + streetsInMap + " Street(s)");
            if (!debugMode) Console.WriteLine("");
        }

        //Prints all crossings and their connections
        public void DebugCrossings()
        {
            if (!debugMode) return;
            foreach (KeyValuePair<String, Crossing> item in crossings)
            {
                Console.Write("\n");
                Console.WriteLine(item.Value.name);
                Console.WriteLine(item.Value.x);
                Console.WriteLine(item.Value.y);

                for (int j = 0; j < item.Value.connections.Count; j++)
                {
                    Console.WriteLine("Connection To " + item.Value.connections[j].name);
                }

            }
        }

        /// <summary>
        /// Finds ALL allowed connections for EVERY crossing(turning right)
        /// </summary>
        void FindAllAllowedConnections()
        {
            foreach (KeyValuePair<String, Crossing> entry in crossings)
            {
                entry.Value.FindAllowedConnectionsForThisCrossing();

                entry.Value.printAllowedConnections(debugMode);

                if(debugMode) Console.WriteLine("");
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

        /// <summary>
        /// The Mainmenu of the program
        /// </summary>
        /// <param name="_taskNumber">if the Menu should start in a specified task</param>
        public void ProgramMainmenu(string _taskNumber = "")
        {
            string userInput = "";

            if (_taskNumber != "") userInput = _taskNumber;
            
            else
            {
                //Output possibilities
                Console.WriteLine("Input one of the following numbers to do the according task...");
                Console.WriteLine("1 -- Find the shortest way From S to D");
                Console.WriteLine("2 -- Decides wether or not it is possible to reach all Crossings from all others");
                Console.WriteLine("3 -- Determinates the start crossing from which the ");
                Console.WriteLine("     waylength to the destination crossing is increased by the ");
                Console.WriteLine("     highest possible factor because of the ban of turning left");
                Console.WriteLine("4 -- Restart Programm and investigate a new File");
                Console.WriteLine("5 -- Close Programm");

                userInput = Console.ReadLine();
            }

            switch (userInput)
            {
                //FIND THE SHORTEST PATH BETWEEN TO CROSSINGS
                case "1":
                    GetInputFor_ProgramTask_1_FindShortestPath();
                    break;
                //DETERMINES IF ALL CROSSINGS ARE REACHABLE
                case "2":
                    ProgramTask_2_DeterminePossiblities();
                    break;
                //FIND MAX WAY LENGTH CAUSED BY BAN OF TURNING LEFT
                case "3":
                    ProgramTask_3_FindLongestWay();
                    break;
                //RESTART PROGRAM
                case "4":
                    ProgramTask_4_RestartApplication();
                    break;
                //CLOSE PROGRAM
                case "5":
                    ProgramTask_5_CloseApplication();
                    break;
                //WRONG INPUT
                default:
                    Console.WriteLine("");
                    Console.WriteLine("WRONG INPUT!!!");
                    Console.WriteLine("");
                    Console.WriteLine("Try again!");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    //Show Mainmenu again
                    ProgramMainmenu();
                    break;
            }
        }

        //GETS THE INPUT FOR THE TASK
        //Find the Shortest Path From a start to a destiation crossing
        public void GetInputFor_ProgramTask_1_FindShortestPath()
        {
            //Input of the Start and Destination crossing
            Console.WriteLine("Input one of the following Crossing names to select a START Crossing...");
            foreach (KeyValuePair<String, Crossing> item in crossings)
            {
                Console.WriteLine(item.Key.ToString());
            }
            String startName = Console.ReadLine().ToUpper();
            //checks if start Crossing is valid
            if (!(crossings.ContainsKey(startName)))
            {
                Console.WriteLine("Wrong Input!");
                ProgramMainmenu("1");
            }
            Console.WriteLine("Input one of the following Crossing names to select a DESTINATION Crossing...");
            foreach (KeyValuePair<String, Crossing> item in crossings)
            {
                Console.WriteLine(item.Key.ToString());
            }
            String destinationName = Console.ReadLine().ToUpper();
            //checks if destination Crossing is valid
            if (!(crossings.ContainsKey(destinationName)))
            {
                Console.WriteLine("Wrong Input!");
                ProgramMainmenu("1");
            }

            ProgramTask_1_FindShortestPath(startName, destinationName);
        }

        /// <summary>
        /// Returns a List with all crossings for the shortest path from S to D
        /// LOWEST LENGTH TO REACH DESTINATION
        /// </summary>
        /// <param name="s">the name of the start crossing</param>
        /// <param name="d">the name of the destination crossing</param>
        /// <returns></returns>
        List<Crossing> ProgramTask_1_FindShortestPath(string _s, string _d)
        {
            //the path from S to D
            List<Crossing> shortestpath = new List<Crossing>();
            shortestpath.Add(crossings[_s]);



            //returns the shortest possible path from S to E
            return shortestpath;
            //return to mainmenu
            ProgramMainmenu();
        }

        //GETS THE INPUT FOR THE TASK
        //Detemines if all Crossings are reachable from every crossings
        public void ProgramTask_2_DeterminePossiblities()
        {
            Console.WriteLine("NOT IMPLEMENTED YET!");

            ProgramMainmenu();
        }

        //GETS THE INPUT FOR THE TASK
        //Finds the way which is increased by the maximum factor because of the ban of turning left
        public void ProgramTask_3_FindLongestWay()
        {
            Console.WriteLine("NOT IMPLEMENTED YET!");
            ProgramMainmenu();
        }

        //Restarts the Application...
        public void ProgramTask_4_RestartApplication()
        {
            Console.Clear();
            //RESTART PROGRAM
            crossings = new Dictionary<String, Crossing>();
            crossingsInMap = 0;
            streetsInMap = 0;
            MainProgram();
        }

        //Closes the Application...
        public void ProgramTask_5_CloseApplication()
        {
            Environment.Exit(0);
        }
    }
}
