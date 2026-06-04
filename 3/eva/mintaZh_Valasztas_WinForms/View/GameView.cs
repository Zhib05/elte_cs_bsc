using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mintaZh_Valasztas_WinForms.Model;

namespace mintaZh_Valasztas_WinForms.View
{
    public partial class GameView: Form
    {
        private readonly GameModel _model;

        private Control[,] _boardControls = new Control[3, 6];
        public GameView()
        {
            InitializeComponent();

            _model = new GameModel();
            _model.BoardChanged += Model_BoardChanged;

            // ÚJ: A tábla háttere fekete, ez adja meg a rács színét a gombok között
            Table.BackColor = Color.Black;
            // Opcionális: A tábla körüli keret eltüntetése, ha zavaró
            Table.Padding = new Padding(2);

            Setuptable();

            UpdateBoardDisplay();

            Button newGameButton = new Button { Text = "Új játék", Location = new Point(0, 0), Size = new Size(90, 30) };
            newGameButton.Click += NewGameButton_Click!;
            Controls.Add(newGameButton);
        }

        // Tábla létrehozása és inicializálása
        private void Setuptable()
        {
            // Töröljük a korábbi vezérlőket, ha lennének (biztonsági lépés)
            Table.Controls.Clear();

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    // Változás: Button használata Panel helyett
                    Button block = new Button();
                    block.Click += TableBlock_Click;

                    // Button alapstílus beállítása a szegélyek jobb kezeléséhez
                    block.Dock = DockStyle.Fill;
                    block.Margin = new Padding(2);
                    block.FlatStyle = FlatStyle.Flat; // A szegély testreszabásához szükséges
                    block.Font = new Font(block.Font, FontStyle.Bold); // P, G betűk kiemelése

                    // Mentjük az azonosításhoz szükséges pozíciót
                    block.Tag = new Position { Row = row, Col = col };

                    _boardControls[row, col] = block;
                    Table.Controls.Add(block, col, row);
                }
            }
        }

        // Egy cella megjelenítésének frissítése
        private void UpdateBlockDisplay(int row, int col)
        {
            if (!(_boardControls[row, col] is Button block)) return;

            Kerulet kerulet = _model.GetKeruletAt(row, col);

            // 1. Szín beállítása
            if (kerulet.District)
            {
                block.BackColor = Color.Purple;
                block.ForeColor = Color.White;
            }
            else
            {
                block.BackColor = Color.LightGreen;
                block.ForeColor = Color.Black;
            }

            // 2. Szöveg beállítása (Párt + Körzet sorszám)
            string partyAbbr = kerulet.District ? "P" : "G";
            string keruletText;

            if (kerulet.KorzetSzam > 0)
            {
                // Már lezárt körzet része
                keruletText = $"{partyAbbr}-{kerulet.KorzetSzam}";

                block.FlatAppearance.BorderSize = 1;
                block.FlatAppearance.BorderColor = Color.Black;
            }
            else if (_model.AktualisKeruletek.Contains(kerulet))
            {
                // Éppen kijelölés alatt áll (aktuális kijelölés sárga háttérrel)
                keruletText = partyAbbr;
                block.BackColor = Color.Yellow;
                block.ForeColor = Color.Black;

                // Eltüntetjük a keretet
                block.FlatAppearance.BorderSize = 0;
            }
            else
            {
                // Nincs kijelölve: csak a párt betűje látszik
                keruletText = partyAbbr;

                // Eltüntetjük a keretet
                block.FlatAppearance.BorderSize = 0;
            }

            block.Text = keruletText;
        }

        // Teljes tábla frissítése
        private void UpdateBoardDisplay()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    UpdateBlockDisplay(row, col);
                }
            }
        }

        // Eseménykezelő egy tábla cella kattintására
        private void TableBlock_Click(object? sender, EventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is Position position)
            {
                _model.KorzetValasztas(position.Row, position.Col);
            }
        }

        // Eseménykezelő a modell tábla változására
        private void Model_BoardChanged(object? sender, EventArgs e)
        {
            UpdateBoardDisplay();
        }

        // Eseménykezelő az "Új játék" gombra
        private void NewGameButton_Click(object? sender, EventArgs e)
        {
            _model.NewGame();
            UpdateBoardDisplay();
        }
    }
}
