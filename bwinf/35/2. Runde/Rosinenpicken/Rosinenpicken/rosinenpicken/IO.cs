using System.IO;
using System;


namespace rosinenpicken
{
    class IO
    {

        private string path;

        public RaisinTree AskForPath()
        {
            RaisinTree tree = new RaisinTree();

            //get a path to a file from the user
            Console.WriteLine("Input Path to a File...");
            this.path = Console.ReadLine();

            if (File.Exists(this.path))
            {
                tree = this.ReadFile();
                if (tree == null)
                    return null;
            }

            else
            {
                return null;
            }
                          
            return tree;
        }

        public RaisinTree ReadFile()
        {
            RaisinTree tree = new RaisinTree();
            try
            {
                StreamReader file = new StreamReader(this.path);
                int counter = 0;
                string line = "";
                string[] splittedLine;
                while ((line = file.ReadLine()) != null)
                {
                    //skip the comments
                    if (line[0] == '#')
                        continue;


                    //Read the first line of the file
                    //--Get the number of raisins in this file
                    if (counter == 0)
                        tree.numberOfRaisins = int.Parse(line);

                    
                    //read all raisins from the file
                    else if (counter <= tree.numberOfRaisins)
                    {
                        splittedLine = line.Split(' ');
                        string name = splittedLine[0];
                        float value = float.Parse(splittedLine[1], System.Globalization.CultureInfo.InvariantCulture);

                        Raisin tmpRaisin = new Raisin(name, (counter - 1),value);
                        tree.raisins.Add(name, tmpRaisin);

                    }
                    //read all connections between raisins
                    else
                    {
                        splittedLine = line.Split(' ');
                        //if it is a self loop
                        if (splittedLine[0] == splittedLine[1])
                            continue;
                        string from = splittedLine[0];
                        string to = splittedLine[1];

                        //add the forward dependencie
                        tree.raisins[from].forwardConnections.Add(tree.raisins[to]);
                        //add the bachward dependencie
                        tree.raisins[to].backwardsConnections.Add(tree.raisins[from]);
                    }

                    //Console.WriteLine(line);
                    counter++;           
                }

                //close the file
                file.Close();
                
            }
            catch
            {
                Console.WriteLine("Error while Reading File");
                Console.WriteLine("Please Enter a valid file!");
                this.AskForPath();
            }

            return tree;
        }

    }
}
