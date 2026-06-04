using Idoutazo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idoutazo.ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        private readonly GameModel _model;
        private FieldVm? _selectedField;

        public ObservableCollection<FieldVm> Fields { get; set; } = new ObservableCollection<FieldVm>();
        //public event EventHandler? BoardChanged;
        public GameViewModel(GameModel model)
        {
            _model = model;
            _model.BoardChanged += (s, e) => RefreshTable();

            //Fields = new ObservableCollection<FieldVm>();

            for (int i = 0; i < 7;  i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    //var field = new FieldVm(_model, i, j);
                    //field.ClickCommand = new DelegateCommand(param => OnFieldClick(param as FieldVm));
                    //Fields.Add(field);
                    Fields.Add(new FieldVm
                    {
                        X = i,
                        Y = j,
                        ClickCommand = new DelegateCommand(param => OnFieldClick(param as FieldVm))
                    });
                }
            }

            RefreshTable();
        }

        private void RefreshTable()
        { 
            foreach (var field in Fields)
            {
                Piece p = _model.Board[field.X, field.Y];
                //if (p.player == Player.None) break;

                field.Text = p.Hp.ToString();
                //field.Refresh();

                if (p.player == Player.Player1) field.Color = "Red";
                else if (p.player == Player.Player2) field.Color = "Blue";
                else field.Color = "White";
            }
        }

        private void OnFieldClick(FieldVm? clickedField)
        {
            if (clickedField == null) return;

            if (_selectedField == null)
            {
                Piece p = _model.Board[clickedField.X, clickedField.Y];
                if ((_model.CurrentPlayer == Player.Player1 && p.player == Player.Player1) || (_model.CurrentPlayer == Player.Player2 && p.player == Player.Player2))
                {
                    _selectedField = clickedField;
                }
            }
            else
            {
                if (_selectedField ==  clickedField)
                {
                    RefreshTable();
                    _selectedField = null;
                }else
                {
                    _model.Step(_selectedField.X, _selectedField.Y, clickedField.X, clickedField.Y);
                    _selectedField = null;
                    RefreshTable();
                }
            }
        }
    }
}
