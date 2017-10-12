using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easemob.WebSocket.Client;
using agsXMPP;
using agsXMPP.Xml.Dom;
using agsXMPP.Xml;
using agsXMPP.Net;
using agsXMPP.protocol.stream;
using agsXMPP.protocol.sasl;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using System.Net;
using Newtonsoft.Json;
using agsXMPP.Sasl;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.session;
using Easemob.PostedFile;
using System.IO;
using System.Threading;
using Easemob.XmppWebSocket.Init;
using Easemob.XmppWebSocket.Grabbers;
using Easemob.XmppWebSocket.Protocol;
using Easemob.XmppWebSocket.Mechanisms;

namespace Easemob.XmppWebSocket
{
    public class XmppWebSocketConnection : IDisposable
    {
        #region 私有属性
        private WebSocketClient webSocketClient = null;


        #endregion

        #region 公有属性
        private XmppConnectionState m_ConnectionState = XmppConnectionState.Disconnected;
        /// <summary>
        /// 连接的状态
        /// </summary>
        public XmppConnectionState ConnectionState
        {
            get
            {
                return m_ConnectionState;
            }
        }

        private WSRosterManager m_RosterManager = null;
        public WSRosterManager RosterManager
        {
            get { return m_RosterManager; }
        }


        private WSIqGrabber m_IqGrabber = null;
        /// <summary>
        /// IQ消息匹配的拦截器，发送IQ和返回IQ匹配
        /// </summary>
        public WSIqGrabber IqGrabber
        {
            get { return m_IqGrabber; }
        }

        private WSPresenceManager m_PresenceManager = null;
        /// <summary>
        /// 
        /// </summary>
        public WSPresenceManager PresenceManager
        {
            get { return m_PresenceManager; }
        }

        private WSPresenceGrabber m_PresenceGrabber = null;
        /// <summary>
        /// 
        /// </summary>
        public WSPresenceGrabber PresenceGrabber
        {
            get { return m_PresenceGrabber; }
        }

        private WSMessageGrabber m_MessageGrabber = null;
        /// <summary>
        /// 
        /// </summary>
        public WSMessageGrabber MessageGrabber
        {
            get { return m_MessageGrabber; }
        }

        /// <summary>
        /// 是否登录验证过
        /// </summary>
        public bool Authenticated { private get; set; }
        /// <summary>
        /// 是否已经绑定
        /// </summary>
        public bool Binded { private get; set; }


        private StreamParser m_StreamParser = null;
        /// <summary>
        /// the underlaying XMPP StreamParser. Normally you don't need it, but we make it accessible for
        /// low level access to the stream
        /// </summary>
        public StreamParser StreamParser
        {
            get { return m_StreamParser; }
        }
        /// <summary>
        /// 心跳包的周期（秒），表示多少秒一次心跳包
        /// </summary>
        public int HeartbeatPeriod { get; set; }

        public string m_UserName = null;
        /// <summary>
        /// 用户名（比如：march3）
        /// </summary>
        public string UserName 
        {
            get
            {
                return m_UserName;
            }
            set
            {
                m_UserName = value;
            }
        }
        /// <summary>
        /// 用户全名（比如：jiaparts#unipeiim_march3）
        /// </summary>
        public string FullUserName 
        {
            get
            {
                return BuildFullUserName(UserName);
            }
        }

