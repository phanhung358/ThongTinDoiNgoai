using FITC.Web.Component;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
        }

        private void addDanhMuc()
        {
            drpWebID.Items.Add(new ListItem("[Chọn]", "0"));
            DataSet dsWeb = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0);
            if (dsWeb != null && dsWeb.Tables.Count > 0 && dsWeb.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsWeb.Tables[0].Rows.Count; i++)
                {
                    DataRow row = dsWeb.Tables[0].Rows[i];
                    drpWebID.Items.Add(new ListItem(row["TenWeb"].ToString() + " (" + row["DiaChiWeb"].ToString() + ")", row["WebID"].ToString()));
                }
            }
            drpChuyenMucID.Items.Clear();
            drpChuyenMucID.Items.Add(new ListItem("[Chọn]", "0"));
        }

        private void addSua()
        {
            try
            {
                db.GetItem(drpWebID, sWebID);
                drpWebID_SelectedIndexChanged(null, null);
                db.GetItem(drpChuyenMucID, sChuyenMucID);

                DataSet ds = db.GetDataSet("TTDN_XPATH_CHUYENMUC_SELECT", 0, sWebID, sChuyenMucID);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    txtDanhSach.Text = row["DanhSach"].ToString();
                    txtBaiViet_Url1.Text = row["BaiViet_Url"].ToString();
                    txtBaiViet_Url2.Text = row["BaiViet_Url1"].ToString();
                    txtBaiViet_Url3.Text = row["BaiViet_Url2"].ToString();
                }
            }
            catch { }
        }

        private string KiemTra()
        {
            FITC_CDataTime dt = new FITC_CDataTime();
            string sLoi = "";
            if (drpWebID.SelectedValue == "0")
                sLoi = "Chưa chọn trang web!";
            if (drpChuyenMucID.SelectedValue == "0")
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
                object[] obj = new object[7];


                obj[0] = txtDanhSach.Text.Trim();
                obj[1] = txtBaiViet_Url1.Text.Trim();
                obj[2] = txtBaiViet_Url2.Text.Trim();
                obj[3] = txtBaiViet_Url3.Text.Trim();
                obj[4] = drpChuyenMucID.SelectedValue;
                obj[5] = drpWebID.SelectedValue;
                obj[6] = TUONGTAC.TenTaiKhoan;

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

        protected void drpWebID_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpChuyenMucID.Items.Clear();
            if (drpWebID.SelectedValue == "0")
                drpChuyenMucID.Items.Add(new ListItem("[Chọn]", "0"));
            else
            {
                drpChuyenMucID.Items.Add(new ListItem("[Chọn]", "0"));
                DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 0, drpWebID.SelectedValue);
                if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsChuyenMuc.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = dsChuyenMuc.Tables[0].Rows[i];
                        drpChuyenMucID.Items.Add(new ListItem(row["TenChuyenMuc"].ToString() + " (" + row["UrlChuyenMuc"].ToString() + ")", row["ChuyenMucID"].ToString()));
                    }
                }
            }
        }

        protected void btnConnect_Click(object sender, EventArgs e)
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8
            };

            try
            {
                DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 1, 0, drpChuyenMucID.SelectedValue);
                if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsChuyenMuc.Tables[0].Rows[0];

                    HtmlDocument html = htmlWeb.Load(row["UrlChuyenMuc"].ToString());

                    string xds = txtDanhSach.Text.Replace("tbody/", "");
                    string xbv = txtBaiViet_Url1.Text.Replace("tbody/", "");
                    string XDanhSach = xds.LastIndexOf(']') == xds.Length - 1 ? xds.Remove(xds.LastIndexOf('['), xds.Length - xds.LastIndexOf('[')) : xds;
                    string XBaiViet_Url = xbv.Replace(xds, ".");

                    var DanhSach = html.DocumentNode.SelectNodes(XDanhSach) != null ? html.DocumentNode.SelectNodes(XDanhSach).ToList() : null;
                    if (DanhSach != null)
                    {
                        foreach (var item in DanhSach)
                        {
                            var BaiViet_Url = item.SelectSingleNode(XBaiViet_Url) != null ? item.SelectSingleNode(XBaiViet_Url).Attributes["href"].Value.Replace("&amp;", "&") : null;

                            if (BaiViet_Url == null)
                            {
                                ham.Alert("Lỗi: Xpath không đúng!");
                                return;
                            }
                        }
                        ham.Alert("Thông báo: Kiểm tra hoàn tất, Xpath này có thể sử dụng được.");
                        return;
                    }
                    else
                    {
                        ham.Alert("Lỗi: Xpath không đúng!");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ham.Alert("Lỗi: " + ex.Message);
            }
        }
    }
}