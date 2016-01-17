using RayTracer.Model.Materials;

namespace RayTracer.Model.Geometries
{
    class Triangle : Geometry
    {
        Vector3 a;

        public Vector3 A
        {
            get { return a; }
        }

        Vector3 b;

        public Vector3 B
        {
            get { return b; }
        }

        Vector3 c;

        public Vector3 C
        {
            get { return c; }
        }

        Vector3 n1;

        public Vector3 N1
        {
            get { return n1; }
        }

        Vector3 n2;

        public Vector3 N2
        {
            get { return n2; }
        }

        Vector3 n3;

        public Vector3 N3
        {
            get { return n3; }
        }

        Vector3 edgeAB;
        Vector3 edgeAC;
        Vector3 normal;
        bool hasVertexNormals = false;

        public Triangle(Vector3 _a, Vector3 _b, Vector3 _c, Material _material = null)
            : base(_material)
        {
            a = _a;
            b = _b;
            c = _c;
            hasVertexNormals = false;
        }

        public Triangle(Vector3 _a, Vector3 _n1, Vector3 _b, Vector3 _n2, Vector3 _c, Vector3 _n3, Material _material = null)
            : base(_material)
        {
            a = _a;
            b = _b;
            c = _c;
            n1 = _n1;
            n2 = _n2;
            n3 = _n3;
            hasVertexNormals = true;
        }

        public override void Initialize()
        {
            edgeAB = b.Subtract(a);
            edgeAC = c.Subtract(a);
            normal = edgeAB.Cross(edgeAC).Normalize();
        }

        public override IntersectResult Intersect(Ray3 ray)
        {
            if (normal.SqrLength() == 0)
                return IntersectResult.NoHit();
            Vector3 w0 = ray.Origin.Subtract(a);
            double x = -normal.Dot(w0);
            double y = normal.Dot(ray.Direction);
            double distance = x / y;
            if (y == 0 || distance < 0)
                return IntersectResult.NoHit();
            Vector3 position = ray.GetPoint(distance);
            double uu = edgeAB.Dot(edgeAB);
            double uv = edgeAB.Dot(edgeAC);
            double vv = edgeAC.Dot(edgeAC);
            Vector3 w = position.Subtract(A);
            double wu = w.Dot(edgeAB);
            double wv = w.Dot(edgeAC);
            double D = uv * uv - uu * vv;
            double beta = (uv * wv - vv * wu) / D;
            if (beta < 0 || beta > 1)
                return IntersectResult.NoHit();
            double gamma = (uv * wu - uu * wv) / D;
            if (gamma < 0 || beta + gamma > 1)
                return IntersectResult.NoHit();
            double alpha = 1 - beta - gamma;
            Vector3 newNormal = normal;
            if (distance > 0 && hasVertexNormals)
            {
                Vector3 n1Interpolated = n1.Multiply(alpha);
                Vector3 n2Interpolated = n2.Multiply(beta);
                Vector3 n3Interpolated = n3.Multiply(gamma);
                newNormal = n1Interpolated.Add(n2Interpolated).Add(n3Interpolated);
            }
            return new IntersectResult(this, distance, position, newNormal);
        }
    }
}
