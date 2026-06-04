using System.ComponentModel.DataAnnotations;

namespace HF7
{
    public class Account
    {
        public String accNum;
        private int _balance;
        public List<Card> cards;

        public Account(String cNum)
        {
            accNum = cNum;
            _balance = 0;
            cards = new List<Card>();
        }

        public int GetBalance()
        {
            return _balance;
        }

        public void Change(int a)
        {
            _balance += a;
        }
    }
}