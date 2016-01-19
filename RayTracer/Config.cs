using System.Drawing;
using RayTracer.Model;
using RayTracer.Model.Geometries;
using RayTracer.Model.Lights;
using RayTracer.Model.Materials;
using RayTracer.Model.ObjModels;

namespace RayTracer
{
    class Config
    {
        static UnionGeometry DefaultGeometries
        {
            get
            {
                UnionGeometry geometries = new UnionGeometry();
                geometries.Add(new Plane(new Vector3(0, 1, 0), 0, new PhongMaterial(Model.Color.White, Model.Color.Black, 0)));
                geometries.Add(new Plane(new Vector3(0, 0, 1), -50, new PhongMaterial(Model.Color.White, Model.Color.Black, 0)));
                geometries.Add(new Plane(new Vector3(1, 0, 0), -20, new PhongMaterial(Model.Color.White, Model.Color.Black, 0)));
                geometries.Add(new Sphere(new Vector3(0, 10, -10), 10, new PhongMaterial(Model.Color.White, Model.Color.Black, 0)));
                return geometries;
            }
        }

        static UnionLight DefaultLight
        {
            get
            {
                UnionLight lights = new UnionLight();
                lights.Add(new DirectionalLight(Model.Color.White, new Vector3(-1, -1, -1), false));
                return lights;
            }
        }

        static PerspectiveCamera DefaultCamera
        {
            get
            {
                return new PerspectiveCamera(new Vector3(0, 10, 10), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 90);
            }
        }

        public static Bitmap GetDepthBitmap(int width, int height)
        {
            UnionGeometry geometries = new UnionGeometry();
            geometries.Add(new Sphere(new Vector3(0, 10, -10), 10));
            geometries.Add(new Plane(new Vector3(0, 1, 0), 0));
            UnionLight lights = new UnionLight();
            Scene scene = new Scene(geometries, lights, DefaultCamera);
            scene.Initialize();
            return scene.GetSystemBitmap(width, height, true);
        }

        public static Bitmap GetMaterialBitmap(int width, int height)
        {
            UnionGeometry geometries = new UnionGeometry();
            geometries.Add(new Plane(new Vector3(0, 1, 0), 0, new CheckerMaterial(0.1)));
            geometries.Add(new Sphere(new Vector3(-10, 10, -10), 10, new PhongMaterial(Model.Color.Red, Model.Color.White, 16)));
            geometries.Add(new Sphere(new Vector3(10, 10, -10), 10, new PhongMaterial(Model.Color.Blue, Model.Color.White, 16)));
            PerspectiveCamera camera = new PerspectiveCamera(new Vector3(0, 5, 15), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 90);
            Scene scene = new Scene(geometries, DefaultLight, camera);
            scene.Initialize();
            return scene.GetSystemBitmap(width, height);
        }

        public static Bitmap GetRayTracingBitmap(int width, int height)
        {
            UnionGeometry geometries = new UnionGeometry();
            geometries.Add(new Plane(new Vector3(0, 1, 0), 0, new CheckerMaterial(0.1, 0.5)));
            geometries.Add(new Sphere(new Vector3(-10, 10, -10), 10, new PhongMaterial(Model.Color.Red, Model.Color.White, 16, 0.25)));
            geometries.Add(new Sphere(new Vector3(10, 10, -10), 10, new PhongMaterial(Model.Color.Blue, Model.Color.White, 16, 0.25)));
            PerspectiveCamera camera = new PerspectiveCamera(new Vector3(0, 5, 15), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 90);
            Scene scene = new Scene(geometries, DefaultLight, camera);
            scene.Initialize();
            return scene.GetSystemBitmap(width, height, false, 3);
        }

        public static Bitmap GetDirectionalLightBitmap(int width, int height)
        {
            UnionLight lights = new UnionLight();
            lights.Add(new DirectionalLight(Model.Color.White, new Vector3(-1.75, -2, -1.5)));
            Scene scene = new Scene(DefaultGeometries, lights, DefaultCamera);
            scene.Initialize();
            return scene.GetSystemBitmap(width, height);
        }

