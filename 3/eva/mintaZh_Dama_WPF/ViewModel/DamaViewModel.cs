using System;
using System.Collections.ObjectModel;
using mintaZh_Dama_WPF.Model;

namespace mintaZh_Dama_WPF.ViewModel
{
    public class DamaViewModel : ViewModelBase
    {
        private readonly DamaModel _model;
        // Itt tároljuk, melyik mező van épp kijelölve
        private DamaField? _selectedField;

        public ObservableCollection<DamaField> Fields { get; set; }

        private string _status = "Következik: Fehér";
        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public DamaViewModel(DamaModel model)
        {
            _model = model;
            Fields = new ObservableCollection<DamaField>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Fields.Add(new DamaField
                    {
                        X = i,
                        Y = j,
                        // Minden mezőnek saját parancsa van, ami meghívja az OnFieldClick-et
                        ClickCommand = new DelegateCommand(param => OnFieldClick(param as DamaField))
                    });
                }
            }
            RefreshTable(); // Kezdőállapot kirajzolása
        }

        private void RefreshTable()
        {
            foreach (var field in Fields)
            {
                Piece p = _model.Board[field.X, field.Y];
                field.Text = p == Piece.None ? "" : "O"; // Ha van bábu, kiírjuk

                // Színek beállítása
                if (p == Piece.White) field.Color = "White";
                else if (p == Piece.Black) field.Color = "Black";
                else field.Color = "Transparent";
            }
            // Kiírjuk, ki következik
            Status = $"Következik: {(_model.CurrentPlayer == Player.White ? "Fehér" : "Fekete")}";
        }

        private void OnFieldClick(DamaField? clickedField)
        {
            if (clickedField == null) return;

            // 1. ÁLLAPOT: Még nincs kijelölve semmi -> Kijelölés
            if (_selectedField == null)
            {
                Piece p = _model.Board[clickedField.X, clickedField.Y];
                // Csak akkor jelöljük ki, ha a játékos a saját bábujára kattintott
                if ((_model.CurrentPlayer == Player.White && p == Piece.White) ||
                    (_model.CurrentPlayer == Player.Black && p == Piece.Black))
                {
                    _selectedField = clickedField;
                    _selectedField.Color = "Red"; // Pirosra színezzük
                }
            }
            // 2. ÁLLAPOT: Már van kijelölt bábu -> Lépés próba
            else
            {
                // Ha ugyanarra kattintunk megint, visszavonjuk a kijelölést
                if (_selectedField == clickedField)
                {
                    RefreshTable(); // Visszaállítjuk a színeket
                    _selectedField = null;
                }
                else
                {
                    // Megpróbáljuk a lépést a Modelben
                    // Ha szabálytalan, a Model eseményt küld az App-nak (hibaüzenet)
                    _model.Step(_selectedField.X, _selectedField.Y, clickedField.X, clickedField.Y);

                    // Akár sikerült, akár nem, töröljük a kijelölést és frissítjük a táblát
                    _selectedField = null;
                    RefreshTable();
                }
            }
        }
    }
}