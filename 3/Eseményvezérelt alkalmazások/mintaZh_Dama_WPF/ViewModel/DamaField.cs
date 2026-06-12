using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mintaZh_Dama_WPF.ViewModel
{
    // ViewModelBase-ből származik, hogy a színe frissülhessen a felületen
    public class DamaField : ViewModelBase
    {
        private string _text = "";
        private string _color = "Transparent";

        public int X { get; set; }
        public int Y { get; set; }
        public DelegateCommand? ClickCommand { get; set; }

        // A bábu jele ("O" vagy üres)
        public string Text
        {
            get => _text;
            set { _text = value; OnPropertyChanged(); }
        }

        // Ez jelzi a kiválasztást (pl. piros keret vagy háttér)
        public string Color
        {
            get => _color;
            set { _color = value; OnPropertyChanged(); }
        }
    }
}
