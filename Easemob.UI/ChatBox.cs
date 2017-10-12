using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Easemob.XmppWebSocket;
using System.Net;
using Newtonsoft.Json;
using Easemob.PostedFile;
using Easemob.XmppWebSocket.Protocol;
using Easemob.UI.Properties;
using System.IO;
using Easemob.UI.Controls;
using DevExpress.XtraEditors;
using Easemob.Audio;
using Easemob.UI.Forms;
using CSharpWin;

namespace Easemob.UI
{
    public partial class ChatBox : UserControl
    {
        private XmppWebSocketConnection conn = null;
        private string toUserName = null;
        ChatBrowser chatBrower = new ChatBrowser();
        public ChatBox()
        {
            InitializeComponent();
            pnlFace.Location = new Point(3, 18);
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="conn">设置连接</param>
        /// <param name="toUserName">设置聊天对象的用户名</param>
        public void SetParams(XmppWebSocketConnection conn, string toUserName)
        {
            if (conn == null)
                throw new ArgumentNullException("conn");

            if (string.IsNullOrEmpty(toUserName))
                throw new ArgumentNullException("toUserName");

            this.conn = conn;
            this.toUserName = toUserName;
            labUserName.Text = toUserName;
            this.conn.OnMessage += new agsXMPP.protocol.client.MessageHandler(conn_OnMessage);
        }

        void conn_OnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {
            if (msg.To.User.Equals(this.conn.FullUserName))
            {
                OnRecieveMessage(msg as WSMessage);
            }
        }

        #region 方法

        private void AppendRevHtml(string html)
        {
            //edChat.AppendHtml(html, Color.Red);
            //edChat.AppendNewLine(2);
            chatBrower.AppendLeftHtml(toUserName, html);

        }

        private void AppendSendHtml(string html)
        {
            chatBrower.AppendRightHtml(conn.UserName, html);
            //edChat.AppendHtml(html, Color.Blue);
            //edChat.AppendNewLine(2);
        }


        private void  RecieveLocationMessage(LocationBody body)
        {
            AppendRevHtml(String.Format("<div>我现在的位置是{0}(经度:{1};纬度:{2})</div>", body.addr, body.lat, body.lng));
        }

        private void RecieveVideoMessage(VideoBody body)
        {
            AppendRevHtml("未处理视频数据");
        }

        private void RecieveFileMessage(FileBody body)
        {
            AppendRevHtml(String.Format("<div>收到文件:{0}({1})</div>", body.filename, body.file_length));
        }

        private void RecieveAudioMessage(AudioBody body)
        {
            byte[] bytes = SdkUtils.DownloadThumbnail(body.url, body.secret, conn.TokenData.access_token);

            string fileTempDir = SdkUtils.GetRevFileTempDir();

            File.WriteAllBytes(fileTempDir + "\\" + body.filename, bytes);

            string wavFile = AudioConverter.ConvertToWave(fileTempDir, body.filename, Path.GetFileNameWithoutExtension(body.filename) + ".wav");

            if (!File.Exists(wavFile))
            {
                return;
            }

            chatBrower.AppendLeftHtml(toUserName, "收到音频");

            //AppendAudioTag(null);

            PlayAudio(wavFile);
        }

        private void AppendAudioTag(string text)
        {
            Bitmap bmp = Easemob.UI.Properties.Resources.audio;
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            byte[] bmpbytes = ms.GetBuffer();
            //ms.Read(bmpbytes, 0, bmpbytes.Length);
            bmp.Dispose();
            ms.Dispose();

            //edChat.AppendImageBytes(bmpbytes, "image/gif");
            //if (!string.IsNullOrEmpty(text))
                //chatBrower.AppendLeftHtml(toUserName, content);

            //edChat.AppendNewLine(2);
        }

        private void PlayAudio(string wavFile)
        {

        }

        /// <summary>
        /// 接收图片消息
        /// </summary>
        /// <param name="imgBody"></param>
        private void RecieveImageMessage(ImageBody imgBody)
        {
            /*
                       * [{"type":"img","url":"https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles/cd6f8050-81f7-11e5-a16a-05187e341cb0",
                   * "secret":"zW-AWoH3EeWmJevV5n4Fpkxnnu3e5okMLIhENE0QHaZbvqg5",
           * "filename":"原生+自定义.jpg",
                   * "thumb":"https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles/cd6f8050-81f7-11e5-a16a-05187e341cb0",
                   * "thumb_secret":"","size":{"width":952,"height":671}}]
                       */

            string url = imgBody.url;
            string secret = imgBody.secret;
            string filename = imgBody.filename;
            string thumb = imgBody.thumb;
            string thumb_secret = imgBody.thumb_secret;
            int width = 0;
            int height = 0;
            byte[] bytes = null;
            if (!string.IsNullOrEmpty(thumb))
            {
                width = imgBody.size.width;
                height = imgBody.size.height;

                bytes = SdkUtils.DownloadThumbnail(thumb, secret, conn.TokenData.access_token);
            }


            if (bytes == null || bytes.Length == 0)
            {

            }
            else
            {
                //edChat.AppendImageBytes(bytes);
            }
           // edChat.AppendNewLine(2);
            chatBrower.AppendLeftImage(toUserName, thumb);
        }

        private void OnRecieveMessage(WSMessage msg)
        {
            var received = msg.Received;
            var acked = msg.Acked;
            if (received != null)
            {
                //服务器接收到消息
                return;
            }
            if (acked != null)
            {
                //对方接收到消息
                return;
            }
            MsgData msgdata = msg.GetBodyData();
            var delay = msg.Delay;
            DateTime? sentTime = null;
            // 说明消息为离线消息，需要获取离线消息的时间
            if (delay != null)
            {
                sentTime = delay.Stamp;
                //delay.From;
            }
            //msg
            foreach (var body in msgdata.bodies)
            {
                string title = BuildMsgTitle(msgdata.from, sentTime ?? DateTime.Now);// string.Format("<b>{1}</b> [{0}]", DateTime.Now.ToString("HH:mm:ss"), msgdata.from);
                //edChat.AppendHtml(title, Color.Red);
                //edChat.AppendNewLine();
                switch (body.type)
                {
                    case "txt":
                        string content = ((TextBody)body).msg;
                        //edChat.AppendHtml(content, Color.Red, HorizontalAlignment.Left);
                        //edChat.AppendNewLine(2);
                        chatBrower.AppendLeftHtml(toUserName, content);
                        //userWeb.
                        break;
                    case "img":
                        RecieveImageMessage((ImageBody)body);
                        break;
                    case "audio":
                        RecieveAudioMessage((AudioBody)body);
                        break;
                    case "loc":
                        RecieveLocationMessage((LocationBody)body);
                        break;
                    case "video":
                        RecieveVideoMessage((VideoBody)body);
                        break;
                    case "file":
                        RecieveFileMessage((FileBody)body);
                        break;
                    default:
                        break;
                }

            }
        }

        private void Send()
        {
            if (conn.ConnectionState != agsXMPP.XmppConnectionState.Connected &&
                  conn.ConnectionState != agsXMPP.XmppConnectionState.SessionStarted)
            {
                MessageBox.Show("请连接后再发送消息");
                return;
            }
            try
            {
                string text = edInput.Text;
                conn.SendMessage(toUserName, text);
                string content = string.Format("{0}\r\n{1}", BuildMsgTitle(conn.UserName), BuildHtml(text));
                //edChat.AppendHtml(content, Color.Blue);
                //edChat.AppendNewLine(2);

                chatBrower.AppendLeftHtml(conn.UserName, edInput.Text);
                edInput.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string BuildHtml(string text)
        {
            return text.Replace("\r\n", "<br/>").Replace("\n", "<br/>").Replace("\r", "<br/>");
        }

        private string BuildMsgTitle(string username)
        {
            string title = string.Format("<b>{0}</b> [{1}]", username, DateTime.Now.ToString("HH:mm:ss"));

            return title;
        }

        private string BuildMsgTitle(string username, DateTime dt)
        {
            string title = string.Format("<b>{0}</b> [{1}]", username, dt.ToString("HH:mm:ss"));

            return title;
        }


        private void WriteLog(params string[] msg)
        {
            System.Diagnostics.Debug.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + string.Concat(msg) + "\r\n\r\n");
        }

        #endregion

        #region 事件处理


        private void SendImage()
        {
            if (conn.ConnectionState != agsXMPP.XmppConnectionState.Connected &&
                conn.ConnectionState != agsXMPP.XmppConnectionState.SessionStarted)
            {
                MessageBox.Show("请连接后再上传文件");
                return;
            }
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            // Path : /{org_name}/{app_name}/chatfiles
            string path = conn.GetUrl("chatfiles");

            PostedFileResp resp = PostedFileManager.PostFile(path, conn.TokenData.access_token, openFileDialog1.FileName);
            string title = BuildMsgTitle(conn.UserName);
            //edChat.AppendHtml(title, Color.Blue);
            //edChat.AppendNewLine();

            byte[] bytes = SdkUtils.DownloadThumbnail(resp.uri + "/" + resp.entities[0].uuid, resp.entities[0].share_secret, conn.TokenData.access_token);

            //edChat.AppendImageBytes(bytes);
            //edChat.AppendNewLine(2);
            conn.SendFile(toUserName, resp);
        }



        private void btnSend_Click(object sender, EventArgs e)
        {
            //
            // 发送消息
            Send();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // 关闭当前聊天窗口
        }
        #endregion

        /// <summary>
        /// 发送图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnImage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SendImage();
        }
        /// <summary>
        /// 点击表情按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnExpression_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pnlFace.Visible)
                this.pnlFace.Hide();
            else
                this.pnlFace.Show();
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatBox_Load(object sender, EventArgs e)
        {
         
            chatBrower.Uri = Application.StartupPath + "/index.html";
            chatBrower.Size = panelControl2.Size;
            chatBrower.UserWidth = panelControl2.Width;
            chatBrower.UserHeight = panelControl2.Height;
            panelControl2.Controls.Add(chatBrower);
            
            this.pnlFace.Hide();
            userImageExpression1.pictureEdit.Click += new EventHandler(pictureEdit_Click);
            #region 创建DataTable用来存储图片信息


            ///创建DataTable用来存储图片信息
            DataTable table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Character", typeof(string));
            table.Columns.Add("BitFile", typeof(Bitmap));
            DataRow row = table.NewRow();
            row["Name"] = "笑脸";
            row["Character"] = "[):]";
            row["BitFile"] = Resources.笑脸;
            table.Rows.Add(row);
            DataRow row1 = table.NewRow();
            row1["Name"] = "大笑";
            row1["Character"] = "[:D]";
            row1["BitFile"] = Resources.大笑;
            table.Rows.Add(row1);
            DataRow row2 = table.NewRow();
            row2["Name"] = "得意";
            row2["Character"] = "[;)]";
            row2["BitFile"] = Resources.得意;
            table.Rows.Add(row2);
            DataRow row3 = table.NewRow();
            row3["Name"] = "惊讶";
            row3["Character"] = "[:-o]";
            row3["BitFile"] = Resources.惊讶;
            table.Rows.Add(row3);
            DataRow row4 = table.NewRow();
            row4["Name"] = "调皮";
            row4["Character"] = "[:p]";
            row4["BitFile"] = Resources.调皮;
            table.Rows.Add(row4);
            DataRow row5 = table.NewRow();
            row5["Name"] = "酷";
            row5["Character"] = "[(H)]";
            row5["BitFile"] = Resources.酷;
            table.Rows.Add(row5);
            DataRow row6 = table.NewRow();
            row6["Name"] = "发怒";
            row6["Character"] = "[:@]";
            row6["BitFile"] = Resources.发怒;
            table.Rows.Add(row6);
            DataRow row7 = table.NewRow();
            row7["Name"] = "饥饿";
            row7["Character"] = "[:s]";
            row7["BitFile"] = Resources.饥饿;
            table.Rows.Add(row7);
            DataRow row8 = table.NewRow();
            row8["Name"] = "害羞";
            row8["Character"] = "[:$]";
            row8["BitFile"] = Resources.害羞;
            table.Rows.Add(row8);
            DataRow row9 = table.NewRow();
            row9["Name"] = "不开心";
            row9["Character"] = "[:(]";
            row9["BitFile"] = Resources.不开心;
            table.Rows.Add(row9);
            DataRow row10 = table.NewRow();
            row10["Name"] = "大哭";
            row10["Character"] = "[:'(]";
            row10["BitFile"] = Resources.大哭;
            table.Rows.Add(row10);
            DataRow row11 = table.NewRow();
            row11["Name"] = "瞪眼";
            row11["Character"] = "[:|]";
            row11["BitFile"] = Resources.瞪眼;
            table.Rows.Add(row11);
            DataRow row12 = table.NewRow();
            row12["Name"] = "开心";
            row12["Character"] = "[(a)]";
            row12["BitFile"] = Resources.开心;
            table.Rows.Add(row12);
            DataRow row13 = table.NewRow();
            row13["Name"] = "大怒";
            row13["Character"] = "[8o|]";
            row13["BitFile"] = Resources.大怒;
            table.Rows.Add(row13);
            DataRow row14 = table.NewRow();
            row14["Name"] = "眼镜";
            row14["Character"] = "[8-|]";
            row14["BitFile"] = Resources.眼镜;
            table.Rows.Add(row14);
            DataRow row15 = table.NewRow();
            row15["Name"] = "绿脸";
            row15["Character"] = "[+o(]";
            row15["BitFile"] = Resources.绿脸;
            table.Rows.Add(row15);
            DataRow row16 = table.NewRow();
            row16["Name"] = "圣诞";
            row16["Character"] = "[<o)]";
            row16["BitFile"] = Resources.圣诞;
            table.Rows.Add(row16);
            DataRow row17 = table.NewRow();
            row17["Name"] = "疲倦";
            row17["Character"] = "[|-)]";
            row17["BitFile"] = Resources.疲倦;
            table.Rows.Add(row17);
            DataRow row18 = table.NewRow();
            row18["Name"] = "想问题";
            row18["Character"] = "[*-)]";
            row18["BitFile"] = Resources.想问题;
            table.Rows.Add(row18);
            DataRow row19 = table.NewRow();
            row19["Name"] = "闭嘴";
            row19["Character"] = "[:-#]";
            row19["BitFile"] = Resources.闭嘴;
            table.Rows.Add(row19);
            DataRow row20 = table.NewRow();
            row20["Name"] = "对骂";
            row20["Character"] = "[:-*]";
            row20["BitFile"] = Resources.对骂;
            table.Rows.Add(row20);
            DataRow row21 = table.NewRow();
            row21["Name"] = "急眼";
            row21["Character"] = "[^o)]";
            row21["BitFile"] = Resources.急眼;
            table.Rows.Add(row21);
            DataRow row22 = table.NewRow();
            row22["Name"] = "无表情";
            row22["Character"] = "[8-)]";
            row22["BitFile"] = Resources.无表情;
            table.Rows.Add(row22);
            DataRow row23 = table.NewRow();
            row23["Name"] = "爱心";
            row23["Character"] = "[(|)]";
            row23["BitFile"] = Resources.爱心;
            table.Rows.Add(row23);
            DataRow row24 = table.NewRow();
            row24["Name"] = "心碎 ";
            row24["Character"] = "[(u)]";
            row24["BitFile"] = Resources.心碎;
            table.Rows.Add(row24);
            DataRow row25 = table.NewRow();
            row25["Name"] = "月亮";
            row25["Character"] = "[(S)]";
            row25["BitFile"] = Resources.月亮;
            table.Rows.Add(row25);
            DataRow row26 = table.NewRow();
            row26["Name"] = "星星";
            row26["Character"] = "[(*)]";
            row26["BitFile"] = Resources.星星;
            table.Rows.Add(row26);
            DataRow row27 = table.NewRow();
            row27["Name"] = "太阳";
            row27["Character"] = "[(#)]";
            row27["BitFile"] = Resources.太阳;
            table.Rows.Add(row27);
            DataRow row28 = table.NewRow();
            row28["Name"] = "彩虹";
            row28["Character"] = "[(R)]";
            row28["BitFile"] = Resources.彩虹;
            table.Rows.Add(row28);
            DataRow row29 = table.NewRow();
            row29["Name"] = "色";
            row29["Character"] = "[({)]";
            row29["BitFile"] = Resources.色;
            table.Rows.Add(row29);
            DataRow row30 = table.NewRow();
            row30["Name"] = "亲嘴";
            row30["Character"] = "[(})]";
            row30["BitFile"] = Resources.亲嘴;
            table.Rows.Add(row30);
            DataRow row31 = table.NewRow();
            row31["Name"] = "嘴唇";
            row31["Character"] = "[(k)]";
            row31["BitFile"] = Resources.嘴唇;
            table.Rows.Add(row31);
            DataRow row32 = table.NewRow();
            row32["Name"] = "玫瑰花";
            row32["Character"] = "[(F)]";
            row32["BitFile"] = Resources.玫瑰花;
            table.Rows.Add(row32);
            DataRow row33 = table.NewRow();
            row33["Name"] = "花谢了";
            row33["Character"] = "[(W)]";
            row33["BitFile"] = Resources.花谢了;
            table.Rows.Add(row33);
            DataRow row34 = table.NewRow();
            row34["Name"] = "顶";
            row34["Character"] = "[(D)]";
            row34["BitFile"] = Resources.顶;
            table.Rows.Add(row34);

            foreach (DataRow item in table.Rows)
            {
                UserImageExpression userImageExpression = new UserImageExpression();
                userImageExpression.Name = item["Name"].ToString();
                userImageExpression.Character = item["Character"].ToString();
                userImageExpression.BitFile = item["BitFile"] as Bitmap;
                userImageExpression.pictureEdit.Click += new EventHandler(pictureEdit_Click_1);
                flowLayoutPanel1.Controls.Add(userImageExpression);
                MemoryStream ms = new MemoryStream();
                ((Bitmap)item["BitFile"]).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] bytes = ms.GetBuffer();
                ms.Close();
            }

            #endregion 

        }

