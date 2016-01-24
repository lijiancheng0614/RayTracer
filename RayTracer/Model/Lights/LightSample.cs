
namespace RayTracer.Model.Lights
{
    class LightSample
    {
        /// <summary>
        /// light vector
        /// </summary>
        Vector3 vector;

        public Vector3 Vector
        {
            get { return vector; }
        }

        /// <summary>
        /// irradiance
        /// </summary>
        Color irradiance;

        public Color Irradiance
        {
            get { return irradiance; }
        }

        public LightSample(Vector3 vector, Color irradiance)
        {
            this.vector = vector;
            this.irradiance = irradiance;
        }

        public static LightSample Zero
        {
            get
            {
                return new LightSample(Vector3.Zero, Color.Black);
            }
        }
    }
}
