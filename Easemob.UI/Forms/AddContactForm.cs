using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using agsXMPP;
using agsXMPP.protocol.client;
using DevExpress.XtraBars.Ribbon;
using Easemob.UI.Utils;
using Easemob.XmppWebSocket;
using Easemob.UI.Definitions;

namespace Easemob.UI.Forms
{
    /// <summary>
    /// 增加好友窗口
    /// </summary>
    public partial class AddContactForm : DevExpress.XtraEditors.XtraForm, IXmppWebSocketConnectionRequired
    {
        /// <summary>
        /// XmppClientManager
        /// </summary>
        private XmppWebSocketConnection connection = null;

        public AddContactForm()
        {
            InitializeComponent();
        }               

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchUserItem">搜索用户信息</param>
        //public AddContactForm(SearchUserItem searchUserItem) 
        //{
        //    InitializeComponent();

        //    if (searchUserItem != null)
        //    {
        //        txtNickName.Text = searchUserItem.Name;                
        //        btnSearch.Visible = false;
        //        txtUser.Text = searchUserItem.Jid;
        //        txtUser.Properties.ReadOnly = true;
        //    }
        //}

        private void AddContactForm_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                InitUI();
            }
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitUI() 
        {
            // 为空时提示文字
            txtUser.Properties.NullValuePrompt = StringUtils.STR_JID_FORMAT;
            // 获取组
            List<String> groups = new List<string>();// XmppRosterManager.Instance.GetGroups();

            if (groups.Count == 0)
            {
                this.cmbGroup.Properties.Items.Add("我的好友");
            }
            else
            {
                foreach (String item in groups)
                {
                    this.cmbGroup.Properties.Items.Add(item);
                }
            }

            if (this.cmbGroup.Properties.Items.Count > 0)
                this.cmbGroup.SelectedIndex = 0;

            txtUser.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 检查必填项是否为空
        /// </summary>
        /// <returns></returns>
        private bool CheckForm()
        {
            StringBuilder sbErr = new StringBuilder();

            if (String.IsNullOrEmpty(txtUser.Text.Trim()))
            {
                sbErr.AppendLine("- 请填写好友的帐号");
            }

            if(String.IsNullOrEmpty(txtNickName.Text.Trim()))
            {
                sbErr.AppendLine("- 请填写好友昵称");
            }

            if (String.IsNullOrEmpty(cmbGroup.Text))
            {
                sbErr.AppendLine("- 请填写好友所在组");
            }

            if (sbErr.Length > 0)
            {
                MsgBox.ShowErr(sbErr.ToString());
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckForm() == false)
                return;

            Jid jid = new Jid( connection.CreateFullJid(txtUser.Text.Trim()));
            // 如果增加的好友为自己
            if (connection.GetMyFullJid().EqualsIgnoreResource(jid))
            {
                MsgBox.ShowErr("不能添加自己为好友！");
            }
            else
            {
                // 试着在好友列表中获取好友昵称
                String nickname = "";//XmppRosterManager.Instance.GetNickName(jid);
                // 如果获取昵称失败，则说明没有添加该好友
                if (String.IsNullOrEmpty(nickname))
                {
                    nickname = txtNickName.Text.Trim();
                    // 添加好友
                    connection.RosterManager.AddRosterItem(jid, nickname, this.cmbGroup.Text);
                    // 请求订阅
                    connection.PresenceManager.Subscribe(jid);
                    MsgBox.ShowInfo(String.Format("[{0}] 已成功添加！", nickname));
                    this.Close();
                }
                else
                {
                    MsgBox.ShowErr(String.Format("[{0}] 已经是您的好友！", nickname));
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 点击查看电子名片（并自动获取昵称到“昵称文本框”）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVcard_Click(object sender, EventArgs e)
        {
            String account = txtUser.Text.Trim();
            // 帐号不能为空
            if (String.IsNullOrEmpty(account))
            {
                MsgBox.ShowInfo("请填写帐号，比如：" + StringUtils.STR_JID_FORMAT);
                return;
            }
            // 简单检查帐号的合法性，如果合法
            if (account.IndexOf("@") < 0)
            {
                account = String.Format("{0}@{1}", account, connection.Server);
                txtUser.Text = account;
            }

            //// 清空昵称文本框，为重新获取昵称做准备
            //txtNickName.TA_SetText("");
            //VcardForm form = new VcardForm(account);
            //form.OnVcard += new VcardHandler(delegate(object obj, JZTVcard vcard) {
            //    // 如果窗口关闭或释放，则返回
            //    if (form.IsFormClosed || form.Disposing)
            //        return;
            //    // 如果没有错误
            //    if (vcard.Error == null)
            //        txtNickName.TA_SetText(vcard.Nickname);
            //    else
            //        txtNickName.TA_SetText("");
            //});
            //form.Show(this);        
        }

        public void AttachConnection(XmppWebSocketConnection connection)
        {
            this.connection = connection; 
        }
    }
}
