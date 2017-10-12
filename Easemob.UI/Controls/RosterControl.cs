using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using agsXMPP.Xml.Dom;
using DevExpress.XtraNavBar;
using agsXMPP.protocol.iq.roster;
using System.Threading;
using agsXMPP;
using System.Collections;
using Easemob.UI.Utils;
using Easemob.UI.Forms;
using Easemob.UI.Definitions;
using Easemob.XmppWebSocket;


namespace Easemob.UI.Controls
{
    /// <summary>
    /// 好友控件
    /// </summary>
    public partial class RosterControl : XtraUserControl, IXmppWebSocketConnectionRequired
    {
        WaitCall waitcall = null;
        private XmppWebSocketConnection connection = null;
        /// <summary>
        /// navBar 的锁
        /// </summary>
        private Object NavLock = new Object();

        public RosterControl()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                waitcall = new WaitCall(SortRoster, 200);
                //wei=>this.navBar.SmallImages = ResManager.Instance.ImageListFace;
                // 加载好友列表的标记设置为阻塞状态，只有当加载结束后，才 Set 为非阻塞状态
                //wei=>XmppRosterManager.Instance.RosterLoadedFlag.Reset();
            }

            //wei=>xmppClient.ClientConnection.OnRosterEnd +=new agsXMPP.ObjectHandler(ClientConnection_OnRosterEnd);
            //wei=>xmppClient.ClientConnection.OnRosterItem +=new agsXMPP.XmppClientConnection.RosterHandler(ClientConnection_OnRosterItem);
            //wei=>xmppClient.ClientConnection.OnPresence +=new agsXMPP.protocol.client.PresenceHandler(ClientConnection_OnPresence);
            //wei=>xmppClient.ClientConnection.OnRosterStart +=new agsXMPP.ObjectHandler(ClientConnection_OnRosterStart);
        }

        /// <summary>
        /// 从所有的分组中移除指定的JID（注意：该方法未使用 lock(NavLock)）
        /// </summary>
        /// <param name="jid"></param>
        private void RemoveJidOnUI(Jid jid)
        {
            foreach (NavBarGroup group in navBar.Groups)
            {
                for (int i = group.ItemLinks.Count - 1; i >= 0; i--)
                {
                    NavBarItemLink link = group.ItemLinks[i];
                    NavBarItem nbItem = link.Item as NavBarItem;
                    RosterItem rosterItem = nbItem.Tag as RosterItem;
                    //wei=>if (jid.EqualsIgnoreResource(rosterItem.Jid))
                    //wei=>{
                        group.ItemLinks.Remove(link);
                    //wei=>}
                }
            }            
        }

        private static String DEFAULT_GROUP_NAME = "我的好友";

        /// <summary>
        /// 好友的信息到达事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item"></param>
        public void ClientConnection_OnRosterItem(object sender, agsXMPP.protocol.iq.roster.RosterItem item)
        {
            //wei=>this.TA_Execute(delegate
            //wei=>{
                //wei=>LockWatcher lkw = LockWatcher.New();
                lock (NavLock)
                {
                   //wei=> lkw.Stop();
                    /*
     Additionally, if subscription='from' or subscription='none', you can have ask='suscribe', 
     which means you sent a subscription request to the item but haven't received an answer yet.
     */

                    /*
                     Therefore, the following types of contacts SHOULD be displayed by clients:

items with subscription='both' or subscription='to' ;
items with subscription='none' or subscription='from' and ask='subscribe'. It is ((subscription='none' or subscription='from') and ask='subscribe') ;
items with subscription='none' or subscription='from' which have a 'name' attribute or a 'group' child set. It is ((subscription='none' or subscription='from') and (name attribute or group child)).
The client MAY display contacts with subscription='from' which don't match one of the above cases in an additional 'Observers' group. If no 'Observers' group is used, the client SHOULD NOT display contacts which don't match one of the above rules.
                      
                     */

                    if ((item.Subscription == SubscriptionType.both || item.Subscription == SubscriptionType.to) ||
                        ((item.Subscription == SubscriptionType.none || item.Subscription == SubscriptionType.from) && item.Ask == AskType.subscribe) ||
                        ((item.Subscription == SubscriptionType.none || item.Subscription == SubscriptionType.from) && (!String.IsNullOrEmpty(item.Name) || item.GetGroups().Count > 0)))
                    {
                        // 是否需要排序标记
                        bool needSort = false;
                        // SHOULD be displayed by clients:
                        ElementList groups = item.GetGroups();
                        List<string> groupNames = new List<string>();

                        foreach (Element g in groups)
                        {
                            groupNames.Add(g.InnerXml);
                        }

                        if (groupNames.Count == 0)
                        {
                            groupNames.Add(DEFAULT_GROUP_NAME);
                        }
                        
                        // 如果movingJid 和事件传入的 JID 相同，则说明该用户移动成功。
                        //wei=>if (movingJid != null && movingJid.EqualsIgnoreResource(item.Jid))
                        //wei=>
                            //wei=>LockWatcher lkw2 = LockWatcher.New();
                            lock (movingJidFlag)
                            {
                                // 首先先移除 movingJid
                                RemoveJidOnUI(movingJid);
                                needSort = true; // 移动好友后，需要做排序处理
                                movingJid = null;
                                // 设置好友移动标记为非阻塞状态
                                movingJidFlag.Set();
                            }
                            //wei=>lkw2.Stop();
                        //wei=>}

                        List<NavBarGroup> testGroups = new List<NavBarGroup>();

                        // 遍历传入好友所属的所有组
                        foreach (String groupName in groupNames)
                        {
                            // 创建组
                            NavBarGroup group = AddGroup(groupName);

                            if (testGroups.Contains(group))
                            {
                                continue;
                            }
                            else
                            {
                                testGroups.Add(group);
                            }

                            bool hasUser = false;
                            // ???????????????????????问题：集合已修改；可能无法执行枚举操作。
                            foreach (NavBarItemLink  link in group.ItemLinks)
                            {
                                RosterItem rosterItem = link.Item.Tag as RosterItem;
                                //wei=>if (rosterItem.Jid.EqualsIgnoreResource(item.Jid))
                                //wei=>{
                                    // 当前组存在该好友
                                    hasUser = true;
                                    break;
                                //wei=>}
                            }
                            // 如果当前组中不存在该好友，则新增好友到当前组。
                            if (hasUser == false)
                            {
                                NavBarItem nbItem = new NavBarItem(item.Name);

                                nbItem.Tag = item;
                                //wei=> LockWatcher lkw3 = LockWatcher.New();
                                //wei=>lock (PresenceManager.Instance.LockStatus)
                                {
                                    //wei=>lkw3.Stop();
                                    // 获取用户状态的图标
                                    int mobileStatus = -1;
                                    int status = -1;
                                    string mobileRes = "";
                                    //wei=>PresenceManager.Instance.GetUserStatus(item.Jid, out mobileStatus, out mobileRes, out status);
                                    if (mobileStatus >= 0)
                                    {
                                        //wei=> nbItem.SmallImageIndex = ResManager.STATUS_MOBILE;
                                    }
                                    if (status >= 0)
                                    {
                                        nbItem.SmallImageIndex = status;
                                    }
                                    else if (item.Subscription == SubscriptionType.from || item.Subscription == SubscriptionType.none)
                                    {
                                        //wei=> nbItem.SmallImageIndex = ResManager.STATUS_UNSUBSCRIBED;
                                    }
                                    else
                                    {
                                        //wei=>nbItem.SmallImageIndex = ResManager.STATUS_OFFLINE;
                                    }
                                }
                                if (nbItem == null)
                                {

                                }
                                if (group == null)
                                {

                                }
                                if (group.ItemLinks == null)
                                {

                                }
                                // ???????????????????????????问题：对象当前正在其他地方使用。
                                group.ItemLinks.Add(nbItem);
                            }
                            // 添加“移动组”子菜单
                            AddMoveGroupMenu(groupName);

                        }
                        // 添加“新增组”菜单
                        AddMoveGroupMenu(MENU_NEW_GROUP);

                        //wei=>XmppRosterManager.Instance.Add(item);
                        
                        if (needSort)
                        {
                            waitcall.Run();
                        }
                    }
                    else if (item.Subscription == SubscriptionType.remove)
                    {
                        RemoveMyRoster(item);
                        //wei=>XmppRosterManager.Instance.Remove(item);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            //wei=>});
        }       

        /// <summary>
        /// 新建组菜单的显示名称
        /// </summary>
        private const String MENU_NEW_GROUP = "新建组...";

        /// <summary>
        /// 移除所有组中的好友，并提交给服务器。
        /// </summary>
        /// <param name="rosterItem">好友</param>
        private void RemoveMyRoster(agsXMPP.protocol.iq.roster.RosterItem rosterItem)
        {
            List<NavBarItem> nbList = FindNavBarItem(rosterItem.Jid.Bare);

             for (int i = nbList.Count - 1; i >= 0; i--)
             {
                 NavBarItem item = nbList[i];

                 foreach (NavBarGroup navGroup in this.navBar.Groups)
                 {
                      navGroup.ItemLinks.Remove(item);
                 }
             }
             hintLink = null;

            //wei=>XmppRosterManager.Instance.Remove(rosterItem);
        }
        /// <summary>
        /// 增加移动到其他组的子菜单（菜单名为组名）
        /// </summary>
        /// <param name="groupName"></param>
        private void AddMoveGroupMenu(String groupName)
        {
            lock (moveToToolStripMenuItem)
            {
                if (!moveToToolStripMenuItem.DropDownItems.ContainsKey(groupName))
                {
                    ToolStripItem item = new ToolStripMenuItem(groupName);
                    item.Name = groupName;

                    if (MENU_NEW_GROUP.Equals(groupName) ||
                        moveToToolStripMenuItem.DropDownItems.Count == 0)
                    {
                        moveToToolStripMenuItem.DropDownItems.Add(item);
                    }
                    else
                    {
                        moveToToolStripMenuItem.DropDownItems.Insert(0, item);
                    }

                    item.Click += new EventHandler(moveToToolStripMenuItem_Click);
                }
            }
        }

        /// <summary>
        /// 排序所有组的Link
        /// </summary>
        private void SortRoster()
        {
            //wei=>this.TA_Execute(delegate
            //wei=> {
            //wei=>    LockWatcher lkw = LockWatcher.New();
                lock (NavLock)
                {
                   //wei=> lkw.Stop();
                    foreach (NavBarGroup group in this.navBar.Groups)
                    {
                        group.ItemLinks.Sort(new RosterComparer());
                    }
                }
           //wei=> });
        }

        /// <summary>
        /// 好友排序
        /// </summary>
        class RosterComparer : IComparer
        {
            #region IComparer 成员

            public int Compare(object x, object y)
            {
                NavBarItemLink itemX = x as NavBarItemLink;
                NavBarItemLink itemY = y as NavBarItemLink;

                int valX = itemX.Item.SmallImageIndex;
                int valY = itemY.Item.SmallImageIndex;
                /*
public const int STATUS_ONLINE = 0;
public const int STATUS_OFFLINE = 1;
public const int STATUS_UNKNOW = 2;

public const int ICON_HOME = 3;
public const int ICON_DEPT = 4;
public const int ICON_HOME2 = 5;

public const int STATUS_AWAY = 5;
public const int STATUS_DND = 6;
public const int STATUS_UNSUBSCRIBED = 7;
public const int STATUS_XA = 8;

public const int STATUS_MOBILE = 9;
                 */
                // 离线
                //wei=>if (valY == ResManager.STATUS_OFFLINE) valY = 100;
                //wei=>if (valX == ResManager.STATUS_OFFLINE) valX = 100;

                // 未订阅
                //wei=>if (valY == ResManager.STATUS_UNSUBSCRIBED) valY = 90;
                //wei=>if (valX == ResManager.STATUS_UNSUBSCRIBED) valX = 90;

                //wei=> if (valY == ResManager.STATUS_MOBILE) valY = ResManager.STATUS_ONLINE;
                //wei=>if (valX == ResManager.STATUS_MOBILE) valX = ResManager.STATUS_ONLINE;

                int result = valX.CompareTo(valY);

                // 如果状态相同，则按名称排序
                if (result == 0)
                {
                    result = itemX.Item.Caption.CompareTo(itemY.Item.Caption);
                }

                return result;
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pres"></param>
        public void ClientConnection_OnPresence(object sender, agsXMPP.protocol.client.Presence pres)
        {
            // 等待好友加载完毕
            //wei=>if (!XmppRosterManager.Instance.RosterLoadedFlag.WaitOne(10000, true))
            //wei=>{
           //wei=>     XmppLogger.Error(String.Format("{0}{1}", LockWatcher.TAG, "MyRosterControl.ClientConnection_OnPresence"));
            //wei=>    return;
                //wei=>}             

            waitcall.Run();

           //wei=> this.TA_Execute(delegate
            //wei=>{
             //wei=>   LockWatcher lkw = LockWatcher.New();
                lock (NavLock)
                {
                    //wei=>lkw.Stop();
                    String jid = pres.From.Bare;

                    //String resource = pres.From.Resource;
                    //if (!String.IsNullOrEmpty(resource))
                    //{
                    //    if (resource.StartsWith("android"))
                    //    {
                    //        // 安卓手机
                    //    }
                    //    else if (resource.StartsWith("JM"))
                    //    {
                    //        // JM
                    //    }
                    //}

                    List<NavBarItem> nbList = FindNavBarItem(jid);

                    int imageIndex = -1;
                    int mobileStatus = -1;
                    int status = -1;
                    string mobileRes = "";
                   //wei=> PresenceManager.Instance.GetUserStatus(jid, out mobileStatus, out mobileRes, out status);
                    if (mobileStatus >= 0)
                    {
                       //wei=> imageIndex = ResManager.STATUS_MOBILE;
                    }
                    else
                    {
                        imageIndex = status;
                    }
                    if (imageIndex >= 0)
                    {
                        foreach (NavBarItem item in nbList)
                        {
                            item.SmallImageIndex = imageIndex;
                        }
                        //return;
                    }
                    /*
                     出席信息节的'type'属性是可选的(OPTIONAL).
                     一个不拥有任何'type'属性的出席信息节用来通知服务器发送者已经在线并且可以进行通信了, 
                      'type' 属性表示缺乏可用性, 请求管理对其他实体的出席信息的订阅, 请求其他实体的当前出席信息, 
                     或发生了和上次发出的出席信息节有关的错误. 如果包含了它, 'type'属性必须(MUST)拥有以下值之一: 
•	unavailable -- 通知实体将不可通信. 
•	subscribe -- 发送者希望订阅接收者的出席信息. 
•	subscribed -- 发送者允许接收者接收他们的出席信息. 
•	unsubscribe -- 发送者取消订阅另一个实体的出席信息. 
•	unsubscribed -- 订阅者的请求被拒绝或以前的订阅被取消. 
•	probe -- 对一个实体当前的出席信息的请求; 只应(SHOULD)由服务器代替一个用户生成. 
•	error -- 处理或递送之前发送的出席信息节的时候发生了错误. 
                     */
                    switch (pres.Type)
                    {
                        // available -- 通知实体可以通信. 
                        case agsXMPP.protocol.client.PresenceType.available:

                           //wei=> imageIndex = ResManager.STATUS_ONLINE;

                            switch (pres.Show)
                            {
                                case agsXMPP.protocol.client.ShowType.NONE:
                                    break;
                                // 离开一会
                                case agsXMPP.protocol.client.ShowType.away:
                                   //wei=> imageIndex = ResManager.STATUS_AWAY;
                                    break;
                                // 可以聊天
                                case agsXMPP.protocol.client.ShowType.chat:
                                    break;
                                // 请勿打扰
                                case agsXMPP.protocol.client.ShowType.dnd:
                                    //wei=>imageIndex = ResManager.STATUS_DND;
                                    break;
                                // 长时间离开
                                case agsXMPP.protocol.client.ShowType.xa:
                                    //wei=>imageIndex = ResManager.STATUS_XA;
                                    break;
                                default:
                                    break;
                            }
                            if (mobileStatus >= 0)
                            {
                               //wei=> imageIndex = ResManager.STATUS_MOBILE;
                            }
                            foreach (NavBarItem item in nbList)
                            {
                                item.SmallImageIndex = imageIndex;
                            }                            

                            break;
                        // unsubscribed -- 订阅者的请求被拒绝或以前的订阅被取消. 
                        case agsXMPP.protocol.client.PresenceType.unsubscribed:
                            foreach (NavBarItem item in nbList)
                            {
                                // 移除好友
                                //item.SmallImageIndex = ResManager.STATUS_UNSUBSCRIBED;
                                item.NavBar.ActiveGroup.ItemLinks.Remove(item);
                            }

                            //wei=>XmppRosterManager.Instance.Remove(pres.From);
                            break;
                        // subscribe -- 发送者希望订阅接收者的出席信息. 
                        case agsXMPP.protocol.client.PresenceType.subscribe:
                            // 对方请求添加为好友
                            //wei=>SubscriptionRequestForm form = new SubscriptionRequestForm(pres.From);
                            //wei=>form.Show();
                            break;
                        // subscribed -- 发送者允许接收者接收他们的出席信息. 
                        case agsXMPP.protocol.client.PresenceType.subscribed:
                            
                            // XmppRosterManager.Instance.Add(item)
                            foreach (NavBarItem item in nbList)
                            {
                                RosterItem roster = item.Tag as RosterItem;
                                roster.Subscription = SubscriptionType.to;
                            }
                            break;
                        // unsubscribe -- 发送者取消订阅另一个实体的出席信息. 
                        case agsXMPP.protocol.client.PresenceType.unsubscribe:
                            // 对方拒绝添加好友
                            //wei=>xmppClient.ClientConnection.RosterManager.RemoveRosterItem(pres.From);
                           //wei=> XmppRosterManager.Instance.Remove(pres.From);
                            break;
                        // probe -- 对一个实体当前的出席信息的请求; 只应(SHOULD)由服务器代替一个用户生成.
                        case agsXMPP.protocol.client.PresenceType.probe:
                            break;
                        // error -- 处理或递送之前发送的出席信息节的时候发生了错误. 
                        case agsXMPP.protocol.client.PresenceType.error:
                        // invisible -- 隐身
                        case agsXMPP.protocol.client.PresenceType.invisible:
                        // unavailable -- 通知实体将不可通信. 
                        case agsXMPP.protocol.client.PresenceType.unavailable:
                        default:
                            //foreach (NavBarItem item in nbList)
                            //    item.SmallImageIndex =  imageIndex;
                            break;
                    }
                }
            //wei=>});

        
        }

        /// <summary>
        /// 查找指定好友的 NavBarItem
        /// </summary>
        /// <param name="jid"></param>
        /// <returns></returns>
        private List<NavBarItem> FindNavBarItem(String jid)
        {
            //wei=>return this.TA_ExecuteResult<List<NavBarItem>>(delegate
            //wei=>{
               //wei=> LockWatcher lkw = LockWatcher.New();
                lock (NavLock)
                {
                    //wei=>lkw.Stop();
                    List<NavBarItem> list = new List<NavBarItem>();

                    foreach (NavBarGroup group in this.navBar.Groups)
                    {
                        foreach (NavBarItemLink link in group.ItemLinks)
                        {
                            NavBarItem nbItem = link.Item as NavBarItem;
                            RosterItem rosterItem = nbItem.Tag as RosterItem;
                            if (jid.ToLower().Equals(rosterItem.Jid.Bare.ToLower()))
                            {
                                list.Add(nbItem);
                            }
                        }
                    }

                    return list;
                }
           //wei=> });
        }

        /// <summary>
        /// 好友列表加载结束事件
        /// </summary>
        /// <param name="sender"></param>
        public void ClientConnection_OnRosterEnd(object sender)
        {
            //wei=>this.TA_Execute(delegate
            //wei=>{
               //wei=> LockWatcher lkw = LockWatcher.New();
                lock (NavLock)
                {
                    //wei=>lkw.Stop();
                   //wei=> if (XmppRosterManager.Instance.Count == 0)
                   //wei=> {
                        NavBarGroup group = AddGroup(DEFAULT_GROUP_NAME);
                   //wei=> }
                }
                //wei=> });
            // 好友加载同步标记设置为“非阻塞”状态
            //wei=> XmppRosterManager.Instance.RosterLoadedFlag.Set();
        }

        /// <summary>
        /// 移动好友到其他组的同步标记
        /// </summary>
        private ManualResetEvent movingJidFlag = new ManualResetEvent(false);
        /// <summary>
        /// 记录正在移动到其他组的好友JID
        /// </summary>
        private Jid movingJid = null;
        /// <summary>
        /// 移动好友
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void moveToToolStripMenuItem_Click(object sender, EventArgs e)
        {  
            // 获取当前要移动的好友
            RosterItem item = GetCurrentRosterItem();
            if (item == null)
                return;

            // 当前选中的组菜单（menuItem.Text 就是组名）
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            String group = "";

            // 如果选中的菜单名为“新建组”，则打开新建组的窗口，由用户自行填写
            if (MENU_NEW_GROUP.Equals(menuItem.Text))
            {
                //wei=>InputValueForm form = new InputValueForm();
                //wei=>if (form.ShowDialog(this.FindForm()) == DialogResult.Cancel)
               //wei=> {
                //wei=>    return;
                //wei=>}
               //wei=> else
               //wei=> {
                    // 新建组的名称
                   //wei=> group = form.InputText;
               //wei=> }
            }
            else
            {
                // 当前选中的组名
                group = menuItem.Text;
            }

            // 需要移动好友的JID赋值给变量 movingJid
            movingJid = item.Jid;
            // 设置同步标记为阻塞状态
            movingJidFlag.Reset();

            // 更新好友的组信息
            //wei=>xmppClient.ClientConnection.RosterManager.UpdateRosterItem(movingJid, item.Name, group);

            // 线程中等待移动好友到其他组成功
            //wei=>ThreadUtils.Run(WaitUpdateRosterItem);
        }

        /// <summary>
        /// 等待移动好友到其他组成功
        /// </summary>
        private void WaitUpdateRosterItem()
        {
            if (!movingJidFlag.WaitOne(10000, true))
            {
                //wei=>XmppLogger.Error(String.Format("{0}{1}", LockWatcher.TAG, "MyRosterControl.WaitUpdateRosterItem"));
                //wei=>LockWatcher lkw = LockWatcher.New();
                lock (movingJidFlag)
                {                   
                    movingJid = null;
                }
                //wei=>lkw.Stop();
            }
        }

        /// <summary>
        /// 好友列表到达开始事件
        /// </summary>
        /// <param name="sender"></param>
        public void ClientConnection_OnRosterStart(object sender)
        {
            //wei=> LockWatcher lkw = LockWatcher.New();
            lock (NavLock)
            {
                //wei=>XmppRosterManager.Instance.Clear();
                //wei=>this.TA_Execute(delegate
                //wei=>{
                    // 清除当前所有组
                    this.navBar.Groups.Clear();
                //wei=> });
            }
            //wei=> lkw.Stop();
            // 好友加载同步标记设置为“阻塞”状态
            //wei=>XmppRosterManager.Instance.RosterLoadedFlag.Reset();
        }

        /// <summary>
        /// 新增一个组，如果相同组名存在，则不会再新增。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private NavBarGroup AddGroup(String name)
        {
            //wei=>LockWatcher lkw = LockWatcher.New();
            lock (NavLock)
            {
                //wei=> lkw.Stop();
                NavBarGroup group = this.navBar.Groups[name];

                if (group == null)
                {
                    group = new NavBarGroup();
                    group.Name = name;
                    group.Expanded = (this.navBar.Groups.Count == 0 ? true : false);
                    group.GroupCaptionUseImage = DevExpress.XtraNavBar.NavBarImage.Small;
                    group.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
                    group.Caption = name;
                    this.navBar.Groups.Add(group);
                }

                return group;
            }
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            // 当前的 Link (好友) 不为空
            if (hintLink != null)
            {
                NavBarItem navBarItem = hintLink.Item as NavBarItem;
                RosterItem roster = navBarItem.Tag as RosterItem;
                moveToToolStripMenuItem.Text = "移动“" + navBarItem.Caption + "”到";
                SetMenuItem(true);

                String jid = roster.Jid.Bare;
                int mobileStatus = -1;
                int status = -1;
                String mobileRes = null;
                //wei=>PresenceManager.Instance.GetUserStatus(jid, out mobileStatus,out mobileRes, out status);

                //wei=>if (mobileStatus == ResManager.STATUS_AWAY ||
                //wei=>    mobileStatus == ResManager.STATUS_DND ||
                //wei=>    mobileStatus == ResManager.STATUS_ONLINE ||
                //wei=>    mobileStatus == ResManager.STATUS_XA)
                //wei=>{
                    locReqToolStripMenuItem.Enabled = true;
                //wei=>}
                    //wei=> else
               //wei=> {
               //wei=>     locReqToolStripMenuItem.Enabled = false;
                    //wei=> }

                return;
            }
            else if (hintGroup != null) // 当前的组不为空
            {
                moveToToolStripMenuItem.Text = "移动好友到";
                SetMenuItem(false);
                return;
            }

            e.Cancel = true;
            return;
        }

        /// <summary>
        /// 设置菜单项的有效状态
        /// </summary>
        /// <param name="isLink">是否为Link(好友)</param>
        private void SetMenuItem(bool isLink)
        {
            // 如果选中的是好友项
            if (isLink)
            {
                NavBarItem navBarItem = hintLink.Item as NavBarItem;
                RosterItem roster = navBarItem.Tag as RosterItem;

                // 遍历所有的组，如果该好友所属的组名和当前遍历的组名相同，则“移动到其他组的菜单”无效，否则有效）
                foreach (ToolStripItem item in moveToToolStripMenuItem.DropDownItems)
                {
                    if (hintLink.Group != null)
                    {
                        item.Enabled = !hintLink.Group.Caption.Equals(item.Text);
                    }
                }

                // 群发消息菜单有效
                massMsgToolStripMenuItem.Text = "群发消息->“" + hintLink.Caption + "”";
                massMsgToolStripMenuItem.Enabled =true;

                // 请求订阅菜单是否有效，需要看当前用户的状态
                if (roster.Subscription == SubscriptionType.none ||
                    roster.Subscription == SubscriptionType.from)
                    presReqToolStripMenuItem.Enabled = true;
                else
                    presReqToolStripMenuItem.Enabled = false;

            } // 如果选中的是组
            else if(hintGroup != null) 
            {
                // 群发消息菜单有效
                massMsgToolStripMenuItem.Text = "群发消息->[" + hintGroup.Caption + "]";
                massMsgToolStripMenuItem.Enabled =true;
                // 请求订阅菜单无效
                presReqToolStripMenuItem.Enabled = false;
            }
            else
            {
                // 群发消息菜单无效
                massMsgToolStripMenuItem.Enabled = false;
                // 请求订阅菜单无效
                presReqToolStripMenuItem.Enabled = false;
            }
            // 发送消息菜单
            sendMsgToolStripMenuItem.Enabled = isLink; 
            // 删除好友菜单
            deleteToolStripMenuItem.Enabled = isLink;
            // 查看名片菜单
            viewInfoToolStripMenuItem.Enabled = isLink;
            // 定位菜单
            locReqToolStripMenuItem.Enabled = isLink;
            // 移动到其他组菜单
            moveToToolStripMenuItem.Enabled = isLink;
            // 重新命名组（未实现）
            renameGroupToolStripMenuItem.Enabled = isLink;

            // 配置上可以设置是否允许群发
            //wei=> if (!AppConfig.AllowMassMessage)
            //wei=>{
                massMsgToolStripMenuItem.Visible = false;
                //wei=>}

                //wei=>if (!AppConfig.AllowLocReq)
            {
                locReqToolStripMenuItem.Visible = false;
            }
        }

        /// <summary>
        /// 获取当前选中项
        /// </summary>
        /// <returns></returns>
        private RosterItem GetCurrentRosterItem()
        {
            if (hintLink == null)
                return null;

            NavBarItem navBarItem = hintLink.Item as NavBarItem;

            if (navBarItem == null)
                return null;

            return navBarItem.Tag as RosterItem;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendMsgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RosterItem item = GetCurrentRosterItem();
            if (item == null)
                return;

            //wei=>ChatManagerForm.Instance.StartChat(item.Jid);        
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RosterItem item = GetCurrentRosterItem();
            if (item == null)
                return;

            //wei=>if (MsgBox.ShowYesNo("是否删除好友 [" + item.Name + "] ？"))
            {
                //wei=>  xmppClient.ClientConnection.RosterManager.RemoveRosterItem(item.Jid);
                //wei=>  XmppRosterManager.Instance.Remove(item.Jid);
            }
        }

        /// <summary>
        /// 查看电子名片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RosterItem item = GetCurrentRosterItem();
            if (item == null)
                return;

            //wei=> VcardFormManager.Instance.ShowVcard(item.Jid);
        }


        /// <summary>
        /// 更新组名（未实现）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renameGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RosterItem item = GetCurrentRosterItem();
            if (item == null)
                return;
            String newName = item.Name;
            List<String> groups = new List<string>();

            foreach (Element elm in item.GetGroups())
            {
                groups.Add(elm.Value);
            }

            //wei=> InputValueForm form = new InputValueForm();
            //wei=> form.InputText = item.Name;
           //wei=> if (form.ShowDialog(this.FindForm(),"重命名","好友名称") == DialogResult.Cancel)
           //wei=> {
            //wei=>    return;
           //wei=> }
            //wei=>else
           //wei=> {
                // 新建组的名称
            //wei=>    newName = form.InputText;
            //wei=>  }


            // 需要移动好友的JID赋值给变量 movingJid
            movingJid = item.Jid;
            // 设置同步标记为阻塞状态
            movingJidFlag.Reset();

            // 更新好友的组信息
            //wei=>xmppClient.ClientConnection.RosterManager.UpdateRosterItem(movingJid, newName, groups.ToArray());

            // 线程中等待移动好友到其他组成功
            //wei=>ThreadUtils.Run(WaitUpdateRosterItem);
        }

        /// <summary>
        /// 定位（未实现）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void locReqToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RosterItem item = GetCurrentRosterItem();
            if (item == null)
                return;

            String to = item.Jid.Bare + "/mobile";
            //wei=>String respTo = GetMyJidBare() + "/JM";
            //wei=>String respTo2 = XmppClientManager.Instance.CreateJid("taskhandler").Bare;

            //wei=> LocReqMessage locReq = TaskFactory.CreateLocReqMessage(to, new String[] { respTo, respTo2 }, 0);
            //wei=> XmppClientManager.Instance.Send(locReq);
        }

        /// <summary>
        /// NavBar 双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBar_DoubleClick(object sender, EventArgs e)
        {
            // 如果是 Link (选中的是用户)
            if (hintLink != null)
            {
                NavBarItem navBarItem = hintLink.Item as NavBarItem;
                RosterItem item = navBarItem.Tag as RosterItem;

                //wei=>ChatManagerForm.Instance.StartChat(item.Jid);
            } // 如果选中的是组
            else if (hintGroup != null)
            {
                hintGroup.Expanded = !hintGroup.Expanded;
                //for (int i = 0; i < hintGroup.ItemLinks.Count; i++)
                //{
                //    NavBarItemLink link = hintGroup.ItemLinks[i];
                //    NavBarItem navBarItem = link.Item as NavBarItem;
                //    RosterItem item = navBarItem.Tag as RosterItem;
                //}
                // 群发
            }

        }

        private void navBar_MouseHover(object sender, EventArgs e)
        {

        }

        #region 自行实现获取当前的组或用户
        //====================================================================================
        
        /// <summary>
        /// 当前选中的组（navbar没有提供该属性，因此只能自己实现）
        /// </summary>
        private NavBarGroup hintGroup = null;
        /// <summary>
        /// 当前选中的用户（navbar没有提供该属性，因此只能自己实现）
        /// </summary>
        private NavBarItemLink hintLink = null;

        private void navBar_GetHint(object sender, NavBarGetHintEventArgs e)
        {
            hintGroup = e.Group;
            hintLink = e.Link;
            if(hintLink != null)
            {
                navBar.SelectedLink = hintLink;
            }
        }

        /// <summary>
        /// navBar鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBar_MouseDown(object sender, MouseEventArgs e)
        {
            int groupHeight = 32;
            if (e.Y <= groupHeight)
            {
                hintLink = null;
                hintGroup = navBar.ActiveGroup;
            }

            Control ctrl = navBar.GetChildAtPoint(new Point(e.X, e.Y));

        }
        //-------------------------------------------------------------------------------------
        #endregion

        /// <summary>
        /// 点击“增加好友”菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addContactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddContactForm form = new AddContactForm();
            form.AttachConnection(this.connection);
            form.Show();
        }

        /// <summary>
        /// 点击“群发”菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void massMsgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RosterItem item = GetCurrentRosterItem();
            List<Jid> list = new List<Jid>();
            // 如果选中的是用户，则只添加一个群发用户
            if (item != null)
            {                
                list.Add(item.Jid);               
            } // 如果选中的是组，则添加组下所有的好友
            else if (hintGroup != null)
            {
                foreach (NavBarItemLink link in hintGroup.ItemLinks)
                {
                    RosterItem rosterItem = link.Item.Tag as RosterItem;
                    //wei=> if (xmppClient.GetMyJid().EqualsIgnoreResource(rosterItem.Jid))
                    //wei=>continue;

                    list.Add(rosterItem.Jid);
                }
            }
            if (list.Count == 0)
                return;

            //wei=> ChatManagerForm.Instance.StartMassChat(list);
        }

        /// <summary>
        /// 统计组在线的人数，显示在 Caption 上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBar_CustomDrawGroupCaption(object sender, DevExpress.XtraNavBar.ViewInfo.CustomDrawNavBarElementEventArgs e)
        {
            e.Caption = e.Caption + GroupStatusCount(e.Caption);

        }


        /// <summary>
        /// 统计组在线的人数，格式为：3/12
        /// </summary>
        /// <param name="group"></param>
        private String GroupStatusCount(String name)
        {
            int count = 0;
            int onlineCount = 0;
            NavBarGroup group = this.navBar.Groups[name];
            if (group != null)
            {
                foreach (NavBarItemLink link in group.ItemLinks)
                {
                    count++;
                    RosterItem rosterItem = link.Item.Tag as RosterItem;
                    //wei=>if (link.Item.SmallImageIndex != ResManager.STATUS_UNSUBSCRIBED && 
                    //wei=>     link.Item.SmallImageIndex != ResManager.STATUS_OFFLINE)
                   //wei=> {
                   //wei=>     onlineCount++;
                    //wei=>}
                }
            }

            return String.Format(" [{0}/{1}]", onlineCount, count);
        }

        /// <summary>
        /// 点击“查找好友”菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //wei=>SearchContactForm form = new SearchContactForm();
            //wei=> form.Show();
        }

        /// <summary>
        /// 点击“请求订阅”菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void presReqToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RosterItem item = GetCurrentRosterItem();
            if (item == null)
                return;

            //wei=> xmppClient.ClientConnection.PresenceManager.Subscribe(item.Jid);
        }

        private void MyRosterControl_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public void AttachConnection(XmppWebSocket.XmppWebSocketConnection connection)
        {
            this.connection = connection;
            this.connection.OnRosterStart += new ObjectHandler(connection_OnRosterStart);
            this.connection.OnRosterItem += new XmppClientConnection.RosterHandler(connection_OnRosterItem);
            this.connection.OnRosterEnd += new ObjectHandler(connection_OnRosterEnd);
            this.connection.OnXmppConnectionStateChanged += new XmppConnectionStateHandler(connection_OnXmppConnectionStateChanged);
            //if (this.connection.ConnectionState == XmppConnectionState.SessionStarted)
            //{
            //    this.connection.RequestRoster();
            //}
        }

        void connection_OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
        {
            //if(state == XmppConnectionState.SessionStarted)
            //    this.connection.RequestRoster();
        }

        void connection_OnRosterEnd(object sender)
        {
            
        }

        void connection_OnRosterStart(object sender)
        {
            
        }

        void connection_OnRosterItem(object sender, RosterItem item)
        {
            string username = item.Jid.User.Replace(connection.AppKey + "_", "");

            switch (item.Subscription)
            {
                case SubscriptionType.both: //双方都订阅了
                    break;
                case SubscriptionType.from:
                    break;
                case SubscriptionType.none:
                    break;
                case SubscriptionType.remove://好友删除了
                    break;
                case SubscriptionType.to:
                    break;
                default:
                    break;
            }
        }
    }
}
