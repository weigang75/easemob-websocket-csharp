using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraRichEdit.Services;
using DevExpress.XtraRichEdit;
using Easemob.UI.Properties;
using DevExpress.XtraRichEdit.API.Native;
using System.IO;

namespace Easemob.UI
{
    public partial class RichEditor : UserControl
    {
        /// <summary>
        /// 构造事件
        /// </summary>
        public RichEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool Readonly
        {
            get
            {
                return this.richEditControl1.ReadOnly;
            }
            set
            {
                this.richEditControl1.ReadOnly = value;
            }
        }


        /// <summary>
        /// 显示值
        /// </summary>
        public override string Text
        {
            get
            {
                return richEditControl1.Text;
            }
            set
            {
                richEditControl1.Text = value;
            }
        }



        /// <summary>
        /// html显示值
        /// </summary>
        public string HtmlText
        {
            get
            {
                return richEditControl1.HtmlText;
            }
            set
            {
                richEditControl1.HtmlText = value;
            }
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RichEditor_Load(object sender, EventArgs e)
        {
            //IAutoCorrectService serviec = richEditControl1.GetService<IAutoCorrectService>();
            richEditControl1.Text = "";
            //if (serviec != null)
            //{
            //    AutoCorrectReplaceInfoCollection replaceTable = new AutoCorrectReplaceInfoCollection();
            //    //Bitmap bit = Resources._654; // new Bitmap((new System.Net.WebClient()).OpenRead(file));
            //    //replaceTable.Add(new AutoCorrectReplaceInfo(":)", bit));
            //    //replaceTable.Add("wnwd", "well-biyrished");
            //    //serviec.SetReplaceTable(replaceTable);
            //}
            //richEditControl1.Focus();
        }



        private DataTable ImageTable;

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public void AppendHtml(string text, Color color)
        {
            if (ImageTable == null)
            {
                #region 创建DataTable用来存储图片信息

                ImageTable = new DataTable();
                ///创建DataTable用来存储图片信息
                ImageTable.Columns.Add("Name", typeof(string));
                ImageTable.Columns.Add("Character", typeof(string));
                ImageTable.Columns.Add("BitFile", typeof(Bitmap));
                DataRow row = ImageTable.NewRow();
                row["Name"] = "笑脸";
                row["Character"] = "[):]";
                row["BitFile"] = Resources.笑脸;
                ImageTable.Rows.Add(row);
                DataRow row1 = ImageTable.NewRow();
                row1["Name"] = "大笑";
                row1["Character"] = "[:D]";
                row1["BitFile"] = Resources.大笑;
                ImageTable.Rows.Add(row1);
                DataRow row2 = ImageTable.NewRow();
                row2["Name"] = "得意";
                row2["Character"] = "[;)]";
                row2["BitFile"] = Resources.得意;
                ImageTable.Rows.Add(row2);
                DataRow row3 = ImageTable.NewRow();
                row3["Name"] = "惊讶";
                row3["Character"] = "[:-o]";
                row3["BitFile"] = Resources.惊讶;
                ImageTable.Rows.Add(row3);
                DataRow row4 = ImageTable.NewRow();
                row4["Name"] = "调皮";
                row4["Character"] = "[:p]";
                row4["BitFile"] = Resources.调皮;
                ImageTable.Rows.Add(row4);
                DataRow row5 = ImageTable.NewRow();
                row5["Name"] = "酷";
                row5["Character"] = "[(H)]";
                row5["BitFile"] = Resources.酷;
                ImageTable.Rows.Add(row5);
                DataRow row6 = ImageTable.NewRow();
                row6["Name"] = "发怒";
                row6["Character"] = "[:@]";
                row6["BitFile"] = Resources.发怒;
                ImageTable.Rows.Add(row6);
                DataRow row7 = ImageTable.NewRow();
                row7["Name"] = "饥饿";
                row7["Character"] = "[:s]";
                row7["BitFile"] = Resources.饥饿;
                ImageTable.Rows.Add(row7);
                DataRow row8 = ImageTable.NewRow();
                row8["Name"] = "害羞";
                row8["Character"] = "[:$]";
                row8["BitFile"] = Resources.害羞;
                ImageTable.Rows.Add(row8);
                DataRow row9 = ImageTable.NewRow();
                row9["Name"] = "不开心";
                row9["Character"] = "[:(]";
                row9["BitFile"] = Resources.不开心;
                ImageTable.Rows.Add(row9);
                DataRow row10 = ImageTable.NewRow();
                row10["Name"] = "大哭";
                row10["Character"] = "[:'(]";
                row10["BitFile"] = Resources.大哭;
                ImageTable.Rows.Add(row10);
                DataRow row11 = ImageTable.NewRow();
                row11["Name"] = "瞪眼";
                row11["Character"] = "[:|]";
                row11["BitFile"] = Resources.瞪眼;
                ImageTable.Rows.Add(row11);
                DataRow row12 = ImageTable.NewRow();
                row12["Name"] = "开心";
                row12["Character"] = "[(a)]";
                row12["BitFile"] = Resources.开心;
                ImageTable.Rows.Add(row12);
                DataRow row13 = ImageTable.NewRow();
                row13["Name"] = "大怒";
                row13["Character"] = "[8o|]";
                row13["BitFile"] = Resources.大怒;
                ImageTable.Rows.Add(row13);
                DataRow row14 = ImageTable.NewRow();
                row14["Name"] = "眼镜";
                row14["Character"] = "[8-|]";
                row14["BitFile"] = Resources.眼镜;
                ImageTable.Rows.Add(row14);
                DataRow row15 = ImageTable.NewRow();
                row15["Name"] = "绿脸";
                row15["Character"] = "[+o(]";
                row15["BitFile"] = Resources.绿脸;
                ImageTable.Rows.Add(row15);
                DataRow row16 = ImageTable.NewRow();
                row16["Name"] = "圣诞";
                row16["Character"] = "[<o)]";
                row16["BitFile"] = Resources.圣诞;
                ImageTable.Rows.Add(row16);
                DataRow row17 = ImageTable.NewRow();
                row17["Name"] = "疲倦";
                row17["Character"] = "[|-)]";
                row17["BitFile"] = Resources.疲倦;
                ImageTable.Rows.Add(row17);
                DataRow row18 = ImageTable.NewRow();
                row18["Name"] = "想问题";
                row18["Character"] = "[*-)]";
                row18["BitFile"] = Resources.想问题;
                ImageTable.Rows.Add(row18);
                DataRow row19 = ImageTable.NewRow();
                row19["Name"] = "闭嘴";
                row19["Character"] = "[:-#]";
                row19["BitFile"] = Resources.闭嘴;
                ImageTable.Rows.Add(row19);
                DataRow row20 = ImageTable.NewRow();
                row20["Name"] = "对骂";
                row20["Character"] = "[:-*]";
                row20["BitFile"] = Resources.对骂;
                ImageTable.Rows.Add(row20);
                DataRow row21 = ImageTable.NewRow();
                row21["Name"] = "急眼";
                row21["Character"] = "[^o)]";
                row21["BitFile"] = Resources.急眼;
                ImageTable.Rows.Add(row21);
                DataRow row22 = ImageTable.NewRow();
                row22["Name"] = "无表情";
                row22["Character"] = "[8-)]";
                row22["BitFile"] = Resources.无表情;
                ImageTable.Rows.Add(row22);
                DataRow row23 = ImageTable.NewRow();
                row23["Name"] = "爱心";
                row23["Character"] = "[(|)]";
                row23["BitFile"] = Resources.爱心;
                ImageTable.Rows.Add(row23);
                DataRow row24 = ImageTable.NewRow();
                row24["Name"] = "心碎 ";
                row24["Character"] = "[(u)]";
                row24["BitFile"] = Resources.心碎;
                ImageTable.Rows.Add(row24);
                DataRow row25 = ImageTable.NewRow();
                row25["Name"] = "月亮";
                row25["Character"] = "[(S)]";
                row25["BitFile"] = Resources.月亮;
                ImageTable.Rows.Add(row25);
                DataRow row26 = ImageTable.NewRow();
                row26["Name"] = "星星";
                row26["Character"] = "[(*)]";
                row26["BitFile"] = Resources.星星;
                ImageTable.Rows.Add(row26);
                DataRow row27 = ImageTable.NewRow();
                row27["Name"] = "太阳";
                row27["Character"] = "[(#)]";
                row27["BitFile"] = Resources.太阳;
                ImageTable.Rows.Add(row27);
                DataRow row28 = ImageTable.NewRow();
                row28["Name"] = "彩虹";
                row28["Character"] = "[(R)]";
                row28["BitFile"] = Resources.彩虹;
                ImageTable.Rows.Add(row28);
                DataRow row29 = ImageTable.NewRow();
                row29["Name"] = "色";
                row29["Character"] = "[({)]";
                row29["BitFile"] = Resources.色;
                ImageTable.Rows.Add(row29);
                DataRow row30 = ImageTable.NewRow();
                row30["Name"] = "亲嘴";
                row30["Character"] = "[(})]";
                row30["BitFile"] = Resources.亲嘴;
                ImageTable.Rows.Add(row30);
                DataRow row31 = ImageTable.NewRow();
                row31["Name"] = "嘴唇";
                row31["Character"] = "[(k)]";
                row31["BitFile"] = Resources.嘴唇;
                ImageTable.Rows.Add(row31);
                DataRow row32 = ImageTable.NewRow();
                row32["Name"] = "玫瑰花";
                row32["Character"] = "[(F)]";
                row32["BitFile"] = Resources.玫瑰花;
                ImageTable.Rows.Add(row32);
                DataRow row33 = ImageTable.NewRow();
                row33["Name"] = "花谢了";
                row33["Character"] = "[(W)]";
                row33["BitFile"] = Resources.花谢了;
                ImageTable.Rows.Add(row33);
                DataRow row34 = ImageTable.NewRow();
                row34["Name"] = "顶";
                row34["Character"] = "[(D)]";
                row34["BitFile"] = Resources.顶;
                ImageTable.Rows.Add(row34);

                #endregion
            }


            ///获取消息来进行比对 是否有图片
            foreach (DataRow row in ImageTable.Rows)
            {
                if (text.Contains(row["Character"].ToString()))
                {
                    Bitmap b = row["BitFile"] as Bitmap;
                    MemoryStream ms = new MemoryStream();
                    b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] bytes = ms.GetBuffer();
                    ms.Close();
                    text = text.Replace(row["Character"].ToString(), GetImageBaseNode(bytes, "image/png"));
                }
            }
            AppendHtml(text, color, HorizontalAlignment.Left);
        }



        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="text">获取接收到的消息</param>
        /// <param name="color"></param>
        /// <param name="align"></param>
        public void AppendHtml(string text, Color color, HorizontalAlignment align)
        {
            string html = string.Format("<p style='color:{1};background-color:yellow;width:200px;float:{2};'>{0}</p>", text.Replace("\r\n", "<br/>").Replace("\n", "<br/>").Replace("\r", "<br/>"), color.Name, align.ToString().ToLower());  //
            this.richEditControl1.Document.AppendHtmlText(html);
            this.richEditControl1.Document.CaretPosition = this.richEditControl1.Document.Range.End;
            richEditControl1.ScrollToCaret();
        }

