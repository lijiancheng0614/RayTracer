using System;
using System.Drawing;
using System.Windows.Forms;

namespace RayTracer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Size size = GetSize();
            int width = size.Width;
            int height = size.Height;
            if (width <= 0 || height <= 0)
                return;
            int state = comboBox1.SelectedIndex + 1;
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
                case 10:
                    bitmap = Config.GetObjModelOctreeBitmap(width, height);
                    break;
                default:
                    break;
            }
            pictureBox1.Size = size;
            pictureBox1.Image = bitmap;
            label3.Text = "Done in " + (DateTime.Now - startDateTime).TotalSeconds.ToString() + " s.";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Size size = GetSize();
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            pictureBox1.Size = size;
            pictureBox1.Image = bitmap;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("You need to draw something first!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = comboBox1.Text + ".png";
            saveFileDialog.DefaultExt = "png";
            saveFileDialog.Filter = "PNG files (*.png)|*.png|" + "All files|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog.FileName);
            }
        }

        private Size GetSize()
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
                MessageBox.Show("Please enter width and height in integer.");
                return new Size();
            }
            return new Size(width, height);
        }

    }
}
