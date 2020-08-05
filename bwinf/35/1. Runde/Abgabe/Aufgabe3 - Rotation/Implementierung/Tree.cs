using System.Collections.Generic;

namespace Rotation
{
    public class Tree
    {
        //alle Drehungen die  schonmal vorkamen
        public List<Matrix> topMatrixs = new List<Matrix>();
        public List<Matrix> rightMatrixs = new List<Matrix>();
        public List<Matrix> downMatrixs = new List<Matrix>();
        public List<Matrix> leftMatrixs = new List<Matrix>();

        //Die Startmatrix dieses Raetsels
        public Node startNode;
    


        public Tree(Node _startNode)
        {
            startNode = _startNode;
        }

        public char[] RotationToSolution(Node _lastNode)
        {
            //die Drehungen um die Loesung zu bekommen
            char[] solution = new char[_lastNode.depth + 6];
            //der jetzige Knoten
            Node current = _lastNode;

            int i = 0;
            while (current.previous != null)
            {
                solution[current.depth + 5] += current.lastRotation; 
                
                current.value.PrintMatrix(current.value.content);
                current = current.previous;
                i++;
            }
            solution[5] += ' ';
            solution[4] += '>';
            solution[3] += '-';
            solution[2] += '-';
            solution[1] += ' ';
            solution[0] += 'S';
            return solution;
        }
    }

    public class Node
    {
        //die vorherige Matrix
        public Node previous;
        //die jetzige Matrix
        public Matrix value;
        //die Tiefe der jetzigen Verdrehung
        public int depth;
        //die letzte Richtung in die gedreht wurde
        public char lastRotation;
        //Ob diese Verdrehung schon einmal vorkam
        public bool deadNode = false;
        

        public Node(Matrix _value, char _lastRotation, int _depth, Node _previous)
        {
            this.value = _value;
            this.depth = _depth;
            this.lastRotation = _lastRotation;
            this.previous = _previous;
        }

    }

}