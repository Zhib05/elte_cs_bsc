namespace HF7
{
    public class Card
    {
        public String cNum;
        private String _pin;

        public Card(string cNum, string pin)
        {
            this.cNum = cNum;
            _pin = pin;
        }

        public void SetPIN(String p)
        {
            _pin = p;
        }

        public bool CheckPIN(String p)
        {
            return _pin.Equals(p);
        }
    }
}