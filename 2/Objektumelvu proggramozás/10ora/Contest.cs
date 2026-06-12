namespace FishingContest {
    public class Contest {
        private string _location;
        private DateTime _date;
        private Organization _organization;
        private List<Fisherman> _fishermen;
        private List<Catch> _catches;
        
        public string Location => _location;
        public DateTime Date => _date;
        public List<Fisherman> Fishermen => new List<Fisherman>(_fishermen);

        public Contest(Organization organization, string location, DateTime date) {
            _organization = organization;
            _location = location;
            _date = date;
            _fishermen = new List<Fisherman>();
            _catches = new List<Catch>();
        }

        public void SignUp(Fisherman fisherman) {
            if (!_organization.Members.Contains(fisherman)) {
                throw new ArgumentException("A horgász nem tagja a szövetségnek!");
            }
            if (_fishermen.Contains(fisherman)) {
                throw new ArgumentException("A horgász már regisztrált a versenyre!");
            }
            _fishermen.Add(fisherman);
        }

        public void Record(Catch c) {
            if (!_fishermen.Contains(c.Fisherman)) {
                throw new ArgumentException("A horgász nem regisztrált a versenyre!");
            }
            if (_catches.Contains(c)) {
                throw new ArgumentException("Ez a fogás már fel lett jegyezve!");
            }
            _catches.Add(c);
        }

        public bool AllCatfishes() {
            foreach (Fisherman fisherman in _fishermen) {
                if (!fisherman.CaughtCatfish(this)) {
                    return false;
                }
            }
            return true;
        }

        public double Sum() {
            double acc = 0;
            foreach (Catch c in _catches) {
                acc += c.Value();
            }
            return acc;
        }
    }
}