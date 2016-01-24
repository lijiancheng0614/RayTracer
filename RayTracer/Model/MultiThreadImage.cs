using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RayTracer.Model
{
    class MultiThreadImage
    {
        Bitmap bitmap;
        BitmapData bitmapData;
        int[] data;

        public MultiThreadImage(Image image)
        {
            bitmap = new Bitmap(image);
            Open();
        }
        public Image Image
        {
            get
            {
                Marshal.Copy(data, 0, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height / 4);
                bitmap.UnlockBits(bitmapData);
                Image result = new Bitmap(bitmap);
                Open();
                return result;
            }
        }
        public System.Drawing.Color GetPixel(int x, int y)
        {
            return System.Drawing.Color.FromArgb(data[y * bitmapData.Stride / 4 + x]);
        }
        public void SetPixel(int x, int y, System.Drawing.Color c)
        {
            data[y * bitmapData.Stride / 4 + x] = c.ToArgb();
        }

        private void Open()
        {
            bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            data = new int[bitmapData.Stride * bitmapData.Height / 4];
            Marshal.Copy(bitmapData.Scan0, data, 0, bitmapData.Stride * bitmapData.Height / 4);
        }
    }
}
