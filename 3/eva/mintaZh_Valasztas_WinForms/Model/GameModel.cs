using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mintaZh_Valasztas_WinForms.Model
{
    public struct Position
    {
        public int Row { get; set; }
        public int Col { get; set; }
    }

    public class Kerulet
    {
        // Igaz, ha Lila párti kerület, hamis, ha zöld párti kerület
        public bool District { get; }
        public Position Position { get; }
        public int KorzetSzam { get; set; } = -1; // Alapértelmezett érték -1, ha nincs körzethez rendelve

        public Kerulet(bool district, Position position)
        {
            this.District = district;
            Position = position;
        }
    }

    public class GameModel
    {
        private Kerulet[,] _board;

        // Inicializálva a konstruktorban
        public List<Kerulet> AktualisKeruletek { get; set; }
        public List<List<Kerulet>> KivalasztottKorzetek { get; set; }

        public GameModel()
        {
            _board = new Kerulet[3, 6]
            {
                { new Kerulet(false, new Position{ Row = 0, Col = 0 }),
                  new Kerulet(false, new Position{ Row = 0, Col = 1 }),
                  new Kerulet(true, new Position{ Row = 0, Col = 2 }),
                  new Kerulet(false, new Position{ Row = 0, Col = 3 }),
                  new Kerulet(true, new Position{ Row = 0, Col = 4 }),
                  new Kerulet(false, new Position{ Row = 0, Col = 5 })
                },
                {
                  new Kerulet(true, new Position{ Row = 1, Col = 0 }),
                  new Kerulet(false, new Position{ Row = 1, Col = 1 }),
                  new Kerulet(true, new Position{ Row = 1, Col = 2 }),
                  new Kerulet(false, new Position{ Row = 1, Col = 3 }),
                  new Kerulet(false, new Position{ Row = 1, Col = 4 }),
                  new Kerulet(true, new Position{ Row = 1, Col = 5 })
                },
                {
                  new Kerulet(false, new Position{ Row = 2, Col = 0 }),
                  new Kerulet(true, new Position{ Row = 2, Col = 1 }),
                  new Kerulet(false, new Position{ Row = 2, Col = 2 }),
                  new Kerulet(false, new Position{ Row = 2, Col = 3 }),
                  new Kerulet(true, new Position{ Row = 2, Col = 4 }),
                  new Kerulet(true, new Position{ Row = 2, Col = 5 })
                }
            };

            KivalasztottKorzetek = new List<List<Kerulet>>();
            AktualisKeruletek = new List<Kerulet>(); // HIÁNYZOTT: Inicializálás
        }

        public event EventHandler? BoardChanged;

        public bool KorzetValasztas(int row, int col)
        {
            Kerulet selectedKerulet = _board[row, col];

            // 1. Ellenőrzés: Már benne van az aktuális körzetben VAGY már lezárt körzet része
            if (AktualisKeruletek.Contains(selectedKerulet) || KivalasztottKorzetek.Any(k => k.Contains(selectedKerulet)))
            {
                return false;
            }

            // 2. Ellenőrzés: Érvényes-e a választás?
            // Az első elemet (AktualisKeruletek.Count == 0) mindig elfogadjuk.
            bool isValidSelection = AktualisKeruletek.Count == 0;

            if (AktualisKeruletek.Count > 0)
            {
                foreach (var currentKerulet in AktualisKeruletek)
                {
                    // Ellenőrizzük, hogy a kiválasztott kerület szomszédos-e valamelyik már kiválasztott kerülettel
                    int rowDiff = Math.Abs(currentKerulet.Position.Row - selectedKerulet.Position.Row);
                    int colDiff = Math.Abs(currentKerulet.Position.Col - selectedKerulet.Position.Col);

                    // Csak akkor szomszédos, ha a különbség 1 az egyik tengelyen ÉS 0 a másikon (nem átlósan)
                    if ((rowDiff == 1 && colDiff == 0) || (rowDiff == 0 && colDiff == 1))
                    {
                        isValidSelection = true;
                        break; // Elég egy szomszéd is a folytatáshoz
                    }
                }
            }

            if (!isValidSelection)
            {
                return false;
            }

            // 3. Kiválasztás
            AktualisKeruletek.Add(selectedKerulet);

            // 4. Körzet lezárása
            if (AktualisKeruletek.Count == 3)
            {
                // ÚJ: Meghatározzuk a következő körzet sorszámát
                int ujKorzetSzam = KivalasztottKorzetek.Count + 1;

                // Hozzárendeljük a sorszámot a 3 kerülethez
                foreach (var kerulet in AktualisKeruletek)
                {
                    kerulet.KorzetSzam = ujKorzetSzam;
                }

                // Hozzáadjuk a lezárt körzetet a listához, majd töröljük az aktuális kerületeket
                KivalasztottKorzetek.Add(new List<Kerulet>(AktualisKeruletek));
                AktualisKeruletek.Clear();
            }

            BoardChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public Kerulet GetKeruletAt(int row, int col)
        {
            // Visszaadjuk a megadott pozíción lévő kerületet
            return _board[row, col];
        }

        public void NewGame()
        {
            // Töröljük a kiválasztott körzeteket és az aktuális kerületeket
            KivalasztottKorzetek.Clear();
            AktualisKeruletek.Clear();

            // Végigmegyünk a táblán, és minden kerületről levesszük a sorszámot
            for (int row = 0; row < _board.GetLength(0); row++)
            {
                for (int col = 0; col < _board.GetLength(1); col++)
                {
                    _board[row, col].KorzetSzam = -1;
                }
            }

            // Jelzés a nézetnek a frissítéshez
            BoardChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
