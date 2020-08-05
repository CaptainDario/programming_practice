using System;
using System.Collections.Generic;


namespace rechtsrum_in_Rechthausen_FORMS
{

    class Node
    {
        public readonly string name;

        public float cost;
        public bool visited;
        public string predecessor;

        public List<String> predecessors;

        public List<String> visitedNodes;

        public Node(string _name, bool _withPath)
        {
            this.name = _name;


            //the variables used to execute dijkstra
            this.cost = float.MaxValue / 2;
            this.visited = false;
            this.predecessor = "";

            if (_withPath)
                predecessors = new List<String>();

            this.visitedNodes = new List<string>();
        }
    }
}
