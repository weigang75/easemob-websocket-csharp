namespace Easemob.UI
{
    partial class RichEditor
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
            this.richEditControl1 = new DevExpress.XtraRichEdit.RichEditControl();
            this.SuspendLayout();
            // 
            // richEditControl1
            // 
            this.richEditControl1.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            this.richEditControl1.Appearance.Text.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.richEditControl1.Appearance.Text.Options.UseFont = true;
            this.richEditControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl1.Location = new System.Drawing.Point(0, 0);
            this.richEditControl1.Name = "richEditControl1";
            this.richEditControl1.Options.AutoCorrect.CorrectTwoInitialCapitals = true;
            this.richEditControl1.Options.AutoCorrect.UseSpellCheckerSuggestions = true;
            this.richEditControl1.Options.Behavior.Drag = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.richEditControl1.Options.Behavior.Drop = DevExpress.XtraRichEdit.DocumentCapability.Disabled;
            this.richEditControl1.Size = new System.Drawing.Size(474, 306);
            this.richEditControl1.TabIndex = 0;
            this.richEditControl1.Text = "你好";
            this.richEditControl1.Views.SimpleView.Padding = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.richEditControl1.DragEnter += new System.Windows.Forms.DragEventHandler(this.richEditControl1_DragEnter);
            // 
            // RichEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richEditControl1);
            this.Name = "RichEditor";
            this.Size = new System.Drawing.Size(474, 306);
            this.Load += new System.EventHandler(this.RichEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraRichEdit.RichEditControl richEditControl1;
    }
}
