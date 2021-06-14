using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ThongTinDoiNgoai.DichVu.HeThong.Email
{
    public partial class DangKyEmail_Tm : System.Web.UI.UserControl
    {
        CacHamChung ham = new CacHamChung();
        string sSuaID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["SuaID"] != null)
                sSuaID = Request.QueryString["SuaID"];

            if (!IsPostBack)
            {
                if (Request.QueryString["SuaID"] != null)
                    AddSua();
            }
            txtIncomingPort.Enabled = chkIncomingPort.Checked;
            txtOutgoingPort.Enabled = chkOutgoingPort.Checked;
        }
        protected void btnThemMoi_Click(object sender, EventArgs e)
        {
            if (TUONGTAC.TaiKhoanID != null)
            {
                string sLoi = "";
                txtHidPassword.Text = txtPassword.Text;
                sLoi = this.ValidForm();
                if (sLoi != "")
                {
                    ham.Alert(this, sLoi, "btnThemMoi");
                    return;
                }

                FITC_EmailSettings emailSettings = this.CollectEmailSettings();

                FITC_MailFunctions mf = new FITC_MailFunctions();
                mf.SetEmailInfo(emailSettings.EmailInfo);

                sLoi = mf.CheckEmailSettings();
                if (sLoi != "")
                {
                    ham.Alert(this, "Lỗi: " + sLoi, "btnThemMoi");
                    return;
                }

                sLoi = mf.SaveEmailSettings(TUONGTAC.TaiKhoanID, "TTDN_DM_EMAIL_INSERT");
                if (sLoi == "")
                {
                    ham.Alert(this, "Thêm mới thành công", "btnThemMoi");
                    btnHuyBo_Click(null, null);
                }
                else
                    ham.Alert(this, "Lỗi: " + sLoi.Replace("'", "\\'"), "btnThemMoi");
            }
            else
                ham.Alert(this, "Lỗi: Đã hết phiên làm việc của SESSION !", "btnThemMoi");
        }
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            string sLoi = "";
            txtHidPassword.Text = txtPassword.Text;
            sLoi = this.ValidForm();
            if (sLoi != "")
            {
                ham.Alert(this, sLoi, "btnCapNhat");
                return;
            }

            FITC_EmailSettings emailSettings = this.CollectEmailSettings();

            FITC_MailFunctions mf = new FITC_MailFunctions();
            mf.SetEmailInfo(emailSettings.EmailInfo);

            sLoi = mf.CheckEmailSettings();
            if (sLoi != "")
            {
                ham.Alert(this, "Lỗi: " + sLoi, "btnCapNhat");
                return;
            }

            sLoi = mf.UpdateEmailSettings(TUONGTAC.TaiKhoanID, sSuaID, "TTDN_DM_EMAIL_UPDATE");
            if (sLoi == "")
            {
                ham.Alert(this, "Cập nhật thành công", "btnCapNhat");
                ScriptManager.RegisterClientScriptBlock(this.FindControl("btnCapNhat"), this.GetType(), "Message_Close", "self.parent.tb_remove();", true);
            }
            else
                ham.Alert(this, "Lỗi: " + sLoi.Replace("'", "\\'"), "btnCapNhat");
        }
        protected void btnHuyBo_Click(object sender, EventArgs e)
        {
            txtEmailAddress.Text = "";
            txtHidPassword.Text = "";
            txtDisplayName.Text = "";
            chkDefaultAccount.Checked = false;
            chkDeleteFromServer.Checked = false;
            drpAccountType.ClearSelection();
            txtIncomingServer.Text = "";
            chkIncomingPort.Checked = false;
            txtIncomingPort.Text = "";
            chkIncomingSecure.Checked = false;
            txtOutgoingServer.Text = "";
            chkOutgoingPort.Checked = false;
            txtOutgoingPort.Text = "";
            chkOutgoingSecure.Checked = false;
            chkOutgoingAuthentication.Checked = false;
        }

        private void AddSua()
        {
            FITC_EmailSettings emailSettings = new FITC_MailFunctions().LoadEmailSettings(sSuaID, "TTDN_DM_EMAIL_SELECT");
            if (emailSettings != null)
            {
                txtEmailAddress.Text = emailSettings.EmailInfo.EmailAddress;
                txtHidPassword.Text = new EncryptDescript().CriptDescript(emailSettings.EmailInfo.Password);
                txtDisplayName.Text = emailSettings.EmailInfo.DisplayName;
                if (emailSettings.EmailInfo.EmailType == EmailType.POP3)
                    drpAccountType.Items[0].Selected = true;
                else
                    drpAccountType.Items[1].Selected = true;
                txtIncomingServer.Text = emailSettings.EmailInfo.IncomingServer;
                chkIncomingPort.Checked = emailSettings.EmailInfo.IsUsingIncomingServerPort;
                if (chkIncomingPort.Checked)
                {
                    txtIncomingPort.Enabled = true;
                    txtIncomingPort.Text = emailSettings.EmailInfo.IncomingServerPort.ToString();
                }
                chkDefaultAccount.Checked = emailSettings.EmailInfo.IsDefaultAccount;
                chkDeleteFromServer.Checked = emailSettings.EmailInfo.IsDeleteFromServer;
                chkIncomingSecure.Checked = emailSettings.EmailInfo.IsIncomingSecureConnection;
                txtOutgoingServer.Text = emailSettings.EmailInfo.OutgoingServer; ;
                chkOutgoingPort.Checked = emailSettings.EmailInfo.IsUsingOutgoingServerPort;
                if (chkOutgoingPort.Checked)
                {
                    txtOutgoingPort.Enabled = true;
                    txtOutgoingPort.Text = emailSettings.EmailInfo.OutgoingServerPort.ToString();
                }
                chkOutgoingSecure.Checked = emailSettings.EmailInfo.IsOutgoingSecureConnection;
                chkOutgoingAuthentication.Checked = emailSettings.EmailInfo.IsOutgoingWithAuthentication;

                btnThemMoi.Visible = false;
                btnCapNhat.Visible = true;
                btnHuyBo.Visible = true;
            }
        }
        private FITC_EmailSettings CollectEmailSettings()
        {
            string emailAddress = txtEmailAddress.Text.Trim();
            string password = txtPassword.Text.Trim();
            string displayName = txtDisplayName.Text.Trim();
            string incomingServer = txtIncomingServer.Text.Trim();
            string outgoingServer = txtOutgoingServer.Text.Trim();
            string accountType = drpAccountType.SelectedValue;
            int portIncomingServer = 0;
            if (chkIncomingPort.Checked)
                portIncomingServer = int.Parse(txtIncomingPort.Text.Trim());
            int portOutgoingServer = 0;
            if (chkOutgoingPort.Checked)
                portOutgoingServer = int.Parse(txtOutgoingPort.Text.Trim());
            bool isDefaultAccount = chkDefaultAccount.Checked;
            bool isDeleteFromServer = chkDeleteFromServer.Checked;
            bool isIncomingSecureConnection = chkIncomingSecure.Checked;
            bool isOutgoingSecureConnection = chkOutgoingSecure.Checked;
            bool isOutgoingWithAuthentication = chkOutgoingAuthentication.Checked;
            bool portIncomingChecked = chkIncomingPort.Checked;
            bool portOutgoingChecked = chkOutgoingPort.Checked;

            FITC_EmailSettings emailSetting = new FITC_EmailSettings();
            emailSetting.EmailInfo.EmailAddress = emailAddress;
            emailSetting.EmailInfo.Password = new EncryptDescript().CriptDescript(password);
            emailSetting.EmailInfo.DisplayName = displayName;
            emailSetting.EmailInfo.IncomingServer = incomingServer;
            emailSetting.EmailInfo.OutgoingServer = outgoingServer;
            emailSetting.EmailInfo.EmailType = accountType == "POP3" ? EmailType.POP3 : EmailType.IMAP4;
            emailSetting.EmailInfo.IncomingServerPort = portIncomingServer;
            emailSetting.EmailInfo.OutgoingServerPort = portOutgoingServer;
            emailSetting.EmailInfo.IsDefaultAccount = isDefaultAccount;
            emailSetting.EmailInfo.IsDeleteFromServer = isDeleteFromServer;
            emailSetting.EmailInfo.IsIncomingSecureConnection = isIncomingSecureConnection;
            emailSetting.EmailInfo.IsOutgoingSecureConnection = isOutgoingSecureConnection;
            emailSetting.EmailInfo.IsOutgoingWithAuthentication = isOutgoingWithAuthentication;
            emailSetting.EmailInfo.IsUsingIncomingServerPort = portIncomingChecked;
            emailSetting.EmailInfo.IsUsingOutgoingServerPort = portOutgoingChecked;

            return emailSetting;
        }
        private string ValidForm()
        {
            if (txtEmailAddress.Text.Trim().Length == 0)
                return "Vui lòng nhập Địa chỉ email !";
            if (txtEmailAddress.Text.Trim().Length > 500)
                return "Địa chỉ email không được nhiều hơn 500 ký tự !";

            if (txtPassword.Text.Trim().Length == 0)
                return "Vui lòng nhập Mật khẩu email !";
            if (txtPassword.Text.Trim().Length > 500)
                return "Mật khẩu email không được nhiều hơn 500 ký tự !";

            if (txtDisplayName.Text.Trim().Length == 0)
                return "Vui lòng nhập Tên hiển thị !";
            if (txtDisplayName.Text.Trim().Length > 500)
                return "Tên hiển thị không được nhiều hơn 500 ký tự !";

            if (txtIncomingServer.Text.Trim().Length == 0)
                return "Vui lòng nhập Địa chỉ máy chủ nhận email !";
            if (txtIncomingServer.Text.Trim().Length == 0)
                return "Địa chỉ máy chủ nhận email không được nhiều hơn 500 ký tự !";

            if (txtOutgoingServer.Text.Trim().Length == 0)
                return "Vui lòng nhập Địa chỉ máy chủ gởi email !";
            if (txtOutgoingServer.Text.Trim().Length == 0)
                return "Địa chỉ máy chủ gởi email không được nhiều hơn 500 ký tự !";

            if (chkIncomingPort.Checked)
            {
                if (txtIncomingPort.Text.Trim().Length == 0)
                    return "Vui lòng nhập Cổng máy chủ nhận email !";
                try
                {
                    int.Parse(txtIncomingPort.Text.Trim());
                }
                catch (Exception)
                {
                    return "Cổng máy chủ nhận email phải là số !";
                }
            }

            if (chkOutgoingPort.Checked)
            {
                if (txtOutgoingPort.Text.Trim().Length == 0)
                    return "Vui lòng nhập Cổng máy chủ gởi email !";
                try
                {
                    int.Parse(txtOutgoingPort.Text.Trim());
                }
                catch (Exception)
                {
                    return "Cổng máy chủ gởi email phải là số !";
                }
            }

            return "";
        }
    }
}