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
    public partial class BaiViet_Tm : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                addDanhMuc();
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
            drpChuyenMuc.Items.Clear();
            drpChuyenMuc.Items.Add(new ListItem("[Chọn]", "0"));
        }

        private string KiemTra()
        {
            FITC_CDataTime dt = new FITC_CDataTime();
            if (drpWeb.SelectedValue == "0")
            {
                drpWeb.Focus();
                return "Chưa chọn trang web!";
            }
            if (txtTieuDe.Text.Trim() == "")
            {
                txtTieuDe.Focus();
                return "Chưa nhập tiêu đề bài viết!";
            }
            if (txtTomTat.Text.Trim() == "")
            {
                txtTomTat.Focus();
                return "Chưa nhập tóm tắt bài viết!";
            }
            if (txtNoiDung.Text.Trim() == "")
            {
                txtNoiDung.Focus();
                return "Chưa nhập nội dung bài viết!";
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

                object[] obj = new object[10];

                obj[0] = txtTieuDe.Text.Trim();
                obj[1] = txtTomTat.Text.Trim();
                obj[2] = txtNoiDung.Text.Trim();
                obj[3] = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                obj[4] = txtTacGia.Text.Trim();
                obj[5] = "";
                obj[6] = txtNoiDung.Text.Trim().Length;
                obj[7] = drpChuyenMuc.SelectedValue;
                obj[8] = drpWeb.SelectedValue;
                obj[9] = "";
                string sLoi = db.ExcuteSP("TTDN_BAIVIET_INSERT", obj);
                if (sLoi == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.FindControl("btnThemMoi"), this.GetType(), "Message_Close", "alert('Cập nhật thành công !'); self.parent.tb_remove();", true);
                }
                else
                {
                    ham.Alert(this, sLoi, "btnThemMoi");
                    return;
                }
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnThemMoi");
            }
        }

        protected void drpWeb_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpChuyenMuc.Items.Clear();
            if (drpWeb.SelectedValue == "0")
                drpChuyenMuc.Items.Add(new ListItem("[Chọn]", "0"));
            else
            {
                drpChuyenMuc.Items.Add(new ListItem("[Chọn]", "0"));
                DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 0, drpWeb.SelectedValue);
                if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsChuyenMuc.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = dsChuyenMuc.Tables[0].Rows[i];
                        drpChuyenMuc.Items.Add(new ListItem(row["TenChuyenMuc"].ToString() + " (" + row["UrlChuyenMuc"].ToString() + ")", row["ChuyenMucID"].ToString()));
                    }
                }
            }
        }
    }
}