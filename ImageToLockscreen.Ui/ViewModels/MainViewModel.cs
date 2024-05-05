using ImageToLockscreen.Ui.Controls;
using ImageToLockscreen.Ui.Core;
using ImageToLockscreen.Ui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ImageToLockscreen.Ui.ViewModels
{
    public class MainViewModel : PropertyObservable
    {
        private static string _fileDialogTitle = "Please select folder";
        private readonly static string _fileBrowserFilter = "Images (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|jpg files (*.jpg;*.jpeg)|*.jpg;*.jpeg|png files (*.png)|*.png";
        private OpenFileDialog _fileBrowserDialog;
        private ObservableCollection<DisplayWithValue> _backgroundFillImageOptions = new ObservableCollection<DisplayWithValue>(new List<DisplayWithValue>()
        {
            new DisplayWithValue("Self", BackgroundFillImageOption.Self, true),
            new DisplayWithValue("Browse", BackgroundFillImageOption.Browse, true),
            new DisplayWithValue("", BackgroundFillImageOption.Url, false)
        });

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
        }

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

        public ICommand BackgroundFillImageOptionSelectionChangedCommand { get; set; }

        private void BackgroundFillImageOptionSelectionChanged()
        {
            var option = _backgroundFillImageOptions.First(o => o.Value == BackgroundFillImageOption.Url);
            
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
            }
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

        private void SetRatios()
        {
            foreach (var ratio in CommonAspectRatios.CommonRatios)
                this.SlideViewerItems.Add(new Controls.SlideViewerItem()
                {
                    Value = ratio,
                    Text = $"{ratio.Description} ({ratio})",
                    Image = ratio.Image, 
                    Tooltip = string.Join(" ", ratio.Resolutions.Select(r => $"[{r.Width} × {r.Height}px]")),
                });
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
