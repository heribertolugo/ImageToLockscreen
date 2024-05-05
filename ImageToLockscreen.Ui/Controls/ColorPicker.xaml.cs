using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPFColorLib;

namespace ImageToLockscreen.Ui.Controls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(nameof(SelectedColor), typeof(SolidColorBrush), typeof(ColorPicker), new UIPropertyMetadata(SelectedColorChangedHandler));
        public static readonly DependencyProperty PreviousColorProperty = DependencyProperty.Register(nameof(PreviousColor), typeof(SolidColorBrush), typeof(ColorPicker));
        private WPFColorLib.SelectColorDlg SelectColorDialog;

        public static void SelectedColorChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)sender).SelectedColor = e.NewValue as SolidColorBrush;
        }

        public ColorPicker()
        {
            InitializeComponent();
            this.selectedColorUi.Background = SelectedColor;
        }

        public SolidColorBrush SelectedColor
        {
            get => (GetValue(SelectedColorProperty) as SolidColorBrush) ?? new SolidColorBrush(Colors.Green);
            set
            {
                SetValue(SelectedColorProperty, value);
                this.selectedColorUi.Background = value;
            }
        }
        public SolidColorBrush PreviousColor
        {
            get => (SolidColorBrush)GetValue(PreviousColorProperty);
            set => SetValue(PreviousColorProperty, value);
        }

        private void ThisControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.SelectColorDialog = new SelectColorDlg(this.SelectedColor.Color) { Owner = Application.Current.MainWindow };
            if (this.SelectColorDialog.ShowDialog() != true)
                return;

            this.PreviousColor = this.SelectedColor;
            this.SelectedColor = new SolidColorBrush(this.SelectColorDialog.SelectedColor);
        }
    }
}
