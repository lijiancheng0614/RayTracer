using System;
using System.Drawing;
using System.IO;

namespace RayTracer.Model.Materials
{
    class ImageTexture
    {
        MultiThreadImage image;
        int width;
        int height;

        public ImageTexture(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new ArgumentException("File not found!", filepath);
            }
            image = new MultiThreadImage(new Bitmap(filepath));
            width = image.Image.Width;
            height = image.Image.Height;
        }

        public Color GetColor(Vector2 textureCoordinates)
        {
            double u = textureCoordinates.X * (width - 1);
            double v = (1 - textureCoordinates.Y) * (height - 1);
            int u0 = (int)Math.Floor(u);
            int v0 = (int)Math.Floor(v);
            int u1 = (int)Math.Ceiling(u);
            int v1 = (int)Math.Ceiling(v);
            u0 = Math.Min(Math.Max(u0, 0), width - 1);
            u1 = Math.Min(Math.Max(u1, 0), width - 1);
            v0 = Math.Min(Math.Max(v0, 0), height - 1);
            v1 = Math.Min(Math.Max(v1, 0), height - 1);
            double wu = (u - u0) / (u1 - u0);
            double wv = (v - v0) / (v1 - v0);
            int r1 = (int)(image.GetPixel(u0, v0).R * (1 - wu)) + (int)(image.GetPixel(u1, v0).R * wu);
            int r2 = (int)(image.GetPixel(u0, v1).R * (1 - wu)) + (int)(image.GetPixel(u1, v1).R * wu);
            int g1 = (int)(image.GetPixel(u0, v0).G * (1 - wu)) + (int)(image.GetPixel(u1, v0).G * wu);
            int g2 = (int)(image.GetPixel(u0, v1).G * (1 - wu)) + (int)(image.GetPixel(u1, v1).G * wu);
            int b1 = (int)(image.GetPixel(u0, v0).B * (1 - wu)) + (int)(image.GetPixel(u1, v0).B * wu);
            int b2 = (int)(image.GetPixel(u0, v1).B * (1 - wu)) + (int)(image.GetPixel(u1, v1).B * wu);
            int r = (int)(r1 * (1 - wv) + r2 * wv);
            int g = (int)(g1 * (1 - wv) + g2 * wv);
            int b = (int)(b1 * (1 - wv) + b2 * wv);
            return new Color(r / 255.0, g / 255.0, b / 255.0);
        }
    }
}
