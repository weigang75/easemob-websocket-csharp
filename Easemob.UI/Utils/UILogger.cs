using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easemob.Common.Logger;

namespace Easemob.UI.Utils
{
    public class UILogger : ILogger
    {
        public void WriteInfo(string source, string message)
        {
            System.Diagnostics.Debugger.Log(1, source, "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + message + "\r\n\r\n");
        }

        public void WriteDebug(string source, string message)
        {
            System.Diagnostics.Debugger.Log(3, source, "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + message + "\r\n\r\n");
        }

        public void WriteError(string source, string message, string stackTrace = "")
        {            
            System.Diagnostics.Debugger.Log(5, source, "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + message + "\r\n" + stackTrace + "\r\n\r\n");
        }

        public void Dispose()
        {
            
        }
    }
}
