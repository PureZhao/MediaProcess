namespace GifTools
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.fromVideoButton = new System.Windows.Forms.Button();
            this.fromUrlButton = new System.Windows.Forms.Button();
            this.fromLocalFileButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.fromVideoButton);
            this.splitContainer.Panel1.Controls.Add(this.fromUrlButton);
            this.splitContainer.Panel1.Controls.Add(this.fromLocalFileButton);
            this.splitContainer.Size = new System.Drawing.Size(800, 450);
            this.splitContainer.SplitterDistance = 128;
            this.splitContainer.TabIndex = 6;
            // 
            // fromVideoButton
            // 
            this.fromVideoButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromVideoButton.Location = new System.Drawing.Point(6, 181);
            this.fromVideoButton.Name = "fromVideoButton";
            this.fromVideoButton.Size = new System.Drawing.Size(118, 50);
            this.fromVideoButton.TabIndex = 2;
            this.fromVideoButton.Text = "FromVideo";
            this.fromVideoButton.UseVisualStyleBackColor = true;
            this.fromVideoButton.Click += new System.EventHandler(this.OnFromVideoClick);
            // 
            // fromUrlButton
            // 
            this.fromUrlButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromUrlButton.Location = new System.Drawing.Point(6, 106);
            this.fromUrlButton.Name = "fromUrlButton";
            this.fromUrlButton.Size = new System.Drawing.Size(118, 50);
            this.fromUrlButton.TabIndex = 1;
            this.fromUrlButton.Text = "FromUrl";
            this.fromUrlButton.UseVisualStyleBackColor = true;
            this.fromUrlButton.Click += new System.EventHandler(this.OnFromUrlClick);
            // 
            // fromLocalFileButton
            // 
            this.fromLocalFileButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromLocalFileButton.Location = new System.Drawing.Point(6, 32);
            this.fromLocalFileButton.Name = "fromLocalFileButton";
            this.fromLocalFileButton.Size = new System.Drawing.Size(118, 50);
            this.fromLocalFileButton.TabIndex = 0;
            this.fromLocalFileButton.Text = "FromLocalFile";
            this.fromLocalFileButton.UseVisualStyleBackColor = true;
            this.fromLocalFileButton.Click += new System.EventHandler(this.OnFromLocalFileClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GifTools";
            this.splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button fromLocalFileButton;
        private System.Windows.Forms.Button fromVideoButton;
        private System.Windows.Forms.Button fromUrlButton;
    }
}

