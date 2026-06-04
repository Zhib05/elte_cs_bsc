using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szappan_adagolo
{
    internal class Adagolo
    {
        private int _tele;
        private int _adag;
        private int _akt;
        public int Tele => _tele;
        public int Akt { get { return _akt; } set { _akt = value; } }

        public Adagolo(int k, int e)
        {
            this._tele = k;
            this._adag = e;
            this._akt = 0;
        }

        public void Nyom()
        {

        }

        public void Feltolt()
        {
            _akt = _tele;
        }
    }
}
