using System;

namespace RayTracer.Model
{
    class Color
    {
        double r;

        public double R
        {
            get { return r; }
        }
        double g;

        public double G
        {
            get { return g; }
        }
        double b;

        public double B
        {
            get { return b; }
        }

        public Color(double _r, double _g, double _b)
        {
            r = _r;
            g = _g;
            b = _b;
        }
        public Color Add(Color c)
        {
            return new Color(r + c.r, g + c.g, b + c.b);
        }
        public Color Multiply(double s)
        {
            return new Color(r * s, g * s, b * s);
        }
        public Color Modulate(Color c)
        {
            return new Color(r * c.r, g * c.g, b * c.b);
        }
        public void Saturate()
        {
            r = Math.Max(Math.Min(r, 1), 0);
            g = Math.Max(Math.Min(g, 1), 0);
            b = Math.Max(Math.Min(b, 1), 0);
        }
        public System.Drawing.Color GetSystemColor()
        {
            this.Saturate();
            return System.Drawing.Color.FromArgb(255, (int)(R * 255), (int)(G * 255), (int)(B * 255));
        }

        public static Color Black
        {
            get
            {
                return new Color(0, 0, 0);
            }
        }
        public static Color White
        {
            get
            {
                return new Color(1, 1, 1);
            }
        }
        public static Color Red
        {
            get
            {
                return new Color(1, 0, 0);
            }
        }
        public static Color Green
        {
            get
            {
                return new Color(0, 1, 0);
            }
        }
        public static Color Blue
        {
            get
            {
                return new Color(0, 0, 1);
            }
        }
    }
}
