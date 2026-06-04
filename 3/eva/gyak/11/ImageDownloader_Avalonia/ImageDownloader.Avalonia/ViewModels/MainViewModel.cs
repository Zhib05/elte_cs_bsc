using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ELTE.ImageDownloader.Model;

namespace ELTE.ImageDownloader.Avalonia.ViewModels;

public class MainViewModel : ViewModelBase, IDisposable
{
    private WebPage? _model;
    private bool _isDownloading;
    private float _progress;
    private static readonly List<string> SupportedExtensions = [".jpg", ".jpeg", ".png", ".gif"];

    public bool IsDownloading
    {
        get => _isDownloading;
        private set
        {
            _isDownloading = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DownloadButtonLabel));
        }
    }

    public string DownloadButtonLabel => _isDownloading ? "Letöltés megszakítása" : "Képek betöltése";

    public float Progress
    {
        get => _progress;
        private set
        {
            _progress = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Bitmap> Images { get; set; }

    public DelegateCommand DownloadCommand { get; set; }
    public DelegateCommand ImageSelectedCommand { get; set; }

    public event EventHandler<Bitmap>? ImageSelected;
    public event EventHandler<string>? ErrorOccured;

    public MainViewModel()
    {
        Images = new ObservableCollection<Bitmap>();

        DownloadCommand = new DelegateCommand(async param =>
        {
            if (!IsDownloading)
            {
                await LoadAsync(new Uri(param?.ToString() ?? string.Empty));
            }
            else
            {
                _model?.CancelLoad();
            }
        });

        ImageSelectedCommand = new DelegateCommand(param =>
        {
            if (param is Bitmap imageSource)
                ImageSelected?.Invoke(this, imageSource);
        });
    }

    public void Dispose()
    {
        _model?.Dispose();
        _model = null;
    }

    private async Task LoadAsync(Uri url)
    {
        IsDownloading = true;
        Images.Clear();
        _model = new WebPage(url);
        _model.ImageLoaded += OnImageLoaded;
        _model.LoadProgress += OnLoadProgress;

        try
        {
            await _model.LoadImagesAsync();
        }
        catch (HttpRequestException e)
        {
            ErrorOccured?.Invoke(this, e.Message);
        }

        IsDownloading = false;
    }

    private void OnLoadProgress(object? sender, int e)
    {
        Progress = e;
    }

    private void OnImageLoaded(object? sender, WebImage webImage)
    {
        if (!IsSupportedExtension(webImage.Url.LocalPath))
            return;

        var bitmap = new Bitmap(new MemoryStream(webImage.Data));
        Images.Add(bitmap);
    }

    private bool IsSupportedExtension(string path)
    {
        return SupportedExtensions.Contains(Path.GetExtension(path));
    }
}