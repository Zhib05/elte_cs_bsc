using Attack.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attack.ViewModel
{
    public partial class CellViewModel : ObservableObject
    {
        [ObservableProperty]
        private Player? _owner;

        [ObservableProperty]
        private string _text = string.Empty;

        public Position Position { get; init; }

        public RelayCommand? ClickCommand { get; set; }
    }
}
