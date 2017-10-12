using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Easemob.XmppWebSocket;
using Newtonsoft.Json;
using System.Net;
using Easemob.UI.Utils;
using Easemob.XmppWebSocket.Protocol;

namespace Easemob.UI
{
    public partial class ChatForm : Form
    {
        private XmppWebSocketConnection conn = null;

        public ChatForm()
        {
            InitializeComponent();
            Open();
        }
        private bool DemoUserName = false;
        private void Open()
        {
            conn = new XmppWebSocketConnection();
            conn.AutoPresence = true;
            conn.OnMessage += new agsXMPP.protocol.client.MessageHandler(conn_OnMessage);
            conn.OnReadSocketData += new agsXMPP.Net.BaseSocket.OnSocketDataHandler(conn_OnReadSocketData);
            conn.OnWriteSocketData += new agsXMPP.Net.BaseSocket.OnSocketDataHandler(conn_OnWriteSocketData);
            conn.OnError += new agsXMPP.ErrorHandler(conn_OnError);
            if (DemoUserName)
            {
                conn.AppKey = "easemob-demo#chatdemoui";
                conn.UserName = "xiaoc";  
                conn.Password = "123456";
            }
            else
            {
                conn.AppKey = "jiaparts#unipeiim";
                conn.UserName = "xiac"; 
                conn.Password = "123456";
            }
            conn.Server = "easemob.com";
            conn.Resource = "win";
            conn.HeartbeatPeriod = 0;
            string retMsg = conn.Open("wss://im-api.easemob.com/ws");

            if (!String.IsNullOrEmpty(retMsg))
            {
                MessageBox.Show(retMsg);
                return;
            }

            chatBox1.SetParams(conn, "xiachao");
        }

        void conn_OnError(object sender, Exception ex)
        {
            Easemob.Common.Logger.Logger.WriteError("log", string.Concat(ex.Message,"\r\n",ex.StackTrace));
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

        void conn_OnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {
            WSMessage m = msg as WSMessage;
            if (m.Delay != null)
            { 
            
            }
        }

        private void WriteLog(params string[] msg)
        {
            Easemob.Common.Logger.Logger.WriteInfo("log", string.Concat(msg));
            //System.Diagnostics.Debugger.Log(1,"log","[" + DateTime.Now.ToString("HH:mm:ss") + "] " + string.Concat(msg) + "\r\n\r\n");
        }
    }
}
