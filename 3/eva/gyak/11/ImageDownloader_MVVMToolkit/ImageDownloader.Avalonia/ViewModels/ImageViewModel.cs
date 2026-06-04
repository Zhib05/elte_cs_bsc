using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using System;

namespace ELTE.ImageDownloader.Avalonia.ViewModels;

public class ImageViewModel : ViewModelBase
{
    public Bitmap? Image { get; set; }

    public RelayCommand SaveImageCommand { get; private set; }
    public RelayCommand CloseCommand { get; private set; }

    public event EventHandler<Bitmap>? SaveImage;

    public event EventHandler? Close;

    public ImageViewModel(Bitmap image)
    {
        Image = image;

        SaveImageCommand = new RelayCommand(() =>
        {
            SaveImage?.Invoke(this, Image);
        });

        CloseCommand = new RelayCommand(() =>
        {
            Close?.Invoke(this, EventArgs.Empty);
        });
    }
}