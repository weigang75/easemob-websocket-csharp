using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easemob.XmppWebSocket;

namespace Easemob.UI.Definitions
{
    public interface IXmppWebSocketConnectionRequired
    {
        void AttachConnection(XmppWebSocketConnection connection);
    }
}
