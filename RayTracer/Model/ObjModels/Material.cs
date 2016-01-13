
namespace RayTracer.Model.ObjModels
{
    class Material
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

        public Material(string _name)
        {
            name = _name;
            shininess = 10.0;
            transparency = 1.0;
            illumination = 2;
            ambientColor = Color.White;
            diffuseColor = Color.White;
            specularColor = Color.Black;
            ambientTexture = new ImageTexture();
            diffuseTexture = new ImageTexture();
            specularTexture = new ImageTexture();
        }
    }
}
