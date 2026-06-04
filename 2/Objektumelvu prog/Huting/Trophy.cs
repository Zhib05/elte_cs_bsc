namespace Hunting {
    public class Trophy {
        private string _location;
        private string _date;
        private Game _game;

        public Game Game => _game;

        public Trophy(string location, string date, Game game) {
            _location = location;
            _date = date;
            _game = game;
        }
    }
}
