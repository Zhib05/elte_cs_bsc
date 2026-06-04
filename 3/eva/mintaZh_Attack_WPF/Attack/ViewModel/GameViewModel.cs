using Attack.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attack.ViewModel
{
    public partial class GameViewModel : ObservableObject
    {
        private GameModel _model;

        public ObservableCollection<CellViewModel> Cells { get; set; }

        public int Size => _model.Size;

        public FigureData CurrentFigure => _model.CurrentFigure;

        public GameViewModel(GameModel model)
        {
            _model = model;
            _model.FigureChanged += OnFigureChanged;

            Cells = new ObservableCollection<CellViewModel>();
            for (int row = 0; row < _model.Size; row++)
            {
                for (int col = 0; col < _model.Size; col++)
                {
                    var data = _model.GetByPosition(row, col);
                    // Létrehozzuk a mező ViewModel-jét (ez egyetlen gomb adatait tárolja)
                    var cell = new CellViewModel()
                    {
                        Position = new Position { Row = row, Col = col }
                    };

                    if (data != null)
                    {
                        cell.Owner = data.Value.Owner;
                        cell.Text = data.Value.Id.ToString();
                    }

                    //var rowCopy = row;
                    //var colCopy = col;

                    cell.ClickCommand = new RelayCommand(() =>
                    {
                        _model.Move(cell.Position.Row, cell.Position.Col);
                    });

                    Cells.Add(cell);
                }
            }
        }

        private void OnFigureChanged(object? sender, FigureChangedEventArgs e)
        {
            // 1. A RÉGI helyről levesszük a bábut.
            // Matek: Mivel a lista egydimenziós (0..35), de a tábla kétdimenziós (sor, oszlop),
            // így számoljuk ki az indexet: (sor * szélesség + oszlop)
            var fromCell = Cells[e.OldPosition.Row * _model.Size + e.OldPosition.Col];
            fromCell.Owner = null;
            fromCell.Text = string.Empty;

            var toCell = Cells[e.NewPosition.Row * _model.Size + e.NewPosition.Col];
            toCell.Owner = e.Data.Owner;
            toCell.Text = e.Data.Id.ToString();

            OnPropertyChanged(nameof(CurrentFigure));
        }
    }
}
