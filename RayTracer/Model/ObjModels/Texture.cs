
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

        public Texture(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
