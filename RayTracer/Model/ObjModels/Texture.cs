
namespace RayTracer.Model.ObjModels
{
    class Texture
    {
        double x;

        public double X
        {
            get { return x; }
        }
        double y;

        public double Y
        {
            get { return y; }
        }

        public Texture(double _x, double _y)
        {
            x = _x;
            y = _y;
        }
    }
}
