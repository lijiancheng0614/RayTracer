using System.Drawing;
using System.Windows.Forms;
using RayTracer.Model;
using RayTracer.Model.Geometries;
using RayTracer.Model.Materials;
using System;
using RayTracer.Model.Lights;

namespace RayTracer
{
    public partial class Form1 : Form
    {
        Graphics g;
        Point startDrawingPoint = new Point(180, 20);
        int state = 0;

        public Form1()
        {
            InitializeComponent();
            g = this.CreateGraphics();
            this.Show();
            comboBox1.SelectedIndex = 0;
        }

        void renderDepth(Graphics g, int w, int h, int maxDepth = 20)
        {
            UnionGeometry scene = new UnionGeometry();
            scene.Add(new Sphere(new Vector3(0, 10, -10), 10));
            scene.Add(new Plane(new Vector3(0, 1, 0), 0));
            scene.Initialize();

            PerspectiveCamera camera = new PerspectiveCamera(new Vector3(0, 10, 10), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 90);
            camera.Initialize();

            double dx = 1.0 / w;
            double dy = 1.0 / h;
            double dD = 255 / maxDepth;
            Bitmap bitmap = new Bitmap(w, h);

            for (int y = 0; y < h; ++y)
            {
                double sy = 1 - dy * y;
                for (int x = 0; x < w; ++x)
                {
                    double sx = dx * x;
                    Ray3 ray = new Ray3(camera.GenerateRay(sx, sy));
                    IntersectResult result = scene.Intersect(ray);
                    if (result.Geometry != null)
                    {
                        int depth = 255 - (int)Math.Min(result.Distance * dD, 255);
                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, depth, depth, depth));
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, System.Drawing.Color.Black);
                    }
                }
            }

            g.DrawImage(bitmap, startDrawingPoint);
        }

        void renderMaterial(Graphics g, int w, int h)
        {
            UnionGeometry scene = new UnionGeometry();
            scene.Add(new Plane(new Vector3(0, 1, 0), 0, new CheckerMaterial(0.1)));
            scene.Add(new Sphere(new Vector3(-10, 10, -10), 10, new PhongMaterial(Model.Color.Red, Model.Color.White, 16)));
            scene.Add(new Sphere(new Vector3(10, 10, -10), 10, new PhongMaterial(Model.Color.Blue, Model.Color.White, 16)));
            scene.Initialize();

            PerspectiveCamera camera = new PerspectiveCamera(new Vector3(0, 5, 15), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 90);
            camera.Initialize();

            double dx = 1.0 / w;
            double dy = 1.0 / h;

            Bitmap bitmap = new Bitmap(w, h);

            for (int y = 0; y < h; ++y)
            {
                double sy = 1 - dy * y;
                for (int x = 0; x < w; ++x)
                {
                    double sx = dx * x;
                    Ray3 ray = new Ray3(camera.GenerateRay(sx, sy));
                    IntersectResult result = scene.Intersect(ray);
                    if (result.Geometry != null)
                    {
                        Model.Color color = result.Geometry.Material.Sample(ray, result.Position, result.Normal);
                        color.Saturate();
                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, (int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255)));
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, System.Drawing.Color.Black);
                    }
                }
            }

            g.DrawImage(bitmap, startDrawingPoint);
        }

        Model.Color rayTracing(Geometry scene, Ray3 ray, int maxReflect)
        {
            IntersectResult result = scene.Intersect(ray);
            if (result.Geometry != null)
            {
                double reflectiveness = result.Geometry.Material.Reflectiveness;
                Model.Color color = result.Geometry.Material.Sample(ray, result.Position, result.Normal);
                color = color.Multiply(1 - reflectiveness);

                if (reflectiveness > 0 && maxReflect > 0)
                {
                    Vector3 r = result.Normal.Multiply(-2 * result.Normal.Dot(ray.Direction)).Add(ray.Direction);
                    Ray3 newRay = new Ray3(result.Position, r);
                    Model.Color reflectedColor = rayTracing(scene, newRay, maxReflect - 1);
                    color = color.Add(reflectedColor.Multiply(reflectiveness));
                }
                return color;
            }
            return Model.Color.Black;
        }

        void rayTracing(Graphics g, int w, int h, int maxReflect = 3)
        {
            UnionGeometry scene = new UnionGeometry();
            scene.Add(new Plane(new Vector3(0, 1, 0), 0, new CheckerMaterial(0.1, 0.5)));
            scene.Add(new Sphere(new Vector3(-10, 10, -10), 10, new PhongMaterial(Model.Color.Red, Model.Color.White, 16, 0.25)));
            scene.Add(new Sphere(new Vector3(10, 10, -10), 10, new PhongMaterial(Model.Color.Blue, Model.Color.White, 16, 0.25)));
            scene.Initialize();

            PerspectiveCamera camera = new PerspectiveCamera(new Vector3(0, 5, 15), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 90);
            camera.Initialize();

            double dx = 1.0 / w;
            double dy = 1.0 / h;

            Bitmap bitmap = new Bitmap(w, h);

            for (int y = 0; y < h; ++y)
            {
                double sy = 1 - dy * y;
                for (int x = 0; x < w; ++x)
                {
                    double sx = dx * x;
                    Ray3 ray = new Ray3(camera.GenerateRay(sx, sy));
                    Model.Color color = rayTracing(scene, ray, maxReflect);
                    color.Saturate();
                    bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, (int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255)));
                }
            }
            g.DrawImage(bitmap, startDrawingPoint);
        }

        void renderLight(Graphics g, int w, int h, UnionLight light)
        {
            UnionGeometry scene = new UnionGeometry();
            scene.Add(new Plane(new Vector3(0, 1, 0), 0));
            scene.Add(new Plane(new Vector3(0, 0, 1), -50));
            scene.Add(new Plane(new Vector3(1, 0, 0), -20));
            scene.Add(new Sphere(new Vector3(0, 10, -10), 10));
            scene.Initialize();

            PerspectiveCamera camera = new PerspectiveCamera(new Vector3(0, 10, 10), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 90);
            camera.Initialize();

            light.Initialize();

            double dx = 1.0 / w;
            double dy = 1.0 / h;

            Bitmap bitmap = new Bitmap(w, h);
            for (int y = 0; y < h; ++y)
            {
                double sy = 1 - dy * y;
                for (int x = 0; x < w; ++x)
                {
                    double sx = dx * x;
                    var ray = camera.GenerateRay(sx, sy);
                    var result = scene.Intersect(ray);
                    if (result.Geometry != null)
                    {
                        Model.Color color = light.GetColor(scene, result);
                        color.Saturate();
                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, (int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255)));
                    }
                }
            }
            g.DrawImage(bitmap, startDrawingPoint);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            int w;
            int h;
            try
            {
                w = int.Parse(textBox1.Text);
                h = int.Parse(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("请输入整数的宽高！");
                return;
            }
            if (w > 1280 || h > 720)
            {
                MessageBox.Show("宽高太大！");
                return;
            }
            state = comboBox1.SelectedIndex + 1;
            switch (state)
            {
                case 1:
                    renderDepth(g, w, h);
                    break;
                case 2:
                    renderMaterial(g, w, h);
                    break;
                case 3:
                    rayTracing(g, w, h);
                    break;
                case 4:
                    UnionLight light4 = new UnionLight();
                    light4.Add(new DirectionalLight(Model.Color.White, new Vector3(-1.75, -2, -1.5)));
                    renderLight(g, w, h, light4);
                    break;
                case 5:
                    UnionLight light5 = new UnionLight();
                    light5.Add(new PointLight(Model.Color.White.Multiply(2000), new Vector3(30, 40, 20)));
                    renderLight(g, w, h, light5);
                    break;
                default:
                    break;
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            state = 0;
            this.Invalidate();
        }
    }
}
