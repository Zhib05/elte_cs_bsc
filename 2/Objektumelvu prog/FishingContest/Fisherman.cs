using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingContest {
    public record struct Catch(string time, string species, double width, double weight);
    public class Fisherman {
        private readonly string _name;
        private readonly List<Catch> _catches;

        public string Name => _name;

        public Fisherman(string name) {
            _catches = new List<Catch>();
            _name = name;
        }

        public void AddCatch(Catch fish) {
            _catches.Add(fish);
        }

        public double SumCarpWeight() {
            double sum = 0.0;
            foreach (Catch fish in _catches) {
                if (fish.species == "ponty" && fish.width >= 0.5) {
                    sum += fish.weight;
                }
            }
            return sum;
        }
    }
}
