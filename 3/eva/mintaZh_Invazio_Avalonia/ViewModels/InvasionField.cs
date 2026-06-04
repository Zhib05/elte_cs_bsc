using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;

namespace mintaZh_Invazio_Avalonia.ViewModels
{
    // Egyetlen mező (négyzet) a rácson
    public class InvasionField : ViewModelBase
    {
        private string _color = "White";
        private string _text = "";

        public int X { get; set; }
        public int Y { get; set; }

        public string Color
        {
            get => _color;
            set { if (_color != value) { _color = value; OnPropertyChanged(); } }
        }

        public string Text
        {
            get => _text;
            set { if (_text != value) { _text = value; OnPropertyChanged(); } }
        }

        // Parancs, ha rákattintanak (katona lerakása)
        public RelayCommand? ClickCommand { get; set; }
    }
}
