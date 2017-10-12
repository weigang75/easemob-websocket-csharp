using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easemob.XmppWebSocket.Protocol
{
    public class Received : agsXMPP.protocol.extensions.msgreceipts.Received
    {
        public string MId
        {
            get { return GetAttribute("mid"); }
            set { SetAttribute("mid", value); }
        }
    }
}
