using ImageToLockscreen.Ui.Core;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImageToLockscreen.Ui.Controls
{
    public class SlideViewer : UserControl
    {
        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register(nameof(BorderColor), typeof(Brush), typeof(SlideViewer));
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<SlideViewerItem>), typeof(SlideViewer));
        public static readonly DependencyProperty ItemsStrokeProperty = DependencyProperty.Register(nameof(ItemsStroke), typeof(Color), typeof(SlideViewer), new UIPropertyMetadata(ItemsStrokeChangedHandler));

        public static void ItemsStrokeChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Color color = (Color)(e.NewValue ?? (((SlideViewer)sender).Foreground as SolidColorBrush).Color);
            ((SlideViewer)sender).ItemsStroke = color;
        }

        public SlideViewer()
        {
            this.Items = new ObservableCollection<SlideViewerItem>();
        }

        public Brush BorderColor
        {
            get { return (Brush)base.GetValue(BorderColorProperty); }
            set { base.SetValue(BorderColorProperty, value); }
        }

        public Color ItemsStroke
        {
            get { return (Color)base.GetValue(ItemsStrokeProperty); }
            set { base.SetValue(ItemsStrokeProperty, value); }
        }

        public ObservableCollection<SlideViewerItem> Items
        {
            get { return (ObservableCollection<SlideViewerItem>)base.GetValue(ItemsProperty); }
            set { base.SetValue(ItemsProperty, value); }
        }
    }

    public class SlideViewerItem : PropertyObservable
    {
        public DrawingImage Image 
        {
            get { return base.GetProperty<DrawingImage>(); } 
            set { base.SetProperty(value); }
        }

        public string Text
        {
            get { return base.GetProperty<string>(); }
            set { base.SetProperty(value); }
        }

        public object Value
        {
            get { return base.GetProperty<object>(); }
            set { base.SetProperty(value); }
        }

        public string Tooltip
        {
            get { return base.GetProperty<string>(); }
            set { base.SetProperty(value); }
        }
    }
}
