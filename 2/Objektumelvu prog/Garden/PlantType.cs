namespace Garden {
    abstract public class PlantType {
        private readonly int _ripeningTime;

        public int RipeningTime => _ripeningTime;

        public PlantType(int ripeningTime) {
            _ripeningTime = ripeningTime;
        }
    }

    // --------- [ Növényfajták ] ---------
    abstract public class Vegetable : PlantType {
        public Vegetable(int i) : base(i) { }
    }

    abstract public class Flower : PlantType {
        public Flower(int i) : base(i) { }
    }


    // --------- [ Növények ] ---------
    class Potato : Vegetable {
        private static Potato? _instance;
        public static Potato Instance => _instance ??= new Potato(5);
        private Potato(int i) : base(i) { }
    }
    class Pea : Vegetable {
        private static Pea? _instance;
        public static Pea Instance => _instance ??= new Pea(3);
        private Pea(int i) : base(i) { }
    }
    class Onion : Vegetable {
        private static Onion? _instance;
        public static Onion Instance => _instance ??= new Onion(2);
        private Onion(int i) : base(i) { }
    }

    class Tulip : Flower {
        private static Tulip? _instance;
        public static Tulip Instance => _instance ??= new Tulip(6);
        private Tulip(int i) : base(i) { }
    }
    class Carnation : Flower {
        private static Carnation? _instance;
        public static Carnation Instance => _instance ??= new Carnation(7);
        private Carnation(int i) : base(i) { }
    }
    class Rose : Flower {
        private static Rose? _instance;
        public static Rose Instance => _instance ??= new Rose(8);
        private Rose(int i) : base(i) { }
    }
}
