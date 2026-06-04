using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Film
    {
        private string _filmeName;
        public List<Ticket> tickets = new List<Ticket>();

        public Film(string filmName)
        {
            this._filmeName = filmName;
            tickets = new List<Ticket>();
        }

        public string FilmName => _filmeName;
        public void AddTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket), "Ticket cannot be null.");
            }
            tickets.Add(ticket);
        }

        public void RemoveTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket), "Ticket cannot be null.");
            }
            if (!tickets.Remove(ticket))
            {
                throw new InvalidOperationException("Ticket not found in film's list.");
            }
        }
    }
}
