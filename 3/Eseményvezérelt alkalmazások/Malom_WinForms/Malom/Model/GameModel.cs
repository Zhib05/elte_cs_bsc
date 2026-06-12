using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malom.Persistence;

namespace Malom.Model
{
    public enum Player { None, Player1, Player2 }
    public enum Phase { Placing, Moving }
    public class GameModel
    {
        private Player _currentPlayer = Player.Player1;
        private Phase _currentPhase = Phase.Placing;
        private int _player1PiecesLeftToPlace = 9;
        private int _player2PiecesLeftToPlace = 9;
        private int _player1PiecesOnBoard = 0;
        private int _player2PiecesOnBoard = 0;
        private bool _isRemoving = false;
        private Player[] _board = new Player[24];
        private readonly IGamePersistence _persistence;


        private readonly List<List<int>> Neighbors = new List<List<int>>
        {
            new List<int> {4, 5, 12}, // 0: button1
            new List<int> {4, 14, 6}, // 1: button2
            new List<int> {5, 15, 7}, // 2: button3
            new List<int> {6, 13, 7}, // 3: button4
            new List<int> {1, 0}, // 4: button5
            new List<int> {0, 2}, // 5: button6
            new List<int> {1, 3}, // 6: button7
            new List<int> {2, 3}, // 7: button8

            new List<int> {12, 14}, // 8: button9
            new List<int> {12, 15}, // 9: button10
            new List<int> {13, 14}, // 10: button11
            new List<int> {13, 15}, // 11: button12
            new List<int> {0, 8, 9, 20}, // 12: button13
            new List<int> {3, 10, 11, 21}, // 13: button14
            new List<int> {1, 8, 10, 22}, // 14: button15
            new List<int> {2, 9, 11, 23}, // 15: button16

            new List<int> {20, 22}, // 16: button17
            new List<int> {20, 23}, // 17: button18
            new List<int> {21, 22}, // 18: button19
            new List<int> {21, 23}, // 19: button20
            new List<int> {12, 16, 17}, // 20: button21
            new List<int> {13, 18, 19}, // 21: button22
            new List<int> {14, 16, 18}, // 22: button23
            new List<int> {15, 17, 19} // 23: button24
        };

        private readonly List<List<int>> MalomLines = new List<List<int>>
        {
            new List<int> {4, 0, 5}, new List<int> {4, 1, 6}, new List<int> {6, 3, 7}, new List<int> {7, 2, 5},
            new List<int> {8, 12, 9}, new List<int> {8, 14, 10}, new List<int> {10, 13, 11}, new List<int> {11, 15, 9},
            new List<int> {16, 20, 17}, new List<int> {16, 22, 18}, new List<int> {18, 21, 19}, new List<int> {19, 23, 17},
            new List<int> {0, 12, 20}, new List<int> {1, 14, 22}, new List<int> {3, 13, 21}, new List<int> {2, 15, 23}
        };

        public event EventHandler<BoardChangedEventArgs>? BoardChanged;
        public event EventHandler<MalomFormedEventArgs>? MalomFormed;
        public event EventHandler<GameOverEventArgs>? GameOver;
        public event EventHandler<PlayerTurnChangedEventArgs>? PlayerTurnChanged;
        public event EventHandler<InvalidActionEventArgs>? InvalidAction;

        /// <summary>
        /// Az előzőleg kialakított malmok tárolása a játékosok szerint.
        /// </summary>
        private Dictionary<Player, HashSet<string>> PreviousMaloms { get; } = new Dictionary<Player, HashSet<string>>
        {
            { Player.Player1, new HashSet<string>() },
            { Player.Player2, new HashSet<string>() }
        };

        /// <summary>
        /// Malom játék példányosítása.
        /// </summary>
        /// <param name="persistence"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public GameModel(IGamePersistence persistence)
        {
            _persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
        }

        /// <summary>
        /// A kiválasztott bábu indexe a táblán. Ha nincs kiválasztva bábu, akkor null értékű.
        /// </summary>
        public int? SelectedPieceIndex { get; set; }

