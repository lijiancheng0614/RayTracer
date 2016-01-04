using System.Drawing;
using System.Windows.Forms;
using RayTracer.Model;
using RayTracer.Model.Geometries;
using RayTracer.Model.Materials;
using System;

namespace RayTracer
{
    public partial class Form1 : Form
    {
        Point startDrawingPoint = new Point(180, 20);
        int state = 0;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            switch (state)
            {
                case 1:
                    renderMaterial(e.Graphics, int.Parse(textBox1.Text), int.Parse(textBox2.Text));
                    break;
                case 2:
                    renderDepth(e.Graphics, int.Parse(textBox1.Text), int.Parse(textBox2.Text));
                    break;
                default:
                    break;
            }
        }

        void renderDepth(Graphics g, int w, int h)
        {
            Union scene = new Union();
            scene.Add(new Sphere(new Vector3(0, 10, -10), 10));
            scene.Add(new Plane(new Vector3(0, 1, 0), 0));
            scene.Initialize();

            PerspectiveCamera camera = new PerspectiveCamera(new Vector3(0, 10, 10), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 90);
            camera.Initialize();

            double dx = 1.0 / w;
            double dy = 1.0 / h;
            int maxDepth = 20;
            double dD = 255.0f / maxDepth;
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
                        int depth = 255 - (int)Math.Min(result.Distance * dD, 255.0f);
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
            Plane plane = new Plane(new Vector3(0, 1, 0), 0);
            plane.Material = new CheckerMaterial(0.1);

            Sphere sphere1 = new Sphere(new Vector3(-10, 10, -10), 10);
            Sphere sphere2 = new Sphere(new Vector3(10, 10, -10), 10);
            sphere1.Material = new PhongMaterial(Model.Color.Red, Model.Color.White, 16);
            sphere2.Material = new PhongMaterial(Model.Color.Blue, Model.Color.White, 16);

            Union scene = new Union();
            scene.Add(plane);
            scene.Add(sphere1);
            scene.Add(sphere2);
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
            }
            state = comboBox1.SelectedIndex + 1;
            this.Invalidate();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            state = 0;
            this.Invalidate();
        }
    }
}
