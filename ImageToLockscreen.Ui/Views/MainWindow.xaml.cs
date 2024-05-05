using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using ComboBox = System.Windows.Controls.ComboBox;

namespace ImageToLockscreen.Ui.Views
{
    public partial class MainWindow : Window
    {
        private DpiScale _dpiInfo;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += this.OnLoaded;
        }

        private void BackgroundFillImageOptionSource_Selected(object sender, RoutedEventArgs e)
        {
            this.ResizeComboBoxByText(e.Source as ComboBox);
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
