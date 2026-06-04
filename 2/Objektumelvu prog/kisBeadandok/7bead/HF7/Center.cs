namespace HF7
{
    public class Center
    {
        private List<Bank> _banks;

        public Center(List<Bank> banks)
        {
            _banks = banks;
        }

        public int GetBalance(String cNum)
        {
            bool l = FindBank(cNum, out Bank? bank);
            if (l)
            {
                return bank.GetBalance(cNum);
            } else
            {
                return -1;
            }
        }

        public void Transaction(String cNum, int amount)
        {
            bool l = FindBank(cNum, out Bank? bank);
            if (l)
            {
                bank.Transaction(cNum, amount);
            }
        }

        private bool FindBank(String cNum, out Bank? bank)
        {
            foreach (Bank b in _banks)
            {
                if(b.CheckAccount(cNum))
                {
                    bank = b;
                    return true;
                }
            }
            bank = null;
            return false;
        }
    }
}