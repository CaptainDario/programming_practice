using System;
using System.Collections.Generic;

namespace Rotation
{
    public class Matrix
    {
        public int size;
        private int level = 0;
        private decimal totalLevels;
        public int last;

        public char[,] content;

        //die Richtung in die die öffnung zeigt
        // 0 -- oben
        // 1 -- rechts
        // 2 -- unten
        // 3 -- links
        public int direction = 999;


        public Matrix(int _size, int _direction, char[,] _content = null)
        {
            this.direction = _direction;
            this.size = _size;
            this.last = _size - 1;
            decimal tmp = size / 2;
            this.totalLevels = Math.Floor(tmp);

            if(_content == null)
                content = new char[this.size, this.size];
        
            else
            {
                content = _content;
            }
        }

        public void PrintMatrix(char[,] _content)
        {
            for (int x = 0; x < this.size; x++)
            {
                for (int y = 0; y < this.size; y++)
                {
                    Console.Write(_content[x,y]);
                }
                Console.WriteLine("");
            }
        }

        public void RotateRight(out bool _foudASolution, char[,] _content = null)
        {
            if (_content == null)
                _content = this.content;

            level = 0;
            last = size - 1;
            while (level < totalLevels)
            {
                for (int x = level; x < this.last; x++)
                {
                    Swap(ref _content[level, x], ref _content[x, last]);
                    Swap(ref _content[level, x], ref _content[last, last - x + level]);
                    Swap(ref _content[level, x], ref _content[last - x + level, level]);
                }
                level++;
                last--;
            }
            
            /*
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("<-- ROTATED MARIX RIGHT -->");
            Console.WriteLine("----------------------------------------");
            */

            _foudASolution = false;
            if (this.ApplyGravity(_content))
                _foudASolution = true;
            /*
            this.PrintMatrix(_content);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("");
            */
        }

        public void RotateLeft(out bool _foudASolution, char[,] _content = null)
        {
            if (_content == null)
                _content = this.content;

            level = 0;
            last = size - 1;
            while (level < totalLevels)
            {
                for (int x = level; x < this.last; x++)
                {
                    Swap(ref _content[level, x], ref _content[last - x + level, level]);
                    Swap(ref _content[level, x], ref _content[last, last - x + level]);
                    Swap(ref _content[level, x], ref _content[x, last]);
                }
                level++;
                last--;
            }
            /*
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("<-- ROTATED MARIX LEFT -->");
            Console.WriteLine("----------------------------------------");
            */

            _foudASolution = false;
            if (this.ApplyGravity(_content))           
                _foudASolution = true;
            /*
            this.PrintMatrix(_content);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("");
            */
        }

        private void Swap(ref char i, ref char j)
        {
            char tmp = i;
            i = j;
            j = tmp;
        }

        public bool ApplyGravity(char[,] _content)
        {
            //ob eine Loesung gefunden wurde
            bool foundSolution = false;

            last = size - 1;
            //Console.WriteLine("");
            //Anfang bei der untersten Reihe 
            for (int y = this.last - 1; y > 0; y--)
            {
                //ganz links
                for (int x = 1; x < this.last; x++)
                {
                    //wenn dieses Feld eine Zahl ist
                    if (char.IsNumber(_content[y, x]))
                    {
                        //wenn der Block horizontal ausgerichtet ist
                        //ob an keiner Stelle ein BLock darunter ist
                        bool isMoveable = false;


                        //wenn der Block unter diesem Feld frei ist
                        //Und es nicht ein Teil aus dem Rahmen ist
                        //ODER 
                        if (_content[y + 1, x] == ' ')// && y + 1 != last )
                            isMoveable = true;

                        //die Ziffer fuer den momentanigenBlock
                        char type = _content[y, x];


                        //die Laenge des Blockes
                        int length = 1;
                        for (int right = x + 1; right < last; right++)
                        {
                            //wenn rechts daneben ein Block  ist 
                            if (_content[y, right] == type)
                                length++;
                            else
                                break;
                                    
                            //wenn das Feld darunter nicht frei ist
                            //UND das Feld rechts daneben vom selben typ ist
                            if (_content[y + 1, right] != ' ' && _content[y, right + 1] == type) 
                                isMoveable = false;

                            if (!isMoveable && _content[y, right] == type)
                                x = right; 
                        }


                        if (isMoveable)
                        {
                            //
                            for (int down = y; down < last; down++)
                            {
                                   
                                //ueber die Laenge des jetztigen Blockes
                                //jeden einzelnen Stein des BLockes ueberpruefen ob er nach unten bewegbar ist
                                for (int m = 0; m < length; m++)
                                {
                                    //wenn der Block darunter keine # ist UND (vom gleichen Typ ist ODER ein freies Feld ist)
                                    if (_content[down + 1, x + m] != '#' && (_content[down + 1, x + m] == type || _content[down + 1, x + m] == ' '))
                                        isMoveable = true;
                                    else
                                    {
                                        isMoveable = false;
                                        break;
                                    }
                                            
                                }
                                //wenn der gesamte Block noch bewegbar ist
                                if (isMoveable)
                                {
                                    //ihn ueber die gesamte laenge verschieben
                                    for (int i = 0; i < length; i++)
                                    {
                                        //tauscht den Block mit dem darunter
                                        Swap(ref _content[down, x + i], ref _content[down + 1, x + i]);
                                        //wenn der letzte Block(der im Rhamen) eine Zahl ist
                                        //-->  es ist eine Loesung gefunden
                                        if (down + 1 == last && _content[down + 1, x] == type)
                                        {
                                            foundSolution = true;
                                        }
                                    }
                                }
                                else
                                    break;
                            }
                        }
                    }
                }
            }
            /*
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("<-- APPLIED GRAVITY TO MARIX -->");
            Console.WriteLine("----------------------------------------");    
            this.PrintMatrix();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("");
            */
            return foundSolution;
        }


        public class MatrixComparer : IComparer<Matrix>
        {
            public int Compare(Matrix _a, Matrix _b)
            {
                
                for (int y = 0; y < _a.size - 1; y++)
                {
                    for (int x = 0; x < _a.size - 1; x++)
                    {
                        if (_a.content[y, x] < _b.content[y, x])
                            return -1;
                        else if (_a.content[y, x] > _b.content[y, x])
                            return 1;
                    }
                }

                return 0;
                
            }
        }
        
    }
}
