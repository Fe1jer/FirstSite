using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace InternetShop.Data.Services
{
    public class FileService
    {
        public string UploadFile(IFormFile img, string path)
        {
            // creates all directories and subdirectories, creates in path
            Directory.CreateDirectory(Path.GetDirectoryName($"wwwroot{path}"));
            // save the file to the path folder in the wwwroot directory
            using (var fileStream = new FileStream($"wwwroot{path}", FileMode.Create))
            {
                img.CopyTo(fileStream);
            }
            // changes the image format to Jpeg, which provides more efficient compression
            Compression(path);

            return path;
        }

        public void Compression(string path)
        {
            Bitmap i2 = null;
            using (var i = Image.FromFile($"wwwroot{path}"))
            {
                i2 = new Bitmap(i);
            }
            i2.Save($"wwwroot{path}", ImageFormat.Jpeg);
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists($"wwwroot{path}") && path != null)
            {
                File.Delete($"wwwroot{path}");
            }
        }

        public void Resize(string srcPath, int width, int height)
        {
            Image image = Image.FromFile($"wwwroot{srcPath}");
            Image resultImage = Resize(image, width, height);
            string[] pathname = srcPath.Split('.');
            resultImage.Save($"wwwroot{pathname[0] + "_" + width + "x" + height + "." + pathname[1]}", ImageFormat.Jpeg);
        }
        
        public void ResizeAndCrop(string srcPath, int width, int height)
        {
            Image image = Image.FromFile($"wwwroot{srcPath}");
            Image resultImage = ResizeAndCrop(image, width, height);
            string[] pathname = srcPath.Split('.');
            resultImage.Save($"wwwroot{pathname[0] + "_" + width + "x" + height + "." + pathname[1]}", ImageFormat.Jpeg);
        }

        public static Image ResizeAndCrop(Image source, int width, int height)
        {
            Image result = null;

            if (source.Width != width || source.Height != height)
            {
                using var target = new Bitmap(width, height);
                using (var g = Graphics.FromImage(target))
                {
                    // Scaling
                    float scaling;
                    float scalingY = (float)source.Height / height;
                    float scalingX = (float)source.Width / width;
                    if (scalingX < scalingY) scaling = scalingX; else scaling = scalingY;

                    int newWidth = (int)(source.Width / scaling);
                    int newHeight = (int)(source.Height / scaling);

                    // Correct float to int rounding
                    if (newWidth < width) newWidth = width;
                    if (newHeight < height) newHeight = height;

                    // See if image needs to be cropped
                    int shiftX = 0;
                    int shiftY = 0;

                    if (newWidth > width)
                    {
                        shiftX = (newWidth - width) / 2;
                    }

                    if (newHeight > height)
                    {
                        shiftY = (newHeight - height) / 2;
                    }

                    // Draw image
                    g.DrawImage(source, -shiftX, -shiftY, newWidth, newHeight);
                }
                result = new Bitmap(target);
            }
            else
            {
                // Image size matched the given size
                result = new Bitmap(source);
            }

            return result;
        }

        // Resize image and if width and height are different ratio, keep it in center.
        public static Bitmap Resize(Image image, int width, int height)
        {
            int drawWidth = width;
            int drawHeight = height;
            if (width != height)
            {
                drawWidth = Math.Min(width, height);
                drawHeight = drawWidth;
            }

            var destRect = new Rectangle((width - drawWidth) / 2, (height - drawHeight) / 2, drawWidth, drawHeight);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage;
        }
    }
}
