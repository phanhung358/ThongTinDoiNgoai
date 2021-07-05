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
    public partial class XpathChiTiet_Tm : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sWebID = "0";
        string sChuyenMucID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            divDanhSach.Style.Add("display", "none");
            if (Request.QueryString["WebID"] != null && Request.QueryString["ChuyenMucID"] != null)
            {
                sWebID = Request.QueryString["WebID"];
                sChuyenMucID = Request.QueryString["ChuyenMucID"];
            }
            if (!IsPostBack)
            {
                addDanhMuc();
            }
                addSua();
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
                    txtXpathID.Value = row["XpathID"].ToString();
                    txtTomTat.Text = row["TomTat"].ToString();
                    txtNoiDung.Text = row["NoiDung"].ToString();
                    txtThoiGian.Text = row["ThoiGian"].ToString();
                    txtDinhDangThoiGian.Text = row["DinhDangThoiGian"].ToString();
                    txtTacGia.Text = row["TacGia"].ToString();
                }

                divDanhSach.Controls.Clear();

                Table tbl = new Table();
                tbl.Width = Unit.Percentage(100);
                tbl.CellPadding = 0;
                tbl.CellSpacing = 0;
                tbl.BorderWidth = 0;

                TableCell tblCell = new TableCell();
                TableRow tblRow = new TableRow();

                int k = 1;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 1)
                {
                    chkKhac.Checked = true;
                    chkKhac_CheckedChanged(null, null);
                    for (k = 1; k < ds.Tables[0].Rows.Count; k++)
                    {
                        DataRow row = ds.Tables[0].Rows[k];

                        tblRow = new TableRow();

                        tblCell = new TableCell();
                        HiddenField hidden = new HiddenField();
                        hidden.ID = "XpathID_" + k.ToString();
                        hidden.Value = row["XpathID"].ToString();
                        tblCell.Controls.Add(hidden);
                        tblRow.Controls.Add(tblCell);
                        tbl.Controls.Add(tblRow);

                        tblRow = new TableRow();

                        tblCell = new TableCell();
                        tblCell.Width = 115;
                        tblCell.Text = "Xpath Tóm tắt " + k.ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        TextBox txtTomTat = new TextBox();
                        txtTomTat.Width = Unit.Percentage(100);
                        txtTomTat.ID = "TomTat_" + k.ToString();
                        txtTomTat.Text = row["TomTat"].ToString();
                        tblCell.Controls.Add(txtTomTat);
                        tblRow.Controls.Add(tblCell);
                        tbl.Controls.Add(tblRow);

                        tblRow = new TableRow();

                        tblCell = new TableCell();
                        tblCell.Width = 115;
                        tblCell.Text = "Xpath Nội dung " + k.ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        TextBox txtNoiDung = new TextBox();
                        txtNoiDung.Width = Unit.Percentage(100);
                        txtNoiDung.ID = "NoiDung_" + k.ToString();
                        txtNoiDung.Text = row["NoiDung"].ToString();
                        tblCell.Controls.Add(txtNoiDung);
                        tblRow.Controls.Add(tblCell);
                        tbl.Controls.Add(tblRow);

                        tblRow = new TableRow();

                        tblCell = new TableCell();
                        tblCell.Width = 115;
                        tblCell.Text = "Xpath Thời gian " + k.ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        TextBox txtThoiGian = new TextBox();
                        txtThoiGian.Width = Unit.Percentage(100);
                        txtThoiGian.ID = "ThoiGian_" + k.ToString();
                        txtThoiGian.Text = row["ThoiGian"].ToString();
                        tblCell.Controls.Add(txtThoiGian);
                        tblRow.Controls.Add(tblCell);
                        tbl.Controls.Add(tblRow);

                        tblRow = new TableRow();

                        tblCell = new TableCell();
                        tblCell.Width = 115;
                        tblCell.Text = "Xpath Tác giả " + k.ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        TextBox txtTacGia = new TextBox();
                        txtTacGia.Width = Unit.Percentage(100);
                        txtTacGia.ID = "TacGia_" + k.ToString();
                        txtTacGia.Text = row["TacGia"].ToString();
                        tblCell.Controls.Add(txtTacGia);
                        tblRow.Controls.Add(tblCell);
                        tbl.Controls.Add(tblRow);
                    }
                }

                for (int i = k; i < 4; i++)
                {
                    tblRow = new TableRow();

                    tblCell = new TableCell();
                    HiddenField hidden = new HiddenField();
                    hidden.ID = "XpathID_" + i.ToString();
                    hidden.Value = "0";
                    tblCell.Controls.Add(hidden);
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);

                    tblRow = new TableRow();

                    tblCell = new TableCell();
                    tblCell.Width = 115;
                    tblCell.Text = "Xpath Tóm tắt " + i.ToString();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    TextBox txtTomTat = new TextBox();
                    txtTomTat.Width = Unit.Percentage(100);
                    txtTomTat.ID = "TomTat_" + i.ToString();
                    tblCell.Controls.Add(txtTomTat);
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);

                    tblRow = new TableRow();

                    tblCell = new TableCell();
                    tblCell.Width = 115;
                    tblCell.Text = "Xpath Nội dung " + i.ToString();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    TextBox txtNoiDung = new TextBox();
                    txtNoiDung.Width = Unit.Percentage(100);
                    txtNoiDung.ID = "NoiDung_" + i.ToString();
                    tblCell.Controls.Add(txtNoiDung);
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);

                    tblRow = new TableRow();

                    tblCell = new TableCell();
                    tblCell.Width = 115;
                    tblCell.Text = "Xpath Thời gian " + i.ToString();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    TextBox txtThoiGian = new TextBox();
                    txtThoiGian.Width = Unit.Percentage(100);
                    txtThoiGian.ID = "ThoiGian_" + i.ToString();
                    tblCell.Controls.Add(txtThoiGian);
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);

                    tblRow = new TableRow();

                    tblCell = new TableCell();
                    tblCell.Width = 115;
                    tblCell.Text = "Xpath Tác giả " + i.ToString();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    TextBox txtTacGia = new TextBox();
                    txtTacGia.Width = Unit.Percentage(100);
                    txtTacGia.ID = "TacGia_" + i.ToString();
                    tblCell.Controls.Add(txtTacGia);
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);
                }
                divDanhSach.Controls.Add(tbl);
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

                obj[0] = txtXpathID.Value;
                obj[1] = txtTomTat.Text.Trim();
                obj[2] = txtNoiDung.Text.Trim();
                obj[3] = txtThoiGian.Text.Trim();
                obj[4] = txtTacGia.Text.Trim();
                obj[5] = drpChuyenMuc.SelectedValue;
                obj[6] = drpWeb.SelectedValue;
                obj[7] = TUONGTAC.TenTaiKhoan;
                string sLoi = db.ExcuteSP("TTDN_XPATH_CHITIET_INSERT", obj);
                if (sLoi == "")
                {
                    if (chkKhac.Checked)
                    {
                        sLoi = Luu(drpWeb.SelectedValue, drpChuyenMuc.SelectedValue);
                    }
                    if (sLoi == "")
                        ScriptManager.RegisterClientScriptBlock(this.FindControl("btnThemMoi"), this.GetType(), "Message_Close", "alert('Cập nhật thành công !'); self.parent.tb_remove();", true);
                    else
                    {
                        ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                        return;
                    }
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
            ChromeOptions options = new ChromeOptions() { Proxy = null };
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--ignore-certificate-errors");
            ChromeDriver driver = new ChromeDriver(HttpContext.Current.Server.MapPath(Static.AppPath() + "/ChromeDriver"), options, TimeSpan.FromMinutes(3));

            try
            {
                List<string> dsTin = new List<string>();
                DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 3, drpWeb.SelectedValue);
                if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsChuyenMuc.Tables[0].Rows[0];

                    DataSet ds = db.GetDataSet("TTDN_XPATH_CHUYENMUC_SELECT", 0, drpWeb.SelectedValue, drpChuyenMuc.SelectedValue);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow rowXpathCM = ds.Tables[0].Rows[0];

                        driver.Navigate().GoToUrl(row["UrlChuyenMuc"].ToString());

                        HtmlDocument html = new HtmlDocument();
                        html.LoadHtml(driver.PageSource);

                        if (html == null)
                        {
                            ham.Alert("Không lấy được dữ liệu của chuyên mục!");
                            return;
                        }
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
                        int count = DanhSach.Count < 5 ? DanhSach.Count : 5;
                        if (DanhSach != null)
                        {
                            for (int i = 0; i < count; i++)
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
                                    BaiViet_Url = (BaiViet_Url.LastIndexOf(row["DiaChiWeb"].ToString()) > -1) ? BaiViet_Url : (row["DiaChiWeb"].ToString() + BaiViet_Url);
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

                if (dsTin.Count > 0)
                {
                    foreach (var item in dsTin)
                    {
                        driver.Navigate().GoToUrl(item);

                        HtmlDocument html = new HtmlDocument();
                        html.LoadHtml(driver.PageSource);

                        if (html == null)
                        {
                            ham.Alert("Không lấy được chi tiết bài viết!");
                            return;
                        }
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
            driver.Quit();
        }

        protected void chkKhac_CheckedChanged(object sender, EventArgs e)
        {
            divDanhSach.Style.Add("display", "none");
            if (chkKhac.Checked)
            {
                divDanhSach.Style.Remove("display");
            }
        }

        private string Luu(string WebID, string ChuyenMucID)
        {
            string sLoi = "";

            for (int i = 1; i < 4; i++)
            {
                HiddenField txtXpathID = (HiddenField)FindControl("XpathID_" + i.ToString());
                TextBox txtTomTat = (TextBox)FindControl("TomTat_" + i.ToString());
                TextBox txtNoiDung = (TextBox)FindControl("NoiDung_" + i.ToString());
                TextBox txtThoiGian = (TextBox)FindControl("ThoiGian_" + i.ToString());
                TextBox txtTacGia = (TextBox)FindControl("TacGia_" + i.ToString());

                object[] obj = new object[8];

                obj[0] = txtXpathID.Value;
                obj[1] = txtTomTat.Text.Trim();
                obj[2] = txtNoiDung.Text.Trim();
                obj[3] = txtThoiGian.Text.Trim();
                obj[4] = txtTacGia.Text.Trim();
                obj[5] = drpChuyenMuc.SelectedValue;
                obj[6] = drpWeb.SelectedValue;
                obj[7] = TUONGTAC.TenTaiKhoan;

                if (!string.IsNullOrEmpty(txtTomTat.Text) || !string.IsNullOrEmpty(txtNoiDung.Text) || !string.IsNullOrEmpty(txtThoiGian.Text) || !string.IsNullOrEmpty(txtTacGia.Text))
                {
                    sLoi = db.ExcuteSP("TTDN_XPATH_CHITIET_INSERT", obj);
                    if (sLoi != "")
                        return sLoi;
                }
                else
                {
                    if (txtXpathID.Value != "0")
                        sLoi = db.ExcuteSP("TTDN_XPATH_CHITIET_DELETE", txtXpathID.Value);
                    if (sLoi != "")
                        return sLoi;
                }
            }

            return sLoi;
        }
    }
}