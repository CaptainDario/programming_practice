using System;
using System.IO;
using System.Collections.Generic;

namespace Rotation
{
    class Program
    {
        static Matrix puzzle;
        static string path = "";

        static void Main(string[] args)
        {
            UserInput();
        }

        static void UserInput()
        {
            Console.WriteLine("Input Path to a Puzzle!");
            path = Console.ReadLine();
            
            if (File.Exists(path))
            {
                Console.WriteLine("");

                ReadFile();
            }
            else
            {
                Console.WriteLine("Can't find file!");
                RestartProgramm();
            }

            FindSolution();

            RestartProgramm();
        }

        static void ReadFile()
        {
            StreamReader r = new StreamReader(path);

            int posX = 0;
            int posY = -1;

            string currentLine = "";
            do{
                char c = (char)r.Read();

                //wenn es die erste ziffer der Datei ist
                //--> die Groesse des puzzles
                if (posY == -1 && char.IsNumber(c))
                {
                    string tmp = c.ToString();


                    while (true)
                    {
                        c = (char)r.Read();
                        if (char.IsNumber(c))
                        {
                            tmp += c;
                        }
                        else
                            break;  
                    }

                    puzzle = new Matrix(int.Parse(tmp), 999);
                }                 
                //wenn es nicht die erste Ziffer ist
                //--> Beschreibung der Bloecke
                if (posY != -1 && char.IsNumber(c))
                    puzzle.content[posY, posX] = c;

                
                if (c == ' ')
                {
                    puzzle.content[posY, posX] = ' ';
                    //die Richtung in die die Oeffnung zeigt 
                    if (posY == 0)
                        puzzle.direction = 0;

                    else if (posY == puzzle.last)
                        puzzle.direction = 2;

                    else if (posX == 0)
                        puzzle.direction = 3;

                    else if (posX == puzzle.last)
                        puzzle.direction = 1;
                }
                    
                if (c == '#')
                    puzzle.content[posY, posX] = '#';
                

                if (c != '\r')
                {
                    currentLine += c;
                    posX++;
                }       
                else if(c == '\r')
                {
                    r.Read();
                    
                    currentLine = "";
                    posY++;
                    posX = 0;
                }
            } while (!r.EndOfStream);
            r.Close();

            Console.WriteLine("----------------------------------------");
            Console.WriteLine("<-- ORIGINAL MARIX -->");
            Console.WriteLine("----------------------------------------");
            puzzle.PrintMatrix(puzzle.content);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("");
        }

