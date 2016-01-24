
namespace RayTracer.Model
{
    class Ray3
    {
        Vector3 origin;

        public Vector3 Origin
        {
            get { return origin; }
        }

        Vector3 direction;

        public Vector3 Direction
        {
            get { return direction; }
        }

        public Ray3(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }
        public Vector3 GetPoint(double t)
        {
            return Origin + Direction * t;
        }
    }
}
