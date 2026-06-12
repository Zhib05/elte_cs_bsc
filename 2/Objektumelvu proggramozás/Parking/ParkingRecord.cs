namespace Parking {
    public class ParkingRecord {
        private DateTime _from;
        private DateTime? _to;
        private Vehicle _vehicle;
        private Zone _zone;

        public DateTime? To => _to;

        public ParkingRecord(Vehicle vehicle, Zone zone) {
            _from = DateTime.Now;
            _vehicle = vehicle;
            _zone = zone;
        }

        public void End() {
            _to = DateTime.Now;   
        }

        public int Fee() {
            return _vehicle.Multiplier(_zone) * (int)Math.Round((_to! - _from).Value.TotalSeconds);
        }
    }
}
