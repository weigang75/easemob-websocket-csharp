using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP.Xml.Dom;

namespace Easemob.XmppWebSocket.Protocol
{
    public class Acked : Element
    {
        public Acked()
        {
            TagName = "acked";
            Namespace = agsXMPP.Uri.MSG_RECEIPT;
        }

        public string Id
        {
            get { return GetAttribute("id"); }
            set { SetAttribute("id", value); }
        }
    }
}
