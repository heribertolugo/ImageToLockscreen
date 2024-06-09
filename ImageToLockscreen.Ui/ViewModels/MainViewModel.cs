using ImageToLockscreen.Ui.Controls;
using ImageToLockscreen.Ui.Core;
using ImageToLockscreen.Ui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ImageToLockscreen.Ui.ViewModels
{
    public class MainViewModel : PropertyObservable
    {
        #region Private Members/Properties
        private static string FileDialogTitle { get; set; } = "Please select image";
        private static string FileBrowserFilter { get; set; }
        private OpenFileDialog FileBrowserDialog { get; set; }
        private ObservableCollection<DisplayWithValue> _backgroundFillImageOptions = new ObservableCollection<DisplayWithValue>(new List<DisplayWithValue>()
        {
            new DisplayWithValue("Self", BackgroundFillImageOption.Self, true),
            new DisplayWithValue("Browse", BackgroundFillImageOption.Browse, true),
            new DisplayWithValue("", BackgroundFillImageOption.Url, false)
        });
        private BackgroundWorker Worker { get; } = new BackgroundWorker();
        #endregion Private Members/Properties

        static MainViewModel()
        {
            FileBrowserFilter = $"Images |{string.Join(";", AllowedFileTypes.FileExtensions.Values.SelectMany(k => k.Select(v => $"*{v}")))}|";
            FileBrowserFilter += string.Join("|", AllowedFileTypes.FileExtensions.Select(f => $"{f.Key} files |{string.Join(";",f.Value.Select(k => $"*{k}"))}"));
        }

        public MainViewModel()
        {
            this.FileBrowserDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Title = MainViewModel.FileDialogTitle,
                Filter = MainViewModel.FileBrowserFilter
            };
            this.SetSlideViewerItemsRatios();

            this.BackgroundFillImageOptionSelectionChangedCommand = new RelayCommand(BackgroundFillImageOptionSelectionChanged);
            this.ConvertImagesCommand = new RelayCommand(this.ConvertImages);
            this.BackgroundFillColor = new SolidColorBrush(Colors.Black);

            this.Worker.WorkerReportsProgress = true;
            this.Worker.WorkerSupportsCancellation = true;
            this.Worker.DoWork += Worker_DoWork;
            this.Worker.ProgressChanged += Worker_ProgressChanged;
            this.Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }


        #region Public Properties
        public string InputDirectory
        {
            get { return base.GetProperty<string>(); }
            set 
            { 
                base.SetProperty(value); 
                this.ResetProgress();
            }
        }
        public string OutputDirectory
        {
            get { return base.GetProperty<string>(); }
            set
            {
                base.SetProperty(value);
                this.ResetProgress();
            }
        }
        public DisplayWithValue SelectedBackgroundFillOption
        {
            get { return base.GetProperty<DisplayWithValue>(); }
            set
            {
                base.SetProperty(value);
                this.ResetProgress();
            }
        }
        public string SelectedBackgroundFillOptionUrl
        {
            get { return base.GetProperty<string>(getDefault: () => string.Empty); }
            set
            {
                base.SetProperty(value);
                this.ResetProgress();
            }
        }
        public bool IsSelectedBackgroundFillOptionUrl
        {
            get { return base.GetProperty<bool>(getDefault: () => false); }
            set
            {
                base.SetProperty(value);
                this.ResetProgress();
            }
        }
        public bool IsBlurBackgroundImage
        {
            get { return base.GetProperty<bool>(getDefault: () => false); }
            set
            {
                base.SetProperty(value);
                this.ResetProgress();
            }
        }
        public bool IsBackgroundFillSolidColor
        {
            get { return base.GetProperty<bool>(getDefault: () => true); }
            set
            {
                base.SetProperty(value);
                this.ResetProgress();
            }
        }
        public SolidColorBrush BackgroundFillColor
        {
            get { return base.GetProperty<SolidColorBrush>(); }
            set
            {
                base.SetProperty(value);
                this.ResetProgress();
            }
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
                this.ResetProgress();
            }
        }

        public SlideViewerItem SelectedAspectRatio
        {
            get { return base.GetProperty<SlideViewerItem>(); }
            set
            {
                base.SetProperty(value);
                this.ResetProgress();
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
                this.ResetProgress();
            }
        }

        public ObservableCollection<SlideViewerItem> SlideViewerItems
        {
            get { return base.GetProperty<ObservableCollection<SlideViewerItem>>(getDefault: () => { return new ObservableCollection<SlideViewerItem>(); }); }
            set
            {
                base.SetProperty(value);
                this.ResetProgress();
            }
        }

        public int ConversionProgress
        {
            get { return base.GetProperty<int>(); }
            set { base.SetProperty(value); }
        }

        public string ConversionProgressMessage
        {
            get { return base.GetProperty<string>(); }
            set { base.SetProperty(value); }
        }

        public bool IsNotBusy
        {
            get { return base.GetProperty<bool>(getDefault: () => true); }
            set { base.SetProperty(value); }
        }
        #endregion Public Properties


        #region Public ICommand
        public ICommand BackgroundFillImageOptionSelectionChangedCommand { get; set; }
        public ICommand ConvertImagesCommand { get; set; }
        #endregion Public ICommand


        #region Private ICommand
        private void ResetProgress()
        {
            if (this.ConversionProgress >= 100)
            {
                this.ConversionProgress = 0;
                this.ConversionProgressMessage = string.Empty;
            }
        }
        private bool BackgroundFillImageOptionSelectionChangedInternally = false;
        private void BackgroundFillImageOptionSelectionChanged()
        {
            if (this.BackgroundFillImageOptionSelectionChangedInternally)
                return;
            var option = this._backgroundFillImageOptions.First(o => o.Value == BackgroundFillImageOption.Url);
            
            if (this.SelectedBackgroundFillOption.Value == BackgroundFillImageOption.Browse
                && this.FileBrowserDialog.ShowDialog() == true)
            {
                this.BackgroundFillImageOptionSelectionChangedInternally = true;
                option.Display = System.IO.Path.GetFileName(this.FileBrowserDialog.FileName);
                this.IsSelectedBackgroundFillOptionUrl = true;
                this.SelectedBackgroundFillOptionUrl = this.FileBrowserDialog.FileName;
                this.SelectedBackgroundFillOption = option;
                this.SelectedBackgroundFillOption.IsVisible = true;
            }
            else
            {
                this.BackgroundFillImageOptionSelectionChangedInternally = true;
                this.IsSelectedBackgroundFillOptionUrl = false;
                option.IsVisible = false;
                option.Display = string.Empty;
                this.SelectedBackgroundFillOption = this._backgroundFillImageOptions.First(o => o.Value == BackgroundFillImageOption.Self);
            }
            this.BackgroundFillImageOptionSelectionChangedInternally = false;
        }
        private void ConvertImages()
        {
            string bgImage = SelectedBackgroundFillOption.Value == BackgroundFillImageOption.Self ? null : this.SelectedBackgroundFillOptionUrl;
            ImageResizerOptions options = this.IsBackgroundFillSolidColor ?
                new ImageResizerOptions(this.InputDirectory, this.OutputDirectory, (this.SelectedAspectRatio.Value as AspectRatio).Ratio, this.BackgroundFillColor.Color) :
                new ImageResizerOptions(this.InputDirectory, this.OutputDirectory, (this.SelectedAspectRatio.Value as AspectRatio).Ratio, bgImage, this.IsBlurBackgroundImage);
            this.IsNotBusy = false;
            this.Worker.RunWorkerAsync(options);
        }
        #endregion Private ICommand

        #region Worker Event Handlers
        private async void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ImageResizer resizer = new ImageResizer();

            this.ConversionProgress = 0;
            this.ConversionProgressMessage = string.Empty;

            resizer.OnProgress += (s, v) =>
            {
                Worker.ReportProgress(v.Progress, v.FileName);
            };

            await resizer.Resize((ImageResizerOptions)e.Argument);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ConversionProgress = e.ProgressPercentage;
            this.ConversionProgressMessage = e.UserState as string;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.IsNotBusy = true;
            this.ConversionProgressMessage = "done";
        }
        #endregion Worker Event Handlers

        private void SetSlideViewerItemsRatios()
        {
            foreach (var ratio in CommonAspectRatios.CommonRatios)
            {
                SlideViewerItem aspectRatioItem = new SlideViewerItem()
                {
                    Value = ratio,
                    Text = $"{ratio.Description} ({ratio})",
                    Image = ratio.Image,
                    Tooltip = string.Join(" ", ratio.Resolutions.Select(r => $"[{r.Width} × {r.Height}px]")),
                };
                this.SlideViewerItems.Add(aspectRatioItem);

                if (ratio.Resolutions.Any(r => r == this.CurrentScreenResolution || Math.Round(ratio.Height / ratio.Width, 1) == Math.Round(r.Height / r.Width, 1)))
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
