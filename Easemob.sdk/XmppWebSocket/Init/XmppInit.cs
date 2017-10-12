using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easemob.XmppWebSocket.Init
{
    public static class XmppInit
    {
        private static object initLock = new object();
        /// <summary>
        /// 是否初始化，保证只能初始化一次
        /// </summary>
        private static bool inited = false;

        public static void Init()
        {
            lock (initLock)
            {
                if (inited)
                    return;

                inited = true;
                // 注册协议
                ProtocolRegister.Register();
                agsXMPP.Id.Prefix = DateTime.Now.ToString("MMddHHmmss_");
                //agsXMPP.Id.Type = agsXMPP.IdType.Guid;
            }
        }
    }
}
