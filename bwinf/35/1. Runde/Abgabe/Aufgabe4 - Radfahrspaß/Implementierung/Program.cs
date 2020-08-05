using System;
using System.Collections.Generic;
using System.IO;

namespace Radfahrspass
{
    class Program
    {
        public static bool DebugMode;

        public static string possible = "It IS possible to run this parcours!";
        public static string notPossible = "It IS NOT possible to run this parcours!";

        //immer wenn eine grade benoetigt wird um einen Berg noch hochfahren zu koennen
        public static int plusses = 0;
        //die geschwindigkeit die er ganz am ende hat wenn 
        //man alle sicheren plusse schon mit zaehlt
        public static int geschw = 0;

        //C:\Users\Dario\Documents\BWINF\2016\Aufgabe_4\TEST.txt
        static void Main(string[] args)
        {
            Console.WriteLine("Run Program in DebugMode?");
            Console.WriteLine("yes/no");
            string debug = Console.ReadLine();

            if (debug == "yes" || debug == "y" || debug == "ja" || debug == "j")
                DebugMode = true;
            else
                DebugMode = false;

            MainVoid();
        }


        static void MainVoid()
        {
            plusses = 0;
            geschw = 0;
            //Den Pfad zu der Datei mit  dem Parcours
            string path = GetPath();
            //wenn kein oder ein ungueltiger Pfad eingegeben wurde
            if (path == "")
            {
                MainVoid();
                return;
            }

            Tuple<int, int, int> tupelCount = ReadFileFirst(path);

            Console.WriteLine("");
            if (DebugMode && tupelCount != null)
            {
                Console.WriteLine("plusMinus = " + tupelCount.Item1);
                Console.WriteLine("neutral = " + tupelCount.Item2);
                Console.WriteLine("minus = " + tupelCount.Item3);
                Console.WriteLine("");
            }
            if (tupelCount != null)
            {
                if (IsParcoursPossible(tupelCount))
                {
                    ReadFileSecond(path);
                }
            }
                      

            //nach der uberpruefung des Parcouurs Program neu starten
            Console.WriteLine("Finished Program!");
            Console.WriteLine("restarting...");
            Console.WriteLine("");
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("");
            MainVoid();
        }

        static string GetPath()
        {
            Console.WriteLine("Input Path to a parcours file...");

            string path = Console.ReadLine();

            if (File.Exists(path))
                return path;
            else
            {
                Console.WriteLine("Can not find file!");
                return "";
            }            
        }

        //Teilaufgabe 1
        static Tuple<int, int, int> ReadFileFirst(string path)
        {
            //neuer streamReader fuer die .txt. Datei mit dem parcours
            StreamReader reader = new StreamReader(path);

            //jeden char aus der .txt Datei einzelnd lesen
            char ch;
            //die addition der Werte von '/' und '\'
            int plusMinus = 0; 
            int neutral = 0;
            //wertet die geraden und schraegen nach oben mit den schreagen nach unten auf
            int minus = 0;
            do
            {
                ch = (char)reader.Read();
                if(DebugMode)
                    Console.WriteLine(ch);


                if (ch == '/')
                {
                    plusMinus--;


                    if (minus > 0)
                        minus--;

                }

                else if (ch == '\\')
                {
                    plusMinus++;

                    minus++;

                }

                else if (ch == '_')
                {
                    neutral++;

                    if (minus > 0)
                        minus--;
                }

                //wenn eine negative Geschw. auftreten wuerde
                if (plusMinus < 0)
                {
                    //und es noch geraden gibt 
                    if (neutral > 0)
                    {
                        plusMinus++;
                        plusses++;
                        neutral--;
                    }
                    else
                    {
                        Console.WriteLine(notPossible);
                        reader.Close();
                        return null;
                    }
                }

                
                //das letzte zeichen
                if (reader.EndOfStream)
                {
                    //wenn das letzte zeichen eine senke ist 
                    //--> da mindestens Geschw. = +1
                    if(ch == '\\')
                    {
                        Console.WriteLine(notPossible);
                        reader.Close();
                        return null;
                    }
                }
                    

            } while (!reader.EndOfStream);

            reader.Close();

            return Tuple.Create(plusMinus, neutral, minus);
        }

        static bool IsParcoursPossible(Tuple<int, int, int> _val, bool _isPossible = true)
        {
            
            if(_val.Item1 <= _val.Item2)
            {
                //die Differenz der beiden zaehler
                int tmp = _val.Item1 - _val.Item2;
                //wenn die Differenz gerade ist
                //oder null
                if((tmp % 2 == 0 || tmp == 0) && _val.Item3 == 0)
                {
                    Console.WriteLine(possible);
                    return true;
                }                
                else
                    Console.WriteLine(notPossible);
            }
            else
                Console.WriteLine(notPossible);

            return false;
            Console.WriteLine("");
        }

        //------------------------------------------------------------------------------------

        static void ReadFileSecond(String path)
        {
            //wieviele geraden man am anfang auf plus setzten muss
            int tmp = plusses;
            //ab der stelle von der man weiß das keine beschleunigungen mehr benoetigt werden
            //die geraden mit zaehlen
            int geraden = 0;

            //neuer streamReader fuer die .txt. Datei mit dem parcours
            StreamReader reader = new StreamReader(path);
            do
            {
                char ch = (char)reader.Read();

                if (ch == '\\')
                    geschw++;

                else if (ch == '/')
                    geschw--;

                if (ch == '_' && plusses > 0)
                {
                    plusses--;
                    geschw++;
                }
                else if (ch == '_' && plusses == 0)
                {
                    geraden++;
                }

            } while (!reader.EndOfStream);


            int plussesMinusses = geraden - geschw;


            Console.WriteLine(geraden);
            Console.WriteLine(tmp + "+'s   " + plussesMinusses + "+/-'s   " + geschw + "-'s");
        }
    }

    class neutral
    {
        public int T1 { get; set; }
        public char T2 { get; set; }

        public neutral(int _T1, char _T2)
        {
            T1 = _T1;
            T2 = _T2;
        }
    }
}
