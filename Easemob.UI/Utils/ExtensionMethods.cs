using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP;


public static class ExtensionMethods
{
    /// <summary>
    /// 在忽略Resource情况下，判断Jid是否相同
    /// </summary>
    /// <param name="myJid"></param>
    /// <param name="jid"></param>
    /// <returns></returns>
    public static bool EqualsIgnoreResource(this Jid myJid, Jid jid)
    {
        // 都为空，无需比较
        if (myJid == null && jid == null)
            return true;

        // 都为空，无需比较
        if (myJid == null || jid == null)
            return false;

        return myJid.Bare.ToLower().Equals(jid.Bare.ToLower());
    }
}

