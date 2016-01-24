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

        public Color(double r, double g, double b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        public static Color operator -(Color c)
        {
            return new Color(-c.r, -c.g, -c.b);
        }
        public static Color operator +(Color c1, Color c2)
        {
            return new Color(c1.r + c2.r, c1.g + c2.g, c1.b + c2.b);
        }
        public static Color operator *(Color c1, double d)
        {
            return new Color(c1.r * d, c1.g * d, c1.b * d);
        }
        public static Color operator *(Color c1, Color c2)
        {
            return new Color(c1.r * c2.r, c1.g * c2.g, c1.b * c2.b);
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
