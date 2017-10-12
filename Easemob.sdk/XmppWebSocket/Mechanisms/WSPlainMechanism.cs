using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP.Xml.Dom;
using agsXMPP.Sasl;

namespace Easemob.XmppWebSocket.Mechanisms
{
    /// <summary>
    ///  Plain加密机制
    /// </summary>
    public class WSPlainMechanism : WSMechanism
	{
        #region << Properties and member variables >>

        ///// <summary>
        ///// 
        ///// </summary>
        //public string Username
        //{
        //    // lower case that until i implement our c# port of libIDN
        //    get { return m_Username; }
        //    set { m_Username = value != null ? value.ToLower() : null; }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public string Password
        //{
        //    get { return m_Password; }
        //    set { m_Password = value; }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public string Server
        //{
        //    get { return m_Server; }
        //    set { m_Server = value.ToLower(); }
        //}

        /// <summary>
        /// Extra data for special Sasl mechanisms
        /// </summary>
        public ExtendedData ExtentedData { get; set; }
        #endregion

        public WSPlainMechanism()
		{			
		}


		public override void Parse(Node e)
		{
			// not needed here in PLAIN mechanism
		}


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override agsXMPP.protocol.sasl.Auth BuildeAuthNode()
        {
            return new agsXMPP.protocol.sasl.Auth(agsXMPP.protocol.sasl.MechanismType.PLAIN, Message());
        }

		private string Message()
		{

            //AuthStr = "YWMtwFZU6n3bEeWPKIERbxAADQAAAVHmL3PeTIwn69ar3nW_uwFcIiqVYO2XgGA";

            //string auth_str = "easemob-demo#chatdemoui_march3@easemob.com";
            //auth_str = auth_str + "\0";
            //auth_str = auth_str + "easemob-demo#chatdemoui_march3";
            //auth_str = auth_str + "\0";
            //auth_str = auth_str + "$t$" + AuthStr;

            //string output = Convert.ToBase64String(System.Text.ASCIIEncoding.Default.GetBytes(auth_str));


			// NULL Username NULL Password
			StringBuilder sb = new StringBuilder();
			
			//sb.Append( (char) 0 );
			//sb.Append(this.m_XmppClient.MyJID.Bare);
            sb.Append(Username + "@" + Server);
			sb.Append( (char) 0 );
            sb.Append(Username);
			sb.Append( (char) 0 );
            sb.Append("$t$" + AuthStr);
			
			byte[] msg = Encoding.UTF8.GetBytes(sb.ToString());
			return Convert.ToBase64String(msg, 0, msg.Length);
		}
	}
}