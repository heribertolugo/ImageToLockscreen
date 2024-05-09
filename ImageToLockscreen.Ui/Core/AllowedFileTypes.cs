using System;
using System.Collections.Generic;

namespace ImageToLockscreen.Ui.Core
{
    internal class AllowedFileTypes
    {
        private static List<AllowedFileTypes> _all { get; set; } = new List<AllowedFileTypes>();
        private static Dictionary<string, string[]> _fileExtensions = new Dictionary<string, string[]>();
        // BMP "BM"
        public static AllowedFileTypes Bitmap = new AllowedFileTypes(new byte[2] { 0x42, 0x4D }, new byte[0],
            new string[] { ".bmp" }, nameof(Bitmap));
        // "GIF87a"
        public static AllowedFileTypes Gif87a = new AllowedFileTypes(new byte[6] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, new byte[0],
            new string[] { ".gif" }, "Gif");
        // "GIF89a"
        public static AllowedFileTypes Gif89a = new AllowedFileTypes(new byte[6] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, new byte[0],
            new string[] { ".gif" }, "Gif");
        // PNG "\x89PNG\x0D\0xA\0x1A\0x0A"
        public static AllowedFileTypes Png = new AllowedFileTypes(new byte[8] { 0x89, 0x50, 0x4e, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, new byte[0],
            new string[] { ".png" }, nameof(Png));
        // TIFF II "II\x2A\x00"
        public static AllowedFileTypes TiffI = new AllowedFileTypes(new byte[4] { 0x49, 0x49, 0x2A, 0x00 }, new byte[0],
            new string[] { ".tiff" }, "Tiff");
        // TIFF MM "MM\x00\x2A"
        public static AllowedFileTypes TiffM = new AllowedFileTypes(new byte[4] { 0x4D, 0x4D, 0x00, 0x2A }, new byte[0],
            new string[] { ".tiff" }, "Tiff");
        // JPEG JFIF (SOI "\xFF\xD8" and half next marker xFF) (EOI "\xFF\xD9")
        public static AllowedFileTypes Jpeg = new AllowedFileTypes(new byte[3] { 0xFF, 0xD8, 0xFF }, new byte[2] { 0xFF, 0xD9 },
            new string[] { ".jpg", ".jpeg" }, nameof(Jpeg));

        private AllowedFileTypes(byte[] header, byte[] tail, string[] extensions, string displayName) 
        { 
            this.Header = header;
            this.Tail = tail;
            this.Extensions = extensions;
            this.DisplayName = displayName;

            AllowedFileTypes._all.Add(this);
            AllowedFileTypes.MaxHeaderSize = Math.Max(AllowedFileTypes.MaxHeaderSize, header.Length);
            AllowedFileTypes.MaxTailSize = Math.Max(AllowedFileTypes.MaxTailSize, header.Length);

            if (AllowedFileTypes.FileExtensions.ContainsKey(DisplayName))
                AllowedFileTypes._fileExtensions[DisplayName] = AllowedFileTypes._fileExtensions[DisplayName].Merge(extensions);
            else AllowedFileTypes._fileExtensions.Add(DisplayName, extensions);
        }

        public byte[] Header { get; private set; }
        public byte[] Tail { get; private set; }
        public string[] Extensions { get; private set; }
        public string DisplayName { get; private set; }

        public static IEnumerable<AllowedFileTypes> All {  get => AllowedFileTypes._all; }
        public static int MaxHeaderSize {  get; private set; }
        public static int MaxTailSize { get; private set; }
        public static IReadOnlyDictionary<string, string[]> FileExtensions {  get => AllowedFileTypes._fileExtensions; }
    }
}
