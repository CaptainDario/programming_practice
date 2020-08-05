using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;



namespace rechtsrum_in_Rechthausen_FORMS
{
    public class Pathfinding
    {

        //number of Crossings in the graph
        int crossingsCnt;
        Dictionary<String, Crossing> crossings;
        //a jagged array with all shortest paths(DISTANCE MEASUREMENT)
        //sorted like it was read by the program
        float[][] allShortestPaths;
        //a jagged array with all shortest paths(CROSSING MEASUREMENT)
        //sorted like it was read by the program
        Tuple<float, List<String>>[][] allShortestPathsCrossingMeasurment;

        bool measurement;
        bool turningLeftAllowed;



        public Pathfinding(Dictionary<String, Crossing> _dic, bool _measurement, bool _turningLeftAllowed)
        {
            //this.max = float.MaxValue / 2;
            this.crossingsCnt = _dic.Count;
            this.crossings = _dic;

            this.allShortestPaths = new float[this.crossingsCnt][];
            this.allShortestPathsCrossingMeasurment = new Tuple<float, List<string>>[this.crossingsCnt][];

            this.measurement = _measurement;
            this.turningLeftAllowed = _turningLeftAllowed;
        }


        public class NodesComparer<TKey> : IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1;
                else
                    return result;
            }
        }

        //_measurement the type of measurment - false = Distance ; true = Crossings
        public float[][] findAllShortestPathes()
        {
            
            //find shortest paths
            //From ONE Node to all Other 
            //repeated for all nodes

            //if the system has multiple cores to use 
            if (Environment.ProcessorCount != 1)
            {
                Parallel.ForEach(this.crossings, item => multi(item.Value));
            }                     
            else
            {
                foreach (KeyValuePair<String, Crossing> item in this.crossings)
                {
                    multi(item.Value);
                }
            }
            

            return this.allShortestPaths;
        }

        //the function that will be spawned parallel on multiple cores
        //_measurement the type of measurment - false = Distance ; true = Crossings
        private void multi(Crossing _value)
        {
            //creates a list with all nodes for dijkstra to use
            initializeListForDijkstra();
            
            this.allShortestPaths[_value.number - 1] = dijkstra(_value);


        }

        /// <summary>
        /// Uses the Dijkstra Algorithm for finding all shortest paths
        /// from one node to all other
        /// </summary>
        /// <param name="start">The Crossing from which all shortest paths will be calculated</param>
        /// <param name="_measurement">the type of measurment - false = Distance ; true = Crossings</param>
        /// <returns></returns>
        private float[] dijkstra(Crossing start)
        {
            //the List with all nodes that need to be worked off
            SortedList<float, Node> queue = new SortedList<float, Node>(new NodesComparer<float>());
            //List with all nodes in ths map
            Dictionary<string, Node> allNodes = initializeListForDijkstra();
            //the costs to reach all other nodes from this one
            float[] costs = new float[crossingsCnt];

            //set the cost for the startnode to zero
            allNodes[start.name].cost = 0;

            //add all crossings connected with the start cossing 
            //--> at the start their is no turning right
            //----> INITIALIZING THE LIST
            for (int i = 0; i < start.connections.Count; i++)
            {
                //add the queue the nodes to work off
                //the direct connections from the start node(no turning right from the start crossing)
                float dist = 1f;
                if (!this.measurement)
                    dist = start.distances[start.connections[i].name];


                Node currentNode = allNodes[start.connections[i].name];
                //set the dist to this node
                currentNode.cost = dist;
                //set the start node as the predecessor
                currentNode.predecessor = start.name;
                //add the node to the visited nodes list from the start node
                allNodes[start.name].visitedNodes.Add(currentNode.name);

                //add the new node reached from the start node to the queue
                queue.Add(dist, currentNode);
            }
            //set the start node as worked off
            if(visitedNodesEqual_To_AllNodes(allNodes[start.name]))
                allNodes[start.name].visited = true;

            
            while (queue.Count != 0)
            {
                //the current node to work with
                Node currentNode = queue.Values[0];

                //add all allowed(turning right) connections from the current node   
                if (!this.turningLeftAllowed)
                {
                    for (int i = 0; i < crossings[currentNode.name].allowedConnections[currentNode.predecessor].Count; i++)
                    {
                        //the crossing we want to go to
                        Crossing nextCrossing = crossings[currentNode.name].allowedConnections[currentNode.predecessor][i];

                        if (allNodes[nextCrossing.name].visited || currentNode.visitedNodes.Contains(nextCrossing.name))
                            continue;


                        //add the next Crossing to the visited ones from this node
                        currentNode.visitedNodes.Add(nextCrossing.name);

                        //checks the distance to the next node
                        //--> if less then current value override it
                        //--> and add the node to the queue
                        float currentDist = 1f + currentNode.cost;
                        if (!this.measurement)
                            currentDist = crossings[currentNode.name].distances[nextCrossing.name] + currentNode.cost;

                        if (currentDist < allNodes[nextCrossing.name].cost)
                        {
                            //set predecessor and cost of the next node
                            //allNodes[nextCrossing.name].predecessor = new List<String>(currentNode.predecessor);
                            allNodes[nextCrossing.name].predecessor = (currentNode.name);
                            allNodes[nextCrossing.name].cost = currentDist;

                            //add the node that was the next-node in this iteration
                            //to the queue
                            queue.Add(currentDist, allNodes[nextCrossing.name]);
                        }
                        //if the distance is greater
                        //--> create a new node with the greater distance(Set predecessor, and the new distance)
                        //--> and add this one to the list
                        else
                        {
                            Node tmp = new Node(nextCrossing.name, false);
                            tmp.cost = currentDist;

                            //tmp.predecessor = new List<string>(currentNode.predecessor);
                            tmp.predecessor = currentNode.name;


                            tmp.visitedNodes = allNodes[nextCrossing.name].visitedNodes;

                            queue.Add(currentDist, tmp);
                        }

                        //check if all connection from this node were used 
                        if (visitedNodesEqual_To_AllNodes(currentNode))
                        {
                            currentNode.visited = true;

                        }

                    }
                }
               
                
                if (this.turningLeftAllowed)
                {
                    for (int i = 0; i < crossings[currentNode.name].connections.Count; i++)
                    {
                        //the crossing we want to go to
                        Crossing nextCrossing = crossings[currentNode.name].connections[i];

                        if (allNodes[nextCrossing.name].visited || currentNode.visitedNodes.Contains(nextCrossing.name))
                            continue;


                        //add the next Crossing to the visited ones from this node
                        currentNode.visitedNodes.Add(nextCrossing.name);

                        //checks the distance to the next node
                        //--> if less then current value override it
                        //--> and add the node to the queue
                        float currentDist = 1f + currentNode.cost;
                        if (!this.measurement)
                            currentDist = crossings[currentNode.name].distances[nextCrossing.name] + currentNode.cost;

                        if (currentDist < allNodes[nextCrossing.name].cost)
                        {
                            //set predecessor and cost of the next node
                            allNodes[nextCrossing.name].predecessor = (currentNode.name);
                            allNodes[nextCrossing.name].cost = currentDist;

                            //add the node that was the next-node in this iteration
                            //to the queue
                            queue.Add(currentDist, allNodes[nextCrossing.name]);
                        }

                    }
                    

                    currentNode.visited = true;
                }
                //remove the node from the queue
                queue.RemoveAt(0);
            }

            int o = 0;
            foreach (KeyValuePair<String, Node> item in allNodes)
            {
                costs[o] = item.Value.cost;
                o++;
            }
            return costs;
        }

        public Tuple<float, List<string>>[] dijkstraWithPath(Crossing start, bool _measurement)
        {
            //the List with all nodes that need to be worked off
            SortedList<float, Node> queue = new SortedList<float, Node>(new NodesComparer<float>());
            //List with all nodes in ths map
            Dictionary<string, Node> allNodes = initializeListForDijkstraWithPath();
            //the costs to reach all other nodes from this one
            Tuple<float, List<string>>[] costs = new Tuple<float, List<string>>[crossingsCnt];

            //set the cost for the startnode to zero
            allNodes[start.name].cost = 0;

            //add all crossings connected with the start cossing 
            //--> at the start their is no turning right
            //----> INITIALIZING THE LIST
            for (int i = 0; i < start.connections.Count; i++)
            {
                //add the queue the nodes to work off
                //the direct connections from the start node(no turning right from the start crossing)
                float dist = 1f;
                if (_measurement)
                    dist = start.distances[start.connections[i].name];

                Node currentNode = allNodes[start.connections[i].name];
                //set the dist to this node
                currentNode.cost = dist;
                //set the start node as the predecessor
                currentNode.predecessors.Add(start.name);
                //add the node to the visited nodes list from the start node
                allNodes[start.name].visitedNodes.Add(currentNode.name);
                //add the new node reached from the start node to the queue
                queue.Add(dist, currentNode);
            }
            //set the start node as worked off
            if (visitedNodesEqual_To_AllNodes(allNodes[start.name]))
                allNodes[start.name].visited = true;


            while (queue.Count != 0)
            {
                //the current node to work with
                Node currentNode = queue.Values[0];
                //distances as measurement
                
                    //add all allowed(turning right) connections from the current node
                    for (int i = 0; i < crossings[currentNode.name].allowedConnections[currentNode.predecessors[currentNode.predecessors.Count - 1]].Count; i++)
                    {
                        //the crossing we want to go to
                        Crossing nextCrossing = crossings[currentNode.name].allowedConnections[currentNode.predecessors[currentNode.predecessors.Count - 1]][i];

                        if (allNodes[nextCrossing.name].visited || currentNode.visitedNodes.Contains(nextCrossing.name))
                            continue;


                        //add the next Crossing to the visited ones from this node
                        currentNode.visitedNodes.Add(nextCrossing.name);

                        //checks the distance to the next node
                        //--> if less then current value override it
                        //--> and add the node to the queue
                        float currentDist = 1f + currentNode.cost;
                        if (_measurement)
                            currentDist = crossings[currentNode.name].distances[nextCrossing.name] + currentNode.cost;
                        if (currentDist < allNodes[nextCrossing.name].cost)
                        {
                            //set predecessor and cost of the next node
                            allNodes[nextCrossing.name].predecessors = new List<String>(currentNode.predecessors);
                            allNodes[nextCrossing.name].predecessors.Add(currentNode.name);
                            allNodes[nextCrossing.name].cost = currentDist;

                            //add the node that was the next-node in this iteration
                            //to the queue
                            queue.Add(currentDist, allNodes[nextCrossing.name]);
                        }
                        //if the distance is greater
                        //--> create a new node with the greater distance(Set predecessor, and the new distance)
                        //--> and add this one to the list
                        else
                        {
                            Node tmp = new Node(nextCrossing.name, true);
                            tmp.cost = currentDist;

                            tmp.predecessors = new List<string>(currentNode.predecessors);
                            tmp.predecessors.Add(currentNode.name);


                            tmp.visitedNodes = allNodes[nextCrossing.name].visitedNodes;

                            queue.Add(currentDist, tmp);
                        }

                        //check if all connection from this node were used 
                        if (visitedNodesEqual_To_AllNodes(currentNode))
                        {
                            currentNode.visited = true;

                        }


                    }
                

                //remove the node from the queue
                queue.RemoveAt(0);
            }

            int o = 0;
            foreach (KeyValuePair<String, Node> item in allNodes)
            {
                costs[o] = new Tuple<float, List<String>>(item.Value.cost, item.Value.predecessors);
                o++;
            }
            return costs;
        }

        //checks if all visited nodes from this node
        //are equal with all connections from this node
        private bool visitedNodesEqual_To_AllNodes(Node _toCheck)
        {
            bool equality = false;

            //if the lists have the same length 
            if (_toCheck.visitedNodes.Count == crossings[_toCheck.name].connections.Count)
            {
                equality = true;
            }           

            return equality;
        }

        //returns a list with all nodes
        //created from the Dictionary with all crossings
        private Dictionary<String, Node> initializeListForDijkstra()
        {
            Dictionary<String, Node> nodes = new Dictionary<String, Node>();

            //Initializes all nodes with infinity and no predecessor
            foreach (KeyValuePair<String, Crossing> item2 in this.crossings)
            {

                Node node = new Node(item2.Key, false);
                nodes.Add(item2.Key, node);

            }

            return nodes;
        }

        private Dictionary<String, Node> initializeListForDijkstraWithPath()
        {
            Dictionary<String, Node> nodes = new Dictionary<String, Node>();

            //Initializes all nodes with infinity and no predecessor
            foreach (KeyValuePair<String, Crossing> item2 in this.crossings)
            {

                Node node = new Node(item2.Key, true);
                nodes.Add(item2.Key, node);

            }

            return nodes;
        }
    }   
}
