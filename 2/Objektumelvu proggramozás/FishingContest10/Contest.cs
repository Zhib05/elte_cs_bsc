namespace FishingContest10
{
    public class Contest
    {
        private string location;
        private DateTime begin;
        private Organization association;
        private List<Catch> catches;
        private List<Fisherman> fishermen;

        public string Location => location;
        public DateTime Begin => begin;
        public List<Fisherman> Fishermen => fishermen;
        public List<Catch> Records => catches;
        public Contest(Organization sz, string h, DateTime k)
        {
            this.association = sz;
            this.location = h;
            this.begin = k;
            this.catches = new List<Catch>();
            this.fishermen = new List<Fisherman>();
        }

        public void Registration(Fisherman ho)
        {
            if (!association.Members.Contains(ho) || this.fishermen.Contains(ho))
            {
                throw new Exception("Fisherman is not a member of the association or already registered.");
            }
            this.fishermen.Add(ho);
        }

        public void RecordCatch(Catch f)
        {
            if (!this.fishermen.Contains(f.Fisherman) || this.catches.Contains(f))
            {
                throw new Exception("Fisherman is not registered or catch already recorded.");
            }
            this.catches.Add(f);
        }
    }
}