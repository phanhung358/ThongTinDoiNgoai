using FITC.Web.Component;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyVanBan.DichVu.DuLieu
{
    public partial class XpathChuyenMuc_Tm : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sWebID = "0";
        string sChuyenMucID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSuKien.Style.Add("display", "none");
            if (Request.QueryString["WebID"] != null && Request.QueryString["ChuyenMucID"] != null)
            {
                sWebID = Request.QueryString["WebID"];
                sChuyenMucID = Request.QueryString["ChuyenMucID"];
            }
            if (!IsPostBack)
            {
                addDanhMuc();
                addSua();
            }
            btnLayTuTrangKhac.Attributes["onclick"] = string.Format("return thickboxPopup('Copy Xpath từ chuyên mục khác', '{0}?control={1}&btn={2}&WebID={3}&ChuyenMucID={4}','100%','100%');", Static.AppPath() + "/home/popup.aspx", "/DichVu/ThongTinDoiNgoai/XpathChuyenMuc_Tm_Copy.ascx", btnSuKien.ClientID, sWebID, sChuyenMucID);
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

        private void addSua()
        {
            try
            {
                db.GetItem(drpWeb, sWebID);
                drpWeb_SelectedIndexChanged(null, null);
                db.GetItem(drpChuyenMuc, sChuyenMucID);

                DataSet ds = db.GetDataSet("TTDN_XPATH_CHUYENMUC_SELECT", 0, sWebID, sChuyenMucID);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    btnLayTuTrangKhac.Visible = false;
                    txtDanhSach.Text = row["DanhSach"].ToString();
                    txtBaiViet_Url1.Text = row["BaiViet_Url"].ToString();
                    txtBaiViet_Url2.Text = row["BaiViet_Url1"].ToString();
                    txtBaiViet_Url3.Text = row["BaiViet_Url2"].ToString();
                    txtAnhDaiDien.Text = row["AnhDaiDien"].ToString();
                    txtThoiGian.Text = row["ThoiGian"].ToString();
                    chkCheDoDacBiet.Checked = row["CheDoDacBiet"].ToString() == "True" ? true : false;
                }
                else
                {
                    btnLayTuTrangKhac.Visible = true;
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
            if (drpChuyenMuc.SelectedValue == "0")
                sLoi = "Chưa chọn chuyên mục!";
            if (txtDanhSach.Text.Trim() == "")
                sLoi = "Chưa nhập Xpath danh sách!";
            if (txtBaiViet_Url1.Text.Trim() == "")
                sLoi = "Chưa nhập Xpath URL bài viết!";

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
                object[] obj = new object[10];

                obj[0] = txtDanhSach.Text.Trim();
                obj[1] = txtBaiViet_Url1.Text.Trim();
                obj[2] = txtBaiViet_Url2.Text.Trim();
                obj[3] = txtBaiViet_Url3.Text.Trim();
                obj[4] = drpChuyenMuc.SelectedValue;
                obj[5] = drpWeb.SelectedValue;
                obj[6] = TUONGTAC.TenTaiKhoan;
                obj[7] = txtAnhDaiDien.Text.Trim();
                obj[8] = chkCheDoDacBiet.Checked;
                obj[9] = txtThoiGian.Text.Trim();

                string sLoi = db.ExcuteSP("TTDN_XPATH_CHUYENMUC_INSERT", obj);
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

        protected void btnSuKien_Click(object sender, EventArgs e)
        {
            addSua();
        }
    }
}