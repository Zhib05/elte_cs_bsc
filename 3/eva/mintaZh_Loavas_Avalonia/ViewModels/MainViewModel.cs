using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using mintaZh_Loavas_Avalonia.Model;

namespace mintaZh_Loavas_Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly GameModel _model;

    public RelayCommand NewGameCommand { get; private set; }

    public ObservableCollection<FieldViewModel> Fields { get; }

    public event EventHandler? NewGame;

    public MainViewModel(GameModel model)
    {
        _model = model;
        Fields = new ObservableCollection<FieldViewModel>();
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                var fieldVM = new FieldViewModel(_model, row, col);
                fieldVM.ClickCommand = new RelayCommand(() => OnFieldClicked(fieldVM));
                Fields.Add(fieldVM);
            }
        }
        NewGameCommand = new RelayCommand(OnNewGame);
        _model.NewGame();
    }

    private void OnFieldClicked(FieldViewModel fieldVM)
    {
        _model.MakeMove(_model.CurrentPlayer == GameModel.Player.Player1
            ? _model.CurrentPlayer1Position.Item1
            : _model.CurrentPlayer2Position.Item1,
            _model.CurrentPlayer == GameModel.Player.Player1
            ? _model.CurrentPlayer1Position.Item2
            : _model.CurrentPlayer2Position.Item2,
            fieldVM.Row,
            fieldVM.Col);
        foreach (var field in Fields)
        {
            field.Refresh();
        }
    }

    private void OnNewGame()
    {
        _model.NewGame();
        foreach (var field in Fields)
        {
            field.Refresh();
        }
        NewGame?.Invoke(this, EventArgs.Empty);
    }

}
