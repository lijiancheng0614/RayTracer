using System;

namespace RayTracer.Model.Materials
{
    class CheckerMaterial : Material
    {
        double scale;

        public CheckerMaterial(double _scale, double _reflectiveness = 0)
            : base(_reflectiveness)
        {
            scale = _scale;
        }
        public override Color Sample(Ray3 ray, Vector3 position, Vector3 normal)
        {
            double d = Math.Abs((Math.Floor(position.X * scale) + Math.Floor(position.Z * scale)));
            d = d % 2;
            return d < 1 ? Color.Black : Color.White;
        }
    }
}
