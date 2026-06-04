namespace FishingContest {
    public class Catch {
        private DateTime _time;
        private double _weight;
        private Contest _contest;
        private IFish _fish;
        private Fisherman _fisherman;

        public DateTime Time => _time;
        public Contest Contest => _contest;
        public IFish Fish => _fish;
        public Fisherman Fisherman => _fisherman;

        public Catch(DateTime time, double weight, Contest contest, IFish fish, Fisherman fisherman) {
            _time = time;
            _weight = weight;
            _contest = contest;
            _fish = fish;
            _fisherman = fisherman;
        }

        public double Value() {
            return _weight * _fish.Multiplier();
        }
    }
}