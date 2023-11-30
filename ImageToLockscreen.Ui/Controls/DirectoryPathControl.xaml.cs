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
        private static string _folderDialogTitle = "Please select folder";
        private FolderBrowserDialog _folderBrowserDialog;
        public DirectoryPathControl()
        {
            InitializeComponent();
            this.FolderDialogTitle = DirectoryPathControl._folderDialogTitle;
            this._folderBrowserDialog = new FolderBrowserDialog();
            this.HintColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#212121"));
        }


        public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register("Hint", typeof(string), typeof(UserControl));
        public static readonly DependencyProperty ValueTextProperty = DependencyProperty.Register("Value", typeof(string), typeof(UserControl));
        public static readonly DependencyProperty FolderDialogTitleProperty = DependencyProperty.Register("FolderDialogTitle", typeof(string), typeof(UserControl));
        public static readonly DependencyProperty HintColorProperty = DependencyProperty.Register("HintColor", typeof(Brush), typeof(UserControl));
        //public static readonly DependencyProperty SearchColorProperty = DependencyProperty.Register("SearchColor", typeof(Brush), typeof(UserControl));

        public string Hint
        {
            get => (string)GetValue(HintTextProperty);
            set => SetValue(HintTextProperty, value);
        }
        public string Value
        {
            get => (string)GetValue(ValueTextProperty);
            set => SetValue(ValueTextProperty, value);
        }
        public string FolderDialogTitle
        {
            get => (string)GetValue(FolderDialogTitleProperty);
            set => SetValue(FolderDialogTitleProperty, value);
        }

        public Brush HintColor
        {
            get => (Brush)GetValue(HintColorProperty);
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
