
namespace RayTracer.Model
{
    class Vector2
    {
        double x;

        public double X
        {
            get { return x; }
        }

        double y;

        public double Y
        {
            get { return y; }
        }

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2 operator *(Vector2 v, double d)
        {
            return new Vector2(v.X * d, v.Y * d);
        }

        public static Vector2 Zero
        {
            get
            {
                return new Vector2(0, 0);
            }
        }
    }
}
