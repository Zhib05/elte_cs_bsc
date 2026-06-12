namespace Parking {
    public abstract class Zone {
        private List<ParkingRecord> _parkingRecords;

        public Zone() {
            _parkingRecords = new List<ParkingRecord>();
        }

        public void New(ParkingRecord parkingRecord) {
            _parkingRecords.Add(parkingRecord);
        }

        public abstract int Tariff(Motorcycle _);
        public abstract int Tariff(Car _);
        public abstract int Tariff(Truck _);

        public int Income(DateOnly date) {
            int acc = 0;
            foreach (ParkingRecord parkingRecord in _parkingRecords) {
                if (parkingRecord.To.HasValue && DateOnly.FromDateTime(parkingRecord.To.Value) == date) {
                    acc += parkingRecord.Fee();
                }
            }
            return acc;
        }
    }

    public class Zone1 : Zone {
        private static Zone1? _instance;
        public static Zone1 Instance => _instance ??= new Zone1();
        private Zone1() { }

        public override int Tariff(Motorcycle _) => 200;
        public override int Tariff(Car _) => 500;
        public override int Tariff(Truck _) => 1000;
    }

    public class Zone2 : Zone {
        private static Zone2? _instance;
        public static Zone2 Instance => _instance ??= new Zone2();
        private Zone2() { }

        public override int Tariff(Motorcycle _) => 400;
        public override int Tariff(Car _) => 1000;
        public override int Tariff(Truck _) => 2000;
    }

    public class Zone3 : Zone {
        private static Zone3? _instance;
        public static Zone3 Instance => _instance ??= new Zone3();
        private Zone3() { }

        public override int Tariff(Motorcycle _) => 600;
        public override int Tariff(Car _) => 1200;
        public override int Tariff(Truck _) => 6000;
    }
}
