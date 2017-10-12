using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easemob.XmppWebSocket.Mechanisms
{
    /// <summary>
    /// 加密机制生成工厂类
    /// </summary>
    public class MechanismFactory
    {
        public static WSMechanism GetMechanism(string mechanism)
        {
            switch (mechanism)
            {
                case "PLAIN":
                    return new WSPlainMechanism();

                default:
                    break;
            }
            // 
            return null;
        }
    }
}
