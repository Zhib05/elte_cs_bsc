using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ELTE.ImageDownloader.Model;

namespace ELTE.ImageDownloader.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase, IDisposable
{
    private WebPage? _model;
    private static readonly List<string> SupportedExtensions = [".jpg", ".jpeg", ".png", ".gif"];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DownloadButtonLabel))]
    private bool _isDownloading;

    [ObservableProperty]
    private float _progress;

    public string DownloadButtonLabel => IsDownloading ? "Letöltés megszakítása" : "Képek betöltése";

    public ObservableCollection<Bitmap> Images { get; set; }

    public event EventHandler<Bitmap>? ImageSelected;
    public event EventHandler<string>? ErrorOccured;

    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task Download(object param)
    {
        if (!IsDownloading)
        {
            await LoadAsync(new Uri(param?.ToString() ?? string.Empty));
        }
        else
        {
            _model?.CancelLoad();
        }
    }

    [RelayCommand]
    private void ImageSelect(Bitmap param)
    {
        ImageSelected?.Invoke(this, param);
    }

    public MainViewModel()
    {
        Images = new ObservableCollection<Bitmap>();
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