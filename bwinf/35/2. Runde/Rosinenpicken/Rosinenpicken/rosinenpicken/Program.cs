using System;
using System.Diagnostics;



namespace rosinenpicken
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                RaisinPicking();

                Console.WriteLine("Press any key to restart!");
                Console.ReadKey();
                Console.WriteLine("Restarting...");
            }       
        }

        static void RaisinPicking()
        {
            RaisinTree tree;
            IO io = new IO();

            //check and read the file
            do
            {
                tree = io.AskForPath();
            } while (tree == null);



            Console.WriteLine("File was read...");


            Stopwatch sw = new Stopwatch();
            sw.Start();

            tree.FindAllLoops();

            Console.WriteLine("Found all loops and removed them...");

            tree.FindAllNegativeSingleNegativeRaisins();

            Console.WriteLine("Found all negative single raisins and removed them...");

            tree.GetRealValueOfAllRaisins();

            Console.WriteLine("Calculated all definitive-values...");

            tree.PickBestRaisinSetForEveryRaisin();

            Console.WriteLine("Found the optimal set for every Raisin..." + '\n');

            tree.FindAllRaisinsToTake();
            sw.Stop();

            Console.WriteLine("Found most valuble set! In " + sw.Elapsed.ToString() + '\n' + '\n');

            tree.PrintMostValubleSet();
        }
    }
}
