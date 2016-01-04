using System;

namespace RayTracer.Model.Materials
{
    class PhongMaterial : Material
    {
        static Vector3 lightDirection = new Vector3(1, 1, 1).Normalize();
        static Color lightColor = Color.White;

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
        public override Color Sample(Ray3 ray, Vector3 position, Vector3 normal)
        {
            double NdotL = normal.Dot(lightDirection);
            Vector3 H = (lightDirection.Subtract(ray.Direction)).Normalize();
            double NdotH = normal.Dot(H);
            Color diffuseTerm = diffuse.Multiply(Math.Max(NdotL, 0));
            Color specularTerm = specular.Multiply(Math.Pow(Math.Max(NdotH, 0), shininess));
            return lightColor.Modulate(diffuseTerm.Add(specularTerm));
        }
    }
}
