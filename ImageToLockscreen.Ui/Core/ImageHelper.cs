using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageToLockscreen.Ui.Core
{
    internal class ImageHelper
    {
        public enum BlurMethod
        {
            Linear,
            Gaussian
        }
        public enum BlurSpeed
        {
            Slow,
            Medium,
            Fast
        }
        public static WriteableBitmap Blur(BitmapSource sourceImage, int blurSize, BlurMethod blurMethod = BlurMethod.Gaussian, BlurSpeed blurSpeed = BlurSpeed.Medium)
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

            writableImage.WritePixels(new Int32Rect(0, 0, sourceImage.PixelWidth, sourceImage.PixelHeight), data, stride, 0);

            try
            {
                switch (blurSpeed)
                {
                    case BlurSpeed.Slow:
                        return blurMethod == BlurMethod.Linear ? LinearBlur(writableImage, blurSize) : GaussianBlur(writableImage, blurSize);
                    case BlurSpeed.Medium:
                        return blurMethod == BlurMethod.Linear ? LinearBlurParallel(writableImage, blurSize) : GaussianBlurParallel(writableImage, blurSize);
                    case BlurSpeed.Fast:
                    default:
                        return blurMethod == BlurMethod.Linear ? LinearBlurParallel(writableImage, blurSize, false) : GaussianBlurParallel(writableImage, blurSize, false);
                }
            }catch(OutOfMemoryException ex)
            {
                return null;
            }
        }

        private static WriteableBitmap LinearBlurParallel(WriteableBitmap sourceImage, int blurSize, bool isLimited = true)
        {
            int pixelWidth = sourceImage.PixelWidth;
            int pixelHeight = sourceImage.PixelHeight;
            int bytesPerPixel = (sourceImage.Format.BitsPerPixel + 7) / 8; 
            int stride = GetStride(pixelWidth, bytesPerPixel);
            byte[] pixelData = new byte[stride * pixelHeight];

            sourceImage.CopyPixels(pixelData, stride, 0);

            WriteableBitmap blurredImage = new WriteableBitmap(sourceImage);

            blurredImage.Lock();

            var options = isLimited ?  new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount } : new ParallelOptions();
            Parallel.For(0, pixelHeight, options, y =>
            {
                LinearBlurWork(y, pixelWidth, pixelHeight, blurSize, stride, bytesPerPixel, pixelData);
            });

            blurredImage.WritePixels(new Int32Rect(0, 0, pixelWidth, pixelHeight), pixelData, stride, 0);

            blurredImage.Unlock();
            return blurredImage;
        }

        private static WriteableBitmap LinearBlur(WriteableBitmap sourceImage, int blurSize)
        {
            int pixelWidth = sourceImage.PixelWidth;
            int pixelHeight = sourceImage.PixelHeight;
            int bytesPerPixel = (sourceImage.Format.BitsPerPixel + 7) / 8;
            int stride = GetStride(pixelWidth, bytesPerPixel);
            byte[] pixelData = new byte[stride * pixelHeight];

            sourceImage.CopyPixels(pixelData, stride, 0);

            WriteableBitmap blurredImage = new WriteableBitmap(sourceImage);

            blurredImage.Lock();

            for (int y = 0; y < pixelHeight; y++)
            {
                LinearBlurWork(y, pixelWidth, pixelHeight, blurSize, stride, bytesPerPixel, pixelData);
            }

            blurredImage.WritePixels(new Int32Rect(0, 0, pixelWidth, pixelHeight), pixelData, stride, 0);

            blurredImage.Unlock();
            return blurredImage;
        }

        private static void LinearBlurWork(int y, int pixelWidth, int pixelHeight, int blurSize, int stride, int bytesPerPixel, byte[] pixelData)
        {
            for (int x = 0; x < pixelWidth; x++)
            {
                int avgR = 0, avgG = 0, avgB = 0;
                int blurPixelCount = 0; 
                int horizontalLocation;
                int verticalLocation;
                int pixelIndex;

                // Calculate the range for the blur
                //int xStart = Math.Max(x - blurSize / 2, 0);
                //int xEnd = Math.Min(x + blurSize / 2, pixelWidth);
                //int yStart = Math.Max(y - blurSize / 2, 0);
                //int yEnd = Math.Min(y + blurSize / 2, pixelHeight);

                // Average the color of the red, green, and blue for each pixel in the blur size
                //for (int xx = xStart; xx < xEnd; xx++)
                //{
                //    for (int yy = yStart; yy < yEnd; yy++)
                //    {
                //        int pixelIndex = yy * stride + xx * bytesPerPixel;
                //        avgB += pixelData[pixelIndex];
                //        avgG += pixelData[pixelIndex + 1];
                //        avgR += pixelData[pixelIndex + 2];
                //        blurPixelCount++;
                //    }
                //}

                for (int xx = x; (xx < x + blurSize && xx < pixelWidth); xx++)
                {
                    horizontalLocation = xx * bytesPerPixel;
                    for (int yy = y; (yy < y + blurSize && yy < pixelHeight); yy++)
                    {
                        verticalLocation = yy * stride;
                        pixelIndex = verticalLocation + horizontalLocation;
                        avgB += pixelData[pixelIndex];
                        avgG += pixelData[pixelIndex + 1];
                        avgR += pixelData[pixelIndex + 2];
                        blurPixelCount++;
                    }
                }

                avgR = Math.Max(Math.Min(255, avgR / blurPixelCount), 0);
                avgG = Math.Max(Math.Min(255, avgG / blurPixelCount), 0);
                avgB = Math.Max(Math.Min(255, avgB / blurPixelCount), 0);

                //// Set the color of the pixel to the average color
                //int baseIndex = y * stride + x * bytesPerPixel;
                //pixelData[baseIndex] = (byte)avgB;
                //pixelData[baseIndex + 1] = (byte)avgG;
                //pixelData[baseIndex + 2] = (byte)avgR;
                //// Keep the alpha channel from the original image
                //pixelData[baseIndex + 3] = pixelData[baseIndex + 3];

                for (int xx = x; (xx < x + blurSize && xx < pixelWidth); xx++)
                {
                    horizontalLocation = xx * bytesPerPixel;
                    for (int yy = y; (yy < y + blurSize && yy < pixelHeight); yy++)
                    {
                        verticalLocation = yy * stride;
                        pixelIndex = verticalLocation + horizontalLocation;

                        pixelData[pixelIndex] = (byte)avgB;
                        pixelData[pixelIndex + 1] = (byte)avgG;
                        pixelData[pixelIndex + 2] = (byte)avgR;
                        pixelData[pixelIndex + 3] = pixelData[pixelIndex + 3];
                    }
                }
            }
        }

        public static WriteableBitmap GaussianBlur(WriteableBitmap sourceImage, int blurSize)
        {
            int pixelWidth = sourceImage.PixelWidth;
            int pixelHeight = sourceImage.PixelHeight;
            int bytesPerPixel = (sourceImage.Format.BitsPerPixel + 7) / 8;
            int stride = GetStride(pixelWidth, bytesPerPixel);
            byte[] pixelData = new byte[stride * pixelHeight];

            sourceImage.CopyPixels(pixelData, stride, 0);

            WriteableBitmap blurredImage = new WriteableBitmap(sourceImage);

            blurredImage.Lock();

            blurSize += blurSize % 2 == 0 ? 1 : 0;
            double sigma = blurSize / 3.0;
            double[,] kernel = GenerateGaussianKernel(blurSize, sigma);

            for (int y = 0; y < pixelHeight; y++)
            {
                for (int x = 0; x < pixelWidth; x++)
                {
                    double avgR = 0, avgG = 0, avgB = 0;
                    double weightSum = 0;
                    int kernelRadius = blurSize / 2;

                    for (int ky = -kernelRadius; ky < kernelRadius; ky++)
                    {
                        for (int kx = -kernelRadius; kx < kernelRadius; kx++)
                        {
                            int px = Math.Min(Math.Max(x + kx, 0), pixelWidth - 1);
                            int py = Math.Min(Math.Max(y + ky, 0), pixelHeight - 1);

                            int pixelIndex = py * stride + px * bytesPerPixel;
                            // Adjust the kernel index to be within the bounds
                            int kernelIndexI = ky + kernelRadius;
                            int kernelIndexJ = kx + kernelRadius;
                            double weight = kernel[kernelIndexI, kernelIndexJ];

                            avgB += weight * pixelData[pixelIndex];
                            avgG += weight * pixelData[pixelIndex + 1];
                            avgR += weight * pixelData[pixelIndex + 2];
                            weightSum += weight;
                        }
                    }

                    avgR /= weightSum;
                    avgG /= weightSum;
                    avgB /= weightSum;

                    // Set the color of the pixel to the weighted average
                    int baseIndex = y * stride + x * bytesPerPixel;
                    pixelData[baseIndex] = (byte)Math.Max(Math.Min(255, avgB), 0);
                    pixelData[baseIndex + 1] = (byte)Math.Max(Math.Min(255, avgG), 0);
                    pixelData[baseIndex + 2] = (byte)Math.Max(Math.Min(255, avgR), 0);
                    // Assuming the alpha channel should be set to 255
                    pixelData[baseIndex + 3] = 255;
                }
            }

            // Write the modified pixel data back to the WriteableBitmap
            blurredImage.WritePixels(new Int32Rect(0, 0, pixelWidth, pixelHeight), pixelData, stride, 0);

            blurredImage.Unlock();
            return blurredImage;
        }

        private static double[,] GenerateGaussianKernel(int size, double sigma)
        {
            double[,] kernel = new double[size, size];
            double sum = 0.0;
            int halfSize = size / 2;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // Calculate the exponent using the distance from the center of the kernel
                    int x = i - halfSize;
                    int y = j - halfSize;
                    double exponent = -(x * x + y * y) / (2 * sigma * sigma);
                    kernel[i, j] = Math.Exp(exponent) / (2 * Math.PI * sigma * sigma);
                    sum += kernel[i, j];
                }
            }

            // Normalize the kernel
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    kernel[i, j] /= sum;
                }
            }

            return kernel;
        }

        // https://github.com/mdymel/superfastblur/blob/master/SuperfastBlur/GaussianBlur.cs
        #region Fast Gaussian Blur
        private static WriteableBitmap GaussianBlurParallel(WriteableBitmap sourceImage, int blurSize, bool isLimited = true)
        {
            int width = sourceImage.PixelWidth;
            int height = sourceImage.PixelHeight;
            var rect = new Int32Rect(0, 0, width, height);
            int bytesPerPixel = (sourceImage.Format.BitsPerPixel + 7) / 8;
            var stride = GetStride(width, bytesPerPixel);
            var source = new int[width * height];

            sourceImage.CopyPixels(rect, source, stride, 0);

            int[] alpha = new int[width * height];
            int[] red   = new int[width * height];
            int[] green = new int[width * height];
            int[] blue  = new int[width * height];

            var options = isLimited ? new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount } : new ParallelOptions();
            Parallel.For(0, source.Length, options, i =>
            {
                alpha[i] = (int)((source[i] & 0xff000000) >> 24);
                red[i] = (source[i] & 0xff0000) >> 16;
                green[i] = (source[i] & 0x00ff00) >> 8;
                blue[i] = (source[i] & 0x0000ff);
            });
            sourceImage = null;
            source = new int[0];
            return GaussianBlurParallelProcess(blurSize, width, height, alpha, red, green, blue, bytesPerPixel, options);
        }

        public static WriteableBitmap GaussianBlurParallelProcess(int blurSize, int width, int height, int[] a, int[] r, int[] g, int[] b, int bytesPerPixel, ParallelOptions options)
        {
            var newAlpha = new int[width * height];
            var newRed = new int[width * height];
            var newGreen = new int[width * height];
            var newBlue = new int[width * height];
            var dest = new int[width * height];

            Parallel.Invoke(
                () => GaussianBlurParallel_4(a, newAlpha, blurSize, width, height, options),
                () => GaussianBlurParallel_4(r, newRed, blurSize, width, height, options),
                () => GaussianBlurParallel_4(g, newGreen, blurSize, width, height, options),
                () => GaussianBlurParallel_4(b, newBlue, blurSize, width, height, options)
                );

            Parallel.For(0, width * height, options, i =>
            {
                newAlpha[i] = (byte)Math.Min(Math.Max(newAlpha[i], 0), 255);
                newRed[i] = (byte)Math.Min(Math.Max(newRed[i], 0), 255);
                newGreen[i] = (byte)Math.Min(Math.Max(newGreen[i], 0), 255);
                newBlue[i] = (byte)Math.Min(Math.Max(newBlue[i], 0), 255);

                dest[i] = (int)((uint)(newAlpha[i] << 24) | (uint)(newRed[i] << 16) | (uint)(newGreen[i] << 8) | (uint)newBlue[i]);
            });

            var writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            writeableBitmap.WritePixels(new Int32Rect(0, 0, width, height), dest, GetStride(width, bytesPerPixel), 0);

            dest = newAlpha = newRed = newGreen = newBlue = dest = a = r = g = b = new int[0];

            return writeableBitmap;
        }
        private static void GaussianBlurParallel_4(int[] source, int[] dest, int r, int width, int height, ParallelOptions options)
        {
            var bxs = GaussianBlurParallelBoxes(r, 3);
            GaussianBlurParallelBox_4(source, dest, width, height, (bxs[0] - 1) / 2, options);
            GaussianBlurParallelBox_4(dest, source, width, height, (bxs[1] - 1) / 2, options);
            GaussianBlurParallelBox_4(source, dest, width, height, (bxs[2] - 1) / 2, options);
        }
        private static int[] GaussianBlurParallelBoxes(int sigma, int n)
        {
            var wIdeal = Math.Sqrt((12 * sigma * sigma / n) + 1);
            var wl = (int)Math.Floor(wIdeal);
            if (wl % 2 == 0) wl--;
            var wu = wl + 2;

            var mIdeal = (double)(12 * sigma * sigma - n * wl * wl - 4 * n * wl - 3 * n) / (-4 * wl - 4);
            var m = Math.Round(mIdeal);

            var sizes = new List<int>();
            for (var i = 0; i < n; i++) sizes.Add(i < m ? wl : wu);
            return sizes.ToArray();
        }
        private static void GaussianBlurParallelBox_4(int[] source, int[] dest, int w, int h, int r, ParallelOptions options)
        {
            for (var i = 0; i < source.Length; i++) dest[i] = source[i];
            GaussianBlurParallelBoxH_4(dest, source, w, h, r, options);
            GaussianBlurParallelBoxT_4(source, dest, w, h, r, options);
        }
        private static void GaussianBlurParallelBoxH_4(int[] source, int[] dest, int w, int h, int r, ParallelOptions options)
        {
            var iar = (double)1 / (r + r + 1);
            Parallel.For(0, h, options, i =>
            {
                var ti = i * w;
                var li = ti;
                var ri = ti + r;
                var fv = source[ti];
                var lv = source[ti + w - 1];
                var val = (r + 1) * fv;
                for (var j = 0; j < r; j++) val += source[ti + j];
                for (var j = 0; j <= r; j++)
                {
                    val += source[ri++] - fv;
                    dest[ti++] = (int)Math.Round(val * iar);
                }
                for (var j = r + 1; j < w - r; j++)
                {
                    val += source[ri++] - dest[li++];
                    dest[ti++] = (int)Math.Round(val * iar);
                }
                for (var j = w - r; j < w; j++)
                {
                    val += lv - source[li++];
                    dest[ti++] = (int)Math.Round(val * iar);
                }
            });
        }
        private static void GaussianBlurParallelBoxT_4(int[] source, int[] dest, int w, int h, int r, ParallelOptions options)
        {
            var iar = (double)1 / (r + r + 1);
            Parallel.For(0, w, options, i =>
            {
                var ti = i;
                var li = ti;
                var ri = ti + r * w;
                var fv = source[ti];
                var lv = source[ti + w * (h - 1)];
                var val = (r + 1) * fv;
                for (var j = 0; j < r; j++) val += source[ti + j * w];
                for (var j = 0; j <= r; j++)
                {
                    val += source[ri] - fv;
                    dest[ti] = (int)Math.Round(val * iar);
                    ri += w;
                    ti += w;
                }
                for (var j = r + 1; j < h - r; j++)
                {
                    val += source[ri] - source[li];
                    dest[ti] = (int)Math.Round(val * iar);
                    li += w;
                    ri += w;
                    ti += w;
                }
                for (var j = h - r; j < h; j++)
                {
                    val += lv - source[li];
                    dest[ti] = (int)Math.Round(val * iar);
                    li += w;
                    ti += w;
                }
            });
        }
        #endregion Fast Gaussian Blur



        public static int GetStride(int width, int bytesPerPixel)
        {
            int stride = width * bytesPerPixel;
            // Correct for the 4 byte boundary requirement:
            stride += stride % 4 == 0 ? 0 : 4 - (stride % 4);

            return stride;
        }

        public static BitmapImage RotateImageByExifOrientationTag(Uri path)
        {
            return RotateImageByExifOrientationTag(new BitmapImage(path));
        }

        public static BitmapImage RotateImageByExifOrientationTag(BitmapImage image)
        {
            BitmapDecoder decoder = BitmapDecoder.Create(
                image.UriSource,
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.OnLoad);

            BitmapFrame bitmapFrame = decoder.Frames[0];
            BitmapMetadata metadata = bitmapFrame.Metadata as BitmapMetadata;
            const string query = "/app1/ifd/{ushort=274}";

            if (metadata == null || !metadata.ContainsQuery(query))
                return image;

            ushort? orientation = metadata?.GetQuery(query) as ushort?;

            // Determine the rotation based on the orientation
            Rotation rotation = Rotation.Rotate0;
            switch (orientation)
            {
                case 2:
                    rotation = Rotation.Rotate0; // Flip horizontally (not supported by Rotation, you would need to use a Transform)
                    break;
                case 3:
                    rotation = Rotation.Rotate180;
                    break;
                case 4:
                    rotation = Rotation.Rotate0; // Flip vertically (not supported by Rotation, you would need to use a Transform)
                    break;
                case 5:
                    rotation = Rotation.Rotate90; // Rotate 90 and flip horizontally (not supported by Rotation, you would need to use a Transform)
                    break;
                case 6:
                    rotation = Rotation.Rotate90;
                    break;
                case 7:
                    rotation = Rotation.Rotate270; // Rotate 270 and flip horizontally (not supported by Rotation, you would need to use a Transform)
                    break;
                case 8:
                    rotation = Rotation.Rotate270;
                    break;
            }

            // Apply the rotation to the image
            if (rotation != Rotation.Rotate0)
            {
                BitmapImage rotatedImage = new BitmapImage();
                rotatedImage.BeginInit();
                rotatedImage.CacheOption = BitmapCacheOption.OnLoad;
                rotatedImage.UriSource = image.UriSource;
                rotatedImage.Rotation = rotation;
                rotatedImage.EndInit();

                return rotatedImage;
            }

            // No rotation needed, just return the original image
            return image;
        }
    }
}
