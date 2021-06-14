using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FITC.Web.Component;
using System.Data;
using System.Text.RegularExpressions;

namespace ThongTinDoiNgoai.DichVu.HeThong.TaiKhoan
{
    public partial class TaiKhoan_Tm : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sTaiKhoanID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSuKien.Style.Add("display", "none");
            if (Request.QueryString["TaiKhoanID"] != null)
                sTaiKhoanID = Request.QueryString["TaiKhoanID"];
            if (!IsPostBack)
            {
                addDonVi();
                addNhomQuyen();
                btnCapNhat.Visible = false;
                if (Request.QueryString["TaiKhoanID"] != null)
                {
                    addSua();
                }
            }
        }

        private void addDonVi()
        {
            drpDonVi.Items.Clear();
            drpDonVi.Items.Add(new ListItem("[Chọn]", ""));
            DataSet ds = db.GetDataSet("TTDN_DM_DONVI_SELECT", 0);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    drpDonVi.Items.Add(new ListItem(row["MaDinhDanh"].ToString() + " - " + row["TenDonVi"].ToString(), row["MaDinhDanh"].ToString()));
                }
            }

            if (drpDonVi.Items.Count == 0)
                drpDonVi.Items.Add(new ListItem("[Bấm chọn]", "0"));
        }
        private void addNhomQuyen()
        {
            drpNhom.Items.Clear();
            drpNhom.Items.Add(new ListItem("[Bấm chọn]", "0"));
            DataSet ds = db.GetDataSet("TTDN_DM_TAIKHOAN_NHOM_SELECT", 3);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    drpNhom.Items.Add(new ListItem(row["TenNhom"].ToString(), row["NhomID"].ToString()));
                }
            }
            drpNhom.Items.Add(new ListItem("<Thêm mới>", "-1"));
        }
        private void addSua()
        {
            DataSet ds = db.GetDataSet("TTDN_DM_TAIKHOAN_SELECT", 1, sTaiKhoanID);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                txtTenTaiKhoan.Text = row["TenTaiKhoan"].ToString().Trim();
                txtChucVu.Text = row["ChucVu"].ToString().Trim();
                txtTenDangNhap.Text = row["TenDangNhap"].ToString().Trim();
                db.GetItem(drpNhom, row["NhomID"].ToString().Trim());
                db.GetItem(drpDonVi, row["MaDinhDanh"].ToString().Trim());
                txtSoDienThoai.Text = row["SoDienThoai"].ToString().Trim();
                btnThemMoi.Visible = false;
                btnCapNhat.Visible = true;
            }
        }
        protected void btnThemMoi_Click(object sender, EventArgs e)
        {
            string sLoi = hopLe();
            if (sLoi != "")
            {
                ham.Alert(this, sLoi, "btnThemMoi");
                return;
            }
            object[] obj = new object[8];
            FITC_CSecurity CSecurity = new FITC_CSecurity();
            obj[0] = 0;
            obj[1] = txtTenTaiKhoan.Text;
            obj[2] = txtTenDangNhap.Text;
            obj[3] = "";
            if (txtMatKhau.Text.Trim() != "")
                obj[3] = CSecurity.EncodePassword(txtTenDangNhap.Text.Trim() + txtMatKhau.Text.Trim(), "SHA1");
            obj[4] = txtChucVu.Text;
            obj[5] = drpNhom.SelectedValue;
            obj[6] = drpDonVi.SelectedValue;
            obj[7] = txtSoDienThoai.Text;

            sLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_INSERT", obj);
            if (sLoi == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.FindControl("btnThemMoi"), this.GetType(), "Message_Close", "alert('Thêm mới thành công !'); self.parent.tb_remove();", true);
                txtTenDangNhap.Text = "";
                txtTenTaiKhoan.Text = "";
                drpNhom.SelectedValue = "0";
                txtSoDienThoai.Text = "";
                txtMatKhau.Text = "";
            }
            else
                ham.Alert(this, sLoi, "btnThemMoi");

        }
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                string sLoi = hopLe();
                if (sLoi != "")
                {
                    ham.Alert(this, sLoi, "btnThemMoi");
                    return;
                }
                object[] obj = new object[8];
                FITC_CSecurity CSecurity = new FITC_CSecurity();
                obj[0] = sTaiKhoanID;
                obj[1] = txtTenTaiKhoan.Text;
                obj[2] = txtTenDangNhap.Text;
                obj[3] = "";
                if (txtMatKhau.Text.Trim() != "")
                    obj[3] = CSecurity.EncodePassword(txtTenDangNhap.Text.Trim() + txtMatKhau.Text.Trim(), "SHA1");
                obj[4] = txtChucVu.Text;
                obj[5] = drpNhom.SelectedValue;
                obj[6] = drpDonVi.SelectedValue;
                obj[7] = txtSoDienThoai.Text;
                sLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_UPDATE", obj);
                if (sLoi == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.FindControl("btnThemMoi"), this.GetType(), "Message_Close", "alert('Cập nhật thành công !'); self.parent.tb_remove();", true);
                    btnCapNhat.Visible = false;
                    btnThemMoi.Visible = true;
                    txtTenDangNhap.Text = "";
                    txtTenTaiKhoan.Text = "";
                    txtChucVu.Text = "";
                    drpNhom.SelectedValue = "0";
                    txtMatKhau.Text = "";
                    txtSoDienThoai.Text = "";
                }
                else
                    ham.Alert(this, sLoi, "btnThemMoi");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        private string hopLe()
        {
            if (drpDonVi.Items.Count == 0 || drpDonVi.SelectedValue == "")
            {
                return "Chưa chọn đơn vị !";
            }

            if (txtTenTaiKhoan.Text.Trim() == "")
            {
                return "Bạn chưa nhập họ và tên !";
            }

            if (drpNhom.Items.Count == 0 || drpNhom.SelectedValue == "0")
            {
                return "Bạn chưa chọn nhóm quyền !";
            }

            if (txtTenDangNhap.Text.Trim() == "")
            {
                return "Bạn chưa nhập tên đăng nhập !";
            }

            return "";
        }
        protected void btnSuKien_Click(object sender, EventArgs e)
        {
            addNhomQuyen();
        }
        protected void drpDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void chkChoPhepTatCa_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void LuuDeQuy(TreeNode node)
        {
            object[] obj = new object[3];
            obj[0] = sTaiKhoanID;
            obj[1] = node.Value;
            obj[2] = node.Checked;
            string sLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_DONVI_INSERT", obj);

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                TreeNode nodeCon = node.ChildNodes[i];
                obj[0] = sTaiKhoanID;
                obj[1] = nodeCon.Value;
                obj[2] = nodeCon.Checked;
                sLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_DONVI_INSERT", obj);
                LuuDeQuy(nodeCon);
            }
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

        protected void drpNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpNhom.SelectedValue == "-1")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ThemMoi", string.Format("thickboxPopup('Thêm mới bộ phận','{0}?control={1}&btn={2}','100%', '100%');", ResolveUrl("~/home/popup.aspx"), "/Dichvu/hethong/PhanQuyen/TaoNhom.ascx", btnSuKien.ClientID), true);
        }
    }
}