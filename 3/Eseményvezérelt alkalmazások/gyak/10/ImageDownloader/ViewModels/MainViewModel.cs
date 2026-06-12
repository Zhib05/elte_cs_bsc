using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace ELTE.ImageDownloader.ViewModel
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private WebPage? _model;
        private bool _isDownloading;
        private float _progress;

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

        public ObservableCollection<Bitmap> Images { get; set; }

        public DelegateCommand DownloadCommand { get; set; }
        public DelegateCommand ImageSelectedCommand { get; set; }

        public event EventHandler<Bitmap>? ImageSelected;

        public MainViewModel()
        {
            Images = new ObservableCollection<Bitmap>();

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
                if (param is Bitmap bitmap)
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
            var Bitmap = new Bitmap();
            Bitmap.BeginInit();
            Bitmap.StreamSource = new MemoryStream(e.Data);
            Bitmap.EndInit();

            Images.Add(Bitmap);
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
