using System;
using System.Collections.Generic;
using System.IO;


namespace rosinenpicken
{
    class RaisinTree
    {
        public int numberOfRaisins = 0;

        //all raisins loaded from the file
        public SortedList<string, Raisin> raisins = new SortedList<string, Raisin>();

        //the sets for finding circles
        private List<Raisin> start;
        private List<Raisin> stack;
        private List<Raisin> end;


        HashSet<Raisin> optimalSet = new HashSet<Raisin>();
        float optimalValue = 0f;

       

        public RaisinTree()
        {
            this.start = new List<Raisin>();
            this.stack = new List<Raisin>();
            this.end = new List<Raisin>();
        }

        
        public void FindAllLoops()
        {
            //add all raisins to the start set
            for (int i = 0; i < raisins.Count; i++)
            {
                this.Move_To(raisins.Values[i], this.start, Raisin.Sets.Start);
            }

            //loop over all raisins in this file
            while(this.start.Count != 0)
            {
                Raisin item = this.start[0];

                //if the raisin is in a loop
                if (item.inLoop)
                    continue;               

                //if this raisin was checked and is in the END-set
                if (item.currentSet == Raisin.Sets.End)
                    continue;
                
                //Raisin is an END-raisin
                if(item.forwardConnections.Count == 0)
                {
                    this.MoveFrom_To(item, this.start, this.end, Raisin.Sets.End);
                    continue;
                }
                //move raisin from start to stack
                this.MoveFrom_To(item, this.start, this.stack, Raisin.Sets.Stack);

                //loop unitl stack is empty
                while(stack.Count != 0)
                {
                    //get the next successor of the raisin 
                    Raisin tmp = this.stack[this.stack.Count - 1].NextForwardConnection();
                    
                    //if successor is null --> move to endset
                    if (tmp == null)
                    {
                        this.MoveFrom_To(stack[this.stack.Count - 1], this.stack, this.end, Raisin.Sets.End);
                        continue;
                    }
                    //set the predeccessor ot the succesor
                    tmp.introducedBy = this.stack[this.stack.Count - 1];

                    //check if the successor of the raisin is in the stack
                    //--> LOOP
                    if (tmp.currentSet == Raisin.Sets.Stack)
                    {
                        //Found cycle
                        Raisin substituedCycle = this.SubstituteCycle(tmp);
                        substituedCycle.currentSet = Raisin.Sets.Start;
                        this.start.Add(substituedCycle);

                        //Console.WriteLine();
                        //Console.WriteLine(substituedCycle.raisinName + " || value = " + substituedCycle.value.ToString());

                        continue;
                    }
                    if(tmp.currentSet == Raisin.Sets.Start)
                    {
                        this.MoveFrom_To(tmp, this.start, this.stack, Raisin.Sets.Stack);
                        continue;
                    }

                    this.Move_To(tmp, this.stack, Raisin.Sets.Stack);
                }
            }
        }


