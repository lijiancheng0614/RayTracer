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

        public Material(double reflectiveness = 0)
        {
            this.reflectiveness = reflectiveness;
        }
        public virtual Color Sample(Ray3 ray, Vector3 normal, Vector3 position, LightSample lightSample)
        {
            return Color.Black;
        }
    }
}
