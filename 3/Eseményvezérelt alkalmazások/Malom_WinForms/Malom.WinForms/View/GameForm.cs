using System;
using System.Drawing;
using System.Windows.Forms;
using Malom.Model;
using Malom.Persistence;

namespace Malom
{
    /// <summary>
    /// Játék főablaka.
    /// </summary>
    public partial class GameForm : Form
    {
        private GameModel _model;
        private readonly GamePersistence _persistence;
        private readonly Button[] _boardButtons;
        private bool _removingPiece = false;

        /// <summary>
        /// Játékablak példányosítása.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public GameForm()
        {
            InitializeComponent();
            _persistence = new GamePersistence();
            _model = new GameModel(_persistence);

            string imagePath = Path.Combine(Application.StartupPath, "malom_board.png");
            if (File.Exists(imagePath))
            {
                this.BackgroundImage = Image.FromFile(imagePath);
                this.BackgroundImageLayout = ImageLayout.Center;
            }
            else
            {
                MessageBox.Show($"A kép ('malom_board.png') nem található a következő helyen: {imagePath}. Helyezd el a projekt mappájában!");
            }

            int buttonSize = 40;
            if (this.BackgroundImage != null)
            {
                int imageWidth = this.BackgroundImage.Width;
                int imageHeight = this.BackgroundImage.Height;
                this.ClientSize = new Size(imageWidth, imageHeight + buttonSize + 50);
            }
            else
            {
                throw new Exception("A háttérkép nem lett betöltve.");
            }

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;

            _boardButtons = new Button[24];

            int[][] relativePositions =
            [
                new int[] {50, 9},   // 0:Külső top mid
                new int[] {3, 56},   // 1:Külső left mid
                new int[] {97, 56}, // 2:Külső right mid
                new int[] {50, 105}, // 3:Külső bottom mid
                new int[] {3, 9},    // 4:Külső top left
                new int[] {97, 9},  // 5:Külső top right
                new int[] {3, 105},  // 6:Külső bottom left
                new int[] {97, 105},// 7:Külső bottom right

                new int[] {17, 23},  // 8:Középső top left
                new int[] {83, 23},  // 9:Középső top right
                new int[] {17, 89},  // 10:Középső bottom left
                new int[] {83, 89},  // 11:Középső bottom right
                new int[] {50, 23},  // 12:Középső top mid
                new int[] {50, 89},  // 13:Középső bottom mid
                new int[] {17, 56},  // 14:Középső left mid
                new int[] {83, 56},  // 15:Középső right mid

                new int[] {32, 38},  // 16:Belső top left
                new int[] {68, 38},  // 17:Belső top right
                new int[] {32, 74},  // 18:Belső bottom left
                new int[] {68, 74},  // 19:Belső bottom right
                new int[] {50, 38},  // 20:Belső top mid
                new int[] {50, 74},  // 21:Belső bottom mid
                new int[] {32, 56},  // 22:Belső left mid
                new int[] {68, 56}   // 23:Belső right mid
            ];

            int boardHeight = this.BackgroundImage?.Height ?? 1000;

            for (int i = 0; i < 24; i++)
            {
                int x = (int)(relativePositions[i][0] / 100.0 * this.ClientSize.Width) - buttonSize / 2;
                int y = (int)(relativePositions[i][1] / 100.0 * boardHeight) - buttonSize / 2;
                _boardButtons[i] = new Button
                {
                    Location = new Point(x, y),
                    Size = new Size(buttonSize, buttonSize),
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 1, BorderColor = Color.Black },
                    Visible = true,
                    Enabled = true,
                    TabIndex = i
                };
                _boardButtons[i].Click += BoardButton_Click!;
                this.Controls.Add(_boardButtons[i]);
            }

            _model.BoardChanged += OnBoardChanged!;
            _model.MalomFormed += OnMalomFormed!;
            _model.GameOver += OnGameOver!;
            _model.PlayerTurnChanged += OnPlayerTurnChanged!;
            _model.InvalidAction += OnInvalidAction!;

            Button saveButton = new Button { Text = "Mentés", Location = new Point(10, 0), Size = new Size(90, 30) };
            saveButton.Click += SaveButton_Click!;
            Controls.Add(saveButton);

            Button loadButton = new Button { Text = "Betöltés", Location = new Point(100, 0), Size = new Size(90, 30) };
            loadButton.Click += LoadButton_Click!;
            Controls.Add(loadButton);

            Button newGameButton = new Button { Text = "Új játék", Location = new Point(200, 0), Size = new Size(90, 30) };
            newGameButton.Click += NewGameButton_Click!;
            Controls.Add(newGameButton);
        }

        /// <summary>
        /// Játékmező gombjának kattintás eseménye.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoardButton_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            int position = Array.IndexOf(_boardButtons, btn);

            if (_removingPiece)
            {
                if (_model.RemoveOpponentPiece(position))
                {
                    _removingPiece = false;
                }
            }
            else
            {
                _model.MakeMove(position);
            }
        }

        /// <summary>
        /// Játékmező változás eseménye.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBoardChanged(object sender, BoardChangedEventArgs e)
        {
            var btn = _boardButtons[e.Position];
            btn.BackColor = e.Player switch
            {
                Player.Player1 => Color.Red,
                Player.Player2 => Color.Blue,
                _ => Color.White
            };
        }

        /// <summary>
        /// Malom kialakítása eseménye.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMalomFormed(object sender, MalomFormedEventArgs e)
        {
            _removingPiece = true;
            MessageBox.Show($"{e.Player} {(e.Maloms.Any() ? "új malmot alkotott" : "folytatja a leszedést")}! Vegyen le egy ellenfél bábuját (kattintson rá).");
        }

        /// <summary>
        /// Játék vége eseménye.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameOver(object sender, GameOverEventArgs e)
        {
            MessageBox.Show($"{e.Winner} nyert!");
        }

        /// <summary>
        /// Játékos körének változása eseménye.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlayerTurnChanged(object sender, PlayerTurnChangedEventArgs e)
        {
            this.Text = $"Malom - {e.CurrentPlayer} következik (Fázis: {_model.CurrentPhase})";
        }

        /// <summary>
        /// Érvénytelen akció eseménye.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInvalidAction(object sender, InvalidActionEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        /// <summary>
        /// Mentés gomb kattintás eseménye.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new() { Filter ="Text files|*.txt" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _model.SaveGame(sfd.FileName);
                }
                catch (GameDataException ex)
                {
                    MessageBox.Show("Mentési hiba: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Betöltés gomb kattintás eseménye.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadButton_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new() { Filter ="Text files|*.txt" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _model.LoadGame(ofd.FileName);
                }
                catch (GameDataException ex)
                {
                    MessageBox.Show("Betöltési hiba: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Új játék gomb kattintás eseménye.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _model.NewGame();
            for (int i = 0; i < 24; i++)
            {
                _boardButtons[i].BackColor = Color.White;
            }
            this.Text = $"Malom - {_model.CurrentPlayer} következik (Fázis: {_model.CurrentPhase})";
            _removingPiece = false;
        }
    }
}
