using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingContest10
{
    public class Catch
    {
        private DateTime time;
        private double weight;
        private FishSpecies fish;
        private Contest contest;
        private Fisherman fisherman;

        public DateTime DateTime => time;

        public Catch(DateTime t, double w, FishSpecies f, Contest c, Fisherman fm)
        {
            this.time = t;
            this.weight = w;
            this.fish = f;
            this.contest = c;
            this.fisherman = fm;
        }

        public DateTime Time
        {
            get { return this.time; }
        }

        public Fisherman Fisherman
        {
            get { return this.fisherman; }
        }

        public double Value()
        {
            return this.weight * this.fish.Multiplier();
        }
    }
}
