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

                DataSet ds = db.GetDataSet("TTDN_XPATH_CHITIET_SELECT", 0, sWebID, sChuyenMucID);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
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
            if (drpWeb.SelectedValue == "0")
                sLoi = "Chưa chọn trang web!";
            if (drpChuyenMuc.SelectedValue == "0")
                sLoi = "Chưa chọn chuyên mục!";
            if (txtNoiDung.Text.Trim() == "")
                sLoi = "Chưa nhập Xpath nội dung bài viết!";
            if (txtThoiGian.Text.Trim() == "")
                sLoi = "Chưa nhập Xpath thời gian!";
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
                object[] obj = new object[8];


                obj[0] = txtTomTat.Text.Trim();
                obj[1] = txtNoiDung.Text.Trim();
                obj[2] = txtThoiGian.Text.Trim();
                obj[3] = txtDinhDangThoiGian.Text.Trim();
                obj[4] = txtTacGia.Text.Trim();
                obj[5] = drpChuyenMuc.SelectedValue;
                obj[6] = drpWeb.SelectedValue;
                obj[7] = TUONGTAC.TenTaiKhoan;
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
                DataSet dsWeb = db.GetDataSet("TTDN_TRANGWEB_SELECT", 1, drpWeb.SelectedValue);
                if (dsWeb != null && dsWeb.Tables.Count > 0 && dsWeb.Tables[0].Rows.Count > 0)
                {
                    DataRow rowWeb = dsWeb.Tables[0].Rows[0];

                    DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 1, 0, drpChuyenMuc.SelectedValue);
                    if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = dsChuyenMuc.Tables[0].Rows[0];

                        DataSet ds = db.GetDataSet("TTDN_XPATH_CHUYENMUC_SELECT", 0, drpWeb.SelectedValue, drpChuyenMuc.SelectedValue);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow rowXpathCM = ds.Tables[0].Rows[0];

                            HtmlDocument html = htmlWeb.Load(row["UrlChuyenMuc"].ToString());
                            html.DocumentNode.InnerHtml = html.DocumentNode.InnerHtml.Replace("<TABLE", "<table").Replace("<TR", "<tr").Replace("<TD", "<td").Replace("<DIV", "<div").Replace("<A", "<a").Replace("<P", "<p").Replace("<SPAN", "<span").Replace("<STRONG", "<strong").Replace("<EM", "<em").Replace("<TITLE", "<title").Replace("<SCRIPT", "<script").Replace("</TABLE", "</table").Replace("</TR", "</tr").Replace("</TD", "</td").Replace("</DIV", "</div").Replace("</A", "</a").Replace("</P", "</p").Replace("</SPAN", "</span").Replace("</STRONG", "</strong").Replace("</EM", "</em").Replace("</TITLE", "</title").Replace("</SCRIPT", "</script").Replace("<TBODY>", "").Replace("</TBODY>", "").Replace("<tbody>", "").Replace("</tbody>", "");

                            string xds = rowXpathCM["DanhSach"].ToString().Replace("tbody/", "");
                            string XDanhSach = xds.LastIndexOf(']') == xds.Length - 1 ? xds.Remove(xds.LastIndexOf('['), xds.Length - xds.LastIndexOf('[')) : xds;
                            string xbv = rowXpathCM["BaiViet_Url"].ToString().Replace("tbody/", "").Replace(XDanhSach, ".");
                            string xbv1 = rowXpathCM["BaiViet_Url1"].ToString().Replace("tbody/", "").Replace(XDanhSach, ".");
                            string xbv2 = rowXpathCM["BaiViet_Url2"].ToString().Replace("tbody/", "").Replace(XDanhSach, ".");
                            string XBaiViet_Url = xbv.IndexOf('[') == 1 ? xbv.Remove(1, xbv.IndexOf(']')) : xbv;
                            string XBaiViet_Url1 = xbv1.IndexOf('[') == 1 ? xbv1.Remove(1, xbv1.IndexOf(']')) : xbv1;
                            string XBaiViet_Url2 = xbv2.IndexOf('[') == 1 ? xbv2.Remove(1, xbv2.IndexOf(']')) : xbv2;

                            var DanhSach = html.DocumentNode.SelectNodes(XDanhSach) != null ? html.DocumentNode.SelectNodes(XDanhSach).ToList() : null;
                            if (DanhSach != null)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    string BaiViet_Url = null;
                                    if (!string.IsNullOrEmpty(XBaiViet_Url) && DanhSach[i].SelectSingleNode(XBaiViet_Url) != null)
                                        BaiViet_Url = DanhSach[i].SelectSingleNode(XBaiViet_Url).Attributes["href"].Value.Replace("&amp;", "&");
                                    else if (!string.IsNullOrEmpty(XBaiViet_Url1) && DanhSach[i].SelectSingleNode(XBaiViet_Url1) != null)
                                        BaiViet_Url = DanhSach[i].SelectSingleNode(XBaiViet_Url1).Attributes["href"].Value.Replace("&amp;", "&");
                                    else if (!string.IsNullOrEmpty(XBaiViet_Url2) && DanhSach[i].SelectSingleNode(XBaiViet_Url2) != null)
                                        BaiViet_Url = DanhSach[i].SelectSingleNode(XBaiViet_Url2).Attributes["href"].Value.Replace("&amp;", "&");
                                    else
                                        BaiViet_Url = null;

                                    if (BaiViet_Url != null)
                                    {
                                        BaiViet_Url = (BaiViet_Url.LastIndexOf(rowWeb["DiaChiWeb"].ToString()) > -1) ? BaiViet_Url : (rowWeb["DiaChiWeb"].ToString() + BaiViet_Url);
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
                        html.DocumentNode.InnerHtml = html.DocumentNode.InnerHtml.Replace("<TABLE", "<table").Replace("<TR", "<tr").Replace("<TD", "<td").Replace("<DIV", "<div").Replace("<A", "<a").Replace("<P", "<p").Replace("<SPAN", "<span").Replace("<STRONG", "<strong").Replace("<EM", "<em").Replace("<TITLE", "<title").Replace("<SCRIPT", "<script").Replace("</TABLE", "</table").Replace("</TR", "</tr").Replace("</TD", "</td").Replace("</DIV", "</div").Replace("</A", "</a").Replace("</P", "</p").Replace("</SPAN", "</span").Replace("</STRONG", "</strong").Replace("</EM", "</em").Replace("</TITLE", "</title").Replace("</SCRIPT", "</script").Replace("<TBODY>", "").Replace("</TBODY>", "").Replace("<tbody>", "").Replace("</tbody>", "");

                        string XTomTat = txtTomTat.Text.Replace("tbody/", "");
                        string XNoiDung = txtNoiDung.Text.Replace("tbody/", "");
                        string XThoiGian = txtThoiGian.Text.Replace("tbody/", "");
                        string XTacGia = txtTacGia.Text.Replace("tbody/", "");

                        var TomTat = XTomTat != "" ? (html.DocumentNode.SelectSingleNode(XTomTat) != null ? html.DocumentNode.SelectSingleNode(XTomTat) : null) : null;
                        var NoiDung = html.DocumentNode.SelectSingleNode(XNoiDung) != null ? html.DocumentNode.SelectSingleNode(XNoiDung) : null;
                        var ThoiGian = XThoiGian != "" ? (html.DocumentNode.SelectSingleNode(XThoiGian) != null ? html.DocumentNode.SelectSingleNode(XThoiGian) : null) : null;
                        var TacGia = XTacGia != "" ? (html.DocumentNode.SelectSingleNode(XTacGia) != null ? html.DocumentNode.SelectSingleNode(XTacGia) : null) : null;

                        if (NoiDung == null)
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