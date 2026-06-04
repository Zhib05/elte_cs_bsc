using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF7
{
    public class Customer
    {
        private String _pin;
        private int _withdraw;
        public List<Account> accounts;

        public Customer(string pin, int withdraw)
        {
            _pin = pin;
            _withdraw = withdraw;
            accounts = new List<Account>();
        }

        public void Withdrawal(ATM atm)
        {
            atm.Process(this);
        }

        public Card ProvidesCard()
        {
            return accounts.First().cards.First();
        }

        public String ProvidesPIN()
        {
            return _pin;
        }

        public int RequestMoney()
        {
            return _withdraw;
        }

        public void AddAccount(Account a)
        {
            accounts.Add(a);
        }
    }
}