        //substituets a cycle
        //set all the connections to a new node
        //--> deletes the cycle
        public Raisin SubstituteCycle(Raisin _tmp)
        {
            List<Raisin> forwardConnections = new List<Raisin>();// = _tmp.forwardConnections;
            List<Raisin> backwardsConnections = new List<Raisin>();// = _tmp.backwardsConnections;

            string name = "";//_tmp.raisinName;
            float value = 0;// _tmp.value;

            Raisin newRaisin = new Raisin("NEW RAISIN CYCLE", 0, 0f);


            Raisin lastInCycle = _tmp.introducedBy;
            _tmp.inLoop = true;
            while (lastInCycle != _tmp)
            {
                lastInCycle.inLoop = true;
                lastInCycle = lastInCycle.introducedBy;
            }


            bool loop = false;
            lastInCycle = _tmp.introducedBy;
            do 
            {
                if (lastInCycle == _tmp)
                    loop = true;

                //set all the forwardConnections to the new Node
                for (int i = 0; i < lastInCycle.forwardConnections.Count; i++)
                {
                    //if the Raisin is not in the loop 
                    if (!forwardConnections.Contains(lastInCycle.forwardConnections[i]))                      
                    {
                        //if the raisin is not in the connections of the new created raisin
                        if (!lastInCycle.forwardConnections[i].inLoop)
                        {
                            forwardConnections.Add(lastInCycle.forwardConnections[i]);

                            lastInCycle.forwardConnections[i].backwardsConnections.Add(newRaisin);
                        }                        

                        lastInCycle.forwardConnections[i].backwardsConnections.Remove(lastInCycle);

                        
                    }
                        
                }
                //set all the backwardsConnections to the new Node
                for (int j = 0; j < lastInCycle.backwardsConnections.Count; j++)
                {
                    //if the Raisin is not in the loop 
                    if (!lastInCycle.backwardsConnections[j].inLoop)
                    {
                        //if the raisin is not in the connections of the new created raisin
                        if (!backwardsConnections.Contains(lastInCycle.backwardsConnections[j]))
                        {
                            backwardsConnections.Add(lastInCycle.backwardsConnections[j]);

                            lastInCycle.backwardsConnections[j].forwardConnections.Add(newRaisin);
                        }
                            
                        lastInCycle.backwardsConnections[j].forwardConnections.Remove(lastInCycle);

                        
                    }                     
                }


                //get a new name made of all the cycle Raisins names
                name += " " + lastInCycle.raisinName;
                //get the new value by adding all the values from the cycle Raisins
                value += lastInCycle.value;
                


                //set the new Raisin which needs to be converted
                Raisin remove = lastInCycle;
                lastInCycle = lastInCycle.introducedBy;

                //remove the converted Raisin
                this.stack.Remove(remove);

                

            } while (!loop);
            //delete the old loop starting raisin
            this.stack.Remove(_tmp);

            newRaisin.raisinName = "";
            newRaisin.raisinName = name;
            newRaisin.ID = this.numberOfRaisins++.ToString();
            newRaisin.value = value;
            //set that this was a cycle
            newRaisin.isCycle = true;

            newRaisin.forwardConnections = forwardConnections;
            newRaisin.backwardsConnections = backwardsConnections;

            this.raisins.Add(newRaisin.raisinName, newRaisin);

            return newRaisin;
        } 


        //Removes all negative raisins that have no predeccessor
        public void FindAllNegativeSingleNegativeRaisins()
        {
            for (int i = this.end.Count - 1; i >= 0; i--)
            {
                //if this raisin does not have any predeccessors
                if (this.end[i].backwardsConnections.Count == 0)
                {
                    //and it does not have positives Value
                    if (this.end[i].value < 0)
                    {
                        //remove all references to this raisin
                        for (int j = 0; j < this.end[i].forwardConnections.Count; j++)
                        {
                            this.end[i].forwardConnections[j].backwardsConnections.Remove(this.end[i]);
                        }
                        //delete all raferences from the raisin

                        //delete the raisin from the end list
                        end.Remove(this.end[i]);
                        i = this.end.Count;
                    }
                }
            }
        }


        //reads over all raisins and adds their value together
        //to get theit Definitivevalue
        public void GetRealValueOfAllRaisins()
        {
            for (int i = 0; i < this.end.Count; i++)
            {
                this.end[i].GetDefintivevalueOfRaisin();
            }
        }


