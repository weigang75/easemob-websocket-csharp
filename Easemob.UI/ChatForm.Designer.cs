namespace Easemob.UI
{
    partial class ChatForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chatBox1 = new Easemob.UI.ChatBox();
            this.SuspendLayout();
            // 
            // chatBox1
            // 
            this.chatBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatBox1.Location = new System.Drawing.Point(0, 0);
            this.chatBox1.Name = "chatBox1";
            this.chatBox1.Size = new System.Drawing.Size(598, 446);
            this.chatBox1.TabIndex = 0;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 446);
            this.Controls.Add(this.chatBox1);
            this.Name = "ChatForm";
            this.Text = "ChatForm";
            this.ResumeLayout(false);

        }

        #endregion

        private ChatBox chatBox1;
    }
}