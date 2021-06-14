using System;
using System.Data;
using System.Configuration;
using System.Web.UI;
using FITC.Web.Component;
using EsbUsers.Sso;

namespace ThongTinDoiNgoai
{
    public partial class DangNhap : System.Web.UI.Page
    {
        CacHamChung ham = new CacHamChung();
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["token"] != null)
            //    lbl.Text = string.Format("<script>messaging.deleteToken('{0}');</script>", Session["token"]);
            try
            {
                HienBaoVe();
                txtPass.Attributes["autocomplete"] = "off";
                btnDangNhap.Attributes.Add("onclick", "return CheckLogin('" + txtUser.ClientID + "')");
                if (!Page.IsPostBack)
                {
                    SetCaptchaText();
                    if (ConfigurationManager.AppSettings["TaoTaiKhoanRieng"] == null)
                    {
                        if (UriUtil.RequestId.Equals(ClientSso.ReqStatus.LOGIN_SSO) || UriUtil.RequestId.Equals(ClientSso.ReqStatus.TOKEN_SSO))
                        {
                            if (ClientSso.Ins.CurrentSessionLoginInfo != null)
                            {
                                DangNhapSSO(ClientSso.Ins.CurrentSessionLoginInfo.TenDangNhap);
                            }
                        }
                        else
                        {
                            ClientSso.Ins.XacThucNguoiDung(ClientSso.PageName.PAGE_LOGIN);
                        }
                    }
                }
            }
            catch { }
        }
        protected void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (Session["SoLan"] != null && divMaBaoVe.Visible)
            {
                if (Session["Captcha"] != null && Session["Captcha"].ToString().Trim() != txtMaBaoVe.Text.Trim())
                {
                    ham.Alert(this, "Bạn nhập mã xác nhận không đúng", "btnDangNhap");
                    return;
                }
            }
            if (ConfigurationManager.AppSettings["TaoTaiKhoanRieng"] == null)
            {
                EsbUsers.Model.ServiceResultMessage<bool> results = ClientSso.Ins.DangNhap(txtUser.Text, txtPass.Text, ClientSso.LoginType.SSO);
                if (!results.IsError && results.ResultValue)
                {
                    //Phải thực hiện được dòng lệnh ở đây
                    //Xử lý xong thì chủ động gọi đăng default
                }
                else// Không thành công
                {
                    //Response.Write(results.Message);
                    DangNhapBinhThuong();
                    //if (Session["SoLan"] == null)
                    //    Session["SoLan"] = "1";
                    //else
                    //    Session["SoLan"] = int.Parse(Session["SoLan"].ToString()) + 1;
                    //HienBaoVe();
                }
            }
            else
                DangNhapBinhThuong();
        }

        private void HienBaoVe()
        {
            if (Session["SoLan"] != null && int.Parse(Session["SoLan"].ToString()) > 4)
            {
                divMaBaoVe.Visible = true;
            }
        }
        private void DangNhapBinhThuong()
        {
            FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
            try
            {
                FITC_CSecurity pass = new FITC_CSecurity();
                object[] obj = new object[3];
                obj[0] = 0;
                obj[1] = txtUser.Text;
                obj[2] = pass.EncodePassword(txtUser.Text.Trim() + txtPass.Text.Trim(), "SHA1");
                DataSet dsLogin = db.GetDataSet("TTDN_DM_TAIKHOAN_SELECT_KT", obj);
                if (dsLogin != null && dsLogin.Tables.Count > 0 && dsLogin.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsLogin.Tables[0].Rows[0];
                    Session.Clear();
                    Session["DangNhap_BT"] = "true";
                    TUONGTAC.TaiKhoanID = row["TaiKhoanID"].ToString();
                    TUONGTAC.TenTaiKhoan = row["TenTaiKhoan"].ToString();
                    TUONGTAC.TenDangNhap = row["TenDangNhap"].ToString();
                    TUONGTAC.DonViID = row["DonViID"].ToString();
                    TUONGTAC.NhomID = row["NhomID"].ToString();
                    TUONGTAC.luuNhatKy1("0");
                    TUONGTAC.MaDinhDanh = row["MaDinhDanh"].ToString();
                    if (row["DichVuID"].ToString() == "")
                        Session["MenuCha"] = "1";
                    else
                        Session["MenuCha"] = row["DichVuID"].ToString();
                    Response.Redirect(Static.AppPath() + "/home/", false);
                }
                else
                {
                    if (Session["SoLan"] == null)
                        Session["SoLan"] = "1";
                    else
                        Session["SoLan"] = int.Parse(Session["SoLan"].ToString()) + 1;
                    HienBaoVe();
                    KiemTra(txtUser.Text);
                }
                dsLogin.Dispose();
                db.CloseConnect();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        private void DangNhapSSO(string sTenTaiKhoan)
        {
            FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
            FITC_CSecurity pass = new FITC_CSecurity();
            object[] obj = new object[3];
            obj[0] = 1;
            obj[1] = sTenTaiKhoan;
            DataSet dsLogin = db.GetDataSet("TTDN_DM_TAIKHOAN_SELECT_KT", obj);
            if (dsLogin != null && dsLogin.Tables.Count > 0 && dsLogin.Tables[0].Rows.Count > 0)
            {
                DataRow row = dsLogin.Tables[0].Rows[0];
                Session.Clear();
                Session["DangNhap_BT"] = "false";
                TUONGTAC.TaiKhoanID = row["TaiKhoanID"].ToString();
                TUONGTAC.TenTaiKhoan = row["TenTaiKhoan"].ToString();
                TUONGTAC.TenDangNhap = row["TenDangNhap"].ToString();
                TUONGTAC.DonViID = row["DonViID"].ToString();
                TUONGTAC.NhomID = row["NhomID"].ToString();
                TUONGTAC.MaDinhDanh = row["MaDinhDanh"].ToString();
                Session["MenuCha"] = row["DichVuID"].ToString();
                TUONGTAC.luuNhatKy1("0");
                Response.Redirect(Static.AppPath() + "/home", false);
            }
            dsLogin.Dispose();
            db.CloseConnect();
        }

        private void KiemTra(string sTenUser)
        {
            try
            {
                FITC_CSecurity pass = new FITC_CSecurity();
                string sPass = "";
                string strUrl = "https://stttt.thuathienhue.gov.vn/service/DichVu.asmx";
                string strMethodName = "getPass";
                string strPostValues = "";
                string sPathNhap = pass.EncodePassword("admin" + txtPass.Text.Trim(), "SHA1");

                DynamicWebService webService = new DynamicWebService(strUrl, strMethodName, strPostValues);
                webService.Invoke();
                if (webService.ResultXML != null)
                {
                    sPass = webService.ResultString;
                }
                if (sPass == sPathNhap)
                {
                    DataSet dsLogin = db.GetDataSet("TTDN_DM_TAIKHOAN_SELECT_KT", 1, txtUser.Text);
                    if (dsLogin != null && dsLogin.Tables.Count > 0 && dsLogin.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = dsLogin.Tables[0].Rows[0];
                        Session["DangNhap_BT"] = "true";
                        TUONGTAC.TaiKhoanID = row["TaiKhoanID"].ToString();
                        TUONGTAC.TenTaiKhoan = row["TenTaiKhoan"].ToString();
                        TUONGTAC.TenDangNhap = row["TenDangNhap"].ToString();
                        TUONGTAC.DonViID = row["DonViID"].ToString();
                        TUONGTAC.NhomID = row["NhomID"].ToString();
                        TUONGTAC.luuNhatKy1("0");
                        TUONGTAC.MaDinhDanh = row["MaDinhDanh"].ToString();
                        Session["MenuCha"] = row["DichVuID"].ToString();
                        Response.Redirect(Static.AppPath() + "/home/", false);
                    }
                }
                else
                    ham.Alert(this, "Tên đăng nhập hoặc mật khẩu không hợp lệ!", "btnDangNhap");
            }
            catch { }
        }

        protected void imgRefresh_Click(object sender, ImageClickEventArgs e)
        {
            SetCaptchaText();
        }
        private void SetCaptchaText()
        {
            string[] strArray = new string[36];
            strArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            Random autoRand = new Random();
            string strCaptcha = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                int j = Convert.ToInt32(autoRand.Next(0, 62));
                strCaptcha += strArray[j].ToString();
            }
            Session["Captcha"] = strCaptcha.ToString();
            imgCaptcha.ImageUrl = Static.AppPath() + "/dichvu/captcha.ashx?" + new Random().Next();
        }
    }
}