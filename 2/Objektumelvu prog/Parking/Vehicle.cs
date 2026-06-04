namespace Parking {
    public abstract class Vehicle {
        private string _licensePlate;
        private List<ParkingRecord> _parkingRecords;

        public string LicensePlate => _licensePlate;

        public Vehicle(string licensePlate) {
            _licensePlate = licensePlate;
            _parkingRecords = new List<ParkingRecord>();
        }

        public void Start(Zone zone, DateTime time) {
            if (CurrentlyParking(out ParkingRecord? _)) {
                throw new Exception("A jármű éppen parkol!");
            }
            ParkingRecord parkingRecord = new ParkingRecord(this, zone);
            _parkingRecords.Add(parkingRecord);
            zone.New(parkingRecord);
        }

        public bool CurrentlyParking(out ParkingRecord? parkingRecord) {
            parkingRecord = null;
            foreach (ParkingRecord record in _parkingRecords) {
                if (record.To == null) {
                    parkingRecord = record;
                    return true;
                }
            }
            return false;
        }

        public void End() {
            if (!CurrentlyParking(out ParkingRecord? parkingRecord)) {
                throw new Exception("Az jármű nem parkol jelenleg!");
            }
            parkingRecord!.End();
            // fizetés ...
        }

        public int HowManyTimes(DateOnly date) {
            int count = 0;
            foreach (ParkingRecord parkingRecord in _parkingRecords) {
                if (parkingRecord.To.HasValue && DateOnly.FromDateTime(parkingRecord.To.Value) == date) {
                    count++;
                }
            }
            return count;
        }

        public abstract int Multiplier(Zone zone);
    }

    public class Motorcycle : Vehicle {
        public Motorcycle(string licensePlate) : base(licensePlate) { }

        public override int Multiplier(Zone zone) => zone.Tariff(this);
    }

    public class Car : Vehicle {
        public Car(string licensePlate) : base(licensePlate) { }

        public override int Multiplier(Zone zone) => zone.Tariff(this);
    }

    public class Truck : Vehicle {
        public Truck(string licensePlate) : base(licensePlate) { }

        public override int Multiplier(Zone zone) => zone.Tariff(this);
    }
}
