using System.Collections.Generic;
using System;

namespace HundeRennen
{
    public class Dijkstra
    {
        //Node mit dem kuerzesten weg
        Node lastNode = new Node("", 0, 0f);

        /// <summary>
        /// ermittelt die costs fuer alle Nodes
        /// </summary>
        /// <param name="_nodes">die Liste mit allen nodes(1d)</param>
        /// <param name="_nodesLL">die Liste mit allen nodes(2d)</param>
        /// <param name="_edges">die Liste mit allen nodes</param>
        /// <param name="_start">der Start Punkt</param>
        /// <returns></returns>
        public void CalculateMinDistForNodes(List<Node> _nodes, List<List<Node>> _nodesLL, List<Edge> _edges, Node _start)
        {
            //die queue mit allen abzuarbeitenden nodes
            List<Node> list = new List<Node>();        

            //die Liste sortieren
            _nodes.Sort((x, y) => x.cost.CompareTo(y.cost));

            //alle knoten der schlange hinzufuegen
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].cost = double.MaxValue;
                list.Add(_nodes[i]);
            }

            //die kosten fuer den startknoten gleich null setzten
            _start.cost = 0;

            //solange noch nodes in der queue sind
            while (list.Count != 0)
            {
                Node v = list[0];

                //alle Edges suchen...
                foreach (var e in _edges)
                {
                    //... die den Node am Anfang der Schlange als origin haben
                    if (e.Origin == v)
                    {
                        Node a = e.Destination;
                        Node b = e.Origin;

                        //wenn die kosten plus die kantenlaenge < als alte kosten
                        if (a.cost > b.cost + e.Distance)
                        {
                            //cost aktualisieren
                            a.cost = b.cost + e.Distance;
                            //vorherigen node setzten
                            a.previous = b;
                        }
                    }
                }

                //den node am anfang der Queue entfernen
                list.RemoveAt(0);

                //die Liste sortieren
                list.Sort((x, y) => x.cost.CompareTo(y.cost));
               
            }
        }

        /// <summary>
        /// den kuerzesten weg zu der letzten buhne finden
        /// </summary>
        public List<Node> ShortestPath(List<List<Node>> _nodesLL, Node targetNode)
        {
            lastNode = targetNode;

            /*
            //node als unendliche cost an nehmen
            lastNode.cost = Double.MaxValue;
 
            //alle  nodes der letzten buhnen
            for (int x = 0; x < _nodesLL[_nodesLL.Count - 1].Count; x++)
            {
                //wenn cost von diesem Node niedriger ist als LastNode
                // --> dieser Node = lastNode
                if (lastNode.cost > _nodesLL[_nodesLL.Count - 1][x].cost)
                    lastNode = _nodesLL[_nodesLL.Count - 1][x];
            }
            */

            //alle nodes von dem kuerzesten Weg
            List<Node> shortestPath = new List<Node>();
            //den last node an das Ende des Path setzen
            shortestPath.Add(lastNode);
            //den lastNode merken
            Node tmpLastNode = lastNode;
            //solange es einen Vorgaenger node gibt...
            while (lastNode.previous != null)
            {
                //...diesen Node an den Anfang des Path setzen
                shortestPath.Insert(0, lastNode.previous);
                //...und ihn als  lastNode annehmen
                lastNode = lastNode.previous;
            }
            //den gemerkten lastNode wieder als  lastNode setzten
            lastNode = tmpLastNode;

            return shortestPath;
        }

        /// <summary>
        /// gibt den Weg des shortestPath zurueck
        /// </summary>
        /// <returns>den weg von dem shortestPath(in metern)</returns>
        public double ShortestPathDistance()
        {
            return lastNode.cost;
        }

        /// <summary>
        /// berechnet die Zeit um eine strecke mit einer Geschwindigkeit zurueck zulegen
        /// </summary>
        /// <param name="dist">Die Strecke</param>
        /// <param name="velocity">Die Geschwindigkeit</param>
        /// <returns>Die Zeit die benoetigt wird</returns>
        public double TimeForRun(double dist, double velocity)
        {
            //dist - Meter
            //velocity - km / h
            //km / h = 1000 * m / 3600 * s

            //s / v = t
            double time = (dist) / (velocity * 1000 / 3600);

            return time;
        }
    }
}
