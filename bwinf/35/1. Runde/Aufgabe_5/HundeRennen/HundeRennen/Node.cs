

namespace HundeRennen
{
    public class Node
    {
        public string Name;
        public int PosX;
        public float PosY;

        public bool active;
        public double cost;
        public double maxiTime;
        public Node previous;

        /// <summary>
        /// Konstuktor
        /// </summary>
        /// <param name="name">Name des Knoten</param>
        /// <param name="posX">Die X-position des Knotens</param>
        /// <param name="posY">Die Y-Position des Knotens</param>
	    public Node(string _name, int _posX, float _posY)
        {
            this.Name = _name;
            this.PosX = _posX;
            this.PosY = _posY;

            this.active = true;
            this.cost = double.MaxValue;
            this.maxiTime = double.MaxValue;
        }
    }
}