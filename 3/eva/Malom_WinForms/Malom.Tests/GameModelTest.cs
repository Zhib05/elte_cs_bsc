using System;
using System.Collections.Generic;
using Malom.Model;
using Malom.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Malom.Test
{
    [TestClass]
    public class GameModelTest
    {
        private GameModel _model = null!;
        private Mock<IGamePersistence> _mock = null!;
        private int _boardChangedCount;
        private int _malomFormedCount;
        private int _gameOverCount;
        private int _playerTurnChangedCount;
        private int _invalidActionCount;

        private int[] _mockedBoard = new int[24];
        private int _mockedCurrentPlayer = (int)Player.Player1;
        private int _mockedCurrentPhase = (int)Phase.Placing;
        private int _mockedP1PiecesLeft = 9;
        private int _mockedP2PiecesLeft = 9;
        private int _mockedP1OnBoard = 0;
        private int _mockedP2OnBoard = 0;
        private bool _mockedIsRemoving = false;

        [TestInitialize]
        public void Initialize()
        {
            _mock = new Mock<IGamePersistence>();
            _mock.Setup(mock => mock.LoadGame(It.IsAny<string>()))
            .Returns((_mockedBoard, _mockedCurrentPlayer, _mockedCurrentPhase, _mockedP1PiecesLeft, _mockedP2PiecesLeft, _mockedP1OnBoard, _mockedP2OnBoard, _mockedIsRemoving));
            _model = new GameModel(_mock.Object);

            _boardChangedCount = 0;
            _malomFormedCount = 0;
            _gameOverCount = 0;
            _playerTurnChangedCount = 0;
            _invalidActionCount = 0;

            _model.BoardChanged += (sender, e) => _boardChangedCount++;
            _model.MalomFormed += (sender, e) => _malomFormedCount++;
            _model.GameOver += (sender, e) => _gameOverCount++;
            _model.PlayerTurnChanged += (sender, e) => _playerTurnChangedCount++;
            _model.InvalidAction += (sender, e) => _invalidActionCount++;
        }

        [TestMethod]
        public void NewGameInitializationTest()
        {
            _model.NewGame();

            Assert.AreEqual(Player.Player1, _model.CurrentPlayer);
            Assert.AreEqual(Phase.Placing, _model.CurrentPhase);
            Assert.AreEqual(9, _model.Player1PiecesLeftToPlace);
            Assert.AreEqual(9, _model.Player2PiecesLeftToPlace);
            Assert.AreEqual(0, _model.Player1PiecesOnBoard);
            Assert.AreEqual(0, _model.Player2PiecesOnBoard);
            Assert.IsFalse(_model.IsRemoving);

            for (int i = 0; i < 24; i++)
            {
                Assert.AreEqual(Player.None, _model.Board[i]);
            }

            Assert.AreEqual(0, _playerTurnChangedCount);
        }

        [TestMethod]
        public void PlacingPhaseTest()
        {
            // Érvényes: Player1 helyez
            _model.MakeMove(0);
            Assert.AreEqual(Player.Player1, _model.Board[0]);
            Assert.AreEqual(8, _model.Player1PiecesLeftToPlace);
            Assert.AreEqual(1, _model.Player1PiecesOnBoard);
            Assert.AreEqual(Player.Player2, _model.CurrentPlayer);
            Assert.AreEqual(1, _boardChangedCount);
            Assert.AreEqual(1, _playerTurnChangedCount);

            // Érvénytelen: foglalt hely
            _model.MakeMove(0);
            Assert.AreEqual(1, _invalidActionCount);

            // Érvényes: Player2 helyez
            _model.MakeMove(1);
            Assert.AreEqual(Player.Player2, _model.Board[1]);
            Assert.AreEqual(8, _model.Player2PiecesLeftToPlace);
            Assert.AreEqual(1, _model.Player2PiecesOnBoard);
            Assert.AreEqual(Player.Player1, _model.CurrentPlayer);
            Assert.AreEqual(2, _boardChangedCount);
            Assert.AreEqual(2, _playerTurnChangedCount);
        }

        [TestMethod]
        public void MalomFormationAndRemovalTest()
        {
            // Helyezés
            _model.MakeMove(0); // Player1
            _model.MakeMove(1); // Player2
            _model.MakeMove(4); // Player1
            _model.MakeMove(14); // Player2
            _model.MakeMove(5); // Player1 -> Malom

            Assert.AreEqual(1, _malomFormedCount);
            Assert.IsTrue(_model.IsRemoving);

            // Érvénytelen eltávolítás: saját bábu eltávolítása
            bool invalidRemoveOwn = _model.RemoveOpponentPiece(0);
            Assert.IsFalse(invalidRemoveOwn);
            Assert.AreEqual(1, _invalidActionCount);
            Assert.IsTrue(_model.IsRemoving);

            // Érvénytelen eltávolítás: nem ellenfél bábú (üres hely)
            bool invalidRemoveNone = _model.RemoveOpponentPiece(3);
            Assert.IsFalse(invalidRemoveNone);
            Assert.AreEqual(2, _invalidActionCount);

            // Érvényes eltávolítás: Player1 eltávolít Player2 bábút
            bool removed = _model.RemoveOpponentPiece(1);
            Assert.IsTrue(removed);
            Assert.AreEqual(Player.None, _model.Board[1]);
            Assert.AreEqual(1, _model.Player2PiecesOnBoard);
            Assert.IsFalse(_model.IsRemoving);
            Assert.AreEqual(Player.Player2, _model.CurrentPlayer);
            Assert.AreEqual(6, _boardChangedCount);
            Assert.AreEqual(5, _playerTurnChangedCount);

            // Érvénytelen eltávolítás: malomban lévő bábu eltávolítása
            _model.MakeMove(1); // Player2 helyez
            _model.MakeMove(3); // Player1 helyez
            _model.MakeMove(22); // Player2 helyez -> Malom
            Assert.AreEqual(2, _malomFormedCount);
            bool invalidRemoveMalom = _model.RemoveOpponentPiece(0);
            Assert.IsFalse(invalidRemoveMalom);
            Assert.AreEqual(3, _invalidActionCount);
        }

        [TestMethod]
        public void MovingPhaseTest()
        {
            _model.MakeMove(0); // Player1
            _model.MakeMove(12); // Player2
            _model.MakeMove(4); // Player1
            _model.MakeMove(8); // Player2
            _model.MakeMove(2); // Player1
            _model.MakeMove(15); // Player2
            _model.MakeMove(3); // Player1
            _model.MakeMove(13); // Player2

            _model.CurrentPhase = Phase.Moving;
            Assert.AreEqual(Phase.Moving, _model.CurrentPhase);
            Assert.AreEqual(Player.Player1, _model.CurrentPlayer);
            
            // Érvényes mozgás
            _model.MakeMove(3); // Kiválasztás (Player1)
            _model.MakeMove(6); // Mozgatás (Player1)
            Assert.AreEqual(Player.None, _model.Board[3]);
            Assert.AreEqual(Player.Player1, _model.Board[6]);
            Assert.AreEqual(Player.Player2, _model.CurrentPlayer);

            // Érvénytelen mozgás: nem szomszéd
            _model.MakeMove(12); // Kiválasztás (Player2)
            _model.MakeMove(3); // Mozgatás (Player2)
            Assert.AreEqual(1, _invalidActionCount);
            Assert.AreEqual(Player.Player2, _model.CurrentPlayer);

            // Érvénytelen mozgás: szomszédos hely foglalt
            _model.MakeMove(12); // Kiválasztás (Player2)
            _model.MakeMove(8); // Mozgatás (Player2)
            Assert.AreEqual(2, _invalidActionCount);
            Assert.AreEqual(Player.Player2, _model.CurrentPlayer);
        }

        [TestMethod]
        public void GameOverConditionsTest()
        {
            _model.MakeMove(0); // Player1
            _model.MakeMove(12); // Player2
            _model.MakeMove(4); // Player1
            _model.MakeMove(8); // Player2
            _model.MakeMove(2); // Player1
            _model.MakeMove(14); // Player2
            _model.MakeMove(5); // Player1
            Assert.AreEqual(Phase.Placing, _model.CurrentPhase);
            _model.CurrentPhase = Phase.Moving;
            Assert.AreEqual(Phase.Moving, _model.CurrentPhase);

            Assert.AreEqual(0, _gameOverCount);
            Assert.AreEqual(Player.Player1, _model.CurrentPlayer);
            Assert.IsTrue(_model.IsRemoving);
            _model.RemoveOpponentPiece(12);
            Assert.AreEqual(1, _gameOverCount);
        }

        [TestMethod]
        public void LoadSaveTest()
        {
            _model.SaveGame("test.txt");
            _mock.Verify(p => p.SaveGame("test.txt", It.IsAny<int[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Once());

            _model.LoadGame("test.txt");
            _mock.Verify(p => p.LoadGame("test.txt"), Times.Once());

            Assert.AreEqual(Player.Player1, _model.CurrentPlayer);
            Assert.AreEqual(Phase.Placing, _model.CurrentPhase);
            Assert.AreEqual(9, _model.Player1PiecesLeftToPlace);
            Assert.AreEqual(9, _model.Player2PiecesLeftToPlace);
            Assert.AreEqual(0, _model.Player1PiecesOnBoard);
            Assert.AreEqual(0, _model.Player2PiecesOnBoard);
            Assert.AreEqual(24, _boardChangedCount);
        }
    }
}