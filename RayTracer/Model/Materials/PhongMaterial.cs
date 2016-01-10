using System;
using RayTracer.Model.Lights;

namespace RayTracer.Model.Materials
{
    class PhongMaterial : Material
    {
        Color diffuse;
        Color specular;
        double shininess;

        public PhongMaterial(Color _diffuse, Color _specular, double _shininess, double _reflectiveness = 0)
            : base(_reflectiveness)
        {
            diffuse = _diffuse;
            specular = _specular;
            shininess = _shininess;
        }
        public override Color Sample(Ray3 ray, Vector3 normal, Vector3 position, LightSample lightSample)
        {
            Color color = Color.Black;
            double NdotL = normal.Dot(lightSample.Vector);
            Vector3 H = (lightSample.Vector.Subtract(ray.Direction)).Normalize();
            double NdotH = normal.Dot(H);
            Color diffuseTerm = diffuse.Multiply(Math.Max(NdotL, 0));
            Color specularTerm = specular.Multiply(Math.Pow(Math.Max(NdotH, 0), shininess));
            color = lightSample.Irradiance.Modulate(diffuseTerm.Add(specularTerm));
            return color;
        }
    }
}
