using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP;
using agsXMPP.protocol.client;
using Newtonsoft.Json;

namespace Easemob.XmppWebSocket.Protocol
{
    public class WSMessage : agsXMPP.protocol.client.Message
    {
        #region << Constructors >>
        public WSMessage()
            : base()
        {
            //this.TagName	= "message";
            //this.Namespace = agsXMPP.Uri.CLIENT;
        }

        public WSMessage(Jid to) : this()
        {
            To      = to;
        }
		public WSMessage(Jid to, string body) : this(to)
		{			
			Body	= body;
		}

        public WSMessage(Jid to, Jid from) : this()
        {
            To      = to;
            From    = from;
        }

		public WSMessage(string to, string body) : this()
		{
			To		= new Jid(to);
			Body	= body;
		}

		public WSMessage(Jid to, string body, string subject) : this()
		{
			To		= to;
			Body	= body;
			Subject	= subject;
		}

		public WSMessage(string to, string body, string subject) : this()
		{
			To		= new Jid(to);
			Body	= body;
			Subject	= subject;
		}

		public WSMessage(string to, string body, string subject, string thread) : this()
		{
			To		= new Jid(to);
			Body	= body;
			Subject	= subject;
			Thread	= thread;
		}

		public WSMessage(Jid to, string body, string subject, string thread) : this()
		{
			To		= to;
			Body	= body;
			Subject	= subject;
			Thread	= thread;
		}

		public WSMessage(string to, MessageType type, string body) : this()
		{
			To		= new Jid(to);
			Type	= type;
			Body	= body;
		}

		public WSMessage(Jid to, MessageType type, string body) : this()
		{
			To		= to;
			Type	= type;
			Body	= body;
		}

		public WSMessage(string to, MessageType type, string body, string subject) : this()
		{
			To		= new Jid(to);
			Type	= type;
			Body	= body;
			Subject	= subject;
		}

		public WSMessage(Jid to, MessageType type, string body, string subject) : this()
		{
			To		= to;
			Type	= type;
			Body	= body;
			Subject	= subject;
		}

		public WSMessage(string to, MessageType type, string body, string subject, string thread) : this()
		{
			To		= new Jid(to);
			Type	= type;
			Body	= body;
			Subject	= subject;
			Thread	= thread;
		}

		public WSMessage(Jid to, MessageType type, string body, string subject, string thread) : this()
		{
			To		= to;
			Type	= type;
			Body	= body;
			Subject	= subject;
			Thread	= thread;
		}
	
		public WSMessage(Jid to, Jid from, string body) : this()
		{
			To		= to;
			From	= from;
			Body	= body;
		}

		public WSMessage(Jid to, Jid from, string body, string subject) : this()
		{
			To		= to;
			From	= from;
			Body	= body;
			Subject	= subject;
		}

		public WSMessage(Jid to, Jid from, string body, string subject, string thread) : this()
		{
			To		= to;
			From	= from;
			Body	= body;
			Subject	= subject;
			Thread	= thread;
		}

		public WSMessage(Jid to, Jid from, MessageType type, string body) : this()
		{
			To		= to;
			From	= from;
			Type	= type;
			Body	= body;
		}

		public WSMessage(Jid to, Jid from, MessageType type, string body, string subject) : this()
		{
			To		= to;
			From	= from;
			Type	= type;
			Body	= body;
			Subject	= subject;
		}

        public WSMessage(Jid to, Jid from, MessageType type, string body, string subject, string thread)
            : this()
		{
			To = to;
			From	= from;
			Type	= type;
			Body	= body;
			Subject	= subject;
			Thread	= thread;
		} 

		#endregion

        /// <summary>
        /// 已收到
        /// </summary>
        public Received Received
        {
            get
            {
                return SelectSingleElement(typeof(Received)) as Received;
            }
            set
            {
                if (HasTag(typeof(Received)))
                    RemoveTag(typeof(Received));

                if (value != null)
                    this.AddChild(value);

            }
        }

        /// <summary>
        /// 已读
        /// </summary>
        public Acked Acked
        {
            get
            {
                return SelectSingleElement(typeof(Acked)) as Acked;
            }
            set
            {
                if (HasTag(typeof(Acked)))
                    RemoveTag(typeof(Acked));

                if (value != null)
                    this.AddChild(value);
            
            }
        }

        /// <summary>
        /// 如果不为空说明是离线消息
        /// </summary>
        public WSDelay Delay
        {
            get
            {
                return SelectSingleElement(typeof(WSDelay)) as WSDelay;
            }
            set
            {
                if (HasTag(typeof(WSDelay)))
                    RemoveTag(typeof(WSDelay));

                if (value != null)
                    this.AddChild(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MsgData GetBodyData()
        {
            MsgData msgdata = null;
            try
            {
                Newtonsoft.Json.Linq.JContainer data = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JContainer>(this.Body);
               
                msgdata = new MsgData();
                msgdata.from = data["from"].ToString();
                msgdata.to = data["to"].ToString();
                msgdata.ext = data["ext"];

                Newtonsoft.Json.Linq.JContainer bodies = data.Value<Newtonsoft.Json.Linq.JContainer>("bodies");

                msgdata.bodies = new BodyBase[bodies.Count];

                for (int i = 0; i < bodies.Count; i++)
                {
                    string type = bodies[i]["type"].ToString();

                    switch (type)
                    {
                        case "img":
                            msgdata.bodies[i] = JsonConvert.DeserializeObject<ImageBody>(bodies[i].ToString());
                            break;
                        case "txt":
                            msgdata.bodies[i] = JsonConvert.DeserializeObject<TextBody>(bodies[i].ToString());
                            break;
                        case "audio":
                            msgdata.bodies[i] = JsonConvert.DeserializeObject<AudioBody>(bodies[i].ToString());
                            break;
                        case "video":
                            msgdata.bodies[i] = JsonConvert.DeserializeObject<VideoBody>(bodies[i].ToString());
                            break;
                        case "loc":
                            msgdata.bodies[i] = JsonConvert.DeserializeObject<LocationBody>(bodies[i].ToString());
                            break;
                        case "file":
                            msgdata.bodies[i] = JsonConvert.DeserializeObject<FileBody>(bodies[i].ToString());
                            break;
                        default:                          
                            //msgdata.bodies[i] = JsonConvert.DeserializeObject<FileBody>(bodies[i].ToString());
                            break;
                    }

                    
                }


                //msgdata = JsonConvert.DeserializeObject<MsgData>(this.Body);
            }
            catch (Exception ex)
            {
                
            }            

            if (msgdata != null)
            { 

            }

            return msgdata;
        }

        public void SetBodyData(MsgData data)
        {
            this.Body = JsonConvert.SerializeObject(data);
        }

        //public object m_BodyData = new { };

        //public object BodyData
        //{
        //    get
        //    {
        //        return m_BodyData;
        //    }
        //    set
        //    {
        //        m_BodyData = value;
        //    }
        //}
    }
}
