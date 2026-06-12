using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook
{
    internal class Notebook
    {
        public class IllegalPagesNumberException : Exception { }
        public class PageNotFoundException : Exception { }
        public enum Type { Plain, Grid, Lined };

        private Type _type;
        private List<String> _pages;
        private int _emptydb;
        public Notebook(int n, Type tip)
        {
            _pages = new List<string>();
            for (int i = 0; i < n; i++)
            {
                _pages.Add("");
            }
            _type = tip;
            _emptydb = n;
        }

        public int LapDB() => _pages.Count;

        public int UresDB() => _emptydb;

        public void Write(int ind, string tart)
        {
            ind--;
            if (!(ind < 0 || ind >= _pages.Count))
            {
                throw new IllegalPagesNumberException();
            }
            if (_pages[ind] != "")
            {
                throw new PageNotFoundException();
            }
            _pages[ind] = tart;
            _emptydb--;
        }

        public void Ripout(int ind)
        {
            ind--;
            if (!(ind < 0 || ind >= _pages.Count))
            {
                throw new IllegalPagesNumberException();
            }
            if (_pages[ind] == "")
            {
                _emptydb--;
            }
            _pages.RemoveAt(ind);
        }

        public bool Search(out int ind)
        {
            ind = 0;
            for (int i = 0; i < _pages.Count; i++)
            {
                string page = _pages[i];
                if (page == "")
                {
                    ind = i;
                    return true;
                }
            }
            return false;
        }
    }
}
