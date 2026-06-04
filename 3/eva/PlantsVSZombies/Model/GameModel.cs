using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantsVsZombies;
using System.Timers;

namespace PlantsVSZombies.Model
{
    public class GameModel
    {
        // --- KONSTANSOK --- 
        public int Rows { get; } = 5;
        public int Cols { get; } = 10;
        private const int PlantCost = 100;

        // --- ADATOK --- 
        public int Sun { get; private set; } // Pénz (Nap) 
        public bool[,] Plants { get; private set; } // Növények helyzete 
        public List<Zombie> Zombies { get; private set; } // Zombik listája 

        private System.Timers.Timer _gameTimer;
        private int _tickCounter = 0; // Hány másodperc telt el 
        private Random _rnd = new Random();

        // --- ESEMÉNYEK (A Form erre iratkozik fel) --- 
        public event EventHandler StateChanged; // Ha bármi változik (újrajzolás) 
        public event EventHandler<string> GameOver; // Ha vége a játéknak 

        public GameModel()
        {
            Plants = new bool[Rows, Cols];
            Zombies = new List<Zombie>();
            Sun = 75; // Kezdő nap mennyiség 

            // Időzítő beállítása: 1 másodpercenként (1000ms) ketyeg 
            _gameTimer = new System.Timers.Timer(1000);
            _gameTimer.Elapsed += OnGameTick;
        }

        public void StartGame()
        {
            _gameTimer.Start();
        }

        public void StopGame()
        {
            _gameTimer.Stop();
        }

        // --- FŐ CIKLUS (Minden másodpercben lefut) --- 
        private void OnGameTick(object sender, ElapsedEventArgs e)
        {
            _tickCounter++;

            // 1. ZOMBI SPAWN (Minden 3. másodpercben, 30% esély) 
            if (_tickCounter % 3 == 0)
            {
                if (_rnd.NextDouble() < 0.3) // 0.0 - 1.0 között 
                {
                    SpawnZombie();
                }
            }

            // 2. BEVÉTEL (Minden 4. másodpercben +25 nap) 
            if (_tickCounter % 4 == 0)
            {
                Sun += 25;
            }

            // 3. LÖVÉS (Minden 4. másodpercben) 
            // Fontos: Előbb lövünk, hogy ne lépjen a halott zombi 
            if (_tickCounter % 4 == 0)
            {
                HandleShooting();
            }

            // 4. MOZGÁS (Minden másodpercben) 
            HandleZombieMovement();

            // Értesítjük a Formot, hogy frissítsen 
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        // --- LOGIKAI RÉSZLETEK --- 

        private void SpawnZombie()
        {
            int r = _rnd.Next(0, Rows);
            // Jobb szélre rakjuk (utolsó oszlop: Cols - 1) 
            Zombies.Add(new Zombie(r, Cols - 1));
        }

        private void HandleShooting()
        {
            // Végigmegyünk a sorokon 
            for (int r = 0; r < Rows; r++)
            {
                // Megkeressük a növényeket az adott sorban (balról jobbra) 
                for (int c = 0; c < Cols; c++)
                {
                    if (Plants[r, c]) // Ha van itt növény (Peashooter) 
                    {
                        // Megkeressük az ELSŐ zombit, aki tőle jobbra van 
                        // OrderBy(z => z.Col) biztosítja, hogy a legközelebbit találjuk meg
                        var target = Zombies
                            .Where(z => z.Row == r && z.Col > c)
                            .OrderBy(z => z.Col)
                            .FirstOrDefault();

                        if (target != null)
                        {
                            Zombies.Remove(target); // BUMM! Zombi törölve. 
                        }
                    }
                }
            }
        }

        private void HandleZombieMovement()
        {
            // Visszafelé iterálunk, ha törölni kéne (biztonságosabb), bár itt most nem törlünk
            for (int i = Zombies.Count - 1; i >= 0; i--)
            {
                var z = Zombies[i];
                int nextCol = z.Col - 1;

                // GAME OVER ELLENŐRZÉS 
                if (nextCol < 0)
                {
                    StopGame();
                    GameOver?.Invoke(this, "A zombik megették az agyadat! (Vége)"); 
                    return;
                }

                // VAN-E NÖVÉNY ELŐTTE? 
                if (Plants[z.Row, nextCol])
                {
                    // Ha van növény, megeszi (eltűnik a növény) 
                    // A feladat szerint: "extra másodpercet tölt el" 
                    // Ez azt jelenti: MOST leveszi a növényt, de NEM lép. 
                    Plants[z.Row, nextCol] = false;
                    // A zombi helyben marad ebben a körben (evés ideje) 
                }
                else
                {
                    // Ha nincs növény, lép egyet balra 
                    z.Col--;
                }
            }
        }

        // --- JÁTÉKOS INTERAKCIÓ (Vásárlás) --- 
        public void PurchasePlant(int r, int c)
        {
            // Ellenőrzések: 
            // 1. Van elég pénz? 
            // 2. Üres a mező? (Nincs növény) 
            // 3. Nincs ott zombi? 
            bool isZombieThere = Zombies.Any(z => z.Row == r && z.Col == c);

            if (Sun >= PlantCost && !Plants[r, c] && !isZombieThere)
            {
                Sun -= PlantCost;
                Plants[r, c] = true;
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // Segédfüggvény a nézetnek: Van itt zombi? 
        public bool IsZombieAt(int r, int c)
        {
            return Zombies.Any(z => z.Row == r && z.Col == c);
        }
    }
}
