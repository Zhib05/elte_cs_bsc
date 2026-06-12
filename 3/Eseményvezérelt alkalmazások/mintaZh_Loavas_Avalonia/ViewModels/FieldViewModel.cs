using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using mintaZh_Loavas_Avalonia.Model;


namespace mintaZh_Loavas_Avalonia.ViewModels
{
    public class FieldViewModel : ViewModelBase
    {
        private readonly GameModel _model;
        private string _text = "";
        public string Text
        {
            get => _text;
            set { if (_text != value) { _text = value; OnPropertyChanged(); } }
        }

        public int Row { get; set; }
        public int Col { get; set; }

        public FieldViewModel(GameModel model, int row, int col)
        {
            _model = model;
            Row = row;
            Col = col;

        }

        public string BackGround => _model.Board[Row, Col] switch
        {
            GameModel.Player.Player1 => "White",
            GameModel.Player.Player2 => "Black",
            _ => "Gray"
        };

        public RelayCommand? ClickCommand { get; set; }

        public void Refresh()
        {
            OnPropertyChanged(nameof(BackGround));
        }
    }
}
