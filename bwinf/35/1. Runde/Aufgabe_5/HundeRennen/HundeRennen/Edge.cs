

namespace HundeRennen
{
    public class Edge
    {

        public Node Origin;
        public Node Destination;
        public double Distance;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="origin">Startknoten</param>
        /// <param name="destination">Zielknoten</param>
        /// <param name="distance">Distanz</param>
        public Edge(Node origin, Node destination, double distance)
        {
            this.Origin = origin;
            this.Destination = destination;
            this.Distance = distance;
        }
    }
}