        /// <summary>
        /// 点击关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        void pictureEdit_Click(object sender, EventArgs e)
        {
            this.pnlFace.Hide();
        }

        /// <summary>
        /// 点击关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pictureEdit_Click_1(object sender, EventArgs e)
        {
            PictureEdit pictureEdit = sender as PictureEdit;
            if (pictureEdit.Parent.GetType().Name == "UserImageExpression")
            {
                UserImageExpression userImageExpression = pictureEdit.Parent as UserImageExpression;
                if (userImageExpression != null)
                    this.edInput.Text += userImageExpression.Character;
            }
        }

        private void barBtnAudio_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (conn.ConnectionState != agsXMPP.XmppConnectionState.Connected &&
                conn.ConnectionState != agsXMPP.XmppConnectionState.SessionStarted)
            {
                MessageBox.Show("请连接后再语音");
                return;
            }
            AudioRecordForm form = new AudioRecordForm();
            form.FileNamePrefix = conn.UserName;
            form.ShowDialog(this);
            string filename = form.AudioFileName;
            if (File.Exists(filename))
                SendAudio(filename);
            else
            { 
                //错误
            }
        }


        private void SendAudio(string audioFileName)
        {

            string path = conn.GetUrl("chatfiles");

            PostedFileResp resp = PostedFileManager.PostFile(path, conn.TokenData.access_token, audioFileName);
            string title = BuildMsgTitle(conn.UserName);
            //edChat.AppendHtml(title, Color.Blue);
            //edChat.AppendNewLine();

            byte[] bytes = SdkUtils.DownloadThumbnail(resp.uri + "/" + resp.entities[0].uuid, resp.entities[0].share_secret, conn.TokenData.access_token);

            AppendAudioTag("我的语音");

            conn.SendFile(toUserName, resp);
        }

        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnScreenshot_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CaptureImageTool capture = new CaptureImageTool();
            if (capture.ShowDialog() == DialogResult.OK)
            {
                Image image = capture.Image;
                byte[] bt = GetByteImage(image);

            }
        }
        /// <summary>
        /// 转换byte
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] GetByteImage(Image img)
        {
            byte[] bt = null;
            if (!img.Equals(null))
            {
                using (MemoryStream mostream = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(img);

                    bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Jpeg);//将图像以指定的格式存入缓存内存流

                    bt = new byte[mostream.Length];

                    mostream.Position = 0;//设置留的初始位置

                    mostream.Read(bt, 0, Convert.ToInt32(bt.Length));

                }
            }
            return bt;
        }
    }
}
