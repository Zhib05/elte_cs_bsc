using System.ComponentModel.DataAnnotations;

namespace HF7
{
    public class ATM
    {
        public String site;
        public Center _center;

        public ATM(String site, Center center)
        {
            this.site = site;
            _center = center;
        }

        public void Process(Customer c)
        {
            Card card = c.ProvidesCard();
            if (card.CheckPIN(c.ProvidesPIN()))
            {
                int a = c.RequestMoney();
                if (_center.GetBalance(card.cNum) >= a)
                {
                    _center.Transaction(card.cNum, -a);
                }
            }
        }
    }
}