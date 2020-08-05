using System;
using System.Collections.Generic;


namespace rosinenpicken
{
    class Raisin
    {
        public enum Sets { Start, Stack, End };


        //VARIABLES CREATED BY READING THE FILE
        #region
        public string raisinName;
        //the value read from file
        public float value;
        //the number this raisin was read from the file
        public string ID;
        //the dependencies from this raisin
        public List<Raisin> forwardConnections;
        public List<Raisin> backwardsConnections;
        #endregion

        //VARIABLES FOR FINDING CYCLES
        #region
        //if I select this raisin all the raisins I have to take too(SUCCESSORS)
        public List<Raisin> successors;
        //if the raisin is part of a loop or not
        public bool inLoop;
        //the set where this raisin is currently
        public Sets currentSet;
        //from which raisin this raisin was introduced
        public Raisin introducedBy;
        //if this raisins consists of multiple raisins that are in a cycle
        public bool isCycle = false;
        #endregion

        //a List with all Raisin-Names i have to take if i take this raisin
        public HashSet<Raisin> raisinsIncluded = new HashSet<Raisin>();
        //the value read from file + the value of all successors from this raisin
        public float definitiveValue;

        //All Raisins that are the optimal-set
        //can only include raisins below this raisin
        public HashSet<Raisin> optimalSet = new HashSet<Raisin>();
        //the value of all Raisins in the optimal-set
        public float optimalValue = 0f;


        public Raisin(string _raisinName, int _ID, float _value)
        {
            this.raisinName = _raisinName;
            this.ID = _ID.ToString();
            this.inLoop = false;

            this.successors = new List<Raisin>();

            this.value = _value;

            this.currentSet = Sets.Start;

            this.forwardConnections = new List<Raisin>();
            this.backwardsConnections = new List<Raisin>();
        }


        public Raisin NextForwardConnection()
        {
            Raisin next = null;

            for (int i = 0; i < this.forwardConnections.Count; i++)
            {
                if (this.forwardConnections[i].currentSet == Sets.Start ||
                    this.forwardConnections[i].currentSet == Sets.Stack)
                {
                    next = this.forwardConnections[i];
                    break;
                }
                    
            }

            return next;
        }


        public void GetDefintivevalueOfRaisin()
        {
            Queue<Raisin> raisinsToAdd = new Queue<Raisin>();
            raisinsToAdd.Enqueue(this);

            HashSet<Raisin> raisinToTake = new HashSet<Raisin>();

            //loop over all predecessors of this Raisin
            while(raisinsToAdd.Count != 0)
            {
                //take first raisin from the queue
                Raisin tmp = raisinsToAdd.Dequeue();

                if(tmp.raisinsIncluded.Count != 0)
                {
                    foreach (Raisin item in tmp.raisinsIncluded)
                    {
                        if (!raisinToTake.Contains(item))
                        {
                            raisinToTake.Add(item);
                        }
                    }
                    
                    continue;
                }

                //add all forward connecrions of this raisin to the queue
                for (int i = 0; i < tmp.forwardConnections.Count; i++)
                {
                    if (raisinToTake.Contains(tmp.forwardConnections[i]))
                        continue;

                    raisinsToAdd.Enqueue(tmp.forwardConnections[i]);
                }

                raisinToTake.Add(tmp);
            }

            foreach (Raisin item in raisinToTake)
            {
                this.raisinsIncluded.Add(item);
                definitiveValue += item.value;
            }
        }


    }
}
