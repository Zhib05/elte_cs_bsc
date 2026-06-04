using CommunityToolkit.Mvvm.Input;
using SameGame.Models;
using System;


namespace SameGame.ViewModels
{
    public class FieldVM : ViewModelBase
    {
        private readonly GameModel _model;

        public int Row;
        public int Col;

        public FieldVM (GameModel model, int row, int col)
        {
            _model = model;
            Row = row;
            Col = col;
        }

        public string BackGround => _model.Board[Row, Col] switch
        {
            Color.Red => "Red",
            Color.Blue => "Blue",
            Color.Gray => "Gray",
            Color.Pink => "Pink",
            Color.Green => "Green",
            Color.Yellow => "Yellow",
            Color.Orange => "Orange",
            _ => "White"
        };

        public void Refresh()
        {
            OnPropertyChanged(nameof(BackGround));
        }

        public RelayCommand? ClickCommand { get; set; }
    }
}