namespace FishingContest {
    public interface IFish {
        public int Multiplier();
    }

    public class Carp : IFish {
        private static Carp? _instance = null;
        public static Carp Instance => _instance ??= new Carp();
        private Carp() { }

        public int Multiplier() {
            return 1;
        }
    }

    public class Bream : IFish {
        private static Bream? _instance = null;
        public static Bream Instance => _instance ??= new Bream();
        private Bream() { }

        public int Multiplier() {
            return 2;
        }
    }

    public class Catfish : IFish {
        private static Catfish? _instance = null;
        public static Catfish Instance => _instance ??= new Catfish();
        private Catfish() { }

        public int Multiplier() {
            return 3;
        }
    }
}