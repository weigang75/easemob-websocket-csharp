using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Easemob.PostedFile;
using Easemob.XmppWebSocket;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Net;
using Easemob.XmppWebSocket.Protocol;

namespace Easemob.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private XmppWebSocketConnection conn = null;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            conn.AppKey = txtAppKey.Text.Trim();
            conn.UserName = txtUserName.Text;  //"easemob-demo#chatdemoui_march3"; // jiaparts#unipeiim
            conn.Password = txtPwd.Text.Trim();
            conn.Server = "easemob.com";
            conn.Resource = "win";// "webim";
            //conn.Token = "YWMtwFZU6n3bEeWPKIERbxAADQAAAVHmL3PeTIwn69ar3nW_uwFcIiqVYO2XgGA";


            btnOpen.Enabled = false;

            //conn.ProxyServer = "127.0.0.1";
            //string retMsg = conn.Open("wss://im-api.easemob.com/ws");
            string retMsg = conn.Open("wss://im-api.easemob.com/ws");
            if (!String.IsNullOrEmpty(retMsg))
            {
                btnOpen.Enabled = true;
                btnClose.Enabled = false;
                MessageBox.Show(retMsg);
                return;
            }

            //rosterControl1.AttachConnection(conn);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //txtAppKey.Text = "jiaparts#unipeiim";
            //txtUserName.Text = "weigang";
            //txtPwd.Text = "123456";

            //agsXMPP.protocol.extensions.msgreceipts.Received received = new agsXMPP.protocol.extensions.msgreceipts.Received();
            ////received.Id = "123121";
            //received.InnerXml = "123123";

            InitConn();
        }

        private void InitConn()
        {
            conn = new XmppWebSocketConnection();

            chatBox1.SetParams(conn, "weigang75");

            conn.HeartbeatPeriod = 0;
            conn.OnXmppConnectionStateChanged += new agsXMPP.XmppConnectionStateHandler(conn_OnXmppConnectionStateChanged);
            conn.OnReadSocketData += new agsXMPP.Net.BaseSocket.OnSocketDataHandler(conn_OnReadSocketData);
            conn.OnWriteSocketData += new agsXMPP.Net.BaseSocket.OnSocketDataHandler(conn_OnWriteSocketData);
            conn.OnError += new agsXMPP.ErrorHandler(conn_OnError);

            conn.OnMessage += new agsXMPP.protocol.client.MessageHandler(conn_OnMessage);
        }

        void conn_OnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {

        }

        void conn_OnError(object sender, Exception ex)
        {
            WriteLog("ERR:", ex.Message);
        }

        void conn_OnXmppConnectionStateChanged(object sender, agsXMPP.XmppConnectionState state)
        {
            switch (state)
            {
                case agsXMPP.XmppConnectionState.Authenticated:
                    break;
                case agsXMPP.XmppConnectionState.Authenticating:
                    break;
                case agsXMPP.XmppConnectionState.Binded:
                    break;
                case agsXMPP.XmppConnectionState.Binding:
                    break;
                case agsXMPP.XmppConnectionState.Compressed:
                    break;
                case agsXMPP.XmppConnectionState.Connected:
                    btnOpen.Enabled = false;
                    btnClose.Enabled = true;
                    break;
                case agsXMPP.XmppConnectionState.Connecting:
                    btnOpen.Enabled = false;
                    btnClose.Enabled = false;
                    break;
                case agsXMPP.XmppConnectionState.Disconnected:
                    btnOpen.Enabled = true;
                    btnClose.Enabled = false;
                    break;
                case agsXMPP.XmppConnectionState.Registered:
                    break;
                case agsXMPP.XmppConnectionState.Registering:
                    break;
                case agsXMPP.XmppConnectionState.Securing:
                    break;
                case agsXMPP.XmppConnectionState.SessionStarted:
                    break;
                case agsXMPP.XmppConnectionState.StartCompression:
                    break;
                case agsXMPP.XmppConnectionState.StartSession:
                    break;
                default:
                    break;
            }
            WriteLog("Connection state:" + state.ToString());
        }

        void conn_OnWriteSocketData(object sender, byte[] data, int count)
        {
            string xml = System.Text.Encoding.UTF8.GetString(data);
            WriteLog("SentData:" + xml);
        }

        void conn_OnReadSocketData(object sender, byte[] data, int count)
        {
            string xml = System.Text.Encoding.UTF8.GetString(data);

            xml = xml.Replace("&quot;", "\"");
            WriteLog("ReceivedData:" + xml);
        }

        private void WriteLog(params string[] msg)
        {

            txtLog.AppendText("["+ DateTime.Now.ToString("HH:mm:ss")+ "] " + string.Concat(msg) + "\r\n\r\n");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (conn != null && conn.ConnectionState != agsXMPP.XmppConnectionState.Disconnected)
            {
                conn.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (conn != null && conn.ConnectionState != agsXMPP.XmppConnectionState.Disconnected)
            //{
               
            //}

            try
            {
                 conn.Close();
            }
            catch (Exception)
            {
                
            }
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


        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image img = Clipboard.GetImage();
            object obj = Clipboard.GetData("text");
            Stream stream = Clipboard.GetAudioStream();
            IDataObject dobj = Clipboard.GetDataObject();
            StringCollection strCol = Clipboard.GetFileDropList();

            object html = Clipboard.GetData(DataFormats.Html);

        }        

    }
}
