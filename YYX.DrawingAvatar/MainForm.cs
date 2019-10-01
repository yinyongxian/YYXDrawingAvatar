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
            SaveImage(null, null);
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
            var size = new Size(sideLength, sideLength);
            var location = new Point(0, 0);
            var rectangle = new Rectangle(location, size);
            graphics.DrawEllipse(pen, rectangle);

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

            var filename = $"Avator-{DateTime.Now.ToString("yyyyMMddhhmmss")}.bmp";
            var path = Path.Combine(selectedPath, filename);
            bitmap.Save(path);
        }
    }
}
