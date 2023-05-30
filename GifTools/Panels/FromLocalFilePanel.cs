using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GifTools.Core;
using System.Drawing.Imaging;

namespace GifTools.Panels
{
    class FromLocalFilePanel : Panel
    {
        private Label filenameLabel;
        private TextBox filenameDisplayBox;
        private Button filenameSelectButton;
        
        private PictureBox preview;
        private FlowLayoutPanel gridLayout;

        private Button saveButton;

        private Image src;
        //private Image thumbnail;


        public FromLocalFilePanel()
        {
            //BackColor = Color.White;
            Dock = DockStyle.Fill;
            filenameLabel = new Label()
            {
                Text = "Filename",
                Location = new Point(32, 55),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = GlobalConfig.DefaultFont,
            };
            filenameDisplayBox = new TextBox()
            {
                ReadOnly = true,
                BackColor = Color.White,
                Location = new Point(101, 55), 
                Size = new Size(450, 33),
            };
            
            filenameSelectButton = new Button()
            {
                Text = "Select",
                Location = new Point(561, 55),
                Size = new Size(100, 33),
                Font = GlobalConfig.DefaultFont,
            };
            filenameSelectButton.Click += OnSelectClick;

            preview = new PictureBox()
            {
                Location = new Point(25, 100),
                Size = new Size(226, 284),
                BackColor = Color.White,
            };
            gridLayout = new FlowLayoutPanel()
            {
                Location = new Point(280, 100),
                Size = new Size(384, 284),
                BackColor = Color.White,
                AutoScroll = true,
            };
            saveButton = new Button()
            {
                Text = "Save",
                Location = new Point(561, 400),
                Size = new Size(100, 33),
                Font = GlobalConfig.DefaultFont,
                Enabled = false,
            };
            saveButton.Click += OnSaveClick;

            Controls.Add(filenameLabel);
            Controls.Add(filenameDisplayBox);
            Controls.Add(filenameSelectButton);
            Controls.Add(preview);
            Controls.Add(gridLayout);
            Controls.Add(saveButton);
            AlignUtils.HorizontalCenterAlign(filenameLabel, filenameDisplayBox, filenameSelectButton);
        }

        ~FromLocalFilePanel()
        {
            filenameLabel.Dispose();
            filenameDisplayBox.Dispose();
            filenameSelectButton.Dispose();
            preview.Dispose();
            ControlCollection controls = gridLayout.Controls;
            foreach(Control control in controls)
            {
                control.Dispose();
            }
            controls.Clear();
            gridLayout.Dispose();
        }

        private void OnSelectClick(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "GIF|*.gif",
            };
            DialogResult result = openFileDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                filenameDisplayBox.Text = openFileDialog.FileName;

                int width = preview.Width;
                int height = preview.Height;
                src = Image.FromFile(openFileDialog.FileName);

                if (src != null)
                {
                    //thumbnail = src.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
                    //if (src.Width > width || src.Height > height)
                    //{
                    //    Graphics g = Graphics.FromImage(thumbnail);
                    //    g.InterpolationMode = InterpolationMode.Bilinear;
                    //    g.SmoothingMode = SmoothingMode.Default;
                    //    g.CompositingQuality = CompositingQuality.Default;
                    //    g.Clear(Color.Transparent);
                    //    g.DrawImage(src, new Rectangle(0, 0, width, height),
                    //        new Rectangle(0, 0, src.Width, src.Height),
                    //        GraphicsUnit.Pixel);
                    //    pictureDisplay.Image = thumbnail;
                    //}
                    //else
                    //{
                    //    pictureDisplay.Image = src;
                    //}
                    preview.Image = src;
                }

                GifExtractor extractor = new GifExtractor(openFileDialog.FileName);
                Bitmap[] bitmaps = extractor.GetSequence();
                foreach (Bitmap bitmap in bitmaps)
                {
                    PictureBox pictureBox = new PictureBox()
                    {
                        Width = bitmap.Width,
                        Height = bitmap.Height,
                        Image = bitmap,
                    };

                    gridLayout.Controls.Add(pictureBox);
                }
                saveButton.Enabled = true;
            }

            openFileDialog.Dispose();
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo(filenameDisplayBox.Text);
            //FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            //DialogResult result = folderBrowserDialog.ShowDialog(this);
            //if (result == DialogResult.OK)
            //{
            string dir = fileInfo.DirectoryName;
            string ext = fileInfo.Extension;
            string filename = fileInfo.Name.Replace(ext, "");
            string saveDir = Path.Combine(dir, filename);
            if (Directory.Exists(saveDir))
            {
                Directory.Delete(saveDir, true);
            }
            Directory.CreateDirectory(saveDir);
            ControlCollection controls = gridLayout.Controls;
            for (int i = 0; i < controls.Count; i++)
            {
                string savePath = Path.Combine(saveDir, $"{i}.png");
                PictureBox pic = (PictureBox)controls[i];
                pic.Image.Save(savePath, ImageFormat.Png);
            }
            //}

            //folderBrowserDialog.Dispose();
        }

    }
}
