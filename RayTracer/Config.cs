using System;
using RayTracer.Model;
using RayTracer.Model.Geometries;
using RayTracer.Model.Lights;
using RayTracer.Model.Materials;
using RayTracer.Model.ObjModels;

namespace RayTracer
{
    class Config
    {
        static PhongMaterial WallMaterial = new PhongMaterial(Model.Color.Black, new Model.Color(0.64, 0.64, 0.64), new Model.Color(0.5, 0.5, 0.5), 96.078431);
        static PhongMaterial FloorMaterial = new PhongMaterial(Model.Color.Black, new Model.Color(0.64, 0.64, 0.64), new Model.Color(0.5, 0.5, 0.5), 96.078431, 0.2);
        static UnionGeometry DefaultGeometries
        {
            get
            {
                UnionGeometry geometries = new UnionGeometry();
                geometries.Add(new Plane(new Vector3(0, 1, 0), 0, WallMaterial));
                geometries.Add(new Plane(new Vector3(0, 0, 1), -50, WallMaterial));
                geometries.Add(new Plane(new Vector3(1, 0, 0), -20, FloorMaterial));
                geometries.Add(new Sphere(new Vector3(0, 10, -10), 10, new PhongMaterial(Model.Color.Black, Model.Color.White, Model.Color.Black, 0)));
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

        public static void TestDepth(int width, int height, EventHandler taskEndEventHandler)
        {
            UnionGeometry geometries = new UnionGeometry();
            geometries.Add(new Sphere(new Vector3(0, 10, -10), 10));
            geometries.Add(new Plane(new Vector3(0, 1, 0), 0));
            UnionLight lights = new UnionLight();
            Scene scene = new Scene(geometries, lights, DefaultCamera);
            scene.Initialize();
            scene.GetImage(width, height, taskEndEventHandler, 8, true);
        }

        public static void TestMaterial(int width, int height, EventHandler taskEndEventHandler)
        {
            UnionGeometry geometries = new UnionGeometry();
            geometries.Add(new Plane(new Vector3(0, 1, 0), 0, new CheckerMaterial(0.1)));
            geometries.Add(new Sphere(new Vector3(-10, 10, -10), 10, new PhongMaterial(Model.Color.Black, Model.Color.Red, Model.Color.White, 16)));
            geometries.Add(new Sphere(new Vector3(10, 10, -10), 10, new PhongMaterial(Model.Color.Black, Model.Color.Blue, Model.Color.White, 16)));
            PerspectiveCamera camera = new PerspectiveCamera(new Vector3(0, 5, 15), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 90);
            Scene scene = new Scene(geometries, DefaultLight, camera);
            scene.Initialize();
            scene.GetImage(width, height, taskEndEventHandler, 8);
        }

        public static void TestRayTracing(int width, int height, EventHandler taskEndEventHandler)
        {
            UnionGeometry geometries = new UnionGeometry();
            geometries.Add(new Plane(new Vector3(0, 1, 0), 0, new CheckerMaterial(0.1, 0.5)));
            geometries.Add(new Sphere(new Vector3(-10, 10, -10), 10, new PhongMaterial(Model.Color.Black, Model.Color.Red, Model.Color.White, 16, 0.25)));
            geometries.Add(new Sphere(new Vector3(10, 10, -10), 10, new PhongMaterial(Model.Color.Black, Model.Color.Blue, Model.Color.White, 16, 0.25)));
            PerspectiveCamera camera = new PerspectiveCamera(new Vector3(0, 5, 15), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 90);
            Scene scene = new Scene(geometries, DefaultLight, camera);
            scene.Initialize();
            scene.GetImage(width, height, taskEndEventHandler, 8, false, 3);
        }

        public static void TestDirectionalLight(int width, int height, EventHandler taskEndEventHandler)
        {
            UnionLight lights = new UnionLight();
            lights.Add(new DirectionalLight(Model.Color.White, new Vector3(-1.75, -2, -1.5)));
            Scene scene = new Scene(DefaultGeometries, lights, DefaultCamera);
            scene.Initialize();
            scene.GetImage(width, height, taskEndEventHandler);
        }

        public static void TestPointLight(int width, int height, EventHandler taskEndEventHandler)
        {
            UnionLight lights = new UnionLight();
            lights.Add(new PointLight(Model.Color.White * 2000, new Vector3(30, 40, 20)));
            Scene scene = new Scene(DefaultGeometries, lights, DefaultCamera);
            scene.Initialize();
            scene.GetImage(width, height, taskEndEventHandler, 8, false, 3);
        }

        public static void TestSpotLight(int width, int height, EventHandler taskEndEventHandler)
        {
            UnionLight lights = new UnionLight();
            lights.Add(new SpotLight(Model.Color.White * 2000, new Vector3(30, 40, 20), new Vector3(-1, -1, -1), 20, 30, 0.5));
            Scene scene = new Scene(DefaultGeometries, lights, DefaultCamera);
            scene.Initialize();
            scene.GetImage(width, height, taskEndEventHandler, 8, false, 3);
        }

        public static void TestTrichromatismLights(int width, int height, EventHandler taskEndEventHandler)
        {
            UnionLight lights = new UnionLight();
            lights.Add(new PointLight(Model.Color.White * 1000, new Vector3(30, 40, 20)));
            lights.Add(new SpotLight(Model.Color.Red * 3000, new Vector3(0, 30, 10), new Vector3(0, -1, -1), 20, 30, 1));
            lights.Add(new SpotLight(Model.Color.Green * 3000, new Vector3(6, 30, 20), new Vector3(0, -1, -1), 20, 30, 1));
            lights.Add(new SpotLight(Model.Color.Blue * 3000, new Vector3(-6, 30, 20), new Vector3(0, -1, -1), 20, 30, 1));
            Scene scene = new Scene(DefaultGeometries, lights, DefaultCamera);
            scene.Initialize();
            scene.GetImage(width, height, taskEndEventHandler, 8, false, 3);
        }

        public static void TestManyLights(int width, int height, EventHandler taskEndEventHandler)
        {
            UnionLight lights = new UnionLight();
            for (int x = 10; x <= 30; x += 4)
                for (int z = 20; z <= 40; z += 4)
                    lights.Add(new PointLight(Model.Color.White * 80, new Vector3(x, 50, z)));
            DirectionalLight fillLight = new DirectionalLight(Model.Color.White * 0.25, new Vector3(1.5, 1, 0.5), false);
            lights.Add(fillLight);
            Scene scene = new Scene(DefaultGeometries, lights, DefaultCamera);
            scene.Initialize();
            scene.GetImage(width, height, taskEndEventHandler, 8, false, 3);
        }


        static PerspectiveCamera ObjCamera
        {
            get
            {
                Vector3 eye = new Vector3(-186.231323, -86.534691, 38.299175);
                Vector3 front = new Vector3(0.906127, 0.375330, -0.195090);
                Vector3 up = new Vector3(0.180237, 0.074646, 0.980787);
                return new PerspectiveCamera(eye, front, up, 30);
            }
        }

        public static void TestObjModel(int width, int height, EventHandler taskEndEventHandler)
        {
            UnionGeometry geometries = new UnionGeometry();
            ObjModel objModel = new ObjModel("models/dinosaur.2k.obj");
            foreach (var triangle in objModel.Triangles)
            {
                geometries.Add(triangle);
            }
            Scene scene = new Scene(geometries, DefaultLight, ObjCamera);
            scene.Initialize();
            scene.GetImage(width, height, taskEndEventHandler, 8, false, 3);
        }

        public static void TestObjModelOctree(int width, int height, EventHandler taskEndEventHandler)
        {
            UnionGeometry geometries = new UnionGeometry();
            ObjModel objModel = new ObjModel("models/dinosaur.2k.obj");
            Octree octree = new Octree(objModel.Triangles);
            geometries.Add(octree);
            Scene scene = new Scene(geometries, DefaultLight, ObjCamera);
            scene.Initialize();
            scene.GetImage(width, height, taskEndEventHandler, 8, false, 3);
        }

        public static void TestObjModelOctreeMultiThread(int width, int height, EventHandler taskEndEventHandler)
        {
            UnionGeometry geometries = new UnionGeometry();
            ObjModel objModel = new ObjModel("models/dinosaur.2k.obj");
            Octree octree = new Octree(objModel.Triangles);
            geometries.Add(octree);
            Scene scene = new Scene(geometries, DefaultLight, ObjCamera);
            scene.Initialize();
            scene.GetImage(width, height, taskEndEventHandler, 8, false, 3);
        }

        public static void TestMtl(int width, int height, EventHandler taskEndEventHandler)
        {
            UnionGeometry geometries = new UnionGeometry();
            geometries.Add(new Plane(new Vector3(0, 1, 0), 0, FloorMaterial));
            geometries.Add(new Plane(new Vector3(1, 0, 1).Normalize(), -40, new PhongMaterial(Model.Color.Black, Model.Color.White, Model.Color.Black, 0)));
            geometries.Add(new Plane(new Vector3(-1, 0, 1).Normalize(), -40, new PhongMaterial(Model.Color.Black, Model.Color.White, Model.Color.Black, 0)));
            ObjModel objModel = new ObjModel("models/male02/male02.obj");
            Octree octree = new Octree(objModel.Triangles);
            geometries.Add(octree);
            Vector3 eye = new Vector3(-223.999146, 162.986465, 305.502930);
            Vector3 front = new Vector3(0.575724, -0.184038, -0.796663);
            Vector3 up = new Vector3(0.106187, 0.982921, -0.150301);
            PerspectiveCamera camera = new PerspectiveCamera(eye, front, up, 30);
            UnionLight lights = new UnionLight();
            lights.Add(new DirectionalLight(Model.Color.White, new Vector3(0, -1, -1), false));
            lights.Add(new PointLight(Model.Color.White * 1000, new Vector3(100, 200, 50)));
            Scene scene = new Scene(geometries, lights, camera);
            scene.Initialize();
            scene.GetImage(width, height, taskEndEventHandler, 8, false, 3);
        }
    }
}
