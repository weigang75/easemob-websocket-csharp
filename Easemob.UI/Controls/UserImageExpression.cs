using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Easemob.UI.Controls
{
    public partial class UserImageExpression : UserControl
    {
        public UserImageExpression()
        {
            InitializeComponent();
        }

        

        /// <summary>
        /// 图片名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Bitmap格式的文件
        /// </summary>
        public Bitmap BitFile { get; set; }

        /// <summary>
        /// 对应的字符
        /// </summary>
        public string Character { get; set; }


        public PictureEdit pictureEdit
        {
            get { return pictureEdit1; }
            set { pictureEdit = value; }
        }


        /// <summary>
        /// 控件加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserImageExpression_Load(object sender, EventArgs e)
        {
            ///判断是手动拉出来的什么都不做
            if (this.DesignMode)
                return;
            ///不是手动拉出来的,是通过加载出来的开始渲染对应的属性和图片
            ///控件的右键菜单屏蔽掉
            pictureEdit1.Properties.ShowMenu = false;
            ///把对应的路径放到现实里面去
            pictureEdit1.Image = BitFile;
        }









    }
}
