namespace Hunting {
    public enum Gender { Male, Female }
    public abstract class Game {
        protected double _weight;
        protected Gender _gender;

        public double Weight => _weight;
        public Gender Gender => _gender;

        protected Game(double weight, Gender gender) {
            _weight = weight;
            _gender = gender;
        }
    }

    public class Elephant : Game {
        private double _left;
        private double _right;

        public double Left => _left;
        public double Right => _right;

        public Elephant(double weight, double right, double left, Gender gender) : base(weight, gender) {
            _left = left;
            _right = right;
        }
    }

    public class Rhino : Game {
        private double _horn;

        public double Horn => _horn;

        public Rhino(double weight, double horn, Gender gender) : base(weight, gender) {
            _horn = horn;
        }
    }

    public class Lion : Game {
        public Lion(double weight, Gender gender) : base(weight, gender) { }
    }
}
