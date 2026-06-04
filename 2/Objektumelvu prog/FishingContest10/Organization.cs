namespace FishingContest10
{
    public class Organization
    {
        private List<Fisherman> members;
        private List<Contest> contests;

        public List<Fisherman> Members => members;

        public Organization()
        {
            this.members = new List<Fisherman>();
            this.contests = new List<Contest>();
        }

        public void Join(Fisherman ho)
        {
            if (this.members.Contains(ho))
            {
                throw new Exception("Fisherman is already a member of the association.");
            }
            this.members.Add(ho);
        }

        public void Organise(string he, DateTime ke)
        {
            bool l = false;
            foreach (Contest e in contests)
            {
                if (e.Location == he && e.Begin == ke)
                {
                    l = true;
                    break;
                }
            }

            if (l)
            {
                throw new Exception("Contest already exists.");
            }
            contests.Add(new Contest(this, he, ke));
        }

        public bool Best (out Contest? contest)
        {
            contest = null;
            double max = 0;
            foreach (Contest e in contests)
            {
                if 
            }
        }
    }
}