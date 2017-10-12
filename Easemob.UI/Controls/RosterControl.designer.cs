namespace Easemob.UI.Controls
{
    partial class RosterControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendMsgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.massMsgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.locReqToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addContactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.presReqToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.navBar = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem1 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem2 = new DevExpress.XtraNavBar.NavBarItem();
            this.contextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.navBar)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendMsgToolStripMenuItem,
            this.massMsgToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.viewInfoToolStripMenuItem,
            this.locReqToolStripMenuItem,
            this.moveToToolStripMenuItem,
            this.renameGroupToolStripMenuItem,
            this.addContactToolStripMenuItem,
            this.searchUserToolStripMenuItem,
            this.presReqToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(135, 224);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
            // 
            // sendMsgToolStripMenuItem
            // 
            this.sendMsgToolStripMenuItem.Name = "sendMsgToolStripMenuItem";
            this.sendMsgToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.sendMsgToolStripMenuItem.Text = "发送消息";
            this.sendMsgToolStripMenuItem.Click += new System.EventHandler(this.sendMsgToolStripMenuItem_Click);
            // 
            // massMsgToolStripMenuItem
            // 
            this.massMsgToolStripMenuItem.Name = "massMsgToolStripMenuItem";
            this.massMsgToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.massMsgToolStripMenuItem.Text = "群发消息";
            this.massMsgToolStripMenuItem.Click += new System.EventHandler(this.massMsgToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.deleteToolStripMenuItem.Text = "删除好友";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // viewInfoToolStripMenuItem
            // 
            this.viewInfoToolStripMenuItem.Name = "viewInfoToolStripMenuItem";
            this.viewInfoToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.viewInfoToolStripMenuItem.Text = "查看名片";
            this.viewInfoToolStripMenuItem.Click += new System.EventHandler(this.viewInfoToolStripMenuItem_Click);
            // 
            // locReqToolStripMenuItem
            // 
            this.locReqToolStripMenuItem.Name = "locReqToolStripMenuItem";
            this.locReqToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.locReqToolStripMenuItem.Text = "定位请求";
            this.locReqToolStripMenuItem.Click += new System.EventHandler(this.locReqToolStripMenuItem_Click);
            // 
            // moveToToolStripMenuItem
            // 
            this.moveToToolStripMenuItem.Name = "moveToToolStripMenuItem";
            this.moveToToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.moveToToolStripMenuItem.Text = "移动好友到";
            // 
            // renameGroupToolStripMenuItem
            // 
            this.renameGroupToolStripMenuItem.Name = "renameGroupToolStripMenuItem";
            this.renameGroupToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.renameGroupToolStripMenuItem.Text = "重命名";
            this.renameGroupToolStripMenuItem.Click += new System.EventHandler(this.renameGroupToolStripMenuItem_Click);
            // 
            // addContactToolStripMenuItem
            // 
            this.addContactToolStripMenuItem.Name = "addContactToolStripMenuItem";
            this.addContactToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.addContactToolStripMenuItem.Text = "添加好友";
            this.addContactToolStripMenuItem.Click += new System.EventHandler(this.addContactToolStripMenuItem_Click);
            // 
            // searchUserToolStripMenuItem
            // 
            this.searchUserToolStripMenuItem.Name = "searchUserToolStripMenuItem";
            this.searchUserToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.searchUserToolStripMenuItem.Text = "搜索好友";
            this.searchUserToolStripMenuItem.Click += new System.EventHandler(this.searchUserToolStripMenuItem_Click);
            // 
            // presReqToolStripMenuItem
            // 
            this.presReqToolStripMenuItem.Name = "presReqToolStripMenuItem";
            this.presReqToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.presReqToolStripMenuItem.Text = "请求订阅";
            this.presReqToolStripMenuItem.Click += new System.EventHandler(this.presReqToolStripMenuItem_Click);
            // 
            // navBar
            // 
            this.navBar.ActiveGroup = this.navBarGroup1;
            this.navBar.AllowSelectedLink = true;
            this.navBar.Appearance.ItemDisabled.Options.UseTextOptions = true;
            this.navBar.Appearance.ItemDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.navBar.Appearance.ItemDisabled.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.navBar.Appearance.ItemHotTracked.Options.UseTextOptions = true;
            this.navBar.Appearance.ItemHotTracked.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.navBar.Appearance.ItemHotTracked.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.navBar.Appearance.ItemPressed.Options.UseTextOptions = true;
            this.navBar.Appearance.ItemPressed.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.navBar.Appearance.ItemPressed.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.navBar.ContentButtonHint = null;
            this.navBar.ContextMenuStrip = this.contextMenu;
            this.navBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBar.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup1});
            this.navBar.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.navBarItem1,
            this.navBarItem2});
            this.navBar.Location = new System.Drawing.Point(0, 0);
            this.navBar.LookAndFeel.SkinName = "Money Twins";
            this.navBar.Name = "navBar";
            this.navBar.OptionsNavPane.ExpandedWidth = 172;
            this.navBar.OptionsNavPane.ShowExpandButton = false;
            this.navBar.OptionsNavPane.ShowOverflowPanel = false;
            this.navBar.SelectLinkOnPress = false;
            this.navBar.Size = new System.Drawing.Size(270, 418);
            this.navBar.StoreDefaultPaintStyleName = true;
            this.navBar.TabIndex = 9;
            this.navBar.Text = "navBarControl1";
            this.navBar.GetHint += new DevExpress.XtraNavBar.NavBarGetHintEventHandler(this.navBar_GetHint);
            this.navBar.DoubleClick += new System.EventHandler(this.navBar_DoubleClick);
            this.navBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.navBar_MouseDown);
            this.navBar.CustomDrawGroupCaption += new DevExpress.XtraNavBar.ViewInfo.CustomDrawNavBarElementEventHandler(this.navBar_CustomDrawGroupCaption);
            this.navBar.MouseHover += new System.EventHandler(this.navBar_MouseHover);
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Caption = "我的好友";
            this.navBarGroup1.Expanded = true;
            this.navBarGroup1.Name = "navBarGroup1";
            // 
            // navBarItem1
            // 
            this.navBarItem1.Caption = "navBarItem1";
            this.navBarItem1.Name = "navBarItem1";
            // 
            // navBarItem2
            // 
            this.navBarItem2.Caption = "navBarItem2";
            this.navBarItem2.Name = "navBarItem2";
            // 
            // MyRosterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navBar);
            this.Name = "MyRosterControl";
            this.Size = new System.Drawing.Size(270, 418);
            this.Load += new System.EventHandler(this.MyRosterControl_Load);
            this.contextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.navBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem sendMsgToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem locReqToolStripMenuItem;
        private DevExpress.XtraNavBar.NavBarControl navBar;
        private System.Windows.Forms.ToolStripMenuItem moveToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addContactToolStripMenuItem;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarItem navBarItem1;
        private DevExpress.XtraNavBar.NavBarItem navBarItem2;
        private System.Windows.Forms.ToolStripMenuItem massMsgToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem presReqToolStripMenuItem;
    }
}
