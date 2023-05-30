using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using GifTools.Core;
using GifTools.Panels;

namespace GifTools
{
    public partial class MainForm : Form
    {
        private Panel curPanel;
        public MainForm()
        {
            InitializeComponent();
            
        }

        ~MainForm()
        {
            curPanel?.Dispose();
        }


        private void OnFromLocalFileClick(object sender, EventArgs e)
        {
            splitContainer.Panel2.Controls.Clear();
            Panel temp = curPanel;
            curPanel = new FromLocalFilePanel();
            temp?.Dispose();
            splitContainer.Panel2.Controls.Add(curPanel);
            fromLocalFileButton.Enabled = false;
            fromUrlButton.Enabled = true;
            fromVideoButton.Enabled = true;
        }

        private void OnFromUrlClick(object sender, EventArgs e)
        {
            splitContainer.Panel2.Controls.Clear();
            Panel temp = curPanel;
            curPanel = new FromUrlPanel();
            temp?.Dispose();
            splitContainer.Panel2.Controls.Add(curPanel);
            fromLocalFileButton.Enabled = true;
            fromUrlButton.Enabled = false;
            fromVideoButton.Enabled = true;
        }

        private void OnFromVideoClick(object sender, EventArgs e)
        {
            splitContainer.Panel2.Controls.Clear();
            Panel temp = curPanel;
            curPanel = new FromVideoPanel();
            temp?.Dispose();
            splitContainer.Panel2.Controls.Add(curPanel);
            fromLocalFileButton.Enabled = true;
            fromUrlButton.Enabled = true;
            fromVideoButton.Enabled = false;
        }
    }
}