        /// <summary>
        /// Token信息，登录成功后获取
        /// </summary>
        public TokenData TokenData { get; private set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// easemob.com
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// webim
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// AppKey 比如：jiaparts#unipeiim
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// Sends the presence Automatically after successful login.
        /// This property works only in combination with AutoRoster (AutoRoster = true).
        /// </summary>
        public bool AutoPresence { get; set; }       
        #endregion

        #region 事件
        public event XmppConnectionStateHandler OnXmppConnectionStateChanged;

        /// <summary>
        /// Data received from the Socket
        /// </summary>
        public event BaseSocket.OnSocketDataHandler OnReadSocketData;

        /// <summary>
        /// Data was sent to the socket for sending
        /// </summary>
        public event BaseSocket.OnSocketDataHandler OnWriteSocketData;
        /// <summary>
        /// 
        /// </summary>
        public event ErrorHandler OnError;

        /// <summary>
        /// 
        /// </summary>        
        public event IqHandler OnIq;

        /// <summary>
        /// We received a message. This could be a chat message, headline, normal message or a groupchat message. 
        /// There are also XMPP extension which are embedded in messages. 
        /// e.g. X-Data forms.
        /// </summary>
        public event MessageHandler OnMessage;

        /// <summary>
        /// We received a presence from a contact or chatroom.
        /// Also subscriptions is handles in this event.
        /// </summary>
        public event PresenceHandler OnPresence;

        /// <summary>
        /// This event is raised when a response to a roster query is received. This event always contains a single RosterItem. 
        /// e.g. you have 150 friends on your contact list, then this event is called 150 times.
        /// </summary>
        /// <remarks>see also OnRosterItem and OnRosterEnd</remarks>
        public event agsXMPP.XmppClientConnection.RosterHandler OnRosterItem;


        /// <summary>
        /// This event is raised when a response to a roster query is received. The roster query contains the contact list.
        /// This lost could be very large and could contain hundreds of contacts. The are all send in a single XML element from 
        /// the server. Normally you show the contact list in a GUI control in you application (treeview, listview). 
        /// When this event occurs you couls Suspend the GUI for faster drawing and show change the mousepointer to the hourglass
        /// </summary>
        /// <remarks>see also OnRosterItem and OnRosterEnd</remarks>
        public event ObjectHandler OnRosterStart;

        /// <summary>
        /// This event is raised when a response to a roster query is received. It notifies you that all RosterItems (contacts) are
        /// received now.
        /// When this event occurs you could Resume the GUI and show the normal mousepointer again.
        /// </summary>
        /// <remarks>see also OnRosterStart and OnRosterItem</remarks>
        public event ObjectHandler OnRosterEnd;

        /// <summary>
        /// Event that occurs on authentication errors
        /// e.g. wrong password, user doesnt exist etc...
        /// </summary>
        public event XmppElementHandler OnAuthError;

        public event SaslEventHandler OnSaslStart;
        public event ObjectHandler OnSaslEnd;

        private AutoResetEvent runHeartbeat = new AutoResetEvent(false);

        #endregion

        #region 构造函数
        public XmppWebSocketConnection()
        {
            // 初始化
            XmppInit.Init();
            // 
            InitConnection();

            InitPropObj();
        }

        //~XmppWebSocketConnection()
        //{
        //    Dispose();
        //}
        #endregion

        #region 初始化方法
        /// <summary>
        /// 初始化属性对象
        /// </summary>
        private void InitPropObj()
        {
            m_StreamParser = new StreamParser();
            m_StreamParser.OnStreamStart += new StreamHandler(StreamParserOnStreamStart);
            m_StreamParser.OnStreamEnd += new StreamHandler(StreamParserOnStreamEnd);
            m_StreamParser.OnStreamElement += new StreamHandler(StreamParserOnStreamElement);
            m_StreamParser.OnStreamError += new StreamError(StreamParserOnStreamError);
            m_StreamParser.OnError += new ErrorHandler(StreamParserOnError);

            AutoPresence = true;

            HeartbeatPeriod = 0;// 默认0分钟,不开启心跳包
            // 
            m_IqGrabber = new WSIqGrabber(this);
            m_PresenceManager = new WSPresenceManager(this);
            m_RosterManager = new WSRosterManager(this);
            m_PresenceGrabber = new WSPresenceGrabber(this);
            m_MessageGrabber = new WSMessageGrabber(this);
        }
        /// <summary>
        /// 初始化连接
        /// </summary>
        private void InitConnection()
        {
            WSOptions wsOptions = new WSOptions();
            wsOptions.MaskingEnabled = true;
            wsOptions.MaxReceiveFrameLength = 1048576; // 包的最大长度
            wsOptions.ActivityTimerEnabled = true;

            // init websocket client
            this.webSocketClient = new WebSocketClient("websocket", wsOptions);
            this.webSocketClient.ConnectionChanged += new WSDelegates.ConnectionChangedEventHandler(webSocketClient_ConnectionChanged);
            this.webSocketClient.TextMessageReceived += new WSDelegates.TextMessageReceivedEventHandler(webSocketClient_TextMessageReceived);
            this.webSocketClient.Error += new WSDelegates.ErrorEventHandler(webSocketClient_Error);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        private void OnReceive(object sender, byte[] data, int count)
        {
            if (OnReadSocketData != null)
                OnReadSocketData(sender, data, count);

            // put the received bytes to the parser
            lock (this)
            {
                StreamParser.Push(data, 0, count);
            }
        }

        /// <summary>
        /// 构建用户名（全用户名）
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private string BuildFullUserName(string username)
        {
            return string.Concat(AppKey, "_", username);
        }
        /// <summary>
        /// 构建JID
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private string BuildJid(string username)
        {
            return string.Concat(AppKey, "_", username, "@", Server);
        }

        /// <summary>
        /// 开启心跳包
        /// </summary>
        /// <param name="state"></param>
        private void DoHeartbeat(object state)
        {
            if (HeartbeatPeriod <= 0)
                return;

            while (true)
            {
                runHeartbeat.WaitOne(HeartbeatPeriod);

                if (ConnectionState == XmppConnectionState.SessionStarted ||
                    ConnectionState == XmppConnectionState.Connected)
                {
                    System.Diagnostics.Debug.WriteLine("发送一个心跳包");
                    SendHeartBeat();
                }
                else
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 发送心跳包
        /// </summary>
        private void SendHeartBeat()
        {
            //agsXMPP.protocol.extensions.ping.Ping ping = new agsXMPP.protocol.extensions.ping.Ping();
            Message msg = new Message();
            msg.To = new Jid(Server);
            msg.Type = MessageType.normal;
            msg.Body = "{}";
            msg.Id = Guid.NewGuid().ToString();
            //<message to='easemob.com' type='normal' id='WEBIM_2a47877c8e' xmlns='jabber:client'><body>{}</body></message>
            Send(msg);
        }

        /// <summary>
        /// 连接断开后重新设置状态
        /// </summary>
        private void Reset()
        {
            this.Authenticated = false;
            this.Binded = false;
            this.runHeartbeat.Set();
        }



        /// <summary>
        /// 发送打开连接包
        /// </summary>
        private void SendOpen()
        {
            Send("<open xmlns='urn:ietf:params:xml:ns:xmpp-framing' to='" + this.Server + "' version='1.0'/>");
        }


        /// <summary>
        /// 处理StreamElement
        /// </summary>
        /// <param name="e"></param>
        private void OnStreamElement(Node e)
        {
            if (ConnectionState == XmppConnectionState.Securing
                   || ConnectionState == XmppConnectionState.StartCompression)
                return;

            if (e is Features)
            {
                Features f = e as Features;

                SaslEventArgs args = new SaslEventArgs(f.Mechanisms);

                if (OnSaslStart != null)
                    OnSaslStart(this, args);

                if (args.Auto)
                {
                    if (!Authenticated)
                    {
                        string mechanism = null;
                        if (f.Mechanisms != null)
                        {
                            if (f.Mechanisms.SupportsMechanism(MechanismType.SCRAM_SHA_1))
                            {
                                mechanism = agsXMPP.protocol.sasl.Mechanism.GetMechanismName(MechanismType.SCRAM_SHA_1);
                            }
                            if (f.Mechanisms.SupportsMechanism(MechanismType.DIGEST_MD5))
                            {
                                mechanism = agsXMPP.protocol.sasl.Mechanism.GetMechanismName(MechanismType.DIGEST_MD5);
                            }
                            else if (f.Mechanisms.SupportsMechanism(MechanismType.PLAIN))
                            {
                                mechanism = agsXMPP.protocol.sasl.Mechanism.GetMechanismName(MechanismType.PLAIN);
                            }
                        }
                        args.Mechanism = mechanism;
                        if (String.IsNullOrEmpty(mechanism))
                        {
                            //
                        }
                        else
                        {
                            WSMechanism wsmechanism = MechanismFactory.GetMechanism(mechanism);
                            // Set properties for the SASL mechanism
                            wsmechanism.Username = FullUserName;
                            wsmechanism.Password = Password;
                            wsmechanism.Server = Server;
                            wsmechanism.AuthStr = TokenData.access_token;// "YWMtwFZU6n3bEeWPKIERbxAADQAAAVHmL3PeTIwn69ar3nW_uwFcIiqVYO2XgGA";
                            // Call Init Method on the mechanism
                            Send(wsmechanism.BuildeAuthNode());
                        }
                    }
                    else if (!Binded) // 如果没有绑定，则发送绑定消息
                    {
                        if (f.SupportsBind)
                        {
                            DoChangeXmppConnectionState(XmppConnectionState.Binding);

                            BindIq bIq = string.IsNullOrEmpty(Resource) ? new BindIq(IqType.set) : new BindIq(IqType.set, Resource);

                            this.IqGrabber.SendIq(bIq, BindResult, null);
                        }
                    }
                }
            }
            else if (e is Success)
            {
                if (OnSaslEnd != null)
                    OnSaslEnd(this);

                // SASL authentication was successfull
                DoChangeXmppConnectionState(XmppConnectionState.Authenticated);
                Authenticated = true;
                SendOpen();
            }
            else if (e is Failure)
            {
                // Authentication failure
                if (OnAuthError != null)
                    OnAuthError(this, (Failure)e);
            }
            else if (e is Message)
            {
                Message msg = (Message)e;
                string body = msg.Body; //msg.Type;
            }
        }

        /// <summary>
        /// 处理绑定返回的IQ消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="iq"></param>
        /// <param name="data"></param>
        private void BindResult(object sender, IQ iq, object data)
        {
            // Once the server has generated a resource identifier for the client or accepted the resource 
            // identifier provided by the client, it MUST return an IQ stanza of type "result" 
            // to the client, which MUST include a <jid/> child element that specifies the full JID for 
            // the connected resource as determined by the server:

            // Server informs client of successful resource binding: 
            // <iq type='result' id='bind_2'>
            //  <bind xmlns='urn:ietf:params:xml:ns:xmpp-bind'>
            //    <jid>somenode@example.com/someresource</jid>
            //  </bind>
            // </iq>
            if (iq.Type == IqType.result)
            {
                // i assume the server could assign another resource here to the client
                // so grep the resource assigned by the server now
                Element bind = iq.SelectSingleElement(typeof(Bind));
                if (bind != null)
                {
                    Jid jid = ((Bind)bind).Jid;
                    Resource = jid.Resource;
                    //UserName = jid.User;
                }
                // 已经绑定
                DoChangeXmppConnectionState(XmppConnectionState.Binded);
                Binded = true;

                //DoRaiseEventBinded();

                // 绑定成功后,可以为开始会话状态
                DoChangeXmppConnectionState(XmppConnectionState.StartSession);
                SessionIq sIq = new SessionIq(IqType.set, new Jid(Server));
                // 发送会话请求IQ
                this.IqGrabber.SendIq(sIq, SessionResult, null);

            }
            else if (iq.Type == IqType.error)
            {
                if (OnError != null)
                {
                    OnError(this, new Exception("绑定请求失败"));
                }
            }
        }
        /// <summary>
        /// 处理返回的会话消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="iq"></param>
        /// <param name="data"></param>
        private void SessionResult(object sender, IQ iq, object data)
        {
            if (iq.Type == IqType.result)
            {
                // 会话已打开
                DoChangeXmppConnectionState(XmppConnectionState.SessionStarted);
                //RaiseOnLogin();
                // 获取好友列表
                RequestRoster();
            }
            else if (iq.Type == IqType.error)
            {
                if (OnError != null)
                {
                    OnError(this, new Exception("会话请求失败"));
                }
            }
        }




        /// <summary>
        /// 状态发生改变使用该函数
        /// </summary>
        /// <param name="state"></param>
        private void DoChangeXmppConnectionState(XmppConnectionState state)
        {
            m_ConnectionState = state;

            if (OnXmppConnectionStateChanged != null)
                OnXmppConnectionStateChanged(this, state);
        }
        #endregion

        #region << StreamParser Events >>
        public virtual void StreamParserOnStreamStart(object sender, Node e)
        {
            string xml = e.ToString().Trim();
            xml = xml.Substring(0, xml.Length - 2) + ">";

            //this.FireOnReadXml(this, xml);

            //protocol.Stream st = (protocol.Stream)e;
            //if (st != null)
            //{
            //    m_StreamId = st.StreamId;
            //    m_StreamVersion = st.Version;
            //}
        }

        public virtual void StreamParserOnStreamEnd(object sender, Node e)
        {
            Element tag = e as Element;

            string qName;
            if (tag.Prefix == null)
                qName = tag.TagName;
            else
                qName = tag.Prefix + ":" + tag.TagName;

            string xml = "</" + qName + ">";

           // this.FireOnReadXml(this, xml);
        }
        //private Node CurrentNode = null;
        public virtual void StreamParserOnStreamElement(object sender, Node e)
        {
            //CurrentNode = e;
            OnStreamElement(e);
            FireStreamElement(e);
        }

        private void OnRosterIQ(IQ iq)
        {
            // if type == result then it must be the "FullRoster" we requested
            // in this case we raise OnRosterStart and OnRosterEnd
            // 
            // if type == set its a new added r updated rosteritem. Here we dont raise
            // OnRosterStart and OnRosterEnd
            if (iq.Type == IqType.result && OnRosterStart != null)
                OnRosterStart(this);

            Roster r = iq.Query as Roster;
            if (r != null)
            {
                foreach (RosterItem i in r.GetRoster())
                {
                    if (OnRosterItem != null)
                        OnRosterItem(this, i);
                }
            }

            if (iq.Type == IqType.result && OnRosterEnd != null)
                OnRosterEnd(this);

            //if (AutoPresence && iq.Type == IqType.result)
            //    SendMyPresence();
        }


        /// <summary>
        /// Sends our Presence, the packet is built of Status, Show and Priority
        /// </summary>
        public void SendMyPresence()
        {
            Presence pres = new Presence(ShowType.chat, "在线", 10);
            this.Send(pres);
        }

        private void FireStreamElement(Node e)
        {
            if (e is IQ)
            {
                if (OnIq != null)
                    OnIq(this, e as IQ);

                IQ iq = e as IQ;
                if (iq != null && iq.Query != null)
                {
                    // Roster
                    if (iq.Query is Roster)
                        OnRosterIQ(iq);
                }
            }
            else if (e is Message)
            {
                if (OnMessage != null)
                    OnMessage(this, e as Message);
            }
            else if (e is Presence)
            {
                if (OnPresence != null)
                    OnPresence(this, e as Presence);
            }
            else if (e is Features)
            {
                // Stream Features
                // StartTLS stuff
                Features f = e as Features;
            }
            //else if (e is Compressed)
            //{
            //    //DoChangeXmppConnectionState(XmppConnectionState.StartCompression);
            //    StreamParser.Reset();
            //    ClientSocket.StartCompression();
            //    // Start new Stream Header compressed.
            //    SendStreamHeader(false);

            //    DoChangeXmppConnectionState(XmppConnectionState.Compressed);
            //}
            else if (e is agsXMPP.protocol.Error)
            {
                if (OnError != null)
                {
                    Exception ex = new Exception(((agsXMPP.protocol.Error)e).Text);
                    OnError(this, ex);
                }
            }

        }
        public virtual void StreamParserOnStreamError(object sender, Exception ex)
        {
            if (OnError != null)
                OnError(this, ex);
        }
        public virtual void StreamParserOnError(object sender, Exception ex)
        {
            if (OnError != null)
                OnError(this, ex);
        }
        #endregion

        #region  事件处理
        private void webSocketClient_ConnectionChanged(WebSocketState websocketState)
        {
            switch (websocketState)
            {
                case WebSocketState.Initialized:
                    DoChangeXmppConnectionState(XmppConnectionState.Disconnected);
                    break;
                case WebSocketState.Connecting:
                    DoChangeXmppConnectionState(XmppConnectionState.Connecting);
                    break;
                case WebSocketState.Connected:
                    DoChangeXmppConnectionState(XmppConnectionState.Connected);
                    // 连接成功后，发送打开
                    SendOpen();
                    // 打开心跳包
                    if (HeartbeatPeriod >0)
                        ThreadPool.QueueUserWorkItem(DoHeartbeat);
                    break;
                case WebSocketState.Disconnecting:
                    DoChangeXmppConnectionState(XmppConnectionState.Disconnected);
                    Reset();
                    break;
                case WebSocketState.Disconnected:
                    //DoChangeXmppConnectionState(XmppConnectionState.Disconnected);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 接收到字节消息
        /// </summary>
        /// <param name="dataMessage"></param>
        void webSocketClient_DataMessageReceived(byte[] dataMessage)
        {
            this.OnReceive(this, dataMessage, dataMessage.Length);
        }

        /// <summary>
        /// 接收到文本消息
        /// </summary>
        /// <param name="textMessage"></param>
        private void webSocketClient_TextMessageReceived(string textMessage)
        {
            byte[] dataMessage = System.Text.Encoding.UTF8.GetBytes(textMessage);

            this.OnReceive(this, dataMessage, dataMessage.Length);
        }



        private void webSocketClient_Error(string message, string stackTrace = null)
        {
            Exception ex = new Exception(message);
            if (OnError != null)
                OnError(this, ex);
        }
        #endregion

        #region 公有函数
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Jid GetMyFullJid()
        {
            return CreateFullJid(FullUserName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Jid CreateFullJid(String userId)
        {
            if (!userId.StartsWith(this.AppKey))
            {
                userId = string.Concat(this.AppKey, "_", userId);
            }

            if (String.IsNullOrEmpty(this.Server))
            {
                return new Jid(userId);
            }

            if (userId.IndexOf("@") < 0)
            {
                return new Jid(userId + "@" + this.Server);
            }

            return new Jid(userId);
        }

        /// <summary>
        /// 请求好友列表
        /// </summary>
        public void RequestRoster()
        {
            RosterIq iq = new RosterIq(IqType.get);
            //Send(iq);

            IqGrabber.SendIq(iq, IqResult, null);
        }


        private void IqResult(object sender, IQ iq, object data)
        {
            if (iq.Type == IqType.result)
            {
                // 会话已打开
                //DoChangeXmppConnectionState(XmppConnectionState.SessionStarted);
                //RaiseOnLogin();
                // 获取好友列表
                //RequestRoster();
                if(AutoPresence)
                    SendMyPresence();
            }
            else if (iq.Type == IqType.error)
            {
                if (OnError != null)
                {
                    OnError(this, new Exception("会话请求失败"));
                }
            }
        }


        /// <summary>
        /// 打开连接 wss://im-api.easemob.com/ws;
        /// </summary>
        public string Open(string serverUrl)
        {
            if (TokenData == null ||
                String.IsNullOrEmpty(TokenData.access_token))
            {

                try
                {
                    GetToken();
                }
                catch (System.Net.WebException wex)
                {
                    return "登录失败";
                }
                catch (Exception ex)
                {
                    return "登录失败";
                }

            }
            // string _serverUrl = "wss://im-api.easemob.com/ws";
            this.webSocketClient.Connect(serverUrl);

            return "";
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            // 关闭已发送消息为主，不主动断开连接
            Send("<close xmlns='urn:ietf:params:xml:ns:xmpp-framing' to='easemob.com'/>");
            //webSocketClient.Disconnect();
            //webSocketClient.Dispose();
        }

        /// <summary>
        /// 发送XML消息
        /// </summary>
        /// <param name="xml"></param>
        public void Send(string xml)
        {
            if (this.webSocketClient.State != WebSocketState.Connected)
            {
                throw new ApplicationException("连接已断开，请重新连接再发送。");
            }

            this.webSocketClient.SendText(xml);

            if (OnWriteSocketData != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                OnWriteSocketData(this, bytes, bytes.Length);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(string toUser, string msg)
        {
            Jid to = new Jid(BuildJid(toUser));
            //to.Resource = "mobile";
            Jid from = new Jid(BuildJid(UserName));

            BodyBase[] bodies = new BodyBase[1];
            bodies[0] = new TextBody { type = "txt", msg = msg };
            /* 
             * 发消息：
             * {"from":"weigang75","to":"march3","bodies":[{"type":"txt","msg":"?"}],"ext":{}};*/

            MsgData data = new MsgData() { from = UserName, to = toUser, bodies = bodies, ext = new { } };
            string msgjson = JsonConvert.SerializeObject(data);
            Message msgNode = new Message(to, from, msgjson);
            msgNode.GenerateId();
            //msgNode.Thread = string.Concat("MSG_", (new Random()).Next(20000, 30000));
            msgNode.Type = MessageType.chat;
            Send(msgNode);
        }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="toUser">发给谁</param>
        /// <param name="postedfile">已经上传的文件返回信息</param>
        public void SendFile(string toUser, PostedFileResp postedfile)
        {
            Jid to = new Jid(BuildJid(toUser));
            Jid from = new Jid(BuildJid(UserName));

            BodyBase[] bodies = new BodyBase[postedfile.entities.Length];
            // 构建发送文件的 message 消息
            for (int i = 0; i < postedfile.entities.Length; i++)
            {
                PostFileEntity entity = postedfile.entities[i];
                // 文件类型 img audio
                string otype = SdkUtils.GetFileType(entity.filename);
                // 文件的url
                string ourl = postedfile.uri + "/" + entity.uuid;
                string osecret = entity.share_secret;
                string ofilename = entity.filename;

                /*
传图片
ReceivedData:
<message xmlns='jabber:client' from='easemob-demo#chatdemoui_weigang75@easemob.com/webim' 
 to='easemob-demo#chatdemoui_march3@easemob.com' id='124420481838219668' type='chat'>
 <body>
 {"from":"weigang75","to":"march3","bodies":
 [{"type":"img","url":"https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles/cd6f8050-81f7-11e5-a16a-05187e341cb0",
 "secret":"zW-AWoH3EeWmJevV5n4Fpkxnnu3e5okMLIhENE0QHaZbvqg5",
 "filename":"原生+自定义.jpg",
 "thumb":"https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles/cd6f8050-81f7-11e5-a16a-05187e341cb0",
 "thumb_secret":"","size":{"width":952,"height":671}}],"ext":{}}</body></message>

传语音
ReceivedData:
<message xmlns='jabber:client' from='easemob-demo#chatdemoui_weigang75@easemob.com/webim'
 to='easemob-demo#chatdemoui_march3@easemob.com' id='124421298246910356' type='chat'>
 <body>
 {"from":"weigang75","to":"march3","bodies":
 [{"type":"audio",
 "url":"https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles/3ec2bb50-81f8-11e5-8e7c-1fa6315dec2d",
 "secret":"PsK7WoH4EeWwmIkeyVsexnkK-Rmqu2X_N2qqK9FQYmUkko8W",
 "filename":"环信通讯 - 副本.mp3",
 "length":3}],"ext":{}}</body></message>
             
                         */

                // 如果是文件，需要多一些字段 thumb、thumb_secret、size
                if ("img".Equals(otype))
                {
                    bodies[i] = new ImageBody
                    {
                        type = otype,
                        url = ourl,
                        secret = osecret,
                        filename = ofilename,
                        thumb = ourl,
                        thumb_secret = "",
                        size = new ImageSize
                        {
                            width = entity.imageSize.Width,
                            height = entity.imageSize.Height
                        }
                    };
                }
                else if ("audio".Equals(otype))
                {
                    bodies[i] = new AudioBody
                    {
                        type = otype,
                        url = ourl,
                        secret = osecret,
                        filename = ofilename
                    };
                }
                else
                {
                    bodies[i] = new FileBody
                    {
                        type = otype,
                        url = ourl,
                        secret = osecret,
                        filename = ofilename
                    };
                }
            }

            MsgData data = new MsgData() { from = UserName, to = toUser, bodies = bodies, ext = new { } };

            WSMessage msgNode = new WSMessage(to, from);
            msgNode.Type = MessageType.chat;
            msgNode.SetBodyData(data);

            Send(msgNode);

        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="node"></param>
        public void Send(Node node)
        {
            Send(node.ToString());
        }

        private string baseUrl = null;

        public string GetUrl(string path)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                string[] baseUrls = new string[] { 
                    "http://a1.easemob.com/",
                    "http://a2.easemob.com/"
                    };

                Random r = new Random(DateTime.Now.Millisecond);
                int idx = r.Next(0, baseUrls.Length - 1);
                baseUrl = baseUrls[idx];
            }

            return string.Concat(baseUrl, AppKey.Replace("#", "/"), "/", path.TrimStart('/'));
        }

        public void GetToken()
        {
            WebClient client = new WebClient();
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.89 Safari/537.36");
            client.Headers.Add("Content-Type", "text/plain;charset=UTF-8");
            client.Headers.Add("Accept-Encoding", "gzip, deflate");
            client.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");

            string data = JsonConvert.SerializeObject(new { grant_type = "password", username = UserName, password = Password });
            string path = GetUrl("token");
            string resp = client.UploadString(path, data);

            this.TokenData = JsonConvert.DeserializeObject<TokenData>(resp);

            /*
             * 
             * {"access_token":"YWMtwFZU6n3bEeWPKIERbxAADQAAAVHmL3PeTIwn69ar3nW_uwFcIiqVYO2XgGA","expires_in":5184000,
             * "user":{"uuid":"ba15d15a-7ddb-11e5-be52-b7d1f2170dba","type":"user",
             * "created":1446081837797,"modified":1446081837797,"username":"march3","activated":true,"nickname":"三月三"}}
1-----------------------------
POST /easemob-demo/chatdemoui/token HTTP/1.1
Host: a1.easemob.com
Connection: keep-alive
Content-Length: 67
Origin: null
User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.89 Safari/537.36
Content-Type: text/plain;charset=UTF-8
Accept: 
Accept-Encoding: gzip, deflate
Accept-Language: zh-CN,zh;q=0.8

{"grant_type":"password","username":"march3","password":"123456"}



HTTP/1.1 200 OK
Server: Tengine/2.0.3
Date: Thu, 29 Oct 2015 08:13:04 GMT
Content-Type: application/json;charset=UTF-8
Transfer-Encoding: chunked
Connection: keep-alive
Access-Control-Allow-Origin: null
Access-Control-Allow-Credentials: true

{"access_token":"YWMtwFZU6n3bEeWPKIERbxAADQAAAVHmL3PeTIwn69ar3nW_uwFcIiqVYO2XgGA","expires_in":5184000,"user":{"uuid":"ba15d15a-7ddb-11e5-be52-b7d1f2170dba","type":"user","created":1446081837797,"modified":1446081837797,"username":"march3","activated":true,"nickname":"三月三"}}
             
             
             */
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.runHeartbeat.Set();

            if (webSocketClient != null)
            {
                if (webSocketClient.State == WebSocketState.Connected)
                {
                    webSocketClient.Disconnect();
                }
                webSocketClient.Dispose();
            }
        }
        #endregion
       
    }


    /// <summary>
    /// 存放返回的token数据
    /// </summary>
    public class TokenData
    {
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public UserData user { get; set; }
    }

    public class UserData
    {
        public string uuid { get; set; }
        public string type { get; set; }
        public long created { get; set; }
        public long modified { get; set; }
        public string username { get; set; }
        public bool activated { get; set; }
        public string nickname { get; set; }

    }
}
