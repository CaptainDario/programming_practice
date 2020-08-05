using System;
using System.Collections.Generic;

using System.Threading.Tasks;

using System.Diagnostics;


namespace rechtsrum_in_Rechthausen
{
    class Pathfinding
    {
        bool debugMode;
        //divided by two to avoid stack overflow
        float max = float.MaxValue / 2;
        //number of Crossings in the graph
        int crossingsCnt;
        Dictionary<String, Crossing> crossings;
        //
        float[][] allShortestPaths;


        public Pathfinding(bool _debugMode, Dictionary<String, Crossing> _dic)
        {
            debugMode = _debugMode;

            this.max = float.MaxValue / 2;
            this.crossingsCnt = _dic.Count;
            this.crossings = _dic;

            this.allShortestPaths = new float[this.crossingsCnt][];
        }


        public class AllNodesComparer<TKey> : IComparer<TKey> where TKey : IComparable
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



        public float[][] findAllShortestPathes()
        {
            
            //find shortest paths
            //From ONE Node to all Other 
            //repeated for all nodes
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //if the system has multiple cores to use 
            if (true)
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
            
            
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);

            return this.allShortestPaths;
        }

        private void multi(Crossing _value)
        {
            //creates a list with all nodes for dijkstra to use
            initializeListForDijkstra();
            
            this.allShortestPaths[_value.number - 1] = dijkstra(_value);
        }

        private float[] dijkstra(Crossing start)
        {
            //the List with all node that need to be worked off
            SortedList<float, Node> queue = new SortedList<float, Node>(new AllNodesComparer<float>());
            //queue.Capacity = this.crossingsCnt;
            //List with all nodes in ths map
            Dictionary<String, Node> allNodes = initializeListForDijkstra();
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
                float dist = start.distances[start.connections[i].name];
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
                for (int i = 0;  i < crossings[currentNode.name].allowedConnections[currentNode.predecessor].Count; i++)
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
                    float currentDist = crossings[currentNode.name].distances[nextCrossing.name] + currentNode.cost;
                    if (currentDist < allNodes[nextCrossing.name].cost)
                    {
                        //set predecessor and cost of the next node
                        allNodes[nextCrossing.name].predecessor = currentNode.name;
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
                        Node tmp = new Node(nextCrossing.name);
                        tmp.cost = currentDist;
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
                
                //remove the node from the queue
                queue.RemoveAt(0);
            }

            if (debugMode)
            {
                foreach (KeyValuePair<String, Node> item in allNodes)
                {
                    Console.WriteLine("From " + start.name + " --> " + item.Key + " in " + item.Value.cost.ToString());
                }


                Console.WriteLine("Calculated shortest paths from " + start.name + " to all other crossings");
                Console.WriteLine("------------------------------------------------------------------------");
                Console.WriteLine("");
            }

            int o = 0;
            foreach (KeyValuePair<String, Node> item in allNodes)
            {
                costs[o] = item.Value.cost;
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

                Node node = new Node(item2.Key);
                nodes.Add(item2.Key, node);

            }

            return nodes;
        }
    }   
}
