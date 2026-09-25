using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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

            var sideLength = 4 * radius;
            bitmap = new Bitmap(sideLength, sideLength);

            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;



            int cx = 2 * radius;
            int cy = 2 * radius;
            float sin30 = (float)(radius * Math.Sin(30 * Math.PI / 180));
            float cos30 = (float)(radius * Math.Cos(30 * Math.PI / 180));

            DrawCircle(radius, graphics, cx, cy);
            DrawCircle(radius, graphics, cx - radius, cy);
            DrawCircle(radius, graphics, cx + radius, cy);
            DrawCircle(radius, graphics, cx - sin30, cy - cos30);
            DrawCircle(radius, graphics, cx - sin30, cy + cos30);
            DrawCircle(radius, graphics, cx + sin30, cy - cos30);
            DrawCircle(radius, graphics, cx + sin30, cy + cos30);
     
            pictureBox.Image = bitmap;
        }



        private static void DrawCircle(int radius, Graphics graphics, float cx, float cy)
        {
            using (Pen pen = new Pen(Color.Black))
            {
                graphics.DrawEllipse(
                    pen,
                    cx - radius,
                    cy - radius,
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

            var folderBrowserDialog = new FolderBrowserDialog();
            var dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            var selectedPath = folderBrowserDialog.SelectedPath;

            var filename = $"Avator-{DateTime.Now:yyyyMMddhhmmss}.bmp";
            var path = Path.Combine(selectedPath, filename);
            bitmap.Save(path);
        }
    }
}