        /// <summary>
        /// 通过路径添加图片
        /// </summary>
        /// <param name="path"></param>
        public void AppendImage(string path)
        {
            AppendImage(path, new Size(0, 0), HorizontalAlignment.Left);
        }

        public void AppendImageBytes(byte[] bytes)
        {
            AppendImageBytes(bytes, "image/jpeg");
        }

        public void AppendImageBytes(byte[] bytes, string mimeType)
        {
            //string base64 = Convert.ToBase64String(bytes);
            //string html = string.Format("<a href=\"javascript:alert('hello');\"><img src=\"data:{1};base64,{0}\"/></a>", base64, mimeType);

            string html = GetImageBaseNode(bytes, mimeType);
            this.richEditControl1.Document.AppendHtmlText(html);
            //DocumentImageSource.FromUri();
            //Image i;

            //this.richEditControl1.Document.
        }

        public string GetImageBaseNode(byte[] bytes, string mimeType)
        {
            string base64 = Convert.ToBase64String(bytes);
            string html = string.Format("<img src=\"data:{1};base64,{0}\"/>", base64, mimeType);
            return html;
        }

        public void AppendNewLine()
        {
            this.richEditControl1.Document.AppendHtmlText("<br/>");
        }

        public void AppendNewLine(uint number)
        {
            for (int i = 0; i < number; i++)
            {
                this.richEditControl1.Document.AppendHtmlText("<br/>");
            }
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="size"></param>
        /// <param name="align"></param>
        public void AppendImage(string path, Size size, HorizontalAlignment align)
        {
            if (size.Width == 0 || size.Height == 0)
            {
                // 忽略大小直接下载
            }

            string html = string.Format("<img src='{0}'/>", path);
            this.richEditControl1.Document.AppendHtmlText(html);
        }

        private void richEditControl1_DragEnter(object sender, DragEventArgs e)
        {
            return;
        }
    }
}
