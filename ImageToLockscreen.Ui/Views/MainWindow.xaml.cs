using ImageToLockscreen.Ui.ViewModels;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
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
            SystemEvents.DisplaySettingsChanged += this.SystemEvents_DisplaySettingsChanged;
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
            this.SetRatioOnViewModel(this.GetCurrentScreenWorkArea(this));
        }

        public Rect GetCurrentScreenWorkArea(System.Windows.Window window)
        {
            // see: https://stackoverflow.com/a/65999556/6368401
            this._dpiInfo = VisualTreeHelper.GetDpi(window);
            //var screen = Screen.FromPoint(new System.Drawing.Point((int)window.Left, (int)window.Top));

            //return new Rect { Width = screen.WorkingArea.Width / this._dpiInfo.DpiScaleX, Height = screen.WorkingArea.Height / this._dpiInfo.DpiScaleY };

            return new Rect { Width = System.Windows.SystemParameters.PrimaryScreenWidth / this._dpiInfo.DpiScaleX, 
                Height = System.Windows.SystemParameters.PrimaryScreenHeight / this._dpiInfo.DpiScaleY };
        }

        protected override void OnDpiChanged(DpiScale oldDpiScaleInfo, DpiScale newDpiScaleInfo)
        {
            this.SetRatioOnViewModel(this.GetCurrentScreenWorkArea(this));
        }

        void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            this.SetRatioOnViewModel(this.GetCurrentScreenWorkArea(this));
        }

        private void SetRatioOnViewModel(Rect rect)
        {
            var vm = this.DataContext as MainViewModel;

            if (vm == null) return;

            vm.CurrentScreenResolution = new Size(rect.Width, rect.Height);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            var hwndSource = (HwndSource)PresentationSource.FromVisual(this);
            var handle = hwndSource.Handle;
            if (DwmSetWindowAttribute(handle, 19, new[] { 1 }, 4) != 0)
                DwmSetWindowAttribute(handle, 20, new[] { 1 }, 4);
            base.OnSourceInitialized(e);
        }

        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
    }
}
