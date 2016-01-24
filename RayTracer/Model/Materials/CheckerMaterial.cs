using System;
using RayTracer.Model.Lights;

namespace RayTracer.Model.Materials
{
    class CheckerMaterial : Material
    {
        double scale;

        public CheckerMaterial(double scale, double reflectiveness = 0)
            : base(reflectiveness)
        {
            this.scale = scale;
        }
        public override Color Sample(Ray3 ray, Vector3 normal, Vector3 position, LightSample lightSample)
        {
            // white square emits white light.
            double d = Math.Abs((Math.Floor(position.X * scale) + Math.Floor(position.Z * scale)));
            d = d % 2;
            return d < 1 ? Color.Black : Color.White;
        }
    }
}
