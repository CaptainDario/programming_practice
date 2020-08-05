using System;
using System.Windows;
using System.Collections.Generic;

namespace rechtsrum_in_Rechthausen
{
    public class Crossing
    {
        readonly public string name;
        readonly public float x;
        readonly public float y;

        //the number of this crossing in the dictionary
        readonly public int number;

        public List<String> connectionsNames;


        


        // All connections to this crossing
        public List<Crossing> connections = new List<Crossing>();

        // the distances to all connections to this crossing
        // KEY(Char) -- the name of the crossing distance is measured to
        // VALUE(double) -- the distance to crossing 
        public Dictionary<String, float> distances = new Dictionary<String, float>();

        // The connections that are allowed to drive
        // Key(Char) of the Dictionary is the name of the Crossing we are coming from
        // List<Crossing> are all allowed directions to drive to coming from the key(Char)
        public Dictionary<String, List<Crossing>> allowedConnections = new Dictionary<String, List<Crossing>>();


        /// <summary>
        /// initializes a new Crossing
        /// </summary>
        /// <param name="_n">Name of the crossing</param>
        /// <param name="_x">X-Coordinate of the crossing</param>
        /// <param name="_y">Y-Coordinate of the crossing</param>
        public Crossing(string _n, float _x, float _y, int _nr)
        {
            this.name = _n;
            this.x = _x;
            this.y = _y;

            this.number = _nr;
        }


        /// <summary>
        /// Finds all allowed connections to crossings from this node
        /// </summary>
        public void FindAllowedConnectionsForThisCrossing()
        {
            
            //loop over all the connections from this Crossing 
            //--> coming from all possible Crossings to this one
            for (int coming = 0; coming < connections.Count; coming++)
            {
                //List with all allowed connections
                //from a specified crossing to a specified Crossing over this Crossing
                List<Crossing> tmpAllowedCrossings = new List<Crossing>();
                //the name of the crossing we're coming from
                String comingName = connections[coming].name;
                //the greatest angle coming from a specified crossing
                //over this crossing to the destination crossing
                //if no allowed connection was found 
                //--> take the one with the greates angle
                double smallestAngle = 360;
                Crossing smallestAngleGoingTo = new Crossing("", 0, 0, 0);

                //if one street is connected with this crossing
                // --> go back the way you are coming from
                if (this.connections.Count == 1)
                {
                    allowedConnections.Add(comingName, new List<Crossing> { connections[coming] });
                }
                //if two streets are connected with this crossing
                else if (this.connections.Count == 2)
                {
                    if(coming == 0) allowedConnections.Add(comingName, new List<Crossing> { connections[1] });
                    else if (coming == 1) allowedConnections.Add(comingName, new List<Crossing> { connections[0] });
                }
                //if more then two streets are connected with this crossing
                else if (this.connections.Count > 2)
                {
                    //loop over all the connections from this Crossing 
                    //--> going to all possible crossings
                    for (int going = 0; going < connections.Count; going++)
                    {
                        //if the node we're coming from is the same we're going to
                        if (coming == going) continue;

                        //Vector pointing from the STARTING crossing
                        //to this crossing
                        Vector comingVec = new Vector((this.x - connections[coming].x),
                                                (this.y - connections[coming].y));
                        //Vector pointing from THIS crossing
                        //to the crossing that must be checked
                        Vector goingVec = new Vector((connections[going].x - this.x),
                                                (connections[going].y - this.y));

                        //calculate the crossproduct and the angle between the Vectors
                        double crossProduct = Vector.CrossProduct(comingVec, goingVec);
                        double angleBetween = Vector.AngleBetween(comingVec, goingVec);

                        if (angleBetween < smallestAngle)
                        {
                            smallestAngle = angleBetween;
                            smallestAngleGoingTo = connections[going];
                        }

                        //it's is an allowed direction(turn right)
                        if (crossProduct <= 0)
                        {
                            //add this direction to the allowed List
                            tmpAllowedCrossings.Add(connections[going]);
                        }
                        //it isn't an allowed direction(turn left)
                        else continue;
                    }
                    //if (a) possible direction(s) was(were) found
                    if (tmpAllowedCrossings.Count > 0)
                    {
                        //adds the connections to the dictionary
                        allowedConnections.Add(comingName, tmpAllowedCrossings);
                    }
                    else if (tmpAllowedCrossings.Count == 0)
                    {
                        allowedConnections.Add(comingName, new List<Crossing> { smallestAngleGoingTo });
                    }
                    //if some Bug occured an you can't go anywhere!
                    else Console.WriteLine("Coming From " + comingName + " You are at " + this.name + " Something went wrong! You can't go anywerhe!!!");
                }
            }     
        }

        //calculates the distance to all Crossings connected to this crossing
        //AND for all crossings that are allowed to drive
        public void CalculateLengthToAllConnectedCrossings()
        {
            for (int i = 0; i < connections.Count; i++)
            {
                //the two points 
                Point point1 = new Point(this.x, this.y);
                Point point2 = new Point(connections[i].x, connections[i].y);
                //calculate the distance
                float distance = (float)Point.Subtract(point2, point1).Length;

                //add the length to the list
                distances.Add(connections[i].name, distance);
            }
        }

        /// <summary>
        /// Prints all allowed connections for this Crossing 
        /// coming from a specified Crossing
        /// </summary>
        public void printAllowedConnections(bool _debugMode)
        {
            if (!_debugMode) return;
            foreach (KeyValuePair<String, List<Crossing>> entry in allowedConnections)
            {
                for (int j = 0; j < entry.Value.Count; j++)
                {
                    Console.WriteLine(entry.Key + " --> " + this.name + " --> " + entry.Value[j].name);
                }
            }            
        }
    }
}
