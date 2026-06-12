namespace FishingContest {
    public class Fisherman {
        private string _name;
        private List<Catch> _catches;

        public string Name => _name; // Felpopuláláshoz

        public Fisherman(string name) {
            _name = name;
            _catches = new List<Catch>();
        }

        public void Catch(DateTime time, IFish fish, double weight, Contest contest) {
            bool exists = false;
            foreach (Catch c in _catches) {
                if (c.Time == time) {
                    exists = true;
                }
            }
            if (exists) {
                throw new ArgumentException("Egy fogás ezzel az időponttal már létezik a horgásznál!");
            }
            if (!contest.Fishermen.Contains(this)) {
                throw new ArgumentException("A horgász nem indul a megadott versenyen!");
            }

            Catch @catch = new(time, weight, contest, fish, this);
            _catches.Add(@catch);
            contest.Record(@catch);
        }

        public bool CaughtCatfish(Contest contest) {
            foreach (Catch c in _catches) {
                if (c.Fish is Catfish && c.Contest == contest) {
                    return true;
                }
            }
            return false;
        }
    }
}