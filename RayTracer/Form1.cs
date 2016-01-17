using System;
using System.Drawing;
using System.Windows.Forms;

namespace RayTracer
{
    public partial class Form1 : Form
    {
        int state = 0;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 8;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            int width;
            int height;
            try
            {
                width = int.Parse(textBox1.Text);
                height = int.Parse(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("请输入整数的宽高！");
                return;
            }
            if (width > 1280 || height > 720)
            {
                MessageBox.Show("宽高太大！");
                return;
            }
            state = comboBox1.SelectedIndex + 1;
            DateTime startDateTime = DateTime.Now;
            Bitmap bitmap = new Bitmap(width, height);
            switch (state)
            {
                case 1:
                    bitmap = Config.GetDepthBitmap(width, height);
                    break;
                case 2:
                    bitmap = Config.GetMaterialBitmap(width, height);
                    break;
                case 3:
                    bitmap = Config.GetRayTracingBitmap(width, height);
                    break;
                case 4:
                    bitmap = Config.GetDirectionalLightBitmap(width, height);
                    break;
                case 5:
                    bitmap = Config.GetPointLightBitmap(width, height);
                    break;
                case 6:
                    bitmap = Config.GetSpotLightBitmap(width, height);
                    break;
                case 7:
                    bitmap = Config.GetTrichromatismLightsBitmap(width, height);
                    break;
                case 8:
                    bitmap = Config.GetManyLightsBitmap(width, height); ;
                    break;
                case 9:
                    bitmap = Config.GetObjModelBitmap(width, height);
                    break;
                default:
                    break;
            }
            pictureBox1.Width = width;
            pictureBox1.Height = height;
            pictureBox1.Image = bitmap;
            label2.Text = "Done in " + (DateTime.Now - startDateTime).TotalSeconds.ToString() + " s.";
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            state = 0;
            this.Invalidate();
        }
    }
}
