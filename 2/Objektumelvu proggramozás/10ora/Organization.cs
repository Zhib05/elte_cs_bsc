namespace FishingContest {
    public class Organization {
        private List<Contest> _contests;
        private List<Fisherman> _members;

        public List<Fisherman> Members => new List<Fisherman>(_members);
        
        public Organization() {
            _contests = new List<Contest>();
            _members = new List<Fisherman>();
        }

        public void Join(Fisherman fisherman) {
            if (_members.Contains(fisherman)) {
                throw new ArgumentException("A horgász már tagja a szövetségnek!");
            }
            _members.Add(fisherman);
        }

        public Contest Organize(string location, DateTime date) {
            bool exists = false;
            foreach (Contest contest in _contests) {
                if (contest.Location == location && contest.Date == date) {
                    exists = true;
                }
            }

            if (exists) {
                throw new ArgumentException("Egy verseny már létezik ezekkel a paraméterekkel!");
            }

            Contest newcontest = new(this, location, date);
            _contests.Add(newcontest);
            return newcontest; // Felpopuláláshoz
        }

        public bool Best(out Contest? contest) {
            contest = null;
            double currMax = 0;

            foreach (Contest c in _contests) {
                if (c.AllCatfishes()) {
                    double currSum = c.Sum();
                    if (contest == null) {
                        currMax = currSum;
                        contest = c;
                    } else {
                        if (currSum > currMax) {
                            currMax = currSum;
                            contest = c;
                        }
                    }
                }
            }
        
            if (contest == null) {
                return false;
            }
            return true;
        }

        public Fisherman? Search(string name) {
            foreach (Fisherman fisherman in _members) {
                if (fisherman.Name == name) {
                    return fisherman;
                }
            }
            return null;
        }
    }
}
