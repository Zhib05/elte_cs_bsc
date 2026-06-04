using Idoutazo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Idoutazo.ViewModel
{
    public class FieldVm : ViewModelBase
    {
        //private readonly GameModel _model;
        private string _text = "";
        private string _color = "White";

        public int X { get; set; }
        public int Y { get; set; }
        public DelegateCommand? ClickCommand { get; set; }


        public string Color
        {
            get => _color;
            set { _color = value; OnPropertyChanged(); }
        }

        public string Text
        {
            get => _text;
            set { _text = value; OnPropertyChanged(); }
        }

        //public string BackGround => _model.Board[X, Y].player switch
        //{
        //    Player.Player1 => "Red",
        //    Player.Player2 => "Blue",
        //    _ => "White"
        //};

        //public FieldVm(GameModel model, int row, int col)
        //{
        //    _model = model;
        //    X = row;
        //    Y = col;
        //}
        //public void Refresh()
        //{
        //    OnPropertyChanged(nameof(BackGround));
        //}
    }
}
