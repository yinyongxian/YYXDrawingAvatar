using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace YYX.DrawingAvatar
{
    public partial class MainForm : Form
    {

        private Bitmap bitmap;
        public MainForm()
        {
            InitializeComponent();
            comboBoxRadius.SelectedIndex = 3;
            buttonOK_Click(null, null);
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            int radius;
            var tryParse = int.TryParse(comboBoxRadius.Text, out radius);
            if (!tryParse)
            {
                MessageBox.Show(@"请设置正确的半径");
                return;
            }

            var imageSize = 4 * radius;
            bitmap?.Dispose();
            bitmap = new Bitmap(imageSize, imageSize, PixelFormat.Format32bppArgb);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Transparent);
                graphics.Clear(Color.White);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality; 
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.CompositingQuality = CompositingQuality.HighQuality;

                float center = (imageSize - 2) / 2f;       
                float sin30 = (float)(radius * Math.Sin(30 * Math.PI / 180));
                float cos30 = (float)(radius * Math.Cos(30 * Math.PI / 180));

                DrawCircle(radius, graphics, center, center, Color.Red);
                DrawCircle(radius, graphics, center + radius, center, Color.Orange);
                DrawCircle(radius, graphics, center + sin30, center - cos30, Color.Yellow);
                DrawCircle(radius, graphics, center - sin30, center - cos30, Color.Green);
                DrawCircle(radius, graphics, center - radius, center, Color.Cyan);
                DrawCircle(radius, graphics, center - sin30, center + cos30, Color.Blue);
                DrawCircle(radius, graphics, center + sin30, center + cos30, Color.Purple);
            }

            pictureBox.Image = bitmap;
        }



        private static void DrawCircle(float radius, Graphics graphics, float cx, float cy, Color color)
        {
            float drawRadius = radius - 1f;
            using (Pen pen = new Pen(color, 2))
            {
                graphics.DrawEllipse(
                    pen,
                    cx - drawRadius,
                    cy - drawRadius,
                    radius * 2,
                    radius * 2);
            }
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            ContextMenuStrip = null;

            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            var contextMenuStrip = new ContextMenuStrip();
            var toolStripItem = contextMenuStrip.Items.Add("图片另存为");
            toolStripItem.Click += SaveImage;

            ContextMenuStrip = contextMenuStrip;
            contextMenuStrip.Show(MousePosition);
        }

        private void SaveImage(object sender, EventArgs e)
        {
            if (bitmap == null) 
            { 
                return; 
            }
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                string filename = $"Avatar-{DateTime.Now:yyyyMMddHHmmss}.png"; 
                string path = Path.Combine(folderBrowserDialog.SelectedPath, filename); 
                bitmap.Save(path, ImageFormat.Png);
            }
        }
    }
}
