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
    public partial class XpathChiTiet_Tm : System.Web.UI.UserControl
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

                DataSet ds = db.GetDataSet("TTDN_XPATH_CHITIET_SELECT", 0, sWebID, sChuyenMucID);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    txtTieuDe.Text = row["TieuDe"].ToString();
                    txtTieuDePhu.Text = row["TieuDePhu"].ToString();
                    txtTomTat.Text = row["TomTat"].ToString();
                    txtNoiDung.Text = row["NoiDung"].ToString();
                    txtThoiGian.Text = row["ThoiGian"].ToString();
                    txtDinhDangThoiGian.Text = row["DinhDangThoiGian"].ToString();
                    txtTacGia.Text = row["TacGia"].ToString();
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
            if (txtTieuDe.Text.Trim() == "")
                sLoi = "Chưa nhập Xpath tiêu đề!";
            if (txtNoiDung.Text.Trim() == "")
                sLoi = "Chưa nhập Xpath nội dung bài viết!";
            if (txtThoiGian.Text.Trim() == "")
                sLoi = "Chưa nhập Xpath thời gian!";
            if (txtDinhDangThoiGian.Text.Trim() == "")
                sLoi = "Chưa nhập định dạng thời gian!";

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


                obj[0] = txtTieuDe.Text.Trim();
                obj[1] = txtTieuDePhu.Text.Trim();
                obj[2] = txtTomTat.Text.Trim();
                obj[3] = txtNoiDung.Text.Trim();
                obj[4] = txtThoiGian.Text.Trim();
                obj[5] = txtDinhDangThoiGian.Text.Trim();
                obj[6] = txtTacGia.Text.Trim();
                obj[7] = drpChuyenMucID.SelectedValue;
                obj[8] = drpWebID.SelectedValue;
                obj[9] = "Phan Hùng";
                string sLoi = db.ExcuteSP("TTDN_XPATH_CHITIET_INSERT", obj);
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
                List<string> dsTin = new List<string>();
                DataSet dsWeb = db.GetDataSet("TTDN_TRANGWEB_SELECT", 1, drpWebID.SelectedValue);
                if (dsWeb != null && dsWeb.Tables.Count > 0 && dsWeb.Tables[0].Rows.Count > 0)
                {
                    DataRow rowWeb = dsWeb.Tables[0].Rows[0];

                    DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 1, 0, drpChuyenMucID.SelectedValue);
                    if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = dsChuyenMuc.Tables[0].Rows[0];

                        DataSet ds = db.GetDataSet("TTDN_XPATH_CHUYENMUC_SELECT", 0, drpWebID.SelectedValue, drpChuyenMucID.SelectedValue);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow rowXpathCM = ds.Tables[0].Rows[0];

                            HtmlDocument html = htmlWeb.Load(row["UrlChuyenMuc"].ToString());

                            string xds = rowXpathCM["DanhSach"].ToString().Replace("tbody/", "");
                            string xbv = rowXpathCM["BaiViet_Url"].ToString().Replace("tbody/", "");
                            string XDanhSach = xds.LastIndexOf(']') == xds.Length - 1 ? xds.Remove(xds.LastIndexOf('['), xds.Length - xds.LastIndexOf('[')) : xds;
                            string XBaiViet_Url = xbv.Replace(xds, ".");

                            var DanhSach = html.DocumentNode.SelectNodes(XDanhSach) != null ? html.DocumentNode.SelectNodes(XDanhSach).ToList() : null;
                            if (DanhSach != null)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    var BaiViet_Url = DanhSach[i].SelectSingleNode(XBaiViet_Url) != null ? DanhSach[i].SelectSingleNode(XBaiViet_Url).Attributes["href"].Value.Replace("&amp;", "&") : null;

                                    if (BaiViet_Url != null)
                                    {
                                        if (BaiViet_Url.LastIndexOf(row["DiaChiWeb"].ToString()) == -1)
                                        {
                                            if (BaiViet_Url.Substring(0, 1) == "/")
                                                BaiViet_Url = row["DiaChiWeb"].ToString() + BaiViet_Url;
                                            else if (BaiViet_Url.IndexOf("https") == 0 && row["DiaChiWeb"].ToString().IndexOf("http") == 0)
                                                BaiViet_Url.Replace("https", "http");
                                            else if (BaiViet_Url.IndexOf("http") == 0 && row["DiaChiWeb"].ToString().IndexOf("https") == 0)
                                                BaiViet_Url.Replace("http", "https");
                                        }

                                        string obj = BaiViet_Url;
                                        dsTin.Add(obj);
                                    }
                                }
                            }
                            else
                            {
                                ham.Alert("Lỗi: Xpath chuyên mục không đúng!");
                                return;
                            }
                        }
                        else
                        {
                            ham.Alert("Lỗi: Xpath chuyên mục không tồn tại! (Bạn phải nhập Xpath chuyên mục trước)");
                            return;
                        }
                    }
                }

                if (dsTin.Count > 0)
                {
                    foreach (var item in dsTin)
                    {
                        HtmlDocument html = htmlWeb.Load(item);

                        string XTieuDe = txtTieuDe.Text.Replace("tbody/", "");
                        string XTieuDePhu = txtTieuDePhu.Text.Replace("tbody/", "");
                        string XTomTat = txtTomTat.Text.Replace("tbody/", "");
                        string XNoiDung = txtNoiDung.Text.Replace("tbody/", "");
                        string XThoiGian = txtThoiGian.Text.Replace("tbody/", "");
                        string XTacGia = txtTacGia.Text.Replace("tbody/", "");

                        var TieuDe = html.DocumentNode.SelectSingleNode(XTieuDe) != null ? html.DocumentNode.SelectSingleNode(XTieuDe) : null;
                        var TieuDePhu = XTieuDePhu != "" ? (html.DocumentNode.SelectSingleNode(XTieuDePhu) != null ? html.DocumentNode.SelectSingleNode(XTieuDePhu) : null) : null;
                        var TomTat = XTomTat != "" ? (html.DocumentNode.SelectSingleNode(XTomTat) != null ? html.DocumentNode.SelectSingleNode(XTomTat) : null) : null;
                        var NoiDung = html.DocumentNode.SelectSingleNode(XNoiDung) != null ? html.DocumentNode.SelectSingleNode(XNoiDung) : null;
                        var ThoiGian = XThoiGian != "" ? (html.DocumentNode.SelectSingleNode(XThoiGian) != null ? html.DocumentNode.SelectSingleNode(XThoiGian) : null) : null;
                        var TacGia = XTacGia != "" ? (html.DocumentNode.SelectSingleNode(XTacGia) != null ? html.DocumentNode.SelectSingleNode(XTacGia) : null) : null;

                        if (TieuDe == null || NoiDung == null)
                        {
                            ham.Alert("Lỗi: Xpath chi tiết không đúng!");
                            return;
                        }
                    }
                }
                else
                {
                    ham.Alert("Lỗi: Xpath chuyên mục không đúng!");
                    return;
                }

                ham.Alert("Thông báo: Kiểm tra hoàn tất, Xpath này có thể sử dụng được.");
                return;
            }
            catch (Exception ex)
            {
                ham.Alert("Lỗi: " + ex.Message);
            }
        }
    }
}