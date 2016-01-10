using RayTracer.Model.Lights;

namespace RayTracer.Model.Materials
{
    class Material
    {
        double reflectiveness;

        public double Reflectiveness
        {
            get { return reflectiveness; }
        }

        public Material(double _reflectiveness = 0)
        {
            reflectiveness = _reflectiveness;
        }
        public virtual Color Sample(Ray3 ray, Vector3 normal, Vector3 position, LightSample lightSample)
        {
            return Color.Black;
        }
    }
}
