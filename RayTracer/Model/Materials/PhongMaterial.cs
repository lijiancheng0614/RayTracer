using System;
using RayTracer.Model.Lights;

namespace RayTracer.Model.Materials
{
    class PhongMaterial : Material
    {
        Color ambient;
        Color diffuse;
        Color specular;
        double shininess;

        public PhongMaterial(Color ambient, Color diffuse, Color specular, double shininess, double reflectiveness = 0)
            : base(reflectiveness)
        {
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
            this.shininess = shininess;
        }
        public override Color Sample(Ray3 ray, Vector3 normal, Vector3 position, LightSample lightSample)
        {
            double NdotL = normal ^ lightSample.Vector;
            Vector3 H = (lightSample.Vector - ray.Direction).Normalize();
            double NdotH = normal ^ H;
            Color diffuseTerm = diffuse * (Math.Max(NdotL, 0));
            Color specularTerm = specular * (Math.Pow(Math.Max(NdotH, 0), shininess));
            Color color = lightSample.Irradiance * (ambient + diffuseTerm + specularTerm);
            return color;
        }
    }
}
