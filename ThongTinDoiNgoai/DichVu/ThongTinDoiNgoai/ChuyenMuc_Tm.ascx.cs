using FITC.Web.Component;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyVanBan.DichVu.DuLieu
{
    public partial class ChuyenMuc_Tm : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sWebID = "0";
        string sChuyenMucID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["WebID"] != null)
                sWebID = Request.QueryString["WebID"];
            if (Request.QueryString["ChuyenMucID"] != null)
                sChuyenMucID = Request.QueryString["ChuyenMucID"];
            if (!IsPostBack)
            {
                addDanhMuc();
                addSua();
            }
        }

        private void addDanhMuc()
        {
            drpWeb.Items.Add(new ListItem("[Chọn]", "0"));
            DataSet dsWeb = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0);
            if (dsWeb != null && dsWeb.Tables.Count > 0 && dsWeb.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsWeb.Tables[0].Rows.Count; i++)
                {
                    DataRow row = dsWeb.Tables[0].Rows[i];
                    drpWeb.Items.Add(new ListItem(row["TenWeb"].ToString() + " (" + row["DiaChiWeb"].ToString() + ")", row["WebID"].ToString()));
                }
            }
        }

        private void addSua()
        {
            try
            {
                if (sChuyenMucID != "0")
                {
                    DataSet ds = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 1, 0, sChuyenMucID);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        db.GetItem(drpWeb, row["WebID"].ToString());
                        txtTenChuyenMuc.Text = row["TenChuyenMuc"].ToString();
                        txtUrlChuyenMuc.Text = row["UrlChuyenMuc"].ToString();
                    }
                }
                else
                {
                    db.GetItem(drpWeb, sWebID);
                }
            }
            catch { }
        }

        private string KiemTra()
        {
            FITC_CDataTime dt = new FITC_CDataTime();
            string sLoi = "";
            if (drpWeb.SelectedValue == "0")
                sLoi = "Chưa chọn trang web!";
            if (txtTenChuyenMuc.Text.Trim() == "")
                sLoi = "Chưa nhập tên chuyên mục!";
            if (txtUrlChuyenMuc.Text.Trim() == "")
                sLoi = "Chưa nhập URL chuyên mục!";

            return sLoi;
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

                string UrlChuyenMuc = txtUrlChuyenMuc.Text.Trim();
                if (txtUrlChuyenMuc.Text.Trim().EndsWith("/"))
                    UrlChuyenMuc = txtUrlChuyenMuc.Text.Trim().Substring(0, txtUrlChuyenMuc.Text.Trim().Length - 1);

                object[] obj = new object[5];
                obj[0] = sChuyenMucID;
                obj[1] = txtTenChuyenMuc.Text.Trim();
                obj[2] = txtUrlChuyenMuc.Text.Trim();
                obj[3] = drpWeb.SelectedValue;
                obj[4] = TUONGTAC.TenTaiKhoan;
                string sLoi = db.ExcuteSP("TTDN_CHUYENMUC_INSERT", obj);
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