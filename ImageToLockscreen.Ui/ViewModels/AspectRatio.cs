using ImageToLockscreen.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImageToLockscreen.Ui.ViewModels
{
    public class AspectRatio : PropertyObservable
    {
        public AspectRatio()
        {
            SetDefaults();
        }
        public string Description { get; set; }
        public Size Region { get; set; }
        public Image Image { get; private set; }
        public Image Background { get; set; }
        public Color BackgroundColor { get; set; }
        public Color StrokeColor { get; set; }
        public int StrokeThickness { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public void SetRatio(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
        private void DrawImage()
        {

        }
        private void SetDefaults()
        {
            this.Description = string.Empty;
            this.Region = new Size(32, 32);
            this.Background = null;
            this.BackgroundColor = Colors.Transparent;
            this.StrokeColor = Colors.White;
        }
        public override string ToString()
        {
            return $"{this.Width}:{this.Height}";
        }
    }
}
