using mintaZh_BlockDocu_WinForms.Model;

namespace mintaZh_BlockDocu_WinForms
{
    public partial class GameView : Form
    {
        private readonly GameModel _model;

        // A Form Designer által létrehozott vezérlők:
        // BoardPanel: 4x4 TableLayoutPanel a fő játéktáblának.
        // NextShapePanel: 2x2 TableLayoutPanel a következő alakzatnak.

        private Control[,] _boardBlocks = new Control[4, 4];
        private Control[,] _nextShapeBlocks = new Control[2, 2];

        public GameView()
        {
            InitializeComponent(); // A Form Designer által létrehozott metódus

            _model = new GameModel();

            // A Modell eseménykezelőinek feliratkozása
            _model.GameBoardChanged += Model_GameBoardChanged;
            _model.NewShapeAvailable += Model_NewShapeAvailable;

            SetupBoard(); // A 4x4-es és 2x2-es panelek feltöltése vezérlőkkel

            // Kezdeti állapot megjelenítése (1. részfeladat)
            UpdateBoardDisplay();
            UpdateNextShapeDisplay();
        }

        // Segédmetódus a vezérlők inicializálására (Panel vagy Button)
        private void SetupBoard()
        {
            // 4x4 Tábla beállítása
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    // Használjunk Panel vezérlőt a blokk jelölésére
                    Panel block = new Panel();
                    block.Click += BoardBlock_Click!;

                    //// Mentjük a koordinátákat a Tag tulajdonságba a kattintás azonosításához
                    //block.Tag = new Point(r, c);

                    _boardBlocks[r, c] = block;
                    BoardPanel.Controls.Add(block, c, r);
                }
            }

            // 2x2 Alakzat panel beállítása (mindig üres marad, csak a szín változik)
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    Panel block = new Panel { Dock = DockStyle.Fill, Margin = new Padding(1) };
                    _nextShapeBlocks[r, c] = block;
                    NextShapePanel.Controls.Add(block, c, r);
                }
            }
        }

        // A Modellből jövő esemény: A tábla frissítése (1. és 2. részfeladat)
        private void Model_GameBoardChanged(object? sender, EventArgs e)
        {
            UpdateBoardDisplay();
        }

        // A Modellből jövő esemény: A következő alakzat frissítése (1. részfeladat)
        private void Model_NewShapeAvailable(object? sender, EventArgs e)
        {
            UpdateNextShapeDisplay();
        }

        // A felhasználói interakció (kattintás) - Ide kattintással jelöli meg a bal felső sarkot
        private void BoardBlock_Click(object? sender, EventArgs e)
        {
            // Az eseményt kiváltó vezérlő lekérése
            Control? clickedBlock = sender as Control;

            if (clickedBlock != null && _model != null)
            {
                // 1. Megkeressük a TableLayoutPanel-ben lévő sor- és oszlopindexet.
                // A GetRow és GetColumn statikus metódusok lekérdezik a vezérlő helyét a Panelben.
                int row = BoardPanel.GetRow(clickedBlock);
                int col = BoardPanel.GetColumn(clickedBlock);

                // 2. Ellenőrizzük, hogy a koordináták érvényesek-e (pl. -1 lehet, ha nem található)
                if (row >= 0 && col >= 0)
                {
                    // 3. Logika hívása a modellben
                    bool success = _model.TryPlaceShape(row, col);

                    if (!success)
                    {
                        MessageBox.Show("Hiba! Az alakzat kilóg a tábláról, vagy fed egy másik blokkot. Próbáld újra!", "Sikertelen elhelyezés");
                    }
                }
            }
        }

        // A 4x4 tábla megjelenítésének frissítése
        private void UpdateBoardDisplay()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    //[cite_start]// Ha a modellben foglalt (true), akkor kék, különben fehér [cite: 6, 13]
                    _boardBlocks[r, c].BackColor = _model.GetCellStatus(r, c) ? Color.Blue : Color.White;
                }
            }
        }

        // A 2x2 alakzat keret megjelenítésének frissítése (1. részfeladat)
        private void UpdateNextShapeDisplay()
        {
            // Először minden blokkot fehérre állítunk
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    _nextShapeBlocks[r, c].BackColor = Color.White;
                }
            }

            // Csak az alakzat blokkjait színezzük kékre a 2x2-es keretben
            if (_model.CurrentShape != null)
            {
                int shapeHeight = _model.CurrentShape.Layout.GetLength(0);
                int shapeWidth = _model.CurrentShape.Layout.GetLength(1);

                for (int r = 0; r < shapeHeight; r++)
                {
                    for (int c = 0; c < shapeWidth; c++)
                    {
                        if (_model.CurrentShape.Layout[r, c])
                        {
                            // Feltételezzük, hogy az alakzat befér a 2x2-es keretbe.
                            if (r < 2 && c < 2)
                            {
                                _nextShapeBlocks[r, c].BackColor = Color.Blue;
                            }
                        }
                    }
                }
            }
        }
    }
}
