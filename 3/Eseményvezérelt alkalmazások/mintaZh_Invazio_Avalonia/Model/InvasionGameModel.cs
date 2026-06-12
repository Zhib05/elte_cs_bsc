using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mintaZh_Invazio_Avalonia.Model
{
    // Mezőtípusok a megjelenítéshez
    public enum Fieldtype { Empty, Wall, Enemy, Soldier }

    public class InvasionGameModel
    {
        #region Konstansok
        public const int Rows = 12;      // Pálya magassága
        public const int Columns = 6;    // Pálya szélessége
        private const int GameTickSpeed = 600; // Játék sebessége (ms)
        #endregion

        #region Mezők
        private readonly ITimer _timer;
        private readonly Random _random;

        // Játékállapot
        private int _castleLives;
        private int _enemiesKilled;
        private int _deployedSoldiers;
        private int _timeElapsed;

        // Entitások pozíciói
        private List<Tuple<int, int>> _enemies;
        private List<Tuple<int, int>> _soldiers;
        #endregion

        #region Tulajdonságok
        public int CastleLives => _castleLives;
        public int TimeElapsed => _timeElapsed;
        public bool IsGameOver => _castleLives <= 0;

        // Kiszámoljuk, vehetünk-e még katonát:
        // Alap 2 + minden 3. ölés után 1 - amennyit már leraktunk
        public int AvailableSoldiers => (2 + (_enemiesKilled / 3)) - _deployedSoldiers;
        #endregion

        #region Események
        public event EventHandler? GameAdvanced; // Léptetéskor (idő, mozgás)
        public event EventHandler? GameOver;     // Játék vége
        #endregion

        #region Konstruktor
        public InvasionGameModel(ITimer timer)
        {
            _timer = timer;
            _timer.Interval = GameTickSpeed;
            _timer.Elapsed += Timer_Elapsed;

            _random = new Random();
            _enemies = new List<Tuple<int, int>>();
            _soldiers = new List<Tuple<int, int>>();
        }
        #endregion

        #region Publikus metódusok
        public void NewGame()
        {
            _castleLives = 3;
            _enemiesKilled = 0;
            _deployedSoldiers = 0;
            _timeElapsed = 0;
            _enemies.Clear();
            _soldiers.Clear();

            _timer.Start();
            // Azonnali frissítés, hogy a UI alaphelyzetbe álljon
            GameAdvanced?.Invoke(this, EventArgs.Empty);
        }

        public void PlaceSoldier(int x, int y)
        {
            if (IsGameOver) return;

            // Csak a fal elé, üres helyre rakhatunk, ha van keretünk
            if (x < Rows - 1 && // Nem a falra
                !IsOccupied(x, y) &&
                AvailableSoldiers > 0)
            {
                _soldiers.Add(new Tuple<int, int>(x, y));
                _deployedSoldiers++;
                GameAdvanced?.Invoke(this, EventArgs.Empty);
            }
        }

        // Segédfüggvény a nézetnek: megmondja, mi van az adott koordinátán
        public Fieldtype GetFieldValue(int x, int y)
        {
            if (x == Rows - 1) return Fieldtype.Wall; // Az utolsó sor a fal

            // Koordináta keresése a listákban
            var pos = new Tuple<int, int>(x, y);
            if (_soldiers.Contains(pos)) return Fieldtype.Soldier;
            if (_enemies.Contains(pos)) return Fieldtype.Enemy;

            return Fieldtype.Empty;
        }
        #endregion

        #region Privát logika
        private bool IsOccupied(int x, int y)
        {
            var pos = new Tuple<int, int>(x, y);
            return _soldiers.Contains(pos) || _enemies.Contains(pos);
        }

        private void Timer_Elapsed(object? sender, EventArgs e)
        {
            _timeElapsed++;

            // 1. Új ellenség generálása (kb. 30% esély minden ütemben)
            if (_random.NextDouble() < 0.3)
            {
                int startY = _random.Next(Columns);
                // Csak akkor jön létre, ha a belépési pont üres
                if (!IsOccupied(0, startY))
                {
                    _enemies.Add(new Tuple<int, int>(0, startY));
                }
            }

            // 2. Mozgatás
            MoveEnemies();

            // 3. Ütközések és harc
            CheckCollisions();

            GameAdvanced?.Invoke(this, EventArgs.Empty);

            // 4. Játék vége ellenőrzés
            if (IsGameOver)
            {
                _timer.Stop();
                GameOver?.Invoke(this, EventArgs.Empty);
            }
        }

        private void MoveEnemies()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                // Mindenki egyet lép lefelé (X koordináta növelése)
                var oldPos = _enemies[i];
                _enemies[i] = new Tuple<int, int>(oldPos.Item1 + 1, oldPos.Item2);
            }
        }

        private void CheckCollisions()
        {
            List<Tuple<int, int>> deadEnemies = new List<Tuple<int, int>>();
            List<Tuple<int, int>> deadSoldiers = new List<Tuple<int, int>>();

            foreach (var enemy in _enemies)
            {
                // A) Elérte a falat? (A fal az utolsó sor: Rows - 1)
                if (enemy.Item1 == Rows - 1)
                {
                    _castleLives--;
                    deadEnemies.Add(enemy);
                    continue;
                }

                // B) Ütközés katonával (ugyanazon a mezőn)
                if (_soldiers.Contains(enemy))
                {
                    deadEnemies.Add(enemy);
                    deadSoldiers.Add(enemy); // A katona is meghal
                    // Itt nem növeljük a _enemiesKilled-et, mert "kiesnek", nem mi öltük meg aktívan (opcionális értelmezés)
                    // De a feladat szerint "minden harmadik megölt ellenség", így számolhatjuk ezt is:
                    _enemiesKilled++;
                    continue;
                }

                // C) Elhaladás katona mellett (szomszédos mezőn van katona)
                // Megnézzük a 4 szomszédot
                bool soldierNearby = false;
                int[] dx = { -1, 1, 0, 0 };
                int[] dy = { 0, 0, -1, 1 };

                for (int k = 0; k < 4; k++)
                {
                    var neighbor = new Tuple<int, int>(enemy.Item1 + dx[k], enemy.Item2 + dy[k]);
                    if (_soldiers.Contains(neighbor))
                    {
                        soldierNearby = true;
                        break;
                    }
                }

                if (soldierNearby)
                {
                    deadEnemies.Add(enemy);
                    _enemiesKilled++;
                }
            }

            // Törlések végrehajtása
            foreach (var e in deadEnemies) _enemies.Remove(e);
            foreach (var s in deadSoldiers) _soldiers.Remove(s);
        }
        #endregion
    }
}
