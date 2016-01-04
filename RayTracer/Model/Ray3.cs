
namespace RayTracer.Model
{
    class Ray3
    {
        Vector3 origin;

        public Vector3 Origin
        {
            get { return origin; }
            set { origin = value; }
        }
        Vector3 direction;

        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Ray3(Vector3 _origin, Vector3 _direction)
        {
            origin = _origin;
            direction = _direction;
        }
        public Ray3(Ray3 r)
        {
            origin = r.origin;
            direction = r.direction;
        }
        public Vector3 GetPoint(double t)
        {
            return Origin.Add(Direction.Multiply(t));
        }
    }
}
