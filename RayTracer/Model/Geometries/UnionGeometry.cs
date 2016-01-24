using System.Collections.Generic;

namespace RayTracer.Model.Geometries
{
    class UnionGeometry : Geometry
    {
        List<Geometry> geometries;

        public UnionGeometry()
        {
            geometries = new List<Geometry>();
        }
        public void Add(Geometry geometry)
        {
            geometries.Add(geometry);
        }
        public override void Initialize()
        {
            foreach (Geometry geometry in geometries)
                geometry.Initialize();
        }
        public override IntersectResult Intersect(Ray3 ray)
        {
            double minDistance = Constant.Infinity;
            IntersectResult minResult = IntersectResult.NoHit();
            foreach (Geometry geometry in geometries)
            {
                IntersectResult result = geometry.Intersect(ray);
                if (result.Geometry != null && result.Distance < minDistance)
                {
                    minDistance = result.Distance;
                    minResult = result;
                }
            }
            return minResult;
        }
        public void Clear()
        {
            geometries.Clear();
        }
    }
}
