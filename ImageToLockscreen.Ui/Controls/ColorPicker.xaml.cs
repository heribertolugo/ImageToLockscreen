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
        public static readonly DependencyProperty DefaultColorProperty = DependencyProperty.Register("DefaultColor", typeof(SolidColorBrush), typeof(UserControl));
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(SolidColorBrush), typeof(UserControl));
        public static readonly DependencyProperty PreviousColorProperty = DependencyProperty.Register("PreviousColor", typeof(SolidColorBrush), typeof(UserControl));
        private WPFColorLib.SelectColorDlg SelectColorDialog;

        public ColorPicker()
        {
            InitializeComponent();
            Color initColor = this.InitDefaultColor();
            this.selectedColorUi.Background = new SolidColorBrush(initColor);
        }

        public SolidColorBrush DefaultColor
        {
            get => (SolidColorBrush)GetValue(DefaultColorProperty);
            set => SetValue(DefaultColorProperty, value);
        }
        public SolidColorBrush SelectedColor
        {
            get => (SolidColorBrush)GetValue(SelectedColorProperty);
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

        private Color InitDefaultColor()
        {
            Color color = Colors.Black;
            if (this.DefaultColor == null && (this.Foreground as SolidColorBrush) != null)
            {
                this.DefaultColor = (this.Foreground as SolidColorBrush);
                color = (this.Foreground as SolidColorBrush).Color;
            }
            else if (this.DefaultColor == null)
            {
                this.DefaultColor = Brushes.Black;
                color = Colors.Black;
            }else if (this.DefaultColor != null)
            {
                color = (this.Foreground as SolidColorBrush).Color;
            }

            return color;
        }

        private void ThisControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //if (this.SelectColorDialog == null)
                this.SelectColorDialog = new SelectColorDlg((this.selectedColorUi.Background as SolidColorBrush).Color) { Owner = Application.Current.MainWindow };
            if (this.SelectColorDialog.ShowDialog() != true)
                return;

            this.PreviousColor = this.SelectedColor;
            this.SelectedColor = new SolidColorBrush(this.SelectColorDialog.SelectedColor);
        }
    }
}
