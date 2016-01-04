using System;

namespace RayTracer.Model
{
    class PerspectiveCamera
    {
        Vector3 eye;
        Vector3 front;
        Vector3 refUp;
        double fov;
        Vector3 right;
        Vector3 up;
        double fovScale;

        public PerspectiveCamera(Vector3 _eye, Vector3 _front, Vector3 _refUp, double _fov)
        {
            eye = _eye;
            front = _front;
            refUp = _refUp;
            fov = _fov;
            right = Vector3.Zero();
            up = Vector3.Zero();
            fovScale = 0;
        }
        public void Initialize()
        {
            right = front.Cross(refUp);
            up = right.Cross(front);
            fovScale = Math.Tan(fov * (Math.PI * 0.5f / 180)) * 2;
        }
        public Ray3 GenerateRay(double x, double y)
        {
            Vector3 r = right.Multiply((x - 0.5f) * fovScale);
            Vector3 u = up.Multiply((y - 0.5f) * fovScale);
            return new Ray3(eye, front.Add(r).Add(u).Normalize());
        }
    }
}
