using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP.Factory;

namespace Easemob.XmppWebSocket.Init
{
    public class ProtocolRegister
    {
        private static bool isRegistered = false;
        public static void Register()
        {
            if (isRegistered)
                return;
            isRegistered = true;

            ElementFactory.AddElementType("acked", agsXMPP.Uri.MSG_RECEIPT, typeof(Easemob.XmppWebSocket.Protocol.Acked));
            ElementFactory.AddElementType("received", agsXMPP.Uri.MSG_RECEIPT, typeof(Easemob.XmppWebSocket.Protocol.Received));
            ElementFactory.AddElementType("delay", "urn:xmpp:delay", typeof(Easemob.XmppWebSocket.Protocol.WSDelay));

            //AddElementType("message", Uri.CLIENT, typeof(agsXMPP.protocol.client.Message));
            ElementFactory.AddElementType("message", agsXMPP.Uri.CLIENT, typeof(Easemob.XmppWebSocket.Protocol.WSMessage));
        }
    }
}
