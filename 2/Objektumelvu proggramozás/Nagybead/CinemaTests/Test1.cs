using Cinema;

namespace CinemaTests
{
    [TestClass]
    public sealed class Test1
    {
        private Cinema.Cinema cinema;
        private Viewer adultViewer;
        private Viewer childViewer;
        private ScreeningRoom vipRoom;
        private ScreeningRoom mediumRoom;
        private Film testFilm1;
        private Film testFilm2;

        [TestInitialize]
        public void Initialize()
        {
            cinema = new Cinema.Cinema();

            adultViewer = new Viewer("V1", Status.Adult);
            childViewer = new Viewer("V2", Status.Child);
            cinema.AddViewer(adultViewer);
            cinema.AddViewer(childViewer);

            vipRoom = new ScreeningRoom("VIP Room", new VIP());
            mediumRoom = new ScreeningRoom("Medium Room", new Medium());
            vipRoom.AddSeat(new Seat("A1"));
            vipRoom.AddSeat(new Seat("A2"));
            vipRoom.AddSeat(new Seat("A3"));
            mediumRoom.AddSeat(new Seat("B1"));
            mediumRoom.AddSeat(new Seat("B2"));
            testFilm1 = new Film("Film1");
            testFilm2 = new Film("Film2");
            vipRoom.AddFilm(testFilm1);
            mediumRoom.AddFilm(testFilm2);

            cinema.AddScreeningRoom(vipRoom);
            cinema.AddScreeningRoom(mediumRoom);
        }

        [TestMethod]
        public void AddViewer_ShouldAddNewViewer()
        {
            Viewer newViewer = new Viewer("V3", Status.Student);
            cinema.AddViewer(newViewer);

            Assert.AreEqual(3, cinema.Viewers.Count);
            Assert.IsTrue(cinema.Viewers.Contains(newViewer));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddViewer_ShouldThrowWhenAddingDuplicate()
        {
            cinema.AddViewer(adultViewer);
        }

        [TestMethod]
        public void BookTicket_ShouldReserveSeat()
        {
            cinema.BookTicket("A1", "VIP Room", "V1", "Film1");

            Assert.AreEqual("Reserved", vipRoom.SeatAvailability("A1"));
            Assert.AreEqual(1, adultViewer.Tickets.Count);
            Assert.AreEqual(1, testFilm1.tickets.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BookTicket_ShouldThrowWhenSeatNotAvailable()
        {
            cinema.BookTicket("A1", "VIP Room", "V1", "Film1");
            cinema.BookTicket("A1", "VIP Room", "V2", "Film1");
        }

        [TestMethod]
        public void BuyTicket_ShouldPurchaseAvailableSeat()
        {
            cinema.BuyTicket("A1", "VIP Room", "V1", "Film1");

            Assert.AreEqual("Purchased", vipRoom.SeatAvailability("A1"));
            Assert.AreEqual(1, adultViewer.Tickets.Count);
            Assert.AreEqual(1, testFilm1.tickets.Count);
        }

        [TestMethod]
        public void BuyTicket_ShouldPurchaseReservedSeatBySameViewer()
        {
            cinema.BookTicket("A1", "VIP Room", "V1", "Film1");
            cinema.BuyTicket("A1", "VIP Room", "V1", "Film1");

            Assert.AreEqual("Purchased", vipRoom.SeatAvailability("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BuyTicket_ShouldThrowWhenReservedByOtherViewer()
        {
            cinema.BuyTicket("A1", "VIP Room", "V1", "Film1");
            cinema.BuyTicket("A1", "VIP Room", "V1", "Film1");
        }

        [TestMethod]
        public void CancelBooking_ShouldFreeReservedSeat()
        {
            cinema.BookTicket("A1", "VIP Room", "V1", "Film1");
            cinema.CancelBooking(adultViewer.Tickets[0]);

            Assert.AreEqual("Available", vipRoom.SeatAvailability("A1"));
            Assert.AreEqual(0, adultViewer.Tickets.Count);
            Assert.AreEqual(0, testFilm1.tickets.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CancelBooking_ShouldThrowWhenNotReserved()
        {
            cinema.BuyTicket("A1", "VIP Room", "V1", "Film1");
            cinema.CancelBooking(adultViewer.Tickets[0]);
        }

        [TestMethod]
        public void MostViewedFilm_ShouldReturnCorrectFilm()
        {
            cinema.BuyTicket("A1", "VIP Room", "V1", "Film1");
            cinema.BuyTicket("A2", "VIP Room", "V2", "Film1");
            cinema.BuyTicket("A3", "VIP Room", "V2", "Film1");
            cinema.BuyTicket("B1", "Medium Room", "V1", "Film2");
            cinema.BuyTicket("B2", "Medium Room", "V1", "Film2");

            string result = cinema.MostViewedFilm();
            Assert.AreEqual("Film1", result);
        }

        [TestMethod]
        public void MostViewedFilm_ShouldReturnDefaultWhenNoViews()
        {
            string result = cinema.MostViewedFilm();
            Assert.AreEqual("No films viewed", result);
        }

        [TestMethod]
        public void TicketPrice_ShouldApplyVIPDiscountForAdult()
        {
            Ticket ticket = new Ticket("A1", "Purchased", adultViewer, vipRoom, testFilm1);
            double price = ticket.Price();

            Assert.AreEqual(2850, price);
        }

        [TestMethod]
        public void TicketPrice_ShouldApplyNoDiscountForChildInVIP()
        {
            Ticket ticket = new Ticket("A1", "Purchased", childViewer, vipRoom, testFilm1);
            double price = ticket.Price();

            Assert.AreEqual(3000, price);
        }

        [TestMethod]
        public void TicketPrice_ShouldApplyMediumDiscountForAdult()
        {
            Ticket ticket = new Ticket("B1", "Purchased", adultViewer, mediumRoom, testFilm2);
            double price = ticket.Price();

            Assert.AreEqual(2850, price);
        }

        [TestMethod]
        public void BecomeRegular_ShouldChangeViewerStatus()
        {
            adultViewer.BecomeRegular();
            Assert.AreEqual(Status.Regular, adultViewer.ViewerStatus);
        }

        [TestMethod]
        public void CountSeats_ShouldReturnCorrectNumbers()
        {
            Assert.AreEqual(3, vipRoom.CountFreeSeats());

            cinema.BookTicket("A1", "VIP Room", "V1", "Film1");
            Assert.AreEqual(2, vipRoom.CountFreeSeats());
            Assert.AreEqual(1, vipRoom.CountReservedSeats());

            cinema.BuyTicket("A1", "VIP Room", "V1", "Film1");
            Assert.AreEqual(1, vipRoom.CountPurchasedSeats());
        }
    }
}
