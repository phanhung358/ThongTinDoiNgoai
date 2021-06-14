using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FITC.Web.Component;
using System.Text.RegularExpressions;

namespace ThongTinDoiNgoai.DichVu.HeThong
{
    public partial class DoiMatKhau : System.Web.UI.UserControl
    {
        CacHamChung ham = new CacHamChung();
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLuuMatKhau_Click(object sender, EventArgs e)
        {
            FITC_CSecurity str = new FITC_CSecurity();
            if (txtMatKhauCu.Text.Trim() == "")
            {
                ham.Alert(this, "Chưa nhập mật khẩu cũ!", "btnLuuMatKhau");
                return;
            }

            if (txtMatKhauMoi.Text.Trim() == "")
            {
                ham.Alert(this, "Chưa nhập mật khẩu mới!", "btnLuuMatKhau");
                return;
            }
            if (txtMatKhauMoi.Text.Trim() != txtXacNhanMK.Text.Trim())
            {
                ham.Alert(this, "Xác nhận mật khẩu không đúng!", "btnLuuMatKhau");
                return;
            }
            if (!Request.Url.ToString().Contains("localhost") && !IsPasswordStrong(txtMatKhauMoi.Text))
            {
                ham.Alert(this, "Mật khẩu chưa đủ mạnh!", "btnLuuMatKhau");
                return;
            }
            if (txtMatKhauCu.Text.Trim() == txtMatKhauMoi.Text.Trim())
            {
                ham.Alert(this, "Mật khẩu mới không được trùng với mật khẩu cũ!", "btnLuuMatKhau");
                return;
            }
            object[] obj = new object[5];
            obj[0] = TUONGTAC.DonViID;
            obj[1] = TUONGTAC.TaiKhoanID;
            obj[2] = TUONGTAC.TenDangNhap;
            obj[3] = str.EncodePassword(TUONGTAC.TenDangNhap + txtMatKhauCu.Text, Static.sMaHoa);
            obj[4] = str.EncodePassword(TUONGTAC.TenDangNhap + txtMatKhauMoi.Text, Static.sMaHoa);
            try
            {
                string sLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_UPDATE_MATKHAU", obj);
                if (sLoi == "")
                {
                    ham.Alert(this, "Đổi mật khẩu thành công !", "btnLuuMatKhau");
                    tblMatKhau.Visible = false;
                }
                else
                {
                    ham.Alert(this, sLoi, "btnLuuMatKhau");
                }
            }
            catch
            {
            }
        }
        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            tblMatKhau.Visible = false;
        }

        public bool IsPasswordStrong(string password)
        {
            //return Regex.IsMatch(password, @"(%[^a-Z0-9]%");
            //return Regex.IsMatch(password, @"(%[^a-Z0-9]%");
            //'%[^a-Z0-9]%'
            // Có ít nhất 6 ký tự ==> (?=^.{6,}$)
            // Có ít nhất 1 ký tự số ==> (?=.*\d)
            // Có ít nhất 1 ký tự đặc biệt ==> (?=.*\W+)
            // Có ít nhất 1 ký tự chữ hoa ==> (?=.*[A-Z])
            // Có ít nhất 1 ký tự chữ thường ==> (?=.*[a-z])
            // Không tồn tại khoảng trắng ==> (?![.\n])
            //return Regex.IsMatch(password, @"(?=^.{8,}$)(?=.*\d)(?=.*\W+)(?![.\n])(?=.*[a-z]).*$");
            return Regex.IsMatch(password, @"(?=^.{8,}$)(?=.*\d)(?=.*\W+)(?![.\n]).*$");
        }
    }
}