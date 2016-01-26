using RayTracer.Model.Lights;

namespace RayTracer.Model.Materials
{
    abstract class Material
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

        public abstract Color Sample(Ray3 ray, LightSample lightSample, Vector3 normal, Vector3 position, Vector2 textureCoordinates = null);
    }
}
