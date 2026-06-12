namespace Hunting {
    public class Hunter {
        private string _name;
        private string _birthyear;
        private List<Trophy> _trophies;

        public Hunter(string name, string birthyear) {
            _name = name;
            _birthyear = birthyear;
            _trophies = new List<Trophy>();
        }

        public void Hunt(string where, string when, Game what) {
            _trophies.Add(new Trophy(where, when, what));
        }

        public int CountMaleLions() {
            int c = 0;
            foreach (Trophy trophy in _trophies) {
                if (trophy.Game is Lion lion && lion.Gender == Gender.Male) {
                    c++;
                }
            }
            return c;
        }

        public bool MaxHornWeightRate(out double? rate, out Trophy? trophy) {
            rate = null;
            trophy = null;

            bool found = false;

            foreach (Trophy t in _trophies) {
                if (t.Game is Rhino rhino) {
                    double currRate = rhino.Horn / rhino.Weight;
                    if (!found) {
                        rate = currRate;
                        trophy = t;
                    } else {
                        if (currRate > rate) {
                            rate = currRate;
                            trophy = t;
                            found = true;
                        }
                    }
                }
            }
            return found;
        }

        public bool SearchEqualTusks() {
            foreach (Trophy trophy in _trophies) {
                if (trophy.Game is Elephant elephant) {
                    if (elephant.Left == elephant.Right) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
