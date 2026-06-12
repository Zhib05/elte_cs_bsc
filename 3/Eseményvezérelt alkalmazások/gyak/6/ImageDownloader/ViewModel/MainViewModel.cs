using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ELTE.ImageDownloader.Model;

namespace ELTE.ImageDownloader.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private WebPage? _model;
        private bool _isDownloading;
        private float _progress;

        public bool IsDownloading
        {
            get { return _isDownloading; }
            private set
            {
                _isDownloading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DownloadButtonLabel));
            }
        }

        public string DownloadButtonLabel
        {
            get => _isDownloading ? "Letöltés megszakítása" : "Képek betöltése";
        }

        public float Progress
        {
            get => _progress;
            private set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BitmapImage> Images { get; set; }

        public DelegateCommand DownloadCommand { get; set; }
        public DelegateCommand ImageSelectedCommand { get; set; }

        public event EventHandler<BitmapImage>? ImageSelected;

        public MainViewModel()
        {
            Images = new ObservableCollection<BitmapImage>();

            DownloadCommand = new DelegateCommand(async param =>
            {
                if (!_isDownloading)
                {
                    await LoadAsync(new Uri(param?.ToString() ?? string.Empty));
                }
                else
                {
                    CancelLoad();
                }
            });

            ImageSelectedCommand = new DelegateCommand(param =>
            {
                if (param is BitmapImage bitmap)
                    ImageSelected?.Invoke(this, bitmap);
            });
        }

        public void Dispose()
        {
            _model?.Dispose();
            _model = null;
        }

        public async Task LoadAsync(Uri url)
        {
            IsDownloading = true;
            Images.Clear();

            _model = new WebPage(url);
            _model.ImageLoaded += OnImageLoaded;
            _model.LoadProgress += OnLoadProgress;
            await _model.LoadImagesAsync();

            IsDownloading = false;
        }

        private void OnImageLoaded(object? sender, WebImage e)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(e.Data);
            bitmapImage.EndInit();

            Images.Add(bitmapImage);
        }

        private void OnLoadProgress(object? sender, int e)
        {
            Progress = e;
        }

        private void CancelLoad()
        {
            if (IsDownloading)
            {
                _model?.CancelLoad();
            }
        }
    }
}
