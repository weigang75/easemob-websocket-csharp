using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP.Xml.Dom;
using agsXMPP.Sasl;

namespace Easemob.XmppWebSocket.Mechanisms
{
    /// <summary>
    ///  加密机制基类
    /// </summary>
    public abstract class WSMechanism
    {
        #region << Properties and member variables >>
        //private XmppWebSocketConnection m_XmppClientConnection;
        private string m_Username;
        private string m_Password;
        private string m_Server;

        //public XmppWebSocketConnection XmppClientConnection
        //{
        //    get { return m_XmppClientConnection; }
        //    set { m_XmppClientConnection = value; }
        //}

        /// <summary>
        /// 
        /// </summary>
        public string Username
        {
            // lower case that until i implement our c# port of libIDN
            get { return m_Username; }
            set { m_Username = value != null ? value.ToLower() : null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Server
        {
            get { return m_Server; }
            set { m_Server = value.ToLower(); }
        }
        public virtual string AuthStr { get; set; }
        /// <summary>
        /// Extra data for special Sasl mechanisms
        /// </summary>
        public ExtendedData ExtentedData { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract agsXMPP.protocol.sasl.Auth BuildeAuthNode();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public abstract void Parse(Node e);
    }
}
