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

        public PerspectiveCamera(Vector3 eye, Vector3 front, Vector3 refUp, double fov)
        {
            this.eye = eye;
            this.front = front;
            this.refUp = refUp;
            this.fov = fov;
            right = Vector3.Zero;
            up = Vector3.Zero;
            fovScale = 0;
        }
        public void Initialize()
        {
            right = front * refUp;
            up = right * front;
            fovScale = Math.Tan(fov * (Math.PI * 0.5f / 180)) * 2;
        }
        public Ray3 GenerateRay(double x, double y)
        {
            Vector3 r = right * ((x - 0.5) * fovScale);
            Vector3 u = up * ((y - 0.5) * fovScale);
            return new Ray3(eye, (front + r + u).Normalize());
        }
    }
}
