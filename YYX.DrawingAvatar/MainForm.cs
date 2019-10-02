using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
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

            var sideLength = 2 * radius;
            bitmap = new Bitmap(sideLength, sideLength);
            var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            var pen = new Pen(Color.Black);
            var size = new Size(sideLength - 1, sideLength - 1);
            var location = new Point(0, 0);
            var rectangle = new Rectangle(location, size);
            graphics.DrawEllipse(pen, rectangle);

            var sizeHeight = size.Height / 1F;
            var circleRadius = sizeHeight / 2F;
            const double d = (30 / 180F) * Math.PI;
            var sin = Math.Sin(d);
            var cos = Math.Cos(d);
            var sinLength = (float)(circleRadius * sin);
            var cosLength = (float)(circleRadius * cos);

            graphics.DrawArc(pen, 0F, -circleRadius, sizeHeight, sizeHeight, 30F, 120F);
            graphics.DrawArc(pen, -(circleRadius - (circleRadius - cosLength)), sinLength, sizeHeight, sizeHeight, 270F, 120F);
            graphics.DrawArc(pen, circleRadius - (circleRadius - cosLength), sinLength, sizeHeight, sizeHeight, 150F, 120F);

            pictureBox.Image = bitmap;
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
