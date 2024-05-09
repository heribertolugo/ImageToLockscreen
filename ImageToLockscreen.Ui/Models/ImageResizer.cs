using ImageToLockscreen.Ui.Core;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageToLockscreen.Ui.Models
{
    internal class ImageResizerEventAgs : EventArgs
    {
        public ImageResizerEventAgs(float progress, string filename)
        {
            this.Progress = progress;
            this.FileName = filename;
        }
        public float Progress { get; private set; }
        public string FileName { get; private set; }
    }

    internal class ImageResizer
    {
        public event EventHandler<ImageResizerEventAgs> OnProgress;
        public async Task Resize(ImageResizerOptions options)
        {
            // if directory ! exists
            foreach(var directory in Directory.EnumerateFiles(options.InputDirectory))
            {

            }
        }

        public double Progress { get; private set; }

        private Image GetImage(string path)
        {
            Image image = null;
            try
            {
                image = Image.FromFile(path);
            }
            catch (OutOfMemoryException ex)
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
                using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    if (fs.Length > buffer.Length)
                    {
                        fs.Read(buffer, 0, buffer.Length);
                        fs.Position = (int)fs.Length - bufferEnd.Length;
                        fs.Read(bufferEnd, 0, bufferEnd.Length);
                    }

                    fs.Close();
                }

                foreach(var fileType in AllowedFileTypes.All.OrderByDescending(f => f.Header))
                {
                    if (buffer.StartsWith(fileType.Header) && (fileType.Tail.Length == 0 || bufferEnd.EndsWith(fileType.Tail)))
                        return true;
                }
            }
            catch (Exception ex) { }

            return false;


            //byte[] buffer = new byte[8];
            //byte[] bufferEnd = new byte[2];

            //var bmp = new byte[] { 0x42, 0x4D };               // BMP "BM"
            //var gif87a = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 };     // "GIF87a"
            //var gif89a = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };     // "GIF89a"
            //var png = new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };   // PNG "\x89PNG\x0D\0xA\0x1A\0x0A"
            //var tiffI = new byte[] { 0x49, 0x49, 0x2A, 0x00 }; // TIFF II "II\x2A\x00"
            //var tiffM = new byte[] { 0x4D, 0x4D, 0x00, 0x2A }; // TIFF MM "MM\x00\x2A"
            //var jpeg = new byte[] { 0xFF, 0xD8, 0xFF };        // JPEG JFIF (SOI "\xFF\xD8" and half next marker xFF)
            //var jpegEnd = new byte[] { 0xFF, 0xD9 };           // JPEG EOI "\xFF\xD9"

            //try
            //{
            //    using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            //    {
            //        if (fs.Length > buffer.Length)
            //        {
            //            fs.Read(buffer, 0, buffer.Length);
            //            fs.Position = (int)fs.Length - bufferEnd.Length;
            //            fs.Read(bufferEnd, 0, bufferEnd.Length);
            //        }

            //        fs.Close();
            //    }

            //    if (buffer.StartsWith(bmp) ||
            //        buffer.StartsWith(gif87a) ||
            //        buffer.StartsWith(gif89a) ||
            //        buffer.StartsWith(png) ||
            //        buffer.StartsWith(tiffI) ||
            //        buffer.StartsWith(tiffM))
            //    {
            //        return true;
            //    }

            //    if (buffer.StartsWith(jpeg))
            //    {
            //        // Offset 0 (Two Bytes): JPEG SOI marker (FFD8 hex)
            //        // Offest 1 (Two Bytes): Application segment (FF?? normally ??=E0)
            //        // Trailer (Last Two Bytes): EOI marker FFD9 hex
            //        if (bufferEnd.StartsWith(jpegEnd))
            //        {
            //            return true;
            //        }
            //    }
            //}
            //catch (Exception ex) { }

            //return false;
        }

    }

    internal class ImageResizerHelper
    {
        public ImageResizerHelper()
        {

        }
    }


    internal struct ImageResizerOptions
    {
        public ImageResizerOptions(string inputDirectory, string outputDirectory, Ratio targetRatio, string backgroundImagePath, bool isBlurred = false)
        {
            this.InputDirectory = inputDirectory;
            this.OutputDirectory = outputDirectory;
            this.TargetRatio = targetRatio;
            this.BackgroundImagePath = backgroundImagePath;
            this.IsBlurred = isBlurred;
            this.BackgroundColor = Color.Transparent;
        }

        public ImageResizerOptions(string inputDirectory, string outputDirectory, Ratio targetRatio, Color backgroundColor)
        {
            this.InputDirectory = inputDirectory;
            this.OutputDirectory = outputDirectory;
            this.TargetRatio = targetRatio;
            this.BackgroundColor = backgroundColor;
            this.IsBlurred = false;
            this.BackgroundImagePath = string.Empty;
        }

        public string InputDirectory { get; private set; }
        public string OutputDirectory { get; private set; }
        public Ratio TargetRatio { get; private set; }
        public bool IsBlurred { get; private set; }
        public Color BackgroundColor { get; private set; }
        public string BackgroundImagePath { get; private set; }
    }
}
