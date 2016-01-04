
namespace RayTracer.Model.Materials
{
    class Material
    {
        double reflectiveness;

        public Material(double _reflectiveness = 0)
        {
            reflectiveness = _reflectiveness;
        }
        public virtual Color Sample(Ray3 ray, Vector3 position, Vector3 normal)
        {
            return Color.Black;
        }
    }
}
