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
            if (((ColorPicker)sender).SelectedColor != e.NewValue as SolidColorBrush)
                ((ColorPicker)sender).SelectedColor = e.NewValue as SolidColorBrush;
        }

        public ColorPicker()
        {
            InitializeComponent();
            this.selectedColorUi.Background = SelectedColor;
        }

        public SolidColorBrush SelectedColor
        {
            get => (base.GetValue(ColorPicker.SelectedColorProperty) as SolidColorBrush) ?? new SolidColorBrush(Colors.Black);
            set
            {
                base.SetValue(ColorPicker.SelectedColorProperty, value);
                this.selectedColorUi.Background = value;
            }
        }
        public SolidColorBrush PreviousColor
        {
            get => (SolidColorBrush)base.GetValue(ColorPicker.PreviousColorProperty);
            set => base.SetValue(ColorPicker.PreviousColorProperty, value);
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
