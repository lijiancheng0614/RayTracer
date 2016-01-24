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
            // TODO
            Form1.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Size size = GetSize();
            int width = size.Width;
            int height = size.Height;
            if (width <= 0 || height <= 0)
                return;
            int state = comboBox1.SelectedIndex + 1;
            EventHandler taskEndEventHandler = new EventHandler(RenderDone);
            switch (state)
            {
                case 1:
                    Config.TestDepth(width, height, taskEndEventHandler);
                    break;
                case 2:
                    Config.TestMaterial(width, height, taskEndEventHandler);
                    break;
                case 3:
                    Config.TestRayTracing(width, height, taskEndEventHandler);
                    break;
                case 4:
                    Config.TestDirectionalLight(width, height, taskEndEventHandler);
                    break;
                case 5:
                    Config.TestPointLight(width, height, taskEndEventHandler);
                    break;
                case 6:
                    Config.TestSpotLight(width, height, taskEndEventHandler);
                    break;
                case 7:
                    Config.TestTrichromatismLights(width, height, taskEndEventHandler);
                    break;
                case 8:
                    Config.TestManyLights(width, height, taskEndEventHandler); ;
                    break;
                case 9:
                    Config.TestObjModel(width, height, taskEndEventHandler);
                    break;
                case 10:
                    Config.TestObjModelOctree(width, height, taskEndEventHandler);
                    break;
                case 11:
                    Config.TestObjModelOctreeMultiThread(width, height, taskEndEventHandler);
                    break;
                default:
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Size size = GetSize();
            int width = size.Width;
            int height = size.Height;
            if (width <= 0 || height <= 0)
                return;
            pictureBox1.Image = new Bitmap(width, height);
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

        private void RenderDone(object sender, EventArgs e)
        {
            RenderEventArgs renderEventArgs = (RenderEventArgs)e;
            pictureBox1.Image = renderEventArgs.Image;
            label3.Text = "Done in " + renderEventArgs.RenderTime.ToString() + " s.";
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
