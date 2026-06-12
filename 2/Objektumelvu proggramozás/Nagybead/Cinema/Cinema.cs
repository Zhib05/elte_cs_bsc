using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Cinema
    {
        private List<Viewer> viewers;
        private List<ScreeningRoom> screeningRooms;

        public Cinema()
        {
            this.viewers = new List<Viewer>();
            this.screeningRooms = new List<ScreeningRoom>();
        }

        public List<Viewer> Viewers => viewers;
        public List<ScreeningRoom> ScreeningRooms => screeningRooms;

        public void AddViewer(Viewer viewer)
        {
            if (viewer == null)
            {
                throw new ArgumentNullException(nameof(viewer), "Viewer cannot be null.");
            }
            if (viewers.Contains(viewer))
            {
                throw new InvalidOperationException("Viewer already exists in the cinema.");
            }
            viewers.Add(viewer);
        }

        public void AddScreeningRoom(ScreeningRoom room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room), "Screening room cannot be null.");
            }
            if (screeningRooms.Contains(room))
            {
                throw new InvalidOperationException("Screening room already exists in the cinema.");
            }
            screeningRooms.Add(room);
        }

        public void RemoveRoom(String roomName)
        {
            foreach (ScreeningRoom room in screeningRooms)
            {
                if (room.RoomName == roomName)
                {
                    screeningRooms.Remove(room);
                    return;
                }
            }
        }

        public String MostViewedFilm()
        {
            List<String> filmNames = new List<String>();
            List<int> filmCounts = new List<int>();

            foreach (ScreeningRoom room in screeningRooms)
            {
                foreach (Film film in room.Films)
                {
                    int viewers = 0;
                    foreach (Ticket ticket in film.tickets)
                    {
                        if (ticket.Status == "Purchased")
                        {
                            viewers++;
                        }
                    }

                    if (filmNames.Contains(film.FilmName))
                    {
                        int index = filmNames.IndexOf(film.FilmName);
                        filmCounts[index] += viewers;
                    } else
                    {
                        filmNames.Add(film.FilmName);
                        filmCounts.Add(viewers);
                    }
                }
            }

            string mostViewedFilm = "No films viewed";
            int maxViewers = 0;
            for (int i = 0; i < filmNames.Count; i++)
            {
                if (filmCounts[i] > maxViewers)
                {
                    maxViewers = filmCounts[i];
                    mostViewedFilm = filmNames[i];
                }
            }

            return mostViewedFilm;
        }

        public void BookTicket(String seatID, String roomName, String viewerID, string filmName)
        {
            ScreeningRoom room = FindRoom(roomName);
            Viewer viewer = FindViewer(viewerID);
            Film film = FindFilm(filmName);
            if (room.SeatAvailability(seatID) != "Available")
            {
                throw new InvalidOperationException($"Seat {seatID} in room {roomName} is not available.");
            }
            Ticket ticket = new Ticket(seatID, "Reserved", viewer, room, film);
            film.AddTicket(ticket);
            viewer.AddTicket(ticket);
            foreach (Seat seat in room.Seats)
            {
                if (seat.ID == seatID)
                {
                    seat.SetStatus("Reserved");
                    return;
                }
            }
        }

        public void BuyTicket(String seatID, String roomName, String viewerID, string filmName)
        {
            ScreeningRoom room = FindRoom(roomName);
            Viewer viewer = FindViewer(viewerID);
            Film film = FindFilm(filmName);
            if (room.SeatAvailability(seatID) != "Available" && room.SeatAvailability(seatID) != "Reserved")
            {
                throw new InvalidOperationException($"Seat {seatID} in room {roomName} is not available/reserved.");
            }
            Ticket ticket = new Ticket(seatID, "Purchased", viewer, room, film);
            film.AddTicket(ticket);
            viewer.AddTicket(ticket);
            foreach (Seat seat in room.Seats)
            {
                if (seat.ID == seatID)
                {
                    seat.SetStatus("Purchased");
                    return;
                }
            }
        }

        public void CancelBooking(Ticket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket), "Ticket cannot be null.");
            }
            ScreeningRoom room = ticket.ScreeningRoom;
            Viewer viewer = ticket.Viewer;
            Film film = ticket.Film;
            if (room.SeatAvailability(ticket.SeatID) != "Reserved")
            {
                throw new InvalidOperationException($"Seat {ticket.SeatID} in room {room.RoomName} cannot be cancelled as it is not reserved.");
            }
            film.RemoveTicket(ticket);
            viewer.RemoveTicket(ticket);
            foreach (Seat seat in room.Seats)
            {
                if (seat.ID == ticket.SeatID)
                {
                    seat.SetStatus("Available");
                    return;
                }
            }
        }

        private ScreeningRoom FindRoom(String roomName)
        {
            foreach (ScreeningRoom room in screeningRooms)
            {
                if (room.RoomName == roomName)
                {
                    return room;
                }
            }
            throw new ArgumentException($"Screening room with name ({roomName}) does not exist.");
        }

        private Viewer FindViewer(String viewerID)
        {
            foreach (Viewer viewer in viewers)
            {
                if (viewer.ID == viewerID)
                {
                    return viewer;
                }
            }
            throw new ArgumentException($"Viewer with ID ({viewerID}) does not exist.");
        }

        private Film FindFilm(String filmName)
        {
            foreach (ScreeningRoom room in screeningRooms)
            {
                foreach (Film film in room.Films)
                {
                    if (film.FilmName == filmName)
                    {
                        return film;
                    }
                }
            }
            throw new ArgumentException($"Film with name ({filmName}) does not exist in any screening room.");
        }
    }
}
