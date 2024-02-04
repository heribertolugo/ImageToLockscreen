using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImageToLockscreen.Ui.Controls
{
    /// <summary>
    /// Interaction logic for Card.xaml
    /// </summary>
    public class Card : UserControl
    {
        public Card()
        {
            //InitializeComponent();
        }

        public static readonly DependencyProperty TitleTextProperty = DependencyProperty.Register("Title", typeof(string), typeof(UserControl));
        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(UserControl));
        public static readonly DependencyProperty TitleBackgroundProperty = DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(UserControl));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(UserControl));

        public string Title
        {
            get { return (string)base.GetValue(TitleTextProperty); }
            set { base.SetValue(TitleTextProperty, value); }
        }
        public Brush TitleForeground
        {
            get { return (Brush)base.GetValue(TitleForegroundProperty); }
            set { base.SetValue(TitleForegroundProperty, value); }
        }
        public Brush TitleBackground
        {
            get { return (Brush)base.GetValue(TitleBackgroundProperty); }
            set { base.SetValue(TitleBackgroundProperty, value); }
        }
        public ImageSource Icon
        {
            get { return (ImageSource)base.GetValue(IconProperty); }
            set { base.SetValue(IconProperty, value); }
        }
    }
}