        static void FindSolution()
        {
            Tree puzzleTree = new Tree(new Node(puzzle, 'S', 0, null));

            bool foundASolution = false;

            //liste mit allen noch nicht untersuchten nodes
            List<Node> nodes = new List<Node>();
            nodes.Add(puzzleTree.startNode);


            Node n = null;

           

            int tmpDep = 999;

            while (!foundASolution && nodes.Count > 0)
            {
                
                if (nodes[0].depth != tmpDep)
                {
                    Console.WriteLine(nodes[0].depth + "  ||  CompareTo = " + (puzzleTree.downMatrixs.Count +
                                                                          puzzleTree.topMatrixs.Count +
                                                                          puzzleTree.rightMatrixs.Count +
                                                                          puzzleTree.leftMatrixs.Count)
                                                     + "  ||  ToCheck: " + nodes.Count);
                    tmpDep = nodes[0].depth;
                }
                
                    
                if (!nodes[0].deadNode)
                {
                    //immer die erste Verdrehung aus der Liste mit allen Verdrehungen nehmen
                    puzzle = nodes[0].value;


                    //uberprueft die jetztige Verdrehung auf gleichheit  mit einer vorherigen Verdrehung des puzzles
                    if (puzzle.direction == 0)
                    {
                        int tmp = puzzleTree.topMatrixs.BinarySearch(puzzle, new Matrix.MatrixComparer());

                        if (tmp < 0)
                        {
                            tmp = tmp * (-1) - 1;
                            puzzleTree.topMatrixs.Insert(tmp, new Matrix(puzzle.size, puzzle.direction, puzzle.content));
                        }
                        else if (tmp > -1)
                        {
                            nodes[0].deadNode = true;
                            nodes.RemoveAt(0);
                            continue;
                        }

                    }
                    else if (puzzle.direction == 1)
                    {
                         
                        int tmp = puzzleTree.rightMatrixs.BinarySearch(puzzle, new Matrix.MatrixComparer());
                           

                        if (tmp < 0)
                        {
                            tmp = tmp * (-1) - 1;
                            puzzleTree.rightMatrixs.Insert(tmp, new Matrix(puzzle.size, puzzle.direction, puzzle.content));
                        }
                        else if (tmp > -1)
                        {
                            nodes[0].deadNode = true;
                            nodes.RemoveAt(0);
                            continue;
                        }
                    }
                    else if (puzzle.direction == 2)
                    {
                        int tmp = puzzleTree.downMatrixs.BinarySearch(puzzle, new Matrix.MatrixComparer());
                        if (tmp < 0)
                        {
                            tmp = tmp * (-1) - 1;
                            puzzleTree.downMatrixs.Insert(tmp, new Matrix(puzzle.size, puzzle.direction, puzzle.content));
                        }
                        else if (tmp > -1)
                        {
                            nodes[0].deadNode = true;
                            nodes.RemoveAt(0);
                            continue;
                        }
                    }
                    else if (puzzle.direction == 3)
                    {
                        int tmp = puzzleTree.leftMatrixs.BinarySearch(puzzle, new Matrix.MatrixComparer());

                        if (tmp < 0)
                        {
                            tmp = tmp * (-1) - 1;
                            puzzleTree.leftMatrixs.Insert(tmp, new Matrix(puzzle.size, puzzle.direction, puzzle.content));
                        }
                        else if (tmp > -1)
                        {
                            nodes[0].deadNode = true;
                            nodes.RemoveAt(0);
                            continue;
                        }
                    }

                    //DREHEN
                    //...das Puzzle nach rechts drehen
                    char[,] copyR = (char[,])puzzle.content.Clone();
                    puzzle.RotateRight(out foundASolution, copyR);

                    //wenn das puzzle nach rechts gedreht wird die direction in die das loch zeigt mit aendern
                    int direction = puzzle.direction + 1;
                    if (direction > 3)
                        direction = 0;

                    //das puzzle nach rechts drehen und der Liste mit allen Nodes hinzufuegen
                    Node rightTransform = new Node(new Matrix(nodes[0].value.size, direction, copyR),
                                                    'r', nodes[0].depth + 1, nodes[0]);
                    nodes.Add(rightTransform);
                    if (foundASolution)
                    {
                        n = rightTransform;
                        break;
                    }

                    //...das Puzzle nach links drehen
                    
                    char[,] copyL = (char[,])puzzle.content.Clone();
                    puzzle.RotateLeft(out foundASolution, copyL);

                    //wenn das puzzle nach links gedreht wird die direction in die das loch zeigt mit aendern
                    direction = puzzle.direction - 1;
                    if (direction < 0)
                        direction = 3;

                    //das puzzle nach links drehen und der Liste mit allen Nodes hinzufuegen
                    Node leftTransform = new Node(new Matrix(nodes[0].value.size, direction, copyL),
                                                    'l', nodes[0].depth + 1, nodes[0]);
                    nodes.Add(leftTransform);
                    if (foundASolution)
                    {
                        n = leftTransform;
                        break;
                    }
                      
                    nodes.RemoveAt(0);
                }
            }

            if (n != null)
            {
                Console.WriteLine("FOUND A SOLUTION!!");
                Console.Write(puzzleTree.RotationToSolution(n));
            }
            else
            {
                Console.WriteLine("Their is no solution for this puzzle...");
            }
        }

        static void RestartProgramm()
        {
            Console.WriteLine("");
            Console.WriteLine("");

            puzzle = null;

            UserInput();
        }
    }
}
