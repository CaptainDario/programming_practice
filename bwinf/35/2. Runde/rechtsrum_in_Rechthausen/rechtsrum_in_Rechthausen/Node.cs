using System;
using System.Collections.Generic;


namespace rechtsrum_in_Rechthausen
{

    class Node
    {
        public readonly string name;

        public float cost;
        public bool visited;
        public string predecessor;

        public List<String> visitedNodes;

        public Node(string _name)
        {
            this.name = _name;


            //the variables used to execute dijkstra
            this.cost = float.MaxValue / 2;
            this.visited = false;
            this.predecessor = "";

            this.visitedNodes = new List<string>();
        }
    }
}