        //find the best sets for all raisins with no successor
        public void PickBestRaisinSetForEveryRaisin()
        {
            for (int i = 0; i < this.end.Count; i++)
            {
                //the raisin for that we want to find the optimal set
                Raisin tmp = this.end[i];

                if (tmp.optimalSet.Count > 0)
                    Console.WriteLine("A BUG OCCURED! RAISIN ALREADY HAS A OPTIMAL VALUE");
                
                if(tmp.forwardConnections.Count == 0)
                {
                    if(tmp.definitiveValue > 0)
                    {
                        tmp.optimalValue = tmp.definitiveValue;
                        tmp.optimalSet = tmp.raisinsIncluded;
                    }
                }
                else if(tmp.forwardConnections.Count == 1)
                {
                    //optimal-value(previous node) > definitive-value(this node)
                    if (tmp.forwardConnections[0].optimalValue >= tmp.definitiveValue)
                    {
                        tmp.optimalValue = tmp.forwardConnections[0].optimalValue;
                        tmp.optimalSet = tmp.forwardConnections[0].optimalSet;
                    }
                    //optimal-value(previous node) < definitive-value(this node)
                    else if (tmp.forwardConnections[0].optimalValue < tmp.definitiveValue)
                    {
                        tmp.optimalValue = tmp.definitiveValue;
                        tmp.optimalSet = tmp.raisinsIncluded;
                    }
                }
                else if(tmp.forwardConnections.Count > 1)
                {
                    HashSet<Raisin> tmpSet = new HashSet<Raisin>();
                    for (int frwC = 0; frwC < tmp.forwardConnections.Count; frwC++)
                    {
                        //find all Raisins that are included in the optimal sets
                        //from the predecessors
                        foreach (Raisin item in tmp.forwardConnections[frwC].optimalSet)
                        {
                            //if it wasn't in one of the other sets...
                            if (!tmpSet.Contains(item))
                            {
                                //Add it to the set
                                tmpSet.Add(item);
                            }
                        }
                    }                      
                    //calculate the REAL optimal value till this Raisin
                    float realOptimalValue = 0f;
                    foreach (Raisin item in tmpSet)
                    {
                        realOptimalValue += item.value;
                    }
                    //compare the REAL optimal-value with the Absolute-value
                    if(realOptimalValue >= tmp.definitiveValue)
                    {
                        //take all of the optimal values
                        tmp.optimalSet = tmpSet;
                        tmp.optimalValue = realOptimalValue;
                    }
                    else if(realOptimalValue < tmp.definitiveValue)
                    {
                        //take all of the raisins that are connected to this one
                        tmp.optimalSet = tmp.raisinsIncluded;
                        tmp.optimalValue = tmp.definitiveValue;
                    }                   
                }                     
            }
        }


        //searches through all optimal sets and removes the doubles
        public void FindAllRaisinsToTake()
        {
            for (int i = 0; i < this.end.Count; i++)
            {
                if(this.end[i].backwardsConnections.Count == 0)
                {
                    foreach (Raisin item in this.end[i].optimalSet)
                    {
                        if (!this.optimalSet.Contains(item))
                        {
                            this.optimalSet.Add(item);
                            this.optimalValue += item.value;
                        }
                    }
                }
            }
        }


        //Outputs the most valuble set
        public void PrintMostValubleSet()
        {
            List<String> listOfNames = new List<string>();
            foreach (Raisin item in this.optimalSet)
            {                     
                if (item.isCycle)
                {
                    string tmp = "";
                    for (int i = 0; i < item.raisinName.Length; i++)
                    {
                        if(item.raisinName[i] == ' ')
                        {
                            if (tmp == "")
                                continue;

                            listOfNames.Add(tmp);
                            tmp = "";
                            continue;
                        }

                        tmp += item.raisinName[i];
                    }
                    if (tmp != "")
                        listOfNames.Add(tmp);
                }
                else
                    listOfNames.Add(item.raisinName);
            }

            Console.WriteLine("Anzahl der Knoten in der Teilmenge:");
            Console.WriteLine(listOfNames.Count);

            Console.WriteLine("Gesamtwert der Knoten:");
            Console.WriteLine(this.optimalValue);

            Console.WriteLine("Nummern der Knoten:");
            for (int i = 0; i < listOfNames.Count; i++)
            {
                Console.WriteLine(listOfNames[i]);
            }
        }


        void Move_To(Raisin _itemToMove, List<Raisin> _to, Raisin.Sets _newSet)
        {
            _to.Add(_itemToMove);
            _itemToMove.currentSet = _newSet;
        }


        void MoveFrom_To(Raisin _itemToMove, List<Raisin> _from, List<Raisin> _to, Raisin.Sets _newSet)
        {
            _from.Remove(_itemToMove);
            _to.Add(_itemToMove);
            _itemToMove.currentSet = _newSet;
        }


    }
}
