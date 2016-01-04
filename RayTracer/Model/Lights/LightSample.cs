
namespace RayTracer.Model.Lights
{
    class LightSample
    {
        /// <summary>
        /// light vector
        /// </summary>
        Vector3 l;

        internal Vector3 L
        {
            get { return l; }
        }

        /// <summary>
        /// irradiance
        /// </summary>
        Color irradiance;

        internal Color Irradiance
        {
            get { return irradiance; }
        }

        public LightSample(Vector3 _l, Color _irradiance)
        {
            l = _l;
            irradiance = _irradiance;
        }

        public static LightSample Zero{
            get
            {
                return new LightSample(Vector3.Zero, Color.Black);
            }
        }
    }
}
