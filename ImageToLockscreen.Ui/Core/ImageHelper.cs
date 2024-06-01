using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageToLockscreen.Ui.Core
{
    internal class ImageHelper
    {
        public static WriteableBitmap Blur(BitmapSource sourceImage, int blurSize)
        {
            int bytesPerPixel = (sourceImage.Format.BitsPerPixel + 7) / 8;
            int stride = GetStride(sourceImage.PixelWidth, bytesPerPixel);
            byte[] data = new byte[stride * sourceImage.PixelHeight];

            sourceImage.CopyPixels(data, stride, 0);

            WriteableBitmap writableImage = new WriteableBitmap(
                sourceImage.PixelWidth,
                sourceImage.PixelHeight,
                sourceImage.DpiX,
                sourceImage.DpiY,
                sourceImage.Format,
                null);

            writableImage.WritePixels(
                new Int32Rect(0, 0, sourceImage.PixelWidth, sourceImage.PixelHeight),
                data,
                stride,
                0);

            return Blur(writableImage, blurSize);
        }

        public static WriteableBitmap Blur(WriteableBitmap sourceImage, int blurSize)
        {
            int pixelWidth = sourceImage.PixelWidth;
            int pixelHeight = sourceImage.PixelHeight;
            int bytesPerPixel = (sourceImage.Format.BitsPerPixel + 7) / 8;
            int stride = GetStride(pixelWidth, bytesPerPixel);
            byte[] pixelData = new byte[stride * pixelHeight];

            sourceImage.CopyPixels(pixelData, stride, 0);

            WriteableBitmap blurredImage = new WriteableBitmap(sourceImage);

            blurredImage.Lock();

            Parallel.For(0, pixelHeight, y =>
            {
                for (int x = 0; x < pixelWidth; x++)
                {
                    int avgR = 0, avgG = 0, avgB = 0;
                    int blurPixelCount = 0;

                    // Calculate the range for the blur
                    int xStart = Math.Max(x - blurSize / 2, 0);
                    int xEnd = Math.Min(x + blurSize / 2, pixelWidth - 1);
                    int yStart = Math.Max(y - blurSize / 2, 0);
                    int yEnd = Math.Min(y + blurSize / 2, pixelHeight - 1);

                    // Average the color of the red, green and blue for each pixel in the blur size
                    for (int xx = xStart; xx <= xEnd; xx++)
                    {
                        for (int yy = yStart; yy <= yEnd; yy++)
                        {
                            int pixelIndex = yy * stride + xx * bytesPerPixel;
                            avgB += pixelData[pixelIndex];
                            avgG += pixelData[pixelIndex + 1];
                            avgR += pixelData[pixelIndex + 2];
                            blurPixelCount++;
                        }
                    }

                    avgR /= blurPixelCount;
                    avgG /= blurPixelCount;
                    avgB /= blurPixelCount;

                    // Set the color of the pixel to the average color
                    int baseIndex = y * stride + x * bytesPerPixel;
                    pixelData[baseIndex] = (byte)avgB;
                    pixelData[baseIndex + 1] = (byte)avgG;
                    pixelData[baseIndex + 2] = (byte)avgR;
                    // Assuming the alpha channel should be set to 255
                    pixelData[baseIndex + 3] = 255;
                }
            });

            // Write the modified pixel data back to the WriteableBitmap
            blurredImage.WritePixels(new Int32Rect(0, 0, pixelWidth, pixelHeight), pixelData, stride, 0);

            blurredImage.Unlock();
            return blurredImage;
        }

        public static int GetStride(int width, int bytesPerPixel)
        {
            int stride = width * bytesPerPixel;
            // Correct for the 4 byte boundary requirement:
            stride += stride % 4 == 0 ? 0 : 4 - (stride % 4);

            return stride;
        }
    }
}
