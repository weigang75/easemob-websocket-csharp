namespace Easemob.UI
{
    partial class ChatBox
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatBox));
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnSend = new DevExpress.XtraEditors.SimpleButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.edInput = new Easemob.UI.RichEditor();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.pnlFace = new DevExpress.XtraEditors.PanelControl();
            this.panelControl8 = new DevExpress.XtraEditors.PanelControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelControl7 = new DevExpress.XtraEditors.PanelControl();
            this.userImageExpression1 = new Easemob.UI.Controls.UserImageExpression();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labUserName = new System.Windows.Forms.Label();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barBtnExpression = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnImage = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnAudio = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnScreenshot = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlFace)).BeginInit();
            this.pnlFace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl8)).BeginInit();
            this.panelControl8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl7)).BeginInit();
            this.panelControl7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(421, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "关闭(&C)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(502, 6);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "发送(&S)";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "图片文件|*.gif;*.png;*.jpg;*.bmp";
            // 
            // edInput
            // 
            this.edInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edInput.HtmlText = resources.GetString("edInput.HtmlText");
            this.edInput.Location = new System.Drawing.Point(2, 2);
            this.edInput.Name = "edInput";
            this.edInput.Readonly = false;
            this.edInput.Size = new System.Drawing.Size(578, 96);
            this.edInput.TabIndex = 1;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl2);
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl5);
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl4);
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl3);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(582, 422);
            this.splitContainerControl1.SplitterPosition = 258;
            this.splitContainerControl1.TabIndex = 6;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.pnlFace);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 69);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(582, 189);
            this.panelControl2.TabIndex = 1;
            // 
            // pnlFace
            // 
            this.pnlFace.Controls.Add(this.panelControl8);
            this.pnlFace.Controls.Add(this.panelControl7);
            this.pnlFace.Location = new System.Drawing.Point(3, 18);
            this.pnlFace.Name = "pnlFace";
            this.pnlFace.Size = new System.Drawing.Size(443, 166);
            this.pnlFace.TabIndex = 6;
            // 
            // panelControl8
            // 
            this.panelControl8.Controls.Add(this.flowLayoutPanel1);
            this.panelControl8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl8.Location = new System.Drawing.Point(2, 24);
            this.panelControl8.Name = "panelControl8";
            this.panelControl8.Size = new System.Drawing.Size(439, 140);
            this.panelControl8.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(435, 136);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // panelControl7
            // 
            this.panelControl7.Controls.Add(this.userImageExpression1);
            this.panelControl7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl7.Location = new System.Drawing.Point(2, 2);
            this.panelControl7.Name = "panelControl7";
            this.panelControl7.Size = new System.Drawing.Size(439, 22);
            this.panelControl7.TabIndex = 0;
            // 
            // userImageExpression1
            // 
            this.userImageExpression1.BitFile = global::Easemob.UI.Properties.Resources.关闭按钮;
            this.userImageExpression1.Character = null;
            this.userImageExpression1.Dock = System.Windows.Forms.DockStyle.Right;
            this.userImageExpression1.Location = new System.Drawing.Point(421, 2);
            this.userImageExpression1.Name = "userImageExpression1";
            this.userImageExpression1.Size = new System.Drawing.Size(16, 18);
            this.userImageExpression1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.pictureBox1);
            this.panelControl1.Controls.Add(this.labUserName);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(582, 69);
            this.panelControl1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(5, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // labUserName
            // 
            this.labUserName.AutoSize = true;
            this.labUserName.Location = new System.Drawing.Point(85, 12);
            this.labUserName.Name = "labUserName";
            this.labUserName.Size = new System.Drawing.Size(53, 12);
            this.labUserName.TabIndex = 0;
            this.labUserName.Text = "UserName";
            // 
            // panelControl5
            // 
            this.panelControl5.Controls.Add(this.edInput);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl5.Location = new System.Drawing.Point(0, 26);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(582, 100);
            this.panelControl5.TabIndex = 2;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.barDockControlLeft);
            this.panelControl4.Controls.Add(this.barDockControlRight);
            this.panelControl4.Controls.Add(this.barDockControlBottom);
            this.panelControl4.Controls.Add(this.barDockControlTop);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl4.Location = new System.Drawing.Point(0, 0);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(582, 26);
            this.panelControl4.TabIndex = 1;
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(2, 2);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 0);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(580, 2);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(2, -3);
            this.barDockControlBottom.Size = new System.Drawing.Size(578, 27);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(2, 2);
            this.barDockControlTop.Size = new System.Drawing.Size(578, 0);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.btnSend);
            this.panelControl3.Controls.Add(this.btnClose);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(0, 126);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(582, 33);
            this.panelControl3.TabIndex = 0;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this.panelControl4;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barBtnExpression,
            this.barBtnImage,
            this.barBtnAudio,
            this.barBtnScreenshot});
            this.barManager1.MaxItemId = 4;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnExpression),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnImage),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnAudio),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnScreenshot)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barBtnExpression
            // 
            this.barBtnExpression.Caption = "表情";
            this.barBtnExpression.Id = 0;
            this.barBtnExpression.Name = "barBtnExpression";
            this.barBtnExpression.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnExpression_ItemClick);
            // 
            // barBtnImage
            // 
            this.barBtnImage.Caption = "图片";
            this.barBtnImage.Id = 1;
            this.barBtnImage.Name = "barBtnImage";
            this.barBtnImage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnImage_ItemClick);
            // 
            // barBtnAudio
            // 
            this.barBtnAudio.Caption = "语音";
            this.barBtnAudio.Id = 2;
            this.barBtnAudio.Name = "barBtnAudio";
            this.barBtnAudio.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnAudio_ItemClick);
            // 
            // barBtnScreenshot
            // 
            this.barBtnScreenshot.Caption = "截图";
            this.barBtnScreenshot.Id = 3;
            this.barBtnScreenshot.Name = "barBtnScreenshot";
            this.barBtnScreenshot.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnScreenshot_ItemClick);
            // 
            // ChatBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "ChatBox";
            this.Size = new System.Drawing.Size(582, 422);
            this.Load += new System.EventHandler(this.ChatBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlFace)).EndInit();
            this.pnlFace.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl8)).EndInit();
            this.panelControl8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl7)).EndInit();
            this.panelControl7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RichEditor edInput;
        private DevExpress.XtraEditors.SimpleButton btnSend;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labUserName;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarButtonItem barBtnExpression;
        private DevExpress.XtraBars.BarButtonItem barBtnImage;
        private DevExpress.XtraBars.BarButtonItem barBtnAudio;
        private DevExpress.XtraEditors.PanelControl pnlFace;
        private DevExpress.XtraEditors.PanelControl panelControl8;
        private DevExpress.XtraEditors.PanelControl panelControl7;
        private Controls.UserImageExpression userImageExpression1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraBars.BarButtonItem barBtnScreenshot;
    }
}
