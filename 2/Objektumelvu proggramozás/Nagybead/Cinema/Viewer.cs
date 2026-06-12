namespace Cinema
{
    public class Viewer
    {
        private string _id;
        private Status status;
        private List<Ticket> _tickets;

        public Viewer(string id, Status status)
        {
            this._id = id;
            this.status = status;
            this._tickets = new List<Ticket>();
        }

        public string ID => _id;
        public Status ViewerStatus => status;
        public List<Ticket> Tickets => _tickets;

        public void BecomeRegular()
        {
            this.status = Status.Regular;
        }

        public void AddTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket), "Ticket cannot be null.");
            }
            this._tickets.Add(ticket);
        }

        public void RemoveTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket), "Ticket cannot be null.");
            }
            if (!this._tickets.Remove(ticket))
            {
                throw new InvalidOperationException("Ticket not found in viewer's list.");
            }
        }
    }
}