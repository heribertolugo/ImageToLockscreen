using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImageToLockscreen.Ui.Controls
{
    /// <summary>
    /// Interaction logic for DirectoryPathControl.xaml
    /// </summary>
    public partial class DirectoryPathControl : UserControl
    {
        private static string _defaultFolderDialogTitle = "Please select folder";
        private FolderBrowserDialog _folderBrowserDialog;
        public DirectoryPathControl()
        {
            InitializeComponent();
            this._folderBrowserDialog = new FolderBrowserDialog();
            this.FolderDialogTitle = DirectoryPathControl._defaultFolderDialogTitle;
            this.HintColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#212121"));
        }


        public static readonly DependencyProperty HintProperty = DependencyProperty.Register(nameof(Hint), typeof(string), typeof(DirectoryPathControl), new UIPropertyMetadata(HintTextChangedHandler));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(string), typeof(DirectoryPathControl), new UIPropertyMetadata(ValueTextChangedHandler));
        public static readonly DependencyProperty FolderDialogTitleProperty = DependencyProperty.Register(nameof(FolderDialogTitle), typeof(string), typeof(DirectoryPathControl), new UIPropertyMetadata(FolderDialogTitleChangedHandler));
        public static readonly DependencyProperty HintColorProperty = DependencyProperty.Register(nameof(HintColor), typeof(SolidColorBrush), typeof(DirectoryPathControl), new UIPropertyMetadata(HintColorChangedHandler));
        //public static readonly DependencyProperty SearchColorProperty = DependencyProperty.Register("SearchColor", typeof(Brush), typeof(UserControl));

        public static void HintTextChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((DirectoryPathControl)sender).Hint = e.NewValue as string;
        }
        public static void ValueTextChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }
        public static void FolderDialogTitleChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((DirectoryPathControl)sender).FolderDialogTitle = e.NewValue as string;
        }
        public static void HintColorChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((DirectoryPathControl)sender).HintColor = e.NewValue as SolidColorBrush;
        }

        public string Hint
        {
            get => (string)GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }
        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        public string FolderDialogTitle
        {
            get => (string)GetValue(FolderDialogTitleProperty);
            set 
            {
                this._folderBrowserDialog.Title = value;
                SetValue(FolderDialogTitleProperty, value);
            }
        }

        public SolidColorBrush HintColor
        {
            get => (SolidColorBrush)GetValue(HintColorProperty);
            set => SetValue(HintColorProperty, value);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this._folderBrowserDialog.ShowDialog() == DialogResult.Ok)
            {
                this.Value = this._folderBrowserDialog.SelectedPath;
            }
        }
    }
}
