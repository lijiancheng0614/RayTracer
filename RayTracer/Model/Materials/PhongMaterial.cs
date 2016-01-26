using System;
using RayTracer.Model.Lights;

namespace RayTracer.Model.Materials
{
    class PhongMaterial : Material
    {
        string name;

        public string Name
        {
            get { return name; }
        }

        double shininess;

        public double Shininess
        {
            get { return shininess; }
            set { shininess = value; }
        }

        double transparency;

        public double Transparency
        {
            get { return transparency; }
            set { transparency = value; }
        }

        int illumination;

        public int Illumination
        {
            get { return illumination; }
            set { illumination = value; }
        }

        Color ambientColor;

        public Color AmbientColor
        {
            get { return ambientColor; }
            set { ambientColor = value; }
        }

        Color diffuseColor;

        public Color DiffuseColor
        {
            get { return diffuseColor; }
            set { diffuseColor = value; }
        }

        Color specularColor;

        public Color SpecularColor
        {
            get { return specularColor; }
            set { specularColor = value; }
        }

        ImageTexture ambientTexture;

        internal ImageTexture AmbientTexture
        {
            get { return ambientTexture; }
            set { ambientTexture = value; }
        }

        ImageTexture diffuseTexture;

        internal ImageTexture DiffuseTexture
        {
            get { return diffuseTexture; }
            set { diffuseTexture = value; }
        }

        ImageTexture specularTexture;

        internal ImageTexture SpecularTexture
        {
            get { return specularTexture; }
            set { specularTexture = value; }
        }

        public PhongMaterial(string name)
        {
            this.name = name;
            shininess = 10.0;
            transparency = 1.0;
            illumination = 2;
            ambientColor = Color.Black;
            diffuseColor = Color.White;
            specularColor = Color.Black;
        }

        public PhongMaterial(Color ambientColor, Color diffuseColor, Color specularColor, double shininess, double reflectiveness = 0)
            : base(reflectiveness)
        {
            this.ambientColor = ambientColor;
            this.diffuseColor = diffuseColor;
            this.specularColor = specularColor;
            this.shininess = shininess;
        }

        public override Color Sample(Ray3 ray, LightSample lightSample, Vector3 normal, Vector3 position, Vector2 textureCoordinates = null)
        {
            double NdotL = normal ^ lightSample.Vector;
            Vector3 H = (lightSample.Vector - ray.Direction).Normalize();
            double NdotH = normal ^ H;
            Color ambientTerm = ambientColor;
            if (textureCoordinates != null && ambientTexture != null)
                ambientTerm = ambientTexture.GetColor(textureCoordinates);
            Color diffuseTerm = diffuseColor;
            if (textureCoordinates != null && diffuseTexture != null)
                diffuseTerm = diffuseTexture.GetColor(textureCoordinates);
            diffuseTerm = diffuseTerm * (Math.Max(NdotL, 0));
            Color specularTerm = specularColor;
            if (textureCoordinates != null && specularTexture != null)
                specularTerm = specularTexture.GetColor(textureCoordinates);
            specularTerm = specularColor * (Math.Pow(Math.Max(NdotH, 0), shininess));
            Color color = lightSample.Irradiance * (ambientTerm + diffuseTerm + specularTerm);
            return color;
        }
    }
}
