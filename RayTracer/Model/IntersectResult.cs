using RayTracer.Model.Geometries;

namespace RayTracer.Model
{
    class IntersectResult
    {
        Geometry geometry;

        public Geometry Geometry
        {
            get { return geometry; }
            set { geometry = value; }
        }
        double distance;

        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }
        Vector3 position;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector3 normal;

        public Vector3 Normal
        {
            get { return normal; }
            set { normal = value; }
        }

        public IntersectResult()
        {
            geometry = null;
            distance = 0;
            position = Vector3.Zero;
            normal = Vector3.Zero;
        }
        public static IntersectResult NoHit()
        {
            return new IntersectResult();
        }
    }
}
