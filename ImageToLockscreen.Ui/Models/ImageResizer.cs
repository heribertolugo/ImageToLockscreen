using ImageToLockscreen.Ui.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageToLockscreen.Ui.Models
{
    internal class ImageResizerEventAgs : EventArgs
    {
        public ImageResizerEventAgs(int progress, string filename, bool success)
        {
            this.Progress = progress;
            this.FileName = filename;
            this.Success = success;
        }
        public int Progress { get; private set; }
        public string FileName { get; private set; }
        public bool Success { get; private set; }
    }

    internal sealed class ImageResizer
    {
        public event EventHandler<ImageResizerEventAgs> OnProgress;

        private int _progress;
        private float Total { get; set; } = 0;

        public async Task Resize(ImageResizerOptions options)
        {
            if (!Directory.Exists(options.InputDirectory))
                throw new DirectoryNotFoundException($"input directory not found: {options.InputDirectory}");
            if (!Directory.Exists(options.OutputDirectory))
                throw new DirectoryNotFoundException($"output directory not found: {options.OutputDirectory}");

            string[] files = Directory.EnumerateFiles(options.InputDirectory)
                .Where(file => AllowedFileTypes.FileExtensions.Values.SelectMany(x => x).Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                .ToArray();
            
            this.Total = files.Length;

            foreach (var filename in files)
            {
                await this.ProcessImage(filename, options.TargetRatio, options.OutputDirectory, options.BackgroundOption);
            }
        }

        public int Progress { get => Volatile.Read(ref this._progress); }
        private async Task ProcessImage(string path, Ratio ratio, string targetDirectory, BackgroundOption backgroundOption)
        {
            string newFileName = Path.Combine(targetDirectory, Path.GetFileName(path)); 

            if (!IsValidImageFile(path))
            {
                this.ProgressReached(new ImageResizerEventAgs((int)(Interlocked.Increment(ref this._progress) / this.Total * 100), newFileName, false));
                return;
            }

            BitmapImage image = GetImage(path);

            if (image is null)
            {
                this.ProgressReached(new ImageResizerEventAgs((int)(Interlocked.Increment(ref this._progress) / this.Total * 100), newFileName, false));
                return;
            }

            if (backgroundOption.IsImage && backgroundOption.BackgroundImagePath is null)
                backgroundOption = new BackgroundOption(path, backgroundOption.IsBlurred, backgroundOption.DpiX, backgroundOption.DpiY);

            var newImage = await GenerateNewImage(image, ratio, backgroundOption);
            image = null;

            if (newImage is null)
            {
                this.ProgressReached(new ImageResizerEventAgs((int)(Interlocked.Increment(ref this._progress) / this.Total * 100), newFileName, false));
                return;
            }

            BitmapEncoder encoder = GetImageEncoder(Path.GetExtension(path));

            encoder.Frames.Add(BitmapFrame.Create(newImage));

            using (var fileStream = new FileStream(newFileName, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
            encoder.Frames.Clear();

            this.ProgressReached(new ImageResizerEventAgs((int)(Interlocked.Increment(ref this._progress) / this.Total * 100), newFileName, true));
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        private void ProgressReached(ImageResizerEventAgs e)
        {
            EventHandler<ImageResizerEventAgs> handler = this.OnProgress;
            handler?.Invoke(this, e);
        }
        private BitmapImage GetImage(string path)
        {
            BitmapImage image = null;
            try
            {
                image = new BitmapImage(new Uri(path));
                image.Freeze();
            }
            catch (Exception ex) when (ex is NotSupportedException || ex is OutOfMemoryException)
            {
                return null;
            }

            return image;
        }
        /// <summary>
        /// Reads the header of different image formats
        /// </summary>
        /// <param name="file">Image file</param>
        /// <returns>true if valid file signature (magic number/header marker) is found</returns>
        /// <remarks><see href="https://stackoverflow.com/a/49683945/6368401">Credit Ricardo González</see></remarks>
        private bool IsValidImageFile(string file)
        {
            byte[] buffer = AllowedFileTypes.All.OrderByDescending(f => f.Header.Length).First().Header;
            byte[] bufferEnd = AllowedFileTypes.All.OrderByDescending(f => f.Tail.Length).First().Tail;

            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length > buffer.Length)
                    {
                        fs.Read(buffer, 0, buffer.Length);
                        fs.Position = (int)fs.Length - bufferEnd.Length;
                        fs.Read(bufferEnd, 0, bufferEnd.Length);
                    }

                    fs.Close();
                }

                foreach(var fileType in AllowedFileTypes.All.OrderByDescending(f => f.Header.Length))
                {
                    if (buffer.StartsWith(fileType.Header) && (fileType.Tail.Length == 0 || bufferEnd.EndsWith(fileType.Tail)))
                        return true;
                }
            }
            catch (Exception) { }

            return false;
        }
        private Task<RenderTargetBitmap> GenerateNewImage(BitmapImage image, Ratio ratio, BackgroundOption backgroundOption)
        {
            Size canvasSize = CalcNewSizeFromRatio(ratio, new Size(image.PixelWidth, image.PixelHeight));
            BitmapSource background = GetBackground(backgroundOption, canvasSize);
            DrawingGroup group = new DrawingGroup();
            Rect overlayRect = new Rect(
                (background.PixelWidth - image.PixelWidth) / 2,
                (background.PixelHeight - image.PixelHeight) / 2,
                image.PixelWidth, image.PixelHeight);

            RenderOptions.SetBitmapScalingMode(group, BitmapScalingMode.HighQuality);
            group.Children.Add(new ImageDrawing(background, new Rect(canvasSize)));
            group.Children.Add(new ImageDrawing(image, overlayRect));

            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                drawingContext.DrawDrawing(group);

            var resizedImage = new RenderTargetBitmap(background.PixelWidth, background.PixelHeight,
                background.DpiX, background.DpiY,PixelFormats.Default);
            resizedImage.Render(drawingVisual);

            return Task.FromResult(resizedImage);
        }

        private BitmapEncoder GetImageEncoder(string imageType)
        {
            switch (imageType.ToLowerInvariant())
            {
                case ".bmp":
                    return new BmpBitmapEncoder();
                case ".gif":
                    return new GifBitmapEncoder();
                case ".png":
                    return new PngBitmapEncoder();
                case ".tiff":
                    return new TiffBitmapEncoder();
                case ".jpg":
                case ".jpeg":
                case ".jiff":
                    return new JpegBitmapEncoder();
                default: throw new ArgumentNullException();
            }
        }

        private Size CalcNewSizeFromRatio(Ratio ratio, Size size)
        {
            bool isWidthChange = ratio.Width > ratio.Height;
            double target = isWidthChange ? size.Height : size.Width;

            double newValue = Math.Round(target/Math.Min(ratio.Height, ratio.Width), 0) * Math.Max(ratio.Width, ratio.Height);

            System.Diagnostics.Debug.Assert(Math.Round(isWidthChange ? newValue/size.Height : size.Width/newValue, 1) == Math.Round(ratio.Width/ratio.Height, 1));
            
            return isWidthChange ? new Size(newValue, size.Height) : new Size(size.Width, newValue);
        }

        private BitmapSource GetBackground(BackgroundOption backgroundOption, Size size)
        {
            if (!backgroundOption.IsImage)
            {
                int stride = ImageHelper.GetStride((int)size.Width, (int)backgroundOption.DpiX);
                BitmapPalette myPalette = new BitmapPalette(new List<Color>() { backgroundOption.BackgroundColor });
                return BitmapSource.Create((int)size.Width, (int)size.Height, backgroundOption.DpiX, backgroundOption.DpiY, PixelFormats.Indexed1, myPalette, 
                    new byte[(int)size.Height * stride], stride);
            }

            BitmapSource background = ResizeImage(GetImage(backgroundOption.BackgroundImagePath), size, backgroundOption.DpiX, backgroundOption.DpiY);

            if (!backgroundOption.IsBlurred)
                return background;

            return ImageHelper.Blur(background, 15);
        }
        private BitmapSource ResizeImage(ImageSource source, Size targetSize, double dpiX = 96, double dpiY = 96)
        {
            DrawingGroup group = new DrawingGroup();
            RenderOptions.SetBitmapScalingMode(group, BitmapScalingMode.HighQuality);
            group.Children.Add(new ImageDrawing(source, new Rect(targetSize)));
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                drawingContext.DrawDrawing(group);
            RenderTargetBitmap resizedImage = new RenderTargetBitmap((int)targetSize.Width, (int)targetSize.Height,
                dpiX, dpiY, PixelFormats.Default);
            resizedImage.Render(drawingVisual);
            
            return resizedImage;
        }
    }


    internal struct ImageResizerOptions
    {
        public ImageResizerOptions(string inputDirectory, string outputDirectory, Ratio targetRatio, string backgroundImagePath, bool isBlurred = false)
        {
            this.InputDirectory = inputDirectory;
            this.OutputDirectory = outputDirectory;
            this.TargetRatio = targetRatio;
            this.BackgroundOption = new BackgroundOption(backgroundImagePath, isBlurred);
        }

        public ImageResizerOptions(string inputDirectory, string outputDirectory, Ratio targetRatio, Color backgroundColor)
        {
            this.InputDirectory = inputDirectory;
            this.OutputDirectory = outputDirectory;
            this.TargetRatio = targetRatio;
            this.BackgroundOption = new BackgroundOption(backgroundColor);
        }

        public string InputDirectory { get; private set; }
        public string OutputDirectory { get; }
        public Ratio TargetRatio { get; }
        public BackgroundOption BackgroundOption { get; }
    }

    internal readonly struct BackgroundOption
    {
        public BackgroundOption(Color backgroundColor, double dpiX = 96, double dpiY = 96)
        {
            this.BackgroundColor = backgroundColor;
            this.IsBlurred = false;
            this.BackgroundImagePath = string.Empty;
            this.IsImage = false;
            this.DpiX = dpiX;
            this.DpiY = dpiY;
        }
        public BackgroundOption(string backgroundImagePath, bool isBlurred = false, double dpiX = 96, double dpiY = 96)
        {
            this.BackgroundColor = Colors.Transparent;
            this.IsBlurred = isBlurred;
            this.BackgroundImagePath = backgroundImagePath;
            this.IsImage = true;
            this.DpiX = dpiX;
            this.DpiY = dpiY;
        }
        public bool IsBlurred { get; }
        public Color BackgroundColor { get; }
        public string BackgroundImagePath { get; }
        public bool IsImage { get; }
        public double DpiX { get; }
        public double DpiY { get; }
    }
}
