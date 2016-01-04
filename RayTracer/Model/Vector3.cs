using System;

namespace RayTracer.Model
{
    class Vector3
    {
        double x;

        public double X
        {
            get { return x; }
            set { x = value; }
        }
        double y;

        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        double z;

        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        public Vector3(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
        public Vector3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            x = v.z;
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
        public Vector3 Negate()
        {
            return new Vector3(-x, -y, -z);
        }
        public Vector3 Add(Vector3 v)
        {
            return new Vector3(x + v.x, y + v.y, z + v.z);
        }
        public Vector3 Subtract(Vector3 v)
        {
            return new Vector3(x - v.x, y - v.y, z - v.z);
        }
        public Vector3 Multiply(double f)
        {
            return new Vector3(x * f, y * f, z * f);
        }
        public Vector3 Divide(double f)
        {
            double invf = 1 / f;
            return new Vector3(x * invf, y * invf, z * invf);
        }
        public double Dot(Vector3 v)
        {
            return x * v.x + y * v.y + z * v.z;
        }
        public Vector3 Cross(Vector3 v)
        {
            return new Vector3(-z * v.y + y * v.z, z * v.x - x * v.z, -y * v.x + x * v.y);
        }

        public static Vector3 Zero()
        {
            return new Vector3(0, 0, 0);
        }
    }
}
