using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vizvezetek.Model;

namespace Vizvezetek.ViewModel
{
    public class PipeFieldViewModel : ViewModelBase
    {
        private readonly PipeItem _model; // Referencia a logikai elemre

        public PipeFieldViewModel(PipeItem model)
        {
            _model = model;

            // ITT A LÉNYEG: Feliratkozunk a Model eseményére!
            // Ha a Modelben lefut a logika, itt frissítjük a nézetet.
            _model.StateChanged += Model_StateChanged;

            // Parancs: ha a nézeten kattintanak, szólunk a Modelnek
            RotateCommand = new DelegateCommand(param => _model.Rotate());
        }

        private void Model_StateChanged(object? sender, EventArgs e)
        {
            // Értesítjük a View-t, hogy frissítse a szöveget
            OnPropertyChanged("TextRepresentation");
        }

        public ICommand RotateCommand { get; private set; }

        // Csak megjelenítési logika (Prezentáció)
        public string TextRepresentation
        {
            get
            {
                if (_model.Type == PipeType.Straight)
                {
                    if (_model.Rotation % 2 == 0) return "V";
                    return "F";
                }
                else
                {
                    int rot = _model.Rotation % 4;
                    if (rot == 0) return "JA";
                    if (rot == 1) return "BA";
                    if (rot == 2) return "BF";
                    return "JF";
                }
            }
        }
    }
}
