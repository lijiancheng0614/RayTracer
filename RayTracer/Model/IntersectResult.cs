using System;
using RayTracer.Model.Geometries;

namespace RayTracer.Model
{
    class IntersectResult
    {
        Geometry geometry;

        public Geometry Geometry
        {
            get { return geometry; }
        }

        double distance;

        public double Distance
        {
            get { return distance; }
        }

        Vector3 position;

        public Vector3 Position
        {
            get { return position; }
        }

        Vector3 normal;

        public Vector3 Normal
        {
            get { return normal; }
        }

        Vector2 textureCoordinates;

        public Vector2 TextureCoordinates
        {
            get { return textureCoordinates; }
            set { textureCoordinates = value; }
        }

        public IntersectResult(Geometry geometry, double distance, Vector3 position, Vector3 normal)
        {
            this.geometry = geometry;
            this.distance = distance;
            this.position = position;
            this.normal = normal;
        }

        public Color GetDepthColor(int maxDepth = 20)
        {
            if (geometry == null)
                return Color.Black;
            double depth = 1.0 - Math.Min(distance / maxDepth, 1.0);
            return new Color(depth, depth, depth);
        }

        public static IntersectResult NoHit()
        {
            return new IntersectResult(null, 0, Vector3.Zero, Vector3.Zero);
        }
    }
}
