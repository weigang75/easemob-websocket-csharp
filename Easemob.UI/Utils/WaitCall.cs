using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections;

namespace Easemob.UI.Utils
{
    /// <summary>
    /// 等待CALL，该类的功能主要用于防止极短时间内做多次函数操作，比如频繁刷新。
    /// Run方法中传入要运行的函数，会等待一段时间（dueTime），如果又有要运行的相同函数，则会再等待一段时间（dueTime）。
    /// 如果 dueTime 时间到了，没有要运行的函数，则运行第一次传入的函数。
    /// 传入的功能可能是同步，刷新。
    /// </summary>
    public class WaitCall
    {
        private int dueTime = 500;
        private int timeout = 3000;
        public WaitCall(ThreadStart call,int dueTime, int timeout)
        {
            if (call == null)
                throw new ArgumentNullException("call");

            this.thisCall = call;
            this.dueTime = dueTime;
            this.timeout = timeout;
        }
        public WaitCall(ThreadStart call,int dueTime)
        {
            if (call == null)
                throw new ArgumentNullException("call");
            this.thisCall = call;
            this.dueTime = dueTime;
        }
        private Stopwatch watch = new Stopwatch();

        private ThreadStart thisCall = null;

        //private List<ThreadStart> callList = new List<ThreadStart>();

        private Timer timer = null;

        private object Lock_Call = new object();
        //{
        //    get
        //    {
        //        return ((IList)callList).SyncRoot;
        //    }
        //}

        private void RefreshTimer()
        {
            timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            timer.Dispose();
            timer = null;
            timer = new Timer(new TimerCallback(Call));
        }

        public void Run()
        {
            if (thisCall == null)
                return;

            //LockWatcher lkw = LockWatcher.New();
            lock (Lock_Call)
            {
                if (timer == null)
                {
                    timer = new Timer(new TimerCallback(Call));
                }
                else
                {
                    RefreshTimer();
                }

                timer.Change(dueTime, System.Threading.Timeout.Infinite);

                if (!watch.IsRunning)
                {
                    watch.Start();
                }
                else
                {
                    if (watch.ElapsedMilliseconds > timeout)
                    {
                        RefreshTimer();
                        System.Diagnostics.Debug.WriteLine(String.Format("timeout->Call->{0}", DateTime.Now.ToString("HH:mm:ss")));
                        (new Thread(Call)).Start(null);
                        return;
                    }
                }
            }
            //lkw.Stop();
        }

        private void Call(object state)
        {
            //LockWatcher lkw = LockWatcher.New();
            System.Diagnostics.Debug.WriteLine(String.Format("Call->{0}", DateTime.Now.ToString("HH:mm:ss")));
            lock (Lock_Call)
            {
                //System.Diagnostics.Debug.WriteLine("Call....");
                if (timer != null)
                {
                    timer.Dispose();
                    timer = null;
                }
                watch.Stop();
                watch.Reset();
            }
            //lkw.Stop();

            thisCall();
        }
    }
}
