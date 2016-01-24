using System;

namespace RayTracer.Model
{
    class Vector3
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

        double z;

        public double Z
        {
            get { return z; }
        }

        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public double SqrLength()
        {
            return x * x + y * y + z * z;
        }
        public double Length()
        {
            return Math.Sqrt(SqrLength());
        }
        public Vector3 Normalize()
        {
            double inv = 1 / Length();
            return new Vector3(x * inv, y * inv, z * inv);
        }
        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.x, -v.y, -v.z);
        }
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }
        public static Vector3 operator *(Vector3 v, double d)
        {
            return new Vector3(v.x * d, v.y * d, v.z * d);
        }
        public static Vector3 operator /(Vector3 v, double d)
        {
            double inv = 1 / d;
            return new Vector3(v.x * inv, v.y * inv, v.z * inv);
        }
        public static double operator ^(Vector3 v1, Vector3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }
        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(-v1.z * v2.y + v1.y * v2.z,
                v1.z * v2.x - v1.x * v2.z,
                -v1.y * v2.x + v1.x * v2.y);
        }

        public static Vector3 Zero
        {
            get
            {
                return new Vector3(0, 0, 0);
            }
        }
    }
}
