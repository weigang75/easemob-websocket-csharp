using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP.Xml.Dom;
using agsXMPP;

namespace Easemob.XmppWebSocket.Protocol
{
    public class WSDelay : Element
    {
        public WSDelay()
        {
            TagName = "delay";
            Namespace = "urn:xmpp:delay";
        }

        public string Id
        {
            get { return GetAttribute("id"); }
            set { SetAttribute("id", value); }
        }

        public Jid From 
        {
            get { return GetAttribute("from"); }
            set { SetAttribute("from", value); }
        }
        
        public DateTime Stamp
        {
            get 
            { 
                string s = GetAttribute("stamp");
                return DateTime.Parse(s);
            }
            set
            {
                SetAttribute("stamp", value.ToString()); 
            }
        }
    }
}
