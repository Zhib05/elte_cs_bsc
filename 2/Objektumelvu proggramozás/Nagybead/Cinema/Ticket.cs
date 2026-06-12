namespace Cinema
{
    public class Ticket
    {
        private string _seatID;
        private string status;
        private Viewer _viewer;
        private ScreeningRoom _screeningRoom;
        private Film film;
        public double basalPrice = 3000;

        public Ticket(string seatID, string status, Viewer viewer, ScreeningRoom screeningRoom, Film film)
        {
            this._seatID = seatID;
            this.status = status;
            this._viewer = viewer;
            this._screeningRoom = screeningRoom;
            this.film = film;
        }

        public string SeatID => _seatID;
        public string Status => status;
        public Viewer Viewer => _viewer;
        public ScreeningRoom ScreeningRoom => _screeningRoom;
        public Film Film => film;

        public double Price()
        {
            if (_viewer == null)
            {
                throw new Exception("Viewer is not assigned to this ticket.");
            }

            return _screeningRoom.RoomType.ApplyDiscount(basalPrice, _viewer.ViewerStatus);
        }

        public void setViewer(Viewer viewer)
        {
            this._viewer = viewer;
        }
    }
}