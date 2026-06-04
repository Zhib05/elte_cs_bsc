using Avalonia.Controls.Shapes;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using mintaZh_Invazio_Avalonia.Model;
using System.Collections.ObjectModel;
using System;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;

namespace mintaZh_Invazio_Avalonia.ViewModels;

public class InvasionViewModel : ViewModelBase
{
    private readonly InvasionGameModel _model;

    // A rács mezői
    public ObservableCollection<InvasionField> Fields { get; private set; }

    // Státusz szövegek
    public string StatusText =>
        $"Élet: {_model.CastleLives} | Idő: {_model.TimeElapsed} mp | Katona: {_model.AvailableSoldiers} db";

    public InvasionViewModel(InvasionGameModel model)
    {
        _model = model;

        // Események feliratkozása
        _model.GameAdvanced += (s, e) => UpdateView();
        _model.GameOver += (s, e) =>
        {
            UpdateView();
            // Itt jelezhetnénk a végén (pl. szöveg átírása), de a státuszsorban is látszik majd
        };

        // Rács inicializálása
        Fields = new ObservableCollection<InvasionField>();
        for (int i = 0; i < InvasionGameModel.Rows; i++)
        {
            for (int j = 0; j < InvasionGameModel.Columns; j++)
            {
                int x = i; int y = j; // Helyi másolat a lambda miatt
                Fields.Add(new InvasionField
                {
                    X = x,
                    Y = y,
                    ClickCommand = new RelayCommand(() => _model.PlaceSoldier(x, y))
                });
            }
        }

        _model.NewGame();
        UpdateView();
    }

    private void UpdateView()
    {
        // Mivel az időzítő más szálon fut, vissza kell térnünk a UI szálra
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            // Státuszsor frissítése
            OnPropertyChanged(nameof(StatusText));

            // Minden mező színének és szövegének frissítése a modell alapján
            foreach (var field in Fields)
            {
                var type = _model.GetFieldValue(field.X, field.Y);
                switch (type)
                {
                    case Fieldtype.Wall:
                        field.Color = "Gray";
                        field.Text = "FAL";
                        break;
                    case Fieldtype.Enemy:
                        field.Color = "Red";
                        field.Text = "O"; // Ellenség jele
                        break;
                    case Fieldtype.Soldier:
                        field.Color = "Blue";
                        field.Text = "X"; // Katona jele
                        break;
                    default:
                        field.Color = "White";
                        field.Text = "";
                        break;
                }
            }
        });
    }
}