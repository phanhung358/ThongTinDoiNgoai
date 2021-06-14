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

namespace ThongTinDoiNgoai.DichVu.HeThong.TaiKhoan
{
    public partial class DonVi_Tm : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sDonViID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                txtMaDinhDanh0.Style.Add("text-align", "Center");
                txtMaDinhDanh1.Style.Add("text-align", "Center");
                txtMaDinhDanh2.Style.Add("text-align", "Center");
                txtMaDinhDanh3.Style.Add("text-align", "Center");

                btnSuKien.Style.Add("display", "none");
                if (Request.QueryString["DonViID"] != null)
                    sDonViID = Request.QueryString["DonViID"].ToString();
                if (!IsPostBack)
                {
                    db.AddToCombo(db.ExcutePro("TTDN_DM_DONVI_NHOM_SELECT", 0), drpNhom, "TenNhom", "NhomID");
                    db.AddToCombo(db.ExcutePro("TTDN_DM_DONVI_SELECT", 4, Request.QueryString["DonViChaID"]), drpDonViCapTren, "TenDonVi", "MaDinhDanh");
                    drpDonViCapTren.Items[0].Text = "[Chọn]";

                    drpNhom.Items[0].Text = "[Chọn]";
                    if (Request.QueryString["DonViID"] != null)
                    {
                        addSua();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        private void addSua()
        {
            try
            {
                DataSet ds = db.GetDataSet("TTDN_DM_DONVI_SELECT", 1, sDonViID);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    txtTenDonVi.Text = row["TenDonVi"].ToString().Trim();
                    txtTenVietTat.Text = row["TenVietTat"].ToString().Trim();
                    string[] arr = row["MaDinhDanh"].ToString().Trim().Split('.');
                    if (arr.Length == 3)
                    {
                        txtMaDinhDanh1.Text = arr[0];
                        txtMaDinhDanh2.Text = arr[1];
                        txtMaDinhDanh3.Text = arr[2];
                    }
                    if (arr.Length == 4)
                    {
                        txtMaDinhDanh0.Text = arr[0];
                        txtMaDinhDanh1.Text = arr[1];
                        txtMaDinhDanh2.Text = arr[2];
                        txtMaDinhDanh3.Text = arr[3];
                    }

                    txtDiaChi.Text = row["DiaChi"].ToString().Trim();
                    txtEmail.Text = row["Email"].ToString().Trim();

                    txtDienThoai.Text = row["SDT"].ToString().Trim();
                    txtFax.Text = row["Fax"].ToString().Trim();
                    txtWebsite.Text = row["Website"].ToString().Trim();
                    db.GetItem(drpCap, row["Cap"].ToString().Trim());
                    db.GetItem(drpNhom, row["NhomID"].ToString().Trim());
                    btnLuu.Visible = true;
                    btnThem.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnSuKien");
            }
        }
        private string getMaDinhDanh()
        {
            string s = "";
            if (txtMaDinhDanh0.Text.Trim() != "")
                s = s + "." + txtMaDinhDanh0.Text;

            if (txtMaDinhDanh1.Text.Trim() == "")
                s = s + ".00";
            else
                s = s + "." + txtMaDinhDanh1.Text.Trim();

            if (txtMaDinhDanh2.Text.Trim() == "")
                s = s + "." + "00";
            else
                s = s + "." + txtMaDinhDanh2.Text.Trim();

            s = s + "." + txtMaDinhDanh3.Text.Trim();
            if (s != "")
                s = s.Substring(1);
            return s;
        }
        private bool KiemTra()
        {
            try
            {
                FITC_CNumber cn = new FITC_CNumber();

                //if (drpDonViCapTren.SelectedValue=="0")
                //{
                //    ham.Alert(this, "Chưa chọn đơn vị cấp trên", "btnSuKien");
                //    return false;
                //}
                if (txtTenDonVi.Text.Trim() == "")
                {
                    ham.Alert(this, "Bạn phải nhập tên đơn vị", "btnSuKien");
                    return false;
                }
                if (txtMaDinhDanh1.Text.Trim() == "" && txtMaDinhDanh2.Text.Trim() == "" && txtMaDinhDanh3.Text.Trim() == "")
                {
                    ham.Alert(this, "Bạn phải nhập mã định danh", "btnSuKien");
                    return false;
                }
                if (drpCap.SelectedValue == "0")
                {
                    ham.Alert(this, "Bạn chưa chọn cấp đơn vị", "btnSuKien");
                    return false;
                }
                if (drpNhom.SelectedValue == "0")
                {
                    ham.Alert(this, "Bạn chưa chọn nhóm đơn vị", "btnSuKien");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnSuKien");
            }
            return true;
        }

        protected void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KiemTra())
                {
                    return;
                }
                object[] obj = new object[12];
                obj[0] = sDonViID;
                if (drpDonViCapTren.SelectedValue == "0")
                    obj[1] = "00.00.H57";
                else
                    obj[1] = drpDonViCapTren.SelectedValue;
                obj[2] = txtTenDonVi.Text.Trim();
                obj[3] = getMaDinhDanh();
                obj[4] = txtDienThoai.Text.Trim();
                obj[5] = txtFax.Text.Trim();
                obj[6] = txtDiaChi.Text.Trim();
                obj[7] = txtEmail.Text.Trim();
                if (txtWebsite.Text != "" && txtWebsite.Text.ToLower().IndexOf("http", 0) == -1)
                    obj[8] = "http://" + txtWebsite.Text.Trim();
                else
                    obj[8] = txtWebsite.Text.Trim();
                obj[9] = drpCap.SelectedValue;
                obj[10] = txtTenVietTat.Text;
                obj[11] = drpNhom.SelectedValue;
                string sLoi = db.ExcuteSP("TTDN_DM_DONVI_INSERT", obj);
                if (sLoi == "")
                {
                    ham.Alert(this, "Thêm thành công !", "btnThem");
                    clearText();
                    return;
                }
                else
                    ham.Alert(this, sLoi, "btnThem");
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnThem");
            }
        }
        protected void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KiemTra())
                {
                    return;
                }
                FITC_CSecurity pass = new FITC_CSecurity();
                object[] obj = new object[12];
                obj[0] = sDonViID;
                if (drpDonViCapTren.SelectedValue == "0")
                    obj[1] = "00.00.H57";
                else
                    obj[1] = drpDonViCapTren.SelectedValue;
                obj[2] = txtTenDonVi.Text.Trim();
                obj[3] = getMaDinhDanh();
                obj[4] = txtDienThoai.Text.Trim();
                obj[5] = txtFax.Text.Trim();
                obj[6] = txtDiaChi.Text.Trim();
                obj[7] = txtEmail.Text.Trim();
                obj[8] = txtWebsite.Text.Trim();
                obj[9] = drpCap.SelectedValue;
                obj[10] = txtTenVietTat.Text;
                obj[11] = drpNhom.SelectedValue;
                string sLoi = db.ExcuteSP("TTDN_DM_DONVI_UPDATE", obj);
                if (sLoi == "")
                {
                    ham.Alert(this, "Cập nhật thành công", "btnLuu");
                    clearText();
                    ScriptManager.RegisterClientScriptBlock(this.FindControl("btnLuu"), this.GetType(), "Message_Close", "self.parent.tb_remove();", true);
                }
                else
                    ham.Alert(this, sLoi, "btnLuu");
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnLuu");
            }
        }
        private void clearText()
        {
            txtDiaChi.Text = txtEmail.Text = txtDienThoai.Text = txtFax.Text = "";
            txtTenDonVi.Text = txtWebsite.Text = txtTenVietTat.Text = "";
            if (txtMaDinhDanh0.Enabled)
                txtMaDinhDanh0.Text = "";
            if (txtMaDinhDanh1.Enabled)
                txtMaDinhDanh1.Text = "";
        }
        protected void btnHuy_Click(object sender, EventArgs e)
        {
            try
            {
                clearText();
                btnLuu.Visible = false;
                btnThem.Visible = true;
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message);
            }
        }
        protected void btnSuKien_Click(object sender, EventArgs e)
        {

        }
    }
}