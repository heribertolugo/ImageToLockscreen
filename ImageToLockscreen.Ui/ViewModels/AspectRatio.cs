using ImageToLockscreen.Ui.Core;
using ImageToLockscreen.Ui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImageToLockscreen.Ui.ViewModels
{
    public class AspectRatio : PropertyObservable
    {
        public AspectRatio()
        {
            this.SetDefaults();
        }

        public AspectRatio(Ratio ratio, Size bounds, string description = null)
        {
            this.SetRatio(ratio);
            this.Bounds = bounds;
            this.Description = description ?? ToString();
            this.SetRatioImage();
        }

        public AspectRatio(Ratio ratio, Size bounds, IEnumerable<Size> sizes, string description = null)
        {
            this.SetRatio(ratio);
            this.Bounds = bounds;
            this.Description = description ?? ToString();
            this.SetRatioImage();
            foreach(Size size in sizes)
                this.Resolutions.Add(size);
        }
        public string Description
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        public Size Bounds
        {
            get { return GetProperty<Size>(); }
            set { SetProperty(value); }
        }
        public Drawing Image
        {
            get { return GetProperty<Drawing>(); }
            private set { SetProperty(value); }
        }
        public Image Background
        {
            get { return GetProperty<Image>(); }
            set { SetProperty(value); }
        }
        public Color BackgroundColor
        {
            get { return GetProperty<Color>(); }
            set { SetProperty(value); }
        }
        public Color StrokeColor
        {
            get { return GetProperty<Color>(); }
            set { SetProperty(value); this.SetRatioImage(); }
        }
        public int StrokeThickness
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); this.SetRatioImage(); }
        }
        public double Width
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); this.SetRatioImage(); }
        }
        public double Height
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); this.SetRatioImage(); }
        }
        public Ratio Ratio
        {
            get { return new Ratio(this.Width, this.Height); }
            set { this.Width = value.Width; this.Height = value.Height; this.SetRatioImage(); }
        }
        public ObservableCollection<Size> Resolutions
        {
            get; private set;
        }
        public void SetRatio(double width, double height)
        {
            this.Width = width;
            this.Height = height;
            this.SetRatioImage();
        }
        public void SetRatio(Ratio ratio)
        {
            SetRatio(ratio.Width, ratio.Height);
        }
        private void SetRatioImage()
        {
            Size ratioRectSize = AspectRatio.GetNewSize(this.Ratio, this.Bounds);
            Point point = this.CalculateCenterPositioning(this.Bounds, ratioRectSize); 

            this.Image = AspectRatio.DrawRatio(point, ratioRectSize, this.StrokeColor, this.StrokeThickness);
        }
        private Point CalculateCenterPositioning(Size rect, Size ratio)
        {
            // use new size to deermine center positioning
            return new Point();
        } 
        private static Drawing DrawRatio(Point point, Size size, Color strokeColor, int strokeThickness)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            Rect rect = new Rect(point, size);
            Pen pen = new Pen(new SolidColorBrush(strokeColor), strokeThickness);
            drawingContext.DrawRectangle(null, pen, rect);
            drawingContext.Close();
            return drawingVisual.Drawing;
        }

        private static Size GetNewSize(Ratio ratio, Size bounds)
        {
            bool useWidth = bounds.Width > bounds.Height;
            int multiplier = (int)Math.Round(useWidth ? bounds.Width / ratio.Width : bounds.Height / ratio.Height, 0);
            return new Size(useWidth ? bounds.Width : ratio.Width * multiplier, useWidth ? ratio.Height * multiplier : bounds.Height);
        }

        private void SetDefaults()
        {
            this.Description = string.Empty;
            this.Bounds = new Size(32, 32);
            this.Background = null;
            this.BackgroundColor = Colors.White; //Colors.Transparent;
            this.StrokeColor = Colors.Black;
            this.Resolutions = new ObservableCollection<Size>();
        }
        public override string ToString()
        {
            return $"{Width}:{Height}";
        }
    }
}
