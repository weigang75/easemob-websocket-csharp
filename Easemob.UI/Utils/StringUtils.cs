using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP;

namespace Easemob.UI.Utils
{
    public static class StringUtils
    {
        public const String STR_JID_FORMAT = "zhangsan";
        /// <summary>
        /// 判断该JID是否为会议室JID。
        /// 一般会议室JID格式为：tech@conference.mob.jzterp.com
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        public static bool IsConferenceJid(this String jid)
        {
            if (jid == null)
                return false;
            
            return jid.IndexOf("@conference.") > 0;
        }

        /// <summary>
        /// 判断该JID是否为会议室JID。
        /// 一般会议室JID格式为：tech@conference.mob.jzterp.com
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        public static bool IsConferenceJid(this Jid jid)
        {
            if (jid == null)
                return false;

            return jid.Bare.IndexOf("@conference.") > 0;
        }
    }
}
