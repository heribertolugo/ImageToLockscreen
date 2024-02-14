using ImageToLockscreen.Ui.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ComboBox = System.Windows.Controls.ComboBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ImageToLockscreen.Ui.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static string _fileDialogTitle = "Please select folder";
        private static string _fileBrowserFilter = "Images (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|jpg files (*.jpg;*.jpeg)|*.jpg;*.jpeg|png files (*.png)|*.png";
        private OpenFileDialog _fileBrowserDialog;
        private ObservableCollection<DisplayWithValue> _backgroundFillImageOptions = new ObservableCollection<DisplayWithValue>(new List<DisplayWithValue>()
        {
            new DisplayWithValue("Self", "Self"), new DisplayWithValue("Browse", "")
        });
        private DpiScale _dpiInfo;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += this.OnLoaded;
            this._fileBrowserDialog = new OpenFileDialog() { 
                Multiselect = false,
                Title = MainWindow._fileDialogTitle,
                Filter = MainWindow._fileBrowserFilter
            };
        }

        internal ObservableCollection<DisplayWithValue> BackgroundFillImageOptions
        {
            get { return this._backgroundFillImageOptions; }
            set { this._backgroundFillImageOptions = value; }
        }

        private void BackgroundFillImageBrowse(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (e.Source as ComboBox);
            Action<ComboBox, object, object> setSelected = (combo, selected, content) =>
            {
                combo.SelectionChanged -= BackgroundFillImageBrowse;
                combo.SelectedValue = selected;
                combo.SelectedItem = selected;
                this.browseImageOptionPath.Content = content;
                this.ResizeComboBoxByText(combo);
                combo.SelectionChanged += BackgroundFillImageBrowse;
            };

            e.Handled = true;

            if (default(DpiScale).Equals(this._dpiInfo))
                return;

            if (comboBox == null || comboBox.SelectedItem != this.browseImageOption)
            {
                setSelected(comboBox, this.selfImageOption, this.selfImageOption.Content);
                return;
            }

            if (this._fileBrowserDialog.ShowDialog() == true)
                setSelected(comboBox, this.browseImageOptionPath, System.IO.Path.GetFileName(this._fileBrowserDialog.FileName));
        }

        private void ResizeComboBoxByText(ComboBox comboBox)
        {
            int toggleSwitchBuffer = 45;
            Size textSize = this.MeasureString(comboBox.Text,
                new Typeface(comboBox.FontFamily, comboBox.FontStyle, comboBox.FontWeight, comboBox.FontStretch),
                comboBox.FontSize);

            comboBox.Width = Math.Min(Math.Max(comboBox.MinWidth - toggleSwitchBuffer, textSize.Width), comboBox.MaxWidth) + toggleSwitchBuffer;
        }

        private Size MeasureString(string candidate, Typeface typeface, double fontSize)
        {
            if (default(DpiScale).Equals(this._dpiInfo))
                throw new InvalidOperationException("Window must be loaded before calling MeasureString");

            var formattedText = new FormattedText(
                candidate, CultureInfo.CurrentUICulture, System.Windows.FlowDirection.LeftToRight,
                typeface, fontSize, Brushes.Black, this._dpiInfo.PixelsPerDip);

            return new Size(formattedText.Width, formattedText.Height);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this._dpiInfo = VisualTreeHelper.GetDpi(this);
        }

        protected override void OnDpiChanged(DpiScale oldDpiScaleInfo, DpiScale newDpiScaleInfo)
        {
            this._dpiInfo = newDpiScaleInfo;
        }
    }
}
