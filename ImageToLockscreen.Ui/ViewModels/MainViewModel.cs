using ImageToLockscreen.Ui.Controls;
using ImageToLockscreen.Ui.Core;
using ImageToLockscreen.Ui.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ImageToLockscreen.Ui.ViewModels
{
    public class MainViewModel : PropertyObservable
    {
        #region Private Members
        private static string _fileDialogTitle = "Please select image";
        private readonly static string _fileBrowserFilter = "Images (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|jpg files (*.jpg;*.jpeg)|*.jpg;*.jpeg|png files (*.png)|*.png";
        private OpenFileDialog _fileBrowserDialog;
        private ObservableCollection<DisplayWithValue> _backgroundFillImageOptions = new ObservableCollection<DisplayWithValue>(new List<DisplayWithValue>()
        {
            new DisplayWithValue("Self", BackgroundFillImageOption.Self, true),
            new DisplayWithValue("Browse", BackgroundFillImageOption.Browse, true),
            new DisplayWithValue("", BackgroundFillImageOption.Url, false)
        });
        #endregion Private Members


        public MainViewModel()
        {
            this._fileBrowserDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Title = MainViewModel._fileDialogTitle,
                Filter = MainViewModel._fileBrowserFilter
            };
            this.SetRatios();

            this.BackgroundFillImageOptionSelectionChangedCommand = new RelayCommand(BackgroundFillImageOptionSelectionChanged);
            this.ConvertImagesCommand = new RelayCommand(this.ConvertImages);
        }


        #region Public Properties
        public string InputDirectory
        {
            get { return base.GetProperty<string>(); }
            set { base.SetProperty(value); }
        }
        public string OutputDirectory
        {
            get { return base.GetProperty<string>(); }
            set { base.SetProperty(value); }
        }
        public DisplayWithValue SelectedBackgroundFillOption
        {
            get { return base.GetProperty<DisplayWithValue>(); }
            set { base.SetProperty(value); }
        }
        public bool IsSelectedBackgroundFillOptionUrl 
        {
            get { return base.GetProperty<bool>(getDefault:() => false); }
            set { base.SetProperty(value); }
        }
        public bool IsBlurBackgroundImage
        {
            get { return base.GetProperty<bool>(getDefault: () => false); }
            set { base.SetProperty(value); }
        }
        public bool IsBackgroundFillSolidColor
        {
            get { return base.GetProperty<bool>(getDefault: () => true); }
            set { base.SetProperty(value); }
        }
        public SolidColorBrush BackgroundFillColor
        {
            get { return base.GetProperty<SolidColorBrush>(getDefault: () => Brushes.Black); }
            set { base.SetProperty(value); }
        }

        public Size CurrentScreenResolution
        {
            get { return base.GetProperty<Size>(); }
            set 
            { 
                base.SetProperty(value);

                this.SelectedAspectRatio = this.SlideViewerItems.FirstOrDefault(i => ((AspectRatio)i.Value).Resolutions.Any(r => r == this.CurrentScreenResolution));
                if (this.SelectedAspectRatio == null)
                    this.SelectedAspectRatio = this.SlideViewerItems.FirstOrDefault(i => ((AspectRatio)i.Value).Height/ ((AspectRatio)i.Value).Width == this.CurrentScreenResolution.Height/ this.CurrentScreenResolution.Width);
            }
        }

        public SlideViewerItem SelectedAspectRatio
        {
            get { return base.GetProperty<SlideViewerItem>(); }
            set { base.SetProperty(value); }
        }

        public ObservableCollection<DisplayWithValue> BackgroundFillImageOptions
        {
            get
            {
                return this._backgroundFillImageOptions;
            }
            set
            {
                this._backgroundFillImageOptions = value;
                base.OnPropertyChanged(nameof(BackgroundFillImageOptions));
            }
        }

        public ObservableCollection<SlideViewerItem> SlideViewerItems
        {
            get { return base.GetProperty<ObservableCollection<SlideViewerItem>>(getDefault: () => { return new ObservableCollection<SlideViewerItem>(); }); }
            set { base.SetProperty(value); }
        }
        #endregion Public Properties


        #region Public ICommand
        public ICommand BackgroundFillImageOptionSelectionChangedCommand { get; set; }
        public ICommand ConvertImagesCommand { get; set; }
        #endregion Public ICommand


        #region Private ICommand
        private void BackgroundFillImageOptionSelectionChanged()
        {
            var option = this._backgroundFillImageOptions.First(o => o.Value == BackgroundFillImageOption.Url);
            
            if (this.SelectedBackgroundFillOption.Value == BackgroundFillImageOption.Browse
                && this._fileBrowserDialog.ShowDialog() == true)
            {
                option.Display = System.IO.Path.GetFileName(this._fileBrowserDialog.FileName);
                this.IsSelectedBackgroundFillOptionUrl = true;
                this.SelectedBackgroundFillOption = option;
                this.SelectedBackgroundFillOption.IsVisible = true;
            }
            else
            {
                this.IsSelectedBackgroundFillOptionUrl = false;
                option.IsVisible = false;
                option.Display = string.Empty;
                this.SelectedBackgroundFillOption = this._backgroundFillImageOptions.First(o => o.Value == BackgroundFillImageOption.Self);
            }
        }
        private void ConvertImages()
        {
            System.Console.WriteLine("Converting images");
        }
        #endregion Private ICommand


        private void SetRatios()
        {
            foreach (var ratio in CommonAspectRatios.CommonRatios)
            {
                SlideViewerItem aspectRatioItem = new Controls.SlideViewerItem()
                {
                    Value = ratio,
                    Text = $"{ratio.Description} ({ratio})",
                    Image = ratio.Image,
                    Tooltip = string.Join(" ", ratio.Resolutions.Select(r => $"[{r.Width} × {r.Height}px]")),
                };
                this.SlideViewerItems.Add(aspectRatioItem);

                if (ratio.Resolutions.Any(r => r == this.CurrentScreenResolution || (ratio.Height / ratio.Width) == (r.Height / r.Width)))
                    this.SelectedAspectRatio = aspectRatioItem;
            }
        }
    }

    public enum BackgroundFillType
    {
        SolidColor,
        Image
    }

    public enum BackgroundFillImageOption
    {
        Self,
        Browse,
        Url
    }
}
