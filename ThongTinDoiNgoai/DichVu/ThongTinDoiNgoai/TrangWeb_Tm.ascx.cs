using FITC.Web.Component;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ThongTinDoiNgoai.DichVu.ThongTinDoiNgoai
{
    public partial class TrangWeb_Tm : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sNhomID = "0";
        string sWebID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["WebID"] != null)
                sWebID = Request.QueryString["WebID"];
            if (Request.QueryString["NhomID"] != null)
                sNhomID = Request.QueryString["NhomID"];
            if (!IsPostBack)
            {
                addSua();
            }
        }

        private void addSua()
        {
            try
            {
                if (sWebID != "0")
                {
                    DataSet ds = db.GetDataSet("TTDN_TRANGWEB_SELECT", 1, sWebID);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        db.GetItem(drpNhom, row["NhomID"].ToString());
                        txtTenWeb.Text = row["TenWeb"].ToString();
                        txtDiaChiWeb.Text = row["DiaChiWeb"].ToString();
                    }
                }
                else
                {
                    db.GetItem(drpNhom, sNhomID);
                }
            }
            catch { }
        }

        private string KiemTra()
        {
            if (drpNhom.SelectedValue == "0")
            {
                return "Vui lòng chọn nhóm thông tin!";
            }
            if (txtTenWeb.Text.Trim().Length == 0)
            {
                txtTenWeb.Focus();
                return "Vui lòng nhập tên trang web!";
            }
            if (txtDiaChiWeb.Text.Trim().Length == 0)
            {
                txtDiaChiWeb.Focus();
                return "Vui lòng nhập địa chỉ trang web(url)!";
            }

            return "";
        }

        protected void btnThemMoi_Click(object sender, EventArgs e)
        {
            try
            {
                string strLoi = KiemTra();
                if (strLoi != "")
                {
                    ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                    return;
                }

                string DiaChiWeb = txtDiaChiWeb.Text.Trim();
                if (txtDiaChiWeb.Text.Trim().EndsWith("/"))
                    DiaChiWeb = txtDiaChiWeb.Text.Trim().Substring(0, txtDiaChiWeb.Text.Trim().Length - 1);

                DataSet ds = db.GetDataSet("TTDN_TRANGWEB_SELECT", 2, sWebID, DiaChiWeb);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ham.Alert(this, string.Format("Địa chỉ trang web(url) \"{0}\" đã tồn tại!", DiaChiWeb), "btnThemMoi");
                    return;
                }

                object[] obj = new object[5];
                obj[0] = sWebID;
                obj[1] = txtTenWeb.Text.Trim();
                obj[2] = DiaChiWeb;
                obj[3] = TUONGTAC.TenTaiKhoan;
                obj[4] = drpNhom.SelectedValue;
                string sLoi = db.ExcuteSP("TTDN_TRANGWEB_INSERT", obj);
                if (sLoi == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.FindControl("btnThemMoi"), this.GetType(), "Message_Close", "alert('Cập nhật thành công !'); self.parent.tb_remove();", true);
                }
                else
                {
                    ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                    return;
                }
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnThemMoi");
            }
        }
    }
}