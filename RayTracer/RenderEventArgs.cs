using System;
using System.Drawing;

namespace RayTracer
{
    class RenderEventArgs : EventArgs
    {
        private Image image;

        public Image Image
        {
            get { return image; }
        }

        private double renderTime;

        public double RenderTime
        {
            get { return renderTime; }
        }

        public RenderEventArgs(Image image, double renderTime)
        {
            this.image = image;
            this.renderTime = renderTime;
        }
    }
}
