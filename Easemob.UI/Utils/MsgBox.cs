using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using agsXMPP.protocol.client;
using DevExpress.XtraEditors;

namespace Easemob.UI.Utils
{
    public static class MsgBox
    {
        private static void RegisterSkin()
        {
            //DevExpress.UserSkins.OfficeSkins.Register();//这两种外观用了哪一种就初始化哪一种
            //DevExpress.Skins.SkinManager.EnableFormSkinsIfNotVista();
            //if (AppConfig.AllowChangeSkin)
            //{               
            //    DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = JZT.Msg.Win.Forms.JZTRibbonForm.SkinName;
            //}
        }

        public static bool ShowYesNo(String msg)
        {
            //XtraMessageBox.Show(
            RegisterSkin();
            return
                (
                XtraMessageBox.Show(
                    msg,
                    "确认",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes
                );
        }

        public static void ShowErr(String msg)
        {
            //if (AppConfig.ShowBalloonMsg)
            //    JMainForm.Instance.ShowNotifyError(msg);
            //else
            //{
                RegisterSkin();
                XtraMessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        public static void ShowInfo(String msg)
        {
            //if (AppConfig.ShowBalloonMsg)
            //    JMainForm.Instance.ShowNotifyInfo(msg);
            //else
            //{
                RegisterSkin();
                XtraMessageBox.Show(msg, "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        public static void ShowWarn(String msg)
        {
            //if (AppConfig.ShowBalloonMsg)
            //    JMainForm.Instance.ShowNotifyWarn(msg);
            //else
            //{
                RegisterSkin();
                XtraMessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        public static void ShowErr(Error error)
        {
            String msg = null;
            switch (error.Condition)
            {
                case ErrorCondition.BadRequest:
                    msg = "请求无效";
                    break;
                case ErrorCondition.Conflict:
                    break;
                case ErrorCondition.FeatureNotImplemented:
                    break;
                case ErrorCondition.Forbidden:
                    break;
                case ErrorCondition.Gone:
                    break;
                case ErrorCondition.InternalServerError:
                    break;
                case ErrorCondition.ItemNotFound:
                    break;
                case ErrorCondition.JidMalformed:
                    msg = "帐号格式不正确！";
                    break;
                case ErrorCondition.NotAcceptable:
                    break;
                case ErrorCondition.NotAllowed:
                    break;
                case ErrorCondition.NotAuthorized:
                    break;
                case ErrorCondition.PaymentRequired:
                    break;
                case ErrorCondition.RecipientUnavailable:
                    break;
                case ErrorCondition.Redirect:
                    break;
                case ErrorCondition.RegistrationRequired:
                    break;
                case ErrorCondition.RemoteServerNotFound:
                    msg = "远程服务器没有找到，域名输入错误！";
                    break;
                case ErrorCondition.RemoteServerTimeout:
                    msg = "访问远程服务器超时！";
                    break;
                case ErrorCondition.ResourceConstraint:
                    break;
                case ErrorCondition.ServiceUnavailable:
                    break;
                case ErrorCondition.SubscriptionRequired:
                    break;
                case ErrorCondition.UndefinedCondition:
                    break;
                case ErrorCondition.UnexpectedRequest:
                    break;
                default:
                    break;
            }
            RegisterSkin();
            if (msg == null)
                ShowErr(error.Condition.ToString());
            else
                ShowErr(msg);
        }
    }
}
