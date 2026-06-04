using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class ScreeningRoom
    {
        private string _roomName;
        private RoomType _roomType;
        private List<Seat> _seats;
        private List<Film> _films;

        public ScreeningRoom(String roomName, RoomType roomType)
        {
            this._roomName = roomName;
            this._roomType = roomType;
            this._seats = new List<Seat>();
            this._films = new List<Film>();
        }

        public string RoomName => _roomName;
        public RoomType RoomType => _roomType;
        public List<Seat> Seats => _seats;
        public List<Film> Films => _films;

        public String SeatAvailability(String seatID)
        {
            foreach (Seat seat in _seats)
            {
                if (seat.ID == seatID)
                {
                    return seat.Status;
                }
            }
            throw new ArgumentException($"Seat with ID {seatID} does not exist in the screening room.");
        }

        public void AddSeat(Seat seat)
        {
            if (seat == null)
            {
                throw new ArgumentNullException(nameof(seat), "Seat cannot be null.");
            }
            if (_seats.Contains(seat))
            {
                throw new InvalidOperationException("Seat already exists in the screening room.");
            }
            _seats.Add(seat);
        }

        public void AddFilm(Film film)
        {
            if (film == null)
            {
                throw new ArgumentNullException(nameof(film), "Film cannot be null.");
            }
            if (_films.Contains(film))
            {
                throw new InvalidOperationException("Film already exists in the screening room.");
            }
            _films.Add(film);
        }

        public int CountFreeSeats()
        {
            int count = CountSeatsWithStatus("Available");
            return count;
        }

        public int CountReservedSeats()
        {
            int count = CountSeatsWithStatus("Reserved");
            return count;
        }

        public int CountPurchasedSeats()
        {
            int count = CountSeatsWithStatus("Purchased");
            return count;
        }

        private int CountSeatsWithStatus(String status)
        {
            int count = 0;
            foreach (Seat seat in _seats)
            {
                if (seat.Status == status)
                    count++;
            }
            return count;
        }
    }
}