        public static Bitmap GetPointLightBitmap(int width, int height)
        {
            UnionLight lights = new UnionLight();
            lights.Add(new PointLight(Model.Color.White.Multiply(2000), new Vector3(30, 40, 20)));
            Scene scene = new Scene(DefaultGeometries, lights, DefaultCamera);
            scene.Initialize();
            return scene.GetSystemBitmap(width, height);
        }

        public static Bitmap GetSpotLightBitmap(int width, int height)
        {
            UnionLight lights = new UnionLight();
            lights.Add(new SpotLight(Model.Color.White.Multiply(2000), new Vector3(30, 40, 20), new Vector3(-1, -1, -1), 20, 30, 0.5));
            Scene scene = new Scene(DefaultGeometries, lights, DefaultCamera);
            scene.Initialize();
            return scene.GetSystemBitmap(width, height);
        }

        public static Bitmap GetTrichromatismLightsBitmap(int width, int height)
        {
            UnionLight lights = new UnionLight();
            lights.Add(new PointLight(Model.Color.White.Multiply(1000), new Vector3(30, 40, 20)));
            lights.Add(new SpotLight(Model.Color.Red.Multiply(3000), new Vector3(0, 30, 10), new Vector3(0, -1, -1), 20, 30, 1));
            lights.Add(new SpotLight(Model.Color.Green.Multiply(3000), new Vector3(6, 30, 20), new Vector3(0, -1, -1), 20, 30, 1));
            lights.Add(new SpotLight(Model.Color.Blue.Multiply(3000), new Vector3(-6, 30, 20), new Vector3(0, -1, -1), 20, 30, 1));
            Scene scene = new Scene(DefaultGeometries, lights, DefaultCamera);
            scene.Initialize();
            return scene.GetSystemBitmap(width, height);
        }

        public static Bitmap GetManyLightsBitmap(int width, int height)
        {
            UnionLight lights = new UnionLight();
            for (int x = 10; x <= 30; x += 4)
                for (int z = 20; z <= 40; z += 4)
                    lights.Add(new PointLight(Model.Color.White.Multiply(80), new Vector3(x, 50, z)));
            DirectionalLight fillLight = new DirectionalLight(Model.Color.White.Multiply(0.25), new Vector3(1.5, 1, 0.5), false);
            lights.Add(fillLight);
            Scene scene = new Scene(DefaultGeometries, lights, DefaultCamera);
            scene.Initialize();
            return scene.GetSystemBitmap(width, height);
        }

        public static Bitmap GetObjModelBitmap(int width, int height)
        {
            UnionGeometry geometries = new UnionGeometry();
            ObjModel objModel = new ObjModel("models/dinosaur.2k.obj");
            foreach (var triangle in objModel.Triangles)
            {
                geometries.Add(triangle);
            }
            Vector3 eye = new Vector3(-186.231323, -86.534691, 38.299175);
            Vector3 front = new Vector3(0.906127, 0.375330, -0.195090);
            Vector3 up = new Vector3(0.180237, 0.074646, 0.980787);
            PerspectiveCamera camera = new PerspectiveCamera(eye, front, up, 30);
            Scene scene = new Scene(geometries, DefaultLight, camera);
            scene.Initialize();
            return scene.GetSystemBitmap(width, height);
        }

        public static Bitmap GetObjModelOctreeBitmap(int width, int height)
        {
            UnionGeometry geometries = new UnionGeometry();
            ObjModel objModel = new ObjModel("models/dinosaur.2k.obj");
            Octree octree = new Octree(objModel.Triangles);
            geometries.Add(octree);
            Vector3 eye = new Vector3(-186.231323, -86.534691, 38.299175);
            Vector3 front = new Vector3(0.906127, 0.375330, -0.195090);
            Vector3 up = new Vector3(0.180237, 0.074646, 0.980787);
            PerspectiveCamera camera = new PerspectiveCamera(eye, front, up, 30);
            Scene scene = new Scene(geometries, DefaultLight, camera);
            scene.Initialize();
            return scene.GetSystemBitmap(width, height);
        }
    }
}
