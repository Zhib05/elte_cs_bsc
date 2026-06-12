using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF7
{
    public class Bank
    {
        private List<Account> accounts;
        public Bank()
        {
            accounts = new List<Account>();
        }

        public void OpenAccount(String cNum, Customer c)
        {
            Account a = new Account(cNum);
            accounts.Add(a);
            c.AddAccount(a);
        }

        public void ProvidesCard(String cNum)
        {
            if (FindAccount(cNum, out Account? a))
            {
                Card card = new Card(cNum, "1234");
                a.cards.Add(card);
            }
        }

        public int GetBalance(String cNum)
        {
            bool l = FindAccount(cNum, out Account? account);
            if (l)
            {
                return account.GetBalance();
            } else
            {
                return -1;
            }
        }

        public void Transaction(String cNum, int amount)
        {
            bool l = FindAccount(cNum, out Account? account);
            if (l)
            {
                account.Change(amount);
            }
        }

        public bool CheckAccount(String cNum)
        {
            foreach (Account account in accounts)
            {
                if (account.accNum == cNum)
                {
                    return true;
                }
            }
            return false;
        }

        private bool FindAccount(String cNum, out Account? a)
        {
            foreach (Account account in accounts)
            {
                if (account.accNum == cNum)
                {
                    a = account;
                    return true;
                }
            }
            a = null;
            return false;
        }
    }
}
