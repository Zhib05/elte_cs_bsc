using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingContest10
{
    public class Fisherman
    {
        private string name;
        private List<Catch> catches;

        public Fisherman(string n)
        {
            this.name = n;
            this.catches = new List<Catch>();
        }

        public void CatchFish(DateTime i, FishSpecies f, double t, Contest v)
        {
            bool l = false;
            foreach (Catch c in this.catches)
            {
                if (c.DateTime == i)
                {
                    l = true;
                    break;
                }
            }

            foreach (Fisherman fisherMan in v.Fishermen)
            {
                if (fisherMan.name == this.name)
                {
                    l = true;
                    break;
                }
            }

            if (l)
            {
                Catch fog = new Catch(i, t, f, v, this);
                this.catches.Add(fog);
                v.Records.Add(fog);
            }
        }
    }
}
