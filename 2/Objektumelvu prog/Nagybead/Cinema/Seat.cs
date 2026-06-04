using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Seat
    {
        private string _ID;
        private string _status;

        public Seat(string Id)
        {
            this._ID = Id;
            this._status = "Available";
        }
        public Seat(string Id, string status)
        {
            this._ID = Id;
            this._status = status;
        }
        public string ID => _ID;
        public string Status => _status;

        public void SetStatus(string status)
        {
            this._status = status;
        }
    }
}