        /// <summary>
        /// A játék aktuális játékosa.
        /// </summary>
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                }
            }
        }

        /// <summary>
        /// A játék aktuális fázisa.
        /// </summary>
        public Phase CurrentPhase
        {
            get => _currentPhase;
            set
            {
                if (_currentPhase != value)
                {
                    _currentPhase = value;
                }
            }
        }

        /// <summary>
        /// Az első játékosnak még le nem helyezett bábui.
        /// </summary>
        public int Player1PiecesLeftToPlace
        {
            get => _player1PiecesLeftToPlace;
            private set => _player1PiecesLeftToPlace = value >= 0 ? value : 0;
        }

        /// <summary>
        /// A második játékosnak még le nem helyezett bábui.
        /// </summary>
        public int Player2PiecesLeftToPlace
        {
            get => _player2PiecesLeftToPlace;
            private set => _player2PiecesLeftToPlace = value >= 0 ? value : 0;
        }

        /// <summary>
        /// Az első játékos táblán lévő bábui.
        /// </summary>
        public int Player1PiecesOnBoard
        {
            get => _player1PiecesOnBoard;
            private set => _player1PiecesOnBoard = value >= 0 ? value : 0;
        }

        /// <summary>
        /// A második játékos táblán lévő bábui.
        /// </summary>
        public int Player2PiecesOnBoard
        {
            get => _player2PiecesOnBoard;
            private set => _player2PiecesOnBoard = value >= 0 ? value : 0;
        }

        /// <summary>
        /// Jelzi, hogy a játékos éppen egy bábut szed le az ellenféltől.
        /// </summary>
        public bool IsRemoving
        {
            get => _isRemoving;
            set => _isRemoving = value;
        }

        /// <summary>
        /// A játéktábla állapota.
        /// </summary>
        public Player[] Board
        {
            get => _board;
            private set => _board = value ?? new Player[24];
        }

        /// <summary>
        /// Elindít egy új játékot.
        /// </summary>
        public void NewGame()
        {
            Array.Clear(Board, 0, Board.Length);
            CurrentPlayer = Player.Player1;
            CurrentPhase = Phase.Placing;
            Player1PiecesLeftToPlace = 9;
            Player2PiecesLeftToPlace = 9;
            Player1PiecesOnBoard = 0;
            Player2PiecesOnBoard = 0;
            IsRemoving = false;
            PreviousMaloms[Player.Player1].Clear();
            PreviousMaloms[Player.Player2].Clear();
            for (int i = 0; i < 24; i++)
            {
                BoardChanged?.Invoke(this, new BoardChangedEventArgs(i, Board[i]));
            }
        }

        /// <summary>
        /// Mozgattja a játékost a megadott pozícióra.
        /// </summary>
        /// <param name="position"></param>
        public void MakeMove(int position)
        {
            if (position < 0 || position >= 24)
                return;

            var previousMalom = GetMalomsHash(CurrentPlayer);

            bool valid = false;

            if (CurrentPhase == Phase.Placing)
            {
                if (Board[position] == Player.None)
                {
                    PlacePiece(position);
                    valid = true;
                    CheckPhaseTransition();
                }
            }
            else
            {
                if (SelectedPieceIndex == null)
                {
                    if (Board[position] == CurrentPlayer)
                    {
                        SelectedPieceIndex = position;
                        return;
                    }
                }
                else
                {
                    if (Board[position] == Player.None && IsValidMove(SelectedPieceIndex.Value, position))
                    {
                        MovePiece(position);
                        valid = true;
                    }
                    else
                    {
                        SelectedPieceIndex = null;
                    }
                }
                CheckGameOver();
            }

            if (!valid)
            {
                InvalidAction?.Invoke(this, new InvalidActionEventArgs("Érvénytelen lépés. Próbálja újra."));
                return;
            }

            var newMalom = GetNewMalom(CurrentPlayer, previousMalom);
            if (newMalom.Count != 0)
            {
                IsRemoving = true;
                MalomFormed?.Invoke(this, new MalomFormedEventArgs(CurrentPlayer, newMalom));
                return;
            }

            if (CurrentPhase == Phase.Moving || CurrentPhase == Phase.Placing)
            {
                SwitchPlayer();
            }
        }

        /// <summary>
        /// Leszedi az ellenfél bábuját a megadott pozícióról.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool RemoveOpponentPiece(int position)
        {
            var opponent = CurrentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
            if (Board[position] != opponent)
            {
                InvalidAction?.Invoke(this, new InvalidActionEventArgs("Érvénytelen leszedés: Nem az ellenfél bábuját választotta. Próbálja újra."));
                return false;
            }

            bool inMalom = IsPieceInMalom(position, opponent);
            if (inMalom && !AllOpponentPiecesInMalom(opponent))
            {
                InvalidAction?.Invoke(this, new InvalidActionEventArgs("Érvénytelen leszedés: Nem vehet le malomból, hacsak nem minden ellenfél bábú malomban van. Próbálja újra."));
                return false;
            }

            Board[position] = Player.None;
            if (opponent == Player.Player1) Player1PiecesOnBoard--;
            else Player2PiecesOnBoard--;

            BoardChanged?.Invoke(this, new BoardChangedEventArgs(position, Player.None));

            IsRemoving = false;
            SwitchPlayer();
            CheckPhaseTransition();
            if (CurrentPhase == Phase.Moving) CheckGameOver();
            return true;
        }

        /// <summary>
        /// Lerak egy bábut a megadott pozícióra.
        /// </summary>
        /// <param name="position"></param>
        private void PlacePiece(int position)
        {
            Board[position] = CurrentPlayer;
            if (CurrentPlayer == Player.Player1)
            {
                Player1PiecesLeftToPlace--;
                Player1PiecesOnBoard++;
            }
            else
            {
                Player2PiecesLeftToPlace--;
                Player2PiecesOnBoard++;
            }

            BoardChanged?.Invoke(this, new BoardChangedEventArgs(position, CurrentPlayer));
        }

        /// <summary>
        /// Mozgat egy bábut a megadott pozícióra.
        /// </summary>
        /// <param name="targetPosition"></param>
        private void MovePiece(int targetPosition)
        {
            Board[targetPosition] = CurrentPlayer;
            Board[SelectedPieceIndex!.Value] = Player.None;
            BoardChanged?.Invoke(this, new BoardChangedEventArgs(targetPosition, CurrentPlayer));
            BoardChanged?.Invoke(this, new BoardChangedEventArgs(SelectedPieceIndex.Value, Player.None));

            SelectedPieceIndex = null;
        }

        /// <summary>
        /// Ellenőrzi, hogy a lépés érvényes-e a megadott pozíciókra.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private bool IsValidMove(int from, int to)
        {
            return Neighbors[from].Contains(to);
        }

        /// <summary>
        /// Visszaadja a játékos által kialakított malmok hash értékeit.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private HashSet<string> GetMalomsHash(Player player)
        {
            var maloms = new HashSet<string>();
            foreach (var line in MalomLines)
            {
                if (line.All(pos => Board[pos] == player))
                {
                    maloms.Add(string.Join(",", line));
                }
            }
            return maloms;
        }

        /// <summary>
        /// Visszaadja a játékos által újonnan kialakított malmokat.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        private List<List<int>> GetNewMalom(Player player, HashSet<string> previous)
        {
            var current = GetMalomsHash(player);
            var newHashes = current.Except(previous);
            PreviousMaloms[player] = current;
            return MalomLines.Where(line => newHashes.Contains(string.Join(",", line))).ToList();
        }

        /// <summary>
        /// Ellenőrzi, hogy a megadott pozíción lévő bábu malomban van-e.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private bool IsPieceInMalom(int position, Player player)
        {
            return MalomLines.Any(line => line.Contains(position) && line.All(pos => Board[pos] == player));
        }

        /// <summary>
        /// Ellenőrzi, hogy az ellenfél összes bábuja malomban van-e.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private bool AllOpponentPiecesInMalom(Player player)
        {
            for (int i = 0; i < 24; i++)
            {
                if (Board[i] == player && !IsPieceInMalom(i, player)) return false;
            }
            return true;
        }

        /// <summary>
        /// Játékos váltás.
        /// </summary>
        private void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
            PlayerTurnChanged?.Invoke(this, new PlayerTurnChangedEventArgs(CurrentPlayer));
        }

        /// <summary>
        /// Ellenőrzi a játék fázisának átmenetét.
        /// </summary>
        private void CheckPhaseTransition()
        {
            if (Player1PiecesLeftToPlace == 0 && Player2PiecesLeftToPlace == 0)
            {
                CurrentPhase = Phase.Moving;
            }
        }

        /// <summary>
        /// Ellenőrzi, hogy vége van-e a játéknak.
        /// </summary>
        public void CheckGameOver()
        {
            Player winner = Player.None;
            if (Player1PiecesOnBoard < 3)
            {
                winner = Player.Player2;
            }
            else if (Player2PiecesOnBoard < 3)
            {
                winner = Player.Player1;
            }

            if (winner != Player.None)
            {
                GameOver?.Invoke(this, new GameOverEventArgs(winner));
            }
        }

        /// <summary>
        /// Játék mentése.
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveGame(string fileName)
        {
            int[] boardArray = Array.ConvertAll(Board, p => (int)p);
            _persistence.SaveGame(fileName, boardArray, (int)CurrentPlayer, (int)CurrentPhase,
                Player1PiecesLeftToPlace, Player2PiecesLeftToPlace, Player1PiecesOnBoard,
                Player2PiecesOnBoard, IsRemoving);
        }

        /// <summary>
        /// Játék betöltése.
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadGame(string fileName)
        {
            var (board, currentPlayer, currentPhase, p1Left, p2Left, p1OnBoard, p2OnBoard, isRemoving) =
                _persistence.LoadGame(fileName);
            Board = Array.ConvertAll(board, p => (Player)p);
            CurrentPlayer = (Player)currentPlayer;
            CurrentPhase = (Phase)currentPhase;
            Player1PiecesLeftToPlace = p1Left;
            Player2PiecesLeftToPlace = p2Left;
            Player1PiecesOnBoard = p1OnBoard;
            Player2PiecesOnBoard = p2OnBoard;
            IsRemoving = isRemoving;
            for (int i = 0; i < 24; i++)
            {
                BoardChanged?.Invoke(this, new BoardChangedEventArgs(i, Board[i]));
            }
            PlayerTurnChanged?.Invoke(this, new PlayerTurnChangedEventArgs(CurrentPlayer));
            if (IsRemoving)
            {
                MalomFormed?.Invoke(this, new MalomFormedEventArgs(CurrentPlayer, new List<List<int>>()));
            }
        }
    }
}
