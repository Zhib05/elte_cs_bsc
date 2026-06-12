namespace Parking {
    public class Company {
        private List<Zone> _zones;
        private List<Vehicle> _vehicles;
        private List<Vehicle> _discounts;

        public Company(List<Zone> zones) {
            if (zones.Count == 0) {
                throw new ArgumentException("Legalább egy zóna szükséges!");
            }

            _zones = new List<Zone>(zones);
            _vehicles = new List<Vehicle>();
            _discounts = new List<Vehicle>();
        }

        public void Register(Vehicle vehicle) {
            if (_vehicles.Contains(vehicle)) {
                throw new ArgumentException("Ez a jármű már regisztrált!");
            }
            _vehicles.Add(vehicle);
        }

        public void Permission(Vehicle vehicle) {
            if (!_vehicles.Contains(vehicle)) {
                throw new ArgumentException("Ez a jármű még nem regisztrált!");
            }

            if (_discounts.Contains(vehicle)) {
                throw new ArgumentException("Ez a jármű már kapott kedvezményt!");
            }

            _discounts.Add(vehicle);
        }

        public bool Where(DateOnly date, out Zone? maxZone) {
            maxZone = null;
            int max = 0;
            bool found = false;

            foreach (Zone zone in _zones) {
                int currIncome = zone.Income(date);
                if (found) {
                    if (currIncome > max) {
                        max = currIncome;
                        maxZone = zone;
                    }
                } else {
                    max = currIncome;
                    maxZone = zone;
                    found = true;
                }
            }

            return found;
        }

        public bool Search(DateOnly date, out Vehicle? vehicle) {
            vehicle = null;

            foreach (Vehicle v in _vehicles) {
                if (!_discounts.Contains(v) && v.HowManyTimes(date) > 1) {
                    vehicle = v;
                    return true;
                }
            }

            return false;
        }
    }
}
