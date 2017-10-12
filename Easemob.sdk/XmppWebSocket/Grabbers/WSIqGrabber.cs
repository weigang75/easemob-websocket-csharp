using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP;
using agsXMPP.protocol.client;
using System.Threading;
using System.Collections;


namespace Easemob.XmppWebSocket.Grabbers
{
    /// <summary>
    /// IQ消息匹配的拦截器，发送IQ和返回IQ匹配
    /// </summary>
    public class WSIqGrabber : WSPacketGrabber
    {
        private Hashtable				m_grabbing		= new Hashtable();		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        public WSIqGrabber(XmppWebSocketConnection conn)
        {
            m_connection = conn;
            conn.OnIq += new IqHandler(OnIq);
        }


        private IQ synchronousResponse = null;

        private int m_SynchronousTimeout = 5000;

        /// <summary>
        /// Timeout for synchronous requests, default value is 5000 (5 seconds)
        /// </summary>
        public int SynchronousTimeout
        {
            get { return m_SynchronousTimeout; }
            set { m_SynchronousTimeout = value; }
        }


        /// <summary>
        /// An IQ Element is received. Now check if its one we are looking for and
        /// raise the event in this case.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnIq(object sender, agsXMPP.protocol.client.IQ iq)
        {
            if (iq == null)
                return;

            // the tracker handles on iq responses, which are either result or error
            if (iq.Type != IqType.error && iq.Type != IqType.result)
                return;

            string id = iq.Id;
            if (id == null)
                return;

            TrackerData td;

            lock (m_grabbing)
            {
                td = (TrackerData)m_grabbing[id];

                if (td == null)
                {
                    return;
                }
                m_grabbing.Remove(id);
            }

            td.cb(this, iq, td.data);
        }

        /// <summary>
        /// Send an IQ Request and store the object with callback in the Hashtable
        /// </summary>
        /// <param name="iq">The iq to send</param>
        /// <param name="cb">the callback function which gets raised for the response</param>
        public void SendIq(IQ iq, IqCB cb)
        {
            SendIq(iq, cb, null);
        }

        /// <summary>
        /// Send an IQ Request and store the object with callback in the Hashtable
        /// </summary>
        /// <param name="iq">The iq to send</param>
        /// <param name="cb">the callback function which gets raised for the response</param>
        /// <param name="cbArg">additional object for arguments</param>
        public void SendIq(IQ iq, IqCB cb, object cbArg)
        {
            // check if the callback is null, in case of wrong usage of this class
            if (cb != null)
            {
                TrackerData td = new TrackerData();
                td.cb = cb;
                td.data = cbArg;

                m_grabbing[iq.Id] = td;
            }
            m_connection.Send(iq);
        }


        /// <summary>
        /// Sends an Iq synchronous and return the response or null on timeout
        /// </summary>
        /// <param name="iq">The IQ to send</param>
        /// <param name="timeout"></param>
        /// <returns>The response IQ or null on timeout</returns>
        public IQ SendIq(agsXMPP.protocol.client.IQ iq, int timeout)
        {
            synchronousResponse = null;
            AutoResetEvent are = new AutoResetEvent(false);

            SendIq(iq, new IqCB(SynchronousIqResult), are);

            if (!are.WaitOne(timeout, true))
            {
                // Timed out
                lock (m_grabbing)
                {
                    if (m_grabbing.ContainsKey(iq.Id))
                        m_grabbing.Remove(iq.Id);
                }
                return null;
            }

            return synchronousResponse;
        }

        /// <summary>
        /// Sends an Iq synchronous and return the response or null on timeout.
        /// Timeout time used is <see cref="SynchronousTimeout"/>
        /// </summary>
        /// <param name="iq">The IQ to send</param>        
        /// <returns>The response IQ or null on timeout</returns>
        public IQ SendIq(IQ iq)
        {
            return SendIq(iq, m_SynchronousTimeout);
        }

        /// <summary>
        /// Callback for synchronous iq grabbing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="iq"></param>
        /// <param name="data"></param>
        private void SynchronousIqResult(object sender, IQ iq, object data)
        {
            synchronousResponse = iq;

            AutoResetEvent are = data as AutoResetEvent;
            are.Set();
        }

        private class TrackerData
        {
            public IqCB cb;
            public object data;
        }
    }
}
