using System;
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

        private void Initialize()
        {
            ResourceDictionary resources = new ResourceDictionary();
            resources.Source = new Uri("/ImageToLockscreen.Ui;component/ContentPanel.xaml", UriKind.RelativeOrAbsolute);
        }

        public static readonly DependencyProperty TitleTextProperty = DependencyProperty.Register("Title", typeof(string), typeof(UserControl));
        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(UserControl));
        public static readonly DependencyProperty TitleBackgroundProperty = DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(UserControl));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(UserControl));

        //public static readonly DependencyProperty TitleTextProperty = DependencyProperty.Register("Title", typeof(string), typeof(UserControl), new PropertyMetadata(TitleChanged));
        //public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(UserControl), new PropertyMetadata(TitleForegroundChanged));
        //public static readonly DependencyProperty TitleBackgroundProperty = DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(UserControl), new PropertyMetadata(TitleBackgroundChanged));
        //public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(UserControl), new PropertyMetadata(IconChanged));


        private static void TitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Card)d).Title = e.NewValue as string;
        }
        private static void TitleForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Card)d).TitleForeground = e.NewValue as Brush;
        }
        private static void TitleBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Card)d).TitleBackground = e.NewValue as Brush;
        }
        private static void IconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Card)d).Icon = e.NewValue as ImageSource;
        }


        //public string Title { get; set; }
        //public Brush TitleForeground { get; set; }
        //public Brush TitleBackground { get; set; }
        //public ImageSource Icon { get; set; }

        public string Title
        {
            get
            {
                return (string)GetValue(TitleTextProperty);
            }
            set
            {
                SetValue(TitleTextProperty, value);
            }
        }
        public Brush TitleForeground
        {
            get
            {
                return (Brush)GetValue(TitleForegroundProperty);
            }
            set
            {
                SetValue(TitleForegroundProperty, value);
            }
        }
        public Brush TitleBackground
        {
            get
            {
                return (Brush)GetValue(TitleBackgroundProperty);
            }
            set
            {
                SetValue(TitleBackgroundProperty, value);
            }
        }
        public ImageSource Icon
        {
            get
            {
                return (ImageSource)GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }
    }
}
