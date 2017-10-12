using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Easemob.UI.Properties;
using System.IO;

namespace Easemob.UI
{
    /// <summary>
    /// 申明一个委托
    /// </summary>
    /// <param name="varTable"></param>
    public delegate void WebMessage(object VarObj);
    /// <summary>
    /// 显示类型
    /// </summary>
    public enum DirectionType
    {
        /// <summary>
        /// 左边
        /// </summary>
        Left = 0,
        /// <summary>
        /// 中间
        /// </summary>
        Middle = 1,
        /// <summary>
        /// 右边
        /// </summary>
        Right = 2
    }
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class UserWebBrowser : UserControl
    {
        /// <summary>
        /// 委托
        /// </summary>
        public WebMessage webMessage = null;
        /// <summary>
        /// 用来存储表情包的图片
        /// </summary>
        public DataTable ImageTable = null;

        /// <summary>
        /// 指向的路径
        /// </summary>
        private string uri;

        /// <summary>
        /// 路径
        /// </summary>
        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }


        public int UserWidth;

        public int UserHeight;
        /// <summary>
        /// 构造事件
        /// </summary>
        public UserWebBrowser()
        {
            InitializeComponent();

        }





        #region 添加信息
        /// <summary>
        /// 添加数据(添加信息)
        /// </summary>
        /// <param name="Direction">枚举信息放的位置</param>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Time">具体时间</param>
        /// <param name="Message">消息内容</param>
        public void AddMessage(DirectionType Direction, string Name, DateTime Time, string Message)
        {
            ///获取消息来进行比对 是否有图片
            foreach (DataRow row in ImageTable.Rows)
            {
                if (Message.Contains(row["Character"].ToString()))
                {
                    Bitmap b = row["BitFile"] as Bitmap;
                    MemoryStream ms = new MemoryStream();
                    b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] bytes = ms.GetBuffer();
                    ms.Close();
                    Message = Message.Replace(row["Character"].ToString(), GetImageBaseNode(bytes, "image/png"));
                }
            }
            this.UserShowWeb.Document.Body.InnerHtml +=
                @"<div  style='text-align:" + Direction + @"; padding:0px; margin:0px;width:500px'>
                <div>" + Name + Time + @"</div>
                <div>" + Message + @"</div>
                </div>";
            if (UserHeight < UserShowWeb.Document.Body.ScrollRectangle.Height)
            {
                UserShowWeb.Height = UserShowWeb.Document.Body.ScrollRectangle.Height;
                UserShowWeb.Document.Window.ScrollTo(0, UserShowWeb.Document.Body.ScrollRectangle.Height);

                xtraScrollableControl1.VerticalScroll.Value = UserShowWeb.Document.Body.ScrollRectangle.Height;
            }
        }
        /// <summary>
        /// 添加数据到左边(添加信息)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Time">具体时间</param>
        /// <param name="Message">消息内容</param>
        public void LeftAddMessage(string Name, DateTime Time, string Message)
        {
            AddMessage(DirectionType.Left, Name, Time, Message);
        }
        /// <summary>
        /// 添加数据到左边（重载）(添加信息)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Message">消息内容</param>
        public void LeftAddMessage(string Name, string Message)
        {
            LeftAddMessage(Name, DateTime.Now, Message);
        }

        /// <summary>
        /// 添加数据到中间居中(添加信息)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Time">具体时间</param>
        /// <param name="Message">消息内容</param>
        public void MiddleAddMessage(string Name, DateTime Time, string Message)
        {
            AddMessage(DirectionType.Middle, Name, Time, Message);
        }
        /// <summary>
        /// 添加数据到中间居中（重载）(添加信息)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Message">消息内容</param>
        public void MiddleAddMessage(string Name, string Message)
        {
            MiddleAddMessage(Name, DateTime.Now, Message);
        }

        /// <summary>
        /// 添加数据到右边(添加信息)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Time">具体时间</param>
        /// <param name="Message">消息内容</param>
        public void RightAddMessage(string Name, DateTime Time, string Message)
        {
            AddMessage(DirectionType.Right, Name, Time, Message);
        }
        /// <summary>
        /// 添加数据到右边（重载）(添加信息)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Message">消息内容</param>
        public void RightAddMessage(string Name, string Message)
        {
            RightAddMessage(Name, DateTime.Now, Message);
        }
        #endregion

        #region 文字信息过滤
        /// <summary>
        /// 转换base64 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public string GetImageBaseNode(byte[] bytes, string mimeType)
        {
            string base64 = Convert.ToBase64String(bytes);
            string html = string.Format("<img src=\"data:{1};base64,{0}\"/>", base64, mimeType);
            return html;
        }
        #endregion

        #region 添加图片信息
        /// <summary>
        /// 添加图片信息
        /// </summary>
        /// <param name="Direction">显示的枚举</param>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Time">具体时间</param>
        /// <param name="ImagePath">图片的路径</param>
        /// <param name="width">图片宽度</param>
        public void AddImage(DirectionType Direction, string Name, DateTime Time, string ImagePath, int width)
        {
            ///获取图片
            Image ReducedImage = Image.FromFile(ImagePath);
            ///用宽度除以图片的宽度
            double a = (double)width / ReducedImage.Width;
            ///用比例算出来高度
            int height = Convert.ToInt32(ReducedImage.Height * a);
            ///把图片用imag标签转换过来
            string img = "<a  onclick='return getSrc(this)'><image  src='1.jpg' width='" + width + "' height='" + height + "' ></a>";
            this.UserShowWeb.Document.Body.InnerHtml +=
                @"<div  style='text-align:" + Direction + @"; padding:0px; margin:0px;width:500px'>
                <div>" + Name + Time + @"</div>
                <div>" + img + @"</div>
                </div>";
            if (UserHeight < UserShowWeb.Document.Body.ScrollRectangle.Height)
            {
                UserShowWeb.Height = UserShowWeb.Document.Body.ScrollRectangle.Height;
                UserShowWeb.Document.Window.ScrollTo(0, UserShowWeb.Document.Body.ScrollRectangle.Height);
                xtraScrollableControl1.VerticalScroll.Value = UserShowWeb.Document.Body.ScrollRectangle.Height;
            }
        }

        /// <summary>
        /// 添加图片信息(图片的宽度默认60)
        /// </summary>
        /// <param name="Direction">显示的枚举</param>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Time">具体时间</param>
        /// <param name="ImagePath">图片的路径</param>
        public void AddImage(DirectionType Direction, string Name, DateTime Time, string ImagePath)
        {
            AddImage(Direction, Name, Time, ImagePath, 60);
        }

        /// <summary>
        /// 添加图片信息左边(图片的宽度默认60)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Time">具体时间</param>
        /// <param name="ImagePath">图片的路径</param>
        public void LeftAddImage(string Name, DateTime Time, string ImagePath)
        {
            AddImage(DirectionType.Left, Name, Time, ImagePath);
        }

        /// <summary>
        /// 添加图片信息左边(图片的宽度默认60)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="ImagePath">图片的路径</param>
        public void LeftAddImage(string Name, string ImagePath)
        {
            LeftAddImage(Name, DateTime.Now, ImagePath);
        }

        /// <summary>
        /// 添加图片信息中间(图片的宽度默认60)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Time">具体时间</param>
        /// <param name="ImagePath">图片的路径</param>
        public void MiddleAddImage(string Name, DateTime Time, string ImagePath)
        {
            AddImage(DirectionType.Middle, Name, Time, ImagePath);
        }

        /// <summary>
        /// 添加图片信息中间(图片的宽度默认60)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="ImagePath">图片的路径</param>
        public void MiddleAddImage(string Name, string ImagePath)
        {
            MiddleAddImage(Name, DateTime.Now, ImagePath);
        }
        /// <summary>
        /// 添加图片信息右边(图片的宽度默认60)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Time">具体时间</param>
        /// <param name="ImagePath">图片的路径</param>
        public void RightAddImage(string Name, DateTime Time, string ImagePath)
        {
            AddImage(DirectionType.Right, Name, Time, ImagePath);
        }

        /// <summary>
        /// 添加图片信息右边(图片的宽度默认60)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="ImagePath">图片的路径</param>
        public void RightAddImage(string Name, string ImagePath)
        {
            RightAddImage(Name, DateTime.Now, ImagePath);
        }



        /// <summary>
        /// 添加图片信息左边
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Time">具体时间</param>
        /// <param name="ImagePath">图片的路径</param>
        /// <param name="width">图片宽度</param>
        public void LeftAddImage(string Name, DateTime Time, string ImagePath, int width)
        {
            AddImage(DirectionType.Left, Name, Time, ImagePath, width);
        }

        /// <summary>
        /// 添加图片信息左边
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="ImagePath">图片的路径</param>
        /// <param name="width">图片宽度</param>
        public void LeftAddImage(string Name, string ImagePath, int width)
        {
            LeftAddImage(Name, DateTime.Now, ImagePath, width);
        }

        /// <summary>
        /// 添加图片信息中间
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Time">具体时间</param>
        /// <param name="ImagePath">图片的路径</param>
        /// <param name="width">图片宽度</param>
        public void MiddleAddImage(string Name, DateTime Time, string ImagePath, int width)
        {
            AddImage(DirectionType.Middle, Name, Time, ImagePath, width);
        }

        /// <summary>
        /// 添加图片信息中间(图片的宽度默认60)
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="ImagePath">图片的路径</param>
        /// <param name="width">图片宽度</param>
        public void MiddleAddImage(string Name, string ImagePath, int width)
        {
            MiddleAddImage(Name, DateTime.Now, ImagePath, width);
        }

        /// <summary>
        /// 添加图片信息右边
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="Time">具体时间</param>
        /// <param name="ImagePath">图片的路径</param>.
        /// <param name="width">图片宽度</param>
        public void RightAddImage(string Name, DateTime Time, string ImagePath, int width)
        {
            AddImage(DirectionType.Right, Name, Time, ImagePath, width);
        }

        /// <summary>
        /// 添加图片信息右边
        /// </summary>
        /// <param name="Name">发送消息人的名称</param>
        /// <param name="ImagePath">图片的路径</param>
        /// <param name="width">图片宽度</param>
        public void RightAddImage(string Name, string ImagePath, int width)
        {
            RightAddImage(Name, DateTime.Now, ImagePath, width);
        }
        #endregion

        #region 返回页面信息
        /// <summary>
        /// 传递消息
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(object message)
        {
            ///获取到页面上的消息传递给前台页面
            if (webMessage != null)
            {
                webMessage(message);
            }
            else
            {
                ///没有调用委托就返回
                return;
            }
        }
        #endregion

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserWebBrowser_Load(object sender, EventArgs e)
        {
            ///拖出来的返回
            if (this.DesignMode)
                return;
            this.xtraScrollableControl1.Width = UserWidth;
            this.UserShowWeb.Width = UserWidth;
            this.xtraScrollableControl1.Height =UserHeight;
            this.UserShowWeb.Height = UserHeight;
            ///指向user
            this.UserShowWeb.Url = new Uri(Uri);//Application.StartupPath + "/index.html"
            ///控件中的网页所包含的脚本代码访问
            UserShowWeb.ObjectForScripting = this;
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
        }

    }







}
