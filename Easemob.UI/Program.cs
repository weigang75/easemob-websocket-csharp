using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Easemob.UI.Utils;

namespace Easemob.UI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            Easemob.Common.Logger.Logger.Initialize(new UILogger(), Common.Logger.LogLevel.Debug);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ChatForm());
        }
    }
}
