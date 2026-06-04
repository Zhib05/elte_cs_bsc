namespace HF10
{
    public class TargetShot
    {
        private List<Gift> _gifts;
        private string _site;

        public TargetShot(string s)
        {
            _site = s;
            _gifts = new List<Gift>();
        }

        public void Shows(Gift g)
        {
            if (g.targetShot != null)
            {
                throw new Exception("Gift already has a target shot.");
            }
            if (_gifts.Contains(g))
            {
                throw new Exception("Gift already added to target shot.");
            }
            g.targetShot = this;
            _gifts.Add(g);
        }

        public List<Gift> GetGifts() => _gifts;
    }
}