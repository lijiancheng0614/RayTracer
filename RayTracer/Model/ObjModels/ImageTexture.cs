using System;
using System.Drawing;
using System.IO;

namespace RayTracer.Model.ObjModels
{
    class ImageTexture
    {
        Bitmap image;

        public Bitmap Image
        {
            get { return image; }
        }

        public ImageTexture()
        {
        }

        public ImageTexture(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new ArgumentException("File not found!", filepath);
            }
            image = new Bitmap(filepath);
        }
    }
}
