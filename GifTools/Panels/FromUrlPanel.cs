using GifTools.Core;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace GifTools.Panels
{
    class FromUrlPanel : Panel
    {
        private Label urlLabel;
        private TextBox urlDisplayBox;
        private Button urlRequestButton;

        private PictureBox preview;
        private FlowLayoutPanel gridLayout;

        private Button saveButton;

        private Image src;
        //private Image thumbnail;


        public FromUrlPanel()
        {
            //BackColor = Color.White;
            Dock = DockStyle.Fill;
            urlLabel = new Label()
            {
                Text = "Url",
                Location = new Point(32, 55),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = GlobalConfig.DefaultFont,
                BackColor = Color.Transparent,
            };
            urlDisplayBox = new TextBox()
            {
                BackColor = Color.White,
                Location = new Point(101, 55),
                Size = new Size(450, 33),
            };

            urlRequestButton = new Button()
            {
                Text = "Request",
                Location = new Point(561, 55),
                Size = new Size(100, 33),
                Font = GlobalConfig.DefaultFont,
            };
            urlRequestButton.Click += OnRequestClick;

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

            Controls.Add(urlLabel);
            Controls.Add(urlDisplayBox);
            Controls.Add(urlRequestButton);
            Controls.Add(preview);
            Controls.Add(gridLayout);
            Controls.Add(saveButton);
            AlignUtils.HorizontalCenterAlign(urlLabel, urlDisplayBox, urlRequestButton);
        }

        ~FromUrlPanel()
        {
            urlLabel.Dispose();
            urlDisplayBox.Dispose();
            urlRequestButton.Dispose();
            preview.Dispose();
            ControlCollection controls = gridLayout.Controls;
            foreach (Control control in controls)
            {
                control.Dispose();
            }
            controls.Clear();
            gridLayout.Dispose();
        }

        private async void OnRequestClick(object sender, EventArgs e)
        {
            string url = urlDisplayBox.Text;
            WebRequest request = WebRequest.Create(url);
            Task<WebResponse> responseTask = request.GetResponseAsync();
            WebResponse response = await responseTask;
            Stream stream = response.GetResponseStream();
            preview.Image = Image.FromStream(stream);
            response.Close();
            response.Dispose();
            stream.Close();
            stream.Dispose();
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo(urlDisplayBox.Text);
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
        }
    }
}
