using FITC.Web.Component;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThongTinDoiNgoai;

namespace QuanLyVanBan.DichVu.DuLieu
{
    public partial class BaiViet : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSuKien.Style.Add("display", "none");
            if (!IsPostBack)
            {
                addDanhMuc();
            }
            addData();
            btnXoa.Attributes["onclick"] = "return confirm('Bạn có chắc chắn xóa không?')";
        }

        private void addDanhMuc()
        {
            drpWeb.Items.Add(new ListItem("[Tất cả]", "0"));
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
            drpChuyenMuc.Items.Add(new ListItem("[Tất cả]", "0"));
        }

        #region Các hàm phân trang
        private void PhanTrang(int TongSoTin, int SoTinTrenTrang, int TrangHienTai, Label lblPhanTrang)
        {
            try
            {
                lblPhanTrang.Controls.Clear();
                Table tbl = new Table();
                TableRow tblRow = new TableRow();
                TableCell tblCell = new TableCell();
                int TongSoTrang = TongSoTin / SoTinTrenTrang;
                if (TongSoTin % SoTinTrenTrang != 0)
                    TongSoTrang = TongSoTrang + 1;
                lblPhanTrang.Visible = true;
                if (TongSoTin <= SoTinTrenTrang)
                {
                    lblPhanTrang.Visible = false;
                    return;
                }

                tblCell = new TableCell();
                tblCell.Text = "Trang:";
                tblCell.Width = 15;
                tblRow.Controls.Add(tblCell);

                int k = TrangHienTai / 10;
                k = TrangHienTai % 10 != 0 ? k + 1 : k;
                int TrangBatDau = (k - 1) * 10 + 1;
                int TrangKetThuc = TongSoTrang > 10 * k ? 10 * k : TongSoTrang;
                if (TrangBatDau > 10)
                {
                    int index = TrangBatDau - 1;
                    tblCell = new TableCell();
                    LinkButton link = new LinkButton();
                    link.Text = "<<";
                    link.ID = "btnLink_" + index.ToString();
                    link.CssClass = "Tin_PhanTrang";
                    link.Click += new EventHandler(LinkTrangTiep_Click);
                    tblCell.Controls.Add(link);
                    tblRow.Controls.Add(tblCell);
                }
                for (int i = TrangBatDau; i <= TrangKetThuc; i++)
                {
                    if (i == TrangHienTai)
                    {
                        tblCell = new TableCell();
                        tblCell.Text = "[" + i.ToString() + "]";
                        tblCell.CssClass = "Tin_PhanTrang_HienTai";
                        tblCell.Width = 2;
                        tblRow.Controls.Add(tblCell);
                    }
                    else
                    {
                        tblCell = new TableCell();
                        LinkButton link = new LinkButton();
                        link.Text = i.ToString();
                        link.ID = "btnLink_" + i.ToString();
                        link.CssClass = "Tin_PhanTrang";
                        tblCell.Width = 2;
                        link.Click += new EventHandler(LinkTrangTiep_Click);
                        tblCell.Controls.Add(link);
                        tblRow.Controls.Add(tblCell);
                    }
                }
                if (TongSoTrang > 10 * k)
                {
                    int index = 10 * k + 1;
                    tblCell = new TableCell();
                    LinkButton link = new LinkButton();
                    link.Text = ">>";
                    link.ID = "btnLink_" + index.ToString();
                    link.CssClass = "Tin_PhanTrang";
                    tblCell.Width = 100;
                    link.Click += new EventHandler(LinkTrangTiep_Click);
                    tblCell.Controls.Add(link);
                    tblRow.Controls.Add(tblCell);
                }
                tbl.Controls.Add(tblRow);
                lblPhanTrang.Controls.Add(tbl);
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnSuKien");
            }
        }

        protected void LinkTrangTiep_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnLink = (LinkButton)sender;
                Static.PhanTrangThu = Int16.Parse(btnLink.ID.Substring(8));
                addData();
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnSuKien");
            }
        }
        protected void drpSoTin_SelectedIndexChanged(object sender, EventArgs e)
        {
            Static.PhanTrangThu = 1;
            addData();
        }


        #endregion

        private void addData()
        {
            divDanhSach.Controls.Clear();
            Table tbl = new Table();
            tbl.Width = Unit.Percentage(100);
            tbl.CssClass = "Vien_Bang";
            tbl.CellPadding = 3;
            tbl.CellSpacing = 1;
            tbl.BorderWidth = 0;

            TableRow tblRow;
            TableCell tblCell;

            tblRow = new TableRow();
            tblRow.CssClass = "Dong_TieuDe";

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 40;
            tblCell.Text = "STT";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 115;
            tblCell.Text = "Ngày đăng";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Tiêu đề";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 400;
            tblCell.Text = "URL bài viết";
            tblRow.Controls.Add(tblCell);

            tbl.Controls.Add(tblRow);

            tblPhanTrang.Visible = false;
            using (DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 1, 0, drpWeb.SelectedValue.ToString(), drpChuyenMuc.SelectedValue.ToString()))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //Phân trang===============================================
                    int iSoTin = int.Parse(drpSoTin.SelectedValue) * (Static.PhanTrangThu - 1);
                    if (ds.Tables[0].Rows.Count <= iSoTin && Static.PhanTrangThu != 1)
                        Static.PhanTrangThu = Static.PhanTrangThu - 1;

                    int TrangHienTai = Static.PhanTrangThu;
                    int TongSoTin = ds.Tables[0].Rows.Count;
                    int SoTinTrenTrang = Convert.ToInt32(drpSoTin.SelectedValue);
                    PhanTrang(TongSoTin, SoTinTrenTrang, TrangHienTai, lblPhanTrang);
                    int TuBanGhi = (TrangHienTai - 1) * SoTinTrenTrang;
                    int DenBanGhi = (TrangHienTai * SoTinTrenTrang) > TongSoTin ? TongSoTin : TrangHienTai * SoTinTrenTrang;
                    if (TongSoTin > SoTinTrenTrang)
                        tblPhanTrang.Visible = true;
                    //End phân trang==========================================

                    for (int i = TuBanGhi; i < DenBanGhi; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        string css = "Dong_Chan";
                        if (i % 2 == 1)
                            css = "Dong_Le";
                        tblRow = new TableRow();
                        tblRow.CssClass = css;

                        tblCell = new TableCell();
                        tblCell.Text = (i + 1).ToString();
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = !string.IsNullOrEmpty(row["ThoiGian"].ToString()) ? DateTime.Parse(row["ThoiGian"].ToString()).ToString("dd/MM/yyyy - HH:mm") : "";
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["TieuDe"].ToString();
                        tblCell.CssClass = "LienKet";
                        tblCell.Attributes["style"] = "cursor: pointer";
                        tblCell.Attributes["onclick"] = string.Format("return thickboxPopup('Chi tiết bài viết', '{0}?control={1}&BaiVietID={2}','1000','98%');", Static.AppPath() + "/home/popup.aspx", "/DichVu/ThongTinDoiNgoai/BaiViet_Xem.ascx", row["BaiVietID"].ToString());
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        HyperLink link = new HyperLink();
                        link.NavigateUrl = row["BaiViet_Url"].ToString();
                        link.Text = row["BaiViet_Url"].ToString();
                        link.ToolTip = row["BaiViet_Url"].ToString();
                        link.Target = "_blank";
                        link.CssClass = "LienKet text-ellipsis";
                        tblCell.Controls.Add(link);
                        tblRow.Controls.Add(tblCell);

                        tbl.Controls.Add(tblRow);
                    }
                }
                else
                {
                    int intCell = tblRow.Cells.Count;
                    tblRow = new TableRow();
                    tblRow.CssClass = "Dong_Chan";
                    tblCell = new TableCell();
                    tblCell.ColumnSpan = intCell;
                    tblCell.Text = "Thông tin chưa được cập nhật";
                    tblCell.HorizontalAlign = HorizontalAlign.Center;
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);
                }
                divDanhSach.Controls.Add(tbl);
            }
        }

        protected void drpWeb_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpChuyenMuc.Items.Clear();
            if (drpWeb.SelectedValue == "0")
                drpChuyenMuc.Items.Add(new ListItem("[Tất cả]", "0"));
            else
            {
                drpChuyenMuc.Items.Add(new ListItem("[Tất cả]", "0"));
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
            addData();
        }

        protected void drpChuyenMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            addData();
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            LayTinTuDong t = new LayTinTuDong();
            t.ThucHien(drpWeb.SelectedValue);

            //HtmlWeb htmlWeb = new HtmlWeb()
            //{
            //    AutoDetectEncoding = false,
            //    OverrideEncoding = Encoding.UTF8
            //};
            //FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());

            //DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 3, drpWeb.SelectedValue);
            //if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
            //{
            //    for (int j = 0; j < dsChuyenMuc.Tables[0].Rows.Count; j++)
            //    {
            //        List<object[]> dsTin = new List<object[]>();
            //        DataRow rowCM = dsChuyenMuc.Tables[0].Rows[j];
            //        try
            //        {
            //            DataSet dsXpathCM = db.GetDataSet("TTDN_XPATH_CHUYENMUC_SELECT", 0, rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString());
            //            if (dsXpathCM != null && dsXpathCM.Tables.Count > 0 && dsXpathCM.Tables[0].Rows.Count > 0)
            //            {
            //                DataRow rowXpathCM = dsXpathCM.Tables[0].Rows[0];

            //                HtmlDocument html = htmlWeb.Load(rowCM["UrlChuyenMuc"].ToString());

            //                if (html == null)
            //                {
            //                    string Loi = "Không lấy được dữ liệu của chuyên mục!";
            //                    string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), Loi, rowCM["UrlChuyenMuc"].ToString());
            //                    continue;
            //                }
            //                html.DocumentNode.InnerHtml = html.DocumentNode.InnerHtml.Replace("<TABLE", "<table").Replace("<TR", "<tr").Replace("<TD", "<td").Replace("<DIV", "<div").Replace("<A", "<a").Replace("<P", "<p").Replace("<SPAN", "<span").Replace("<STRONG", "<strong").Replace("<EM", "<em").Replace("<TITLE", "<title").Replace("<SCRIPT", "<script").Replace("</TABLE", "</table").Replace("</TR", "</tr").Replace("</TD", "</td").Replace("</DIV", "</div").Replace("</A", "</a").Replace("</P", "</p").Replace("</SPAN", "</span").Replace("</STRONG", "</strong").Replace("</EM", "</em").Replace("</TITLE", "</title").Replace("</SCRIPT", "</script").Replace("<TBODY>", "").Replace("</TBODY>", "").Replace("<tbody>", "").Replace("</tbody>", "");

            //                string xds = rowXpathCM["DanhSach"].ToString().Replace("tbody/", "");
            //                string XDanhSach = xds.LastIndexOf(']') == xds.Length - 1 ? xds.Remove(xds.LastIndexOf('['), xds.Length - xds.LastIndexOf('[')) : xds;
            //                string xbv = rowXpathCM["BaiViet_Url"].ToString().Replace("tbody/", "").Replace(XDanhSach, ".");
            //                string xbv1 = rowXpathCM["BaiViet_Url1"].ToString().Replace("tbody/", "").Replace(XDanhSach, ".");
            //                string xbv2 = rowXpathCM["BaiViet_Url2"].ToString().Replace("tbody/", "").Replace(XDanhSach, ".");
            //                string XBaiViet_Url = xbv.IndexOf('[') == 1 ? xbv.Remove(1, xbv.IndexOf(']')) : xbv;
            //                string XBaiViet_Url1 = xbv1.IndexOf('[') == 1 ? xbv1.Remove(1, xbv1.IndexOf(']')) : xbv1;
            //                string XBaiViet_Url2 = xbv2.IndexOf('[') == 1 ? xbv2.Remove(1, xbv2.IndexOf(']')) : xbv2;

            //                var DanhSach = html.DocumentNode.SelectNodes(XDanhSach) != null ? html.DocumentNode.SelectNodes(XDanhSach).ToList() : null;
            //                if (DanhSach != null)
            //                {
            //                    int count = 0;
            //                    foreach (var item in DanhSach)
            //                    {
            //                        string BaiViet_Url = null;
            //                        if (!string.IsNullOrEmpty(XBaiViet_Url) && item.SelectSingleNode(XBaiViet_Url) != null)
            //                            BaiViet_Url = item.SelectSingleNode(XBaiViet_Url).Attributes["href"].Value.Replace("&amp;", "&").Replace("&#x3a;", ":").Replace("&#x2f;", "/").Replace("&#x2e;", ".");
            //                        else if (!string.IsNullOrEmpty(XBaiViet_Url1) && item.SelectSingleNode(XBaiViet_Url1) != null)
            //                            BaiViet_Url = item.SelectSingleNode(XBaiViet_Url1).Attributes["href"].Value.Replace("&amp;", "&").Replace("&#x3a;", ":").Replace("&#x2f;", "/").Replace("&#x2e;", ".");
            //                        else if (!string.IsNullOrEmpty(XBaiViet_Url2) && item.SelectSingleNode(XBaiViet_Url2) != null)
            //                            BaiViet_Url = item.SelectSingleNode(XBaiViet_Url2).Attributes["href"].Value.Replace("&amp;", "&").Replace("&#x3a;", ":").Replace("&#x2f;", "/").Replace("&#x2e;", ".");
            //                        else
            //                            BaiViet_Url = null;

            //                        if (BaiViet_Url != null)
            //                        {
            //                            if (BaiViet_Url.Contains("http"))
            //                            {
            //                                if (rowCM["DiaChiWeb"].ToString().Substring(0, 5) == "https" && BaiViet_Url.Substring(0, 5) != "https")
            //                                    BaiViet_Url = BaiViet_Url.Replace("http", "https");
            //                                else if (rowCM["DiaChiWeb"].ToString().Substring(0, 5) != "https" && BaiViet_Url.Substring(0, 5) == "https")
            //                                    BaiViet_Url = BaiViet_Url.Replace("https", "http");
            //                            }
            //                            else
            //                            {
            //                                if (BaiViet_Url.LastIndexOf(rowCM["DiaChiWeb"].ToString()) == -1)
            //                                {
            //                                    BaiViet_Url = rowCM["DiaChiWeb"].ToString() + (BaiViet_Url.IndexOf('/') == 0 ? BaiViet_Url : "/" + BaiViet_Url);
            //                                }
            //                            }

            //                            object[] obj = new object[3];
            //                            obj[0] = BaiViet_Url;
            //                            obj[1] = rowXpathCM["ChuyenMucID"].ToString();
            //                            obj[2] = rowXpathCM["WebID"].ToString();
            //                            dsTin.Add(obj);
            //                        }
            //                        else
            //                        {
            //                            count++;
            //                        }
            //                    }
            //                    if (count == DanhSach.Count)
            //                    {
            //                        string Loi = "Không lấy được đường dẫn (URL) của bài viết trong chuyên mục!";
            //                        string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), Loi, rowCM["UrlChuyenMuc"].ToString());
            //                    }
            //                }
            //                else
            //                {
            //                    string Loi = "Không lấy được danh sách tin của chuyên mục!";
            //                    string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), Loi, rowCM["UrlChuyenMuc"].ToString());
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), ex.Message, "");
            //        }

            //        foreach (var item in dsTin)
            //        {
            //            try
            //            {
            //                DataSet dsXpathCT = db.GetDataSet("TTDN_XPATH_CHITIET_SELECT", 0, item[2].ToString(), item[1].ToString());
            //                if (dsXpathCT != null && dsXpathCT.Tables.Count > 0 && dsXpathCT.Tables[0].Rows.Count > 0)
            //                {
            //                    DataRow rowXpathCT = dsXpathCT.Tables[0].Rows[0];

            //                    HtmlDocument html = htmlWeb.Load(item[0].ToString());

            //                    if (html == null)
            //                    {
            //                        string Loi = "Không lấy được chi tiết bài viết!";
            //                        string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCT["WebID"].ToString(), rowXpathCT["ChuyenMucID"].ToString(), Loi, item[0].ToString());
            //                        continue;
            //                    }
            //                    html.DocumentNode.InnerHtml = html.DocumentNode.InnerHtml.Replace("<TABLE", "<table").Replace("<TR", "<tr").Replace("<TD", "<td").Replace("<DIV", "<div").Replace("<A", "<a").Replace("<P", "<p").Replace("<SPAN", "<span").Replace("<STRONG", "<strong").Replace("<EM", "<em").Replace("<TITLE", "<title").Replace("<SCRIPT", "<script").Replace("</TABLE", "</table").Replace("</TR", "</tr").Replace("</TD", "</td").Replace("</DIV", "</div").Replace("</A", "</a").Replace("</P", "</p").Replace("</SPAN", "</span").Replace("</STRONG", "</strong").Replace("</EM", "</em").Replace("</TITLE", "</title").Replace("</SCRIPT", "</script").Replace("<TBODY>", "").Replace("</TBODY>", "").Replace("<tbody>", "").Replace("</tbody>", "");

            //                    string XTieuDe = rowXpathCT["TieuDe"].ToString().Replace("tbody/", "");
            //                    string XTieuDePhu = rowXpathCT["TieuDePhu"].ToString().Replace("tbody/", "");
            //                    string XTomTat = rowXpathCT["TomTat"].ToString().Replace("tbody/", "");
            //                    string XNoiDung = rowXpathCT["NoiDung"].ToString().Replace("tbody/", "");
            //                    string XThoiGian = rowXpathCT["ThoiGian"].ToString().Replace("tbody/", "");
            //                    string XTacGia = rowXpathCT["TacGia"].ToString().Replace("tbody/", "");

            //                    var TieuDe = html.DocumentNode.SelectSingleNode(XTieuDe) != null ? html.DocumentNode.SelectSingleNode(XTieuDe) : null;
            //                    var TieuDePhu = XTieuDePhu != "" ? (html.DocumentNode.SelectSingleNode(XTieuDePhu) != null ? html.DocumentNode.SelectSingleNode(XTieuDePhu) : null) : null;
            //                    var TomTat = XTomTat != "" ? (html.DocumentNode.SelectSingleNode(XTomTat) != null ? html.DocumentNode.SelectSingleNode(XTomTat) : null) : null;
            //                    var NoiDung = html.DocumentNode.SelectSingleNode(XNoiDung) != null ? html.DocumentNode.SelectSingleNode(XNoiDung) : null;
            //                    var ThoiGian = XThoiGian != "" ? (html.DocumentNode.SelectSingleNode(XThoiGian) != null ? html.DocumentNode.SelectSingleNode(XThoiGian) : null) : null;
            //                    var TacGia = XTacGia != "" ? (html.DocumentNode.SelectSingleNode(XTacGia) != null ? html.DocumentNode.SelectSingleNode(XTacGia) : null) : null;


            //                    string tgian = null;
            //                    string strNewsDatePosted = null;
            //                    if (ThoiGian != null)
            //                    {
            //                        strNewsDatePosted = ThoiGian.InnerText.Trim();
            //                        tgian = LayNgay(strNewsDatePosted);
            //                    }

            //                    if (!string.IsNullOrEmpty(XTieuDe) && TieuDe == null)
            //                    {
            //                        string Loi = "Không lấy được tiêu đề của bài viết!";
            //                        string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCT["WebID"].ToString(), rowXpathCT["ChuyenMucID"].ToString(), Loi, item[0].ToString());
            //                    }
            //                    if (!string.IsNullOrEmpty(XTomTat) && TomTat == null)
            //                    {
            //                        string Loi = "Không lấy được tóm tắt bài viết!";
            //                        string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCT["WebID"].ToString(), rowXpathCT["ChuyenMucID"].ToString(), Loi, item[0].ToString());
            //                    }
            //                    if (!string.IsNullOrEmpty(XNoiDung) && NoiDung == null)
            //                    {
            //                        string Loi = "Không lấy được nội dung của bài viết!";
            //                        string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCT["WebID"].ToString(), rowXpathCT["ChuyenMucID"].ToString(), Loi, item[0].ToString());
            //                    }
            //                    if (!string.IsNullOrEmpty(XThoiGian) && ThoiGian == null)
            //                    {
            //                        string Loi = "Không lấy được thời gian đăng bài!";
            //                        string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCT["WebID"].ToString(), rowXpathCT["ChuyenMucID"].ToString(), Loi, item[0].ToString());
            //                    }

            //                    string DiaChiWeb = "";
            //                    DataSet ds1 = db.GetDataSet("TTDN_TRANGWEB_SELECT", 1, item[2].ToString());
            //                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            //                    {
            //                        DataRow rowWeb = ds1.Tables[0].Rows[0];
            //                        DiaChiWeb = rowWeb["DiaChiWeb"].ToString();
            //                    }
            //                    string WebHost = new Uri(item[0].ToString()).Host;
            //                    string GiaoThuc = new Uri(item[0].ToString()).Scheme;

            //                    if (NoiDung != null)
            //                    {
            //                        string DirUpload = Static.GetPath() + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DiaChiWeb.Remove(0, DiaChiWeb.IndexOf("/") + 2) + "/";
            //                        var dsFile = NoiDung.SelectNodes(".//img");
            //                        if (dsFile != null)
            //                        {
            //                            foreach (var file in dsFile)
            //                            {
            //                                string strSource = file.Attributes["src"].Value;
            //                                if (!strSource.Contains(WebHost))
            //                                {
            //                                    if (strSource.IndexOf("/") == 0)
            //                                        strSource = GiaoThuc + "://" + WebHost + strSource;
            //                                    else if (strSource.IndexOf("http") != 0)
            //                                        strSource = GiaoThuc + "://" + WebHost + "/" + strSource;
            //                                }

            //                                string err = DownloadFile(strSource, DirUpload);
            //                                if (err != "")
            //                                {
            //                                    string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", item[2].ToString(), item[1].ToString(), err, strSource);
            //                                }
            //                            }
            //                        }
            //                    }

            //                    object[] obj = new object[10];
            //                    obj[0] = TieuDe != null ? TieuDe.InnerText : null;
            //                    obj[1] = TieuDePhu != null ? TieuDePhu.InnerText : null;
            //                    obj[2] = TomTat != null ? TomTat.InnerText : null;
            //                    obj[3] = NoiDung != null ? NoiDung.InnerHtml : null;
            //                    obj[4] = ThoiGian != null ? tgian : null;
            //                    obj[5] = TacGia != null ? TacGia.InnerText : null;
            //                    obj[6] = item[0].ToString();
            //                    obj[7] = NoiDung?.InnerHtml.Length;
            //                    obj[8] = item[1].ToString();
            //                    obj[9] = item[2].ToString();
            //                    string sLoi = db.ExcuteSP("TTDN_BAIVIET_INSERT", obj);
            //                    if (sLoi != "")
            //                    {

            //                    }
            //                    if (item == dsTin[dsTin.Count - 1])
            //                    {
            //                        string s = db.ExcuteSP("TTDN_CHUYENMUC_UPDATE_THOIGIANDONGBO", item[1].ToString());
            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", item[2].ToString(), item[1].ToString(), ex.Message, "");
            //            }
            //        }
            //    }
            //}



            addData();
        }

        protected void btnXoa_Click(object sender, EventArgs e)
        {
            db.ExcuteSP("TTDN_BAIVIET_DELETE", drpWeb.SelectedValue);
            addData();
        }

        private string LayNgay(string inputText)
        {
            inputText = inputText.ToLower();
            string kq = "";
            try
            {
                //string inputText = " gg gd ngay cap nhat 07/9/2021 2:00:23 AM gdf dgd gdg";
                string regex = @"(?<ngay>[0|1|2]?[0-9]|3[01])[/-](?<thang>0?[1-9]|1[012])[/-](?<nam>[1-9][0-9][0-9][0-9])[\s]?(?<gio>[0|1]?[0-9]|2[0-4])?[:]?(?<phut>[0-5][0-9])?[:]?(?<giay>[0-5][0-9])?[\s]?(?<buoi>am|pm|sa|ch)?";
                MatchCollection matchCollection = Regex.Matches(inputText.ToLower(), regex);
                if (matchCollection.Count > 0)
                {
                    string ngayThang = matchCollection[0].Value; // 07/9/2021 14:00:00
                }
                CacHamChung ham = new CacHamChung();
                foreach (Match match in matchCollection)
                {
                    kq = kq + match.Groups["thang"].Value;
                    kq = kq + "/" + match.Groups["ngay"].Value;
                    kq = kq + "/" + match.Groups["nam"].Value;
                    kq = kq + " " + (match.Groups["gio"].Value == "" ? "00" : match.Groups["gio"].Value);
                    kq = kq + ":" + (match.Groups["phut"].Value == "" ? "00" : match.Groups["phut"].Value);
                    kq = kq + ":" + (match.Groups["giay"].Value == "" ? "00" : match.Groups["giay"].Value);
                    kq = kq + " " + match.Groups["buoi"].Value;
                }
                kq = kq.Replace("sa", "am");
                kq = kq.Replace("ch", "pm");
            }
            catch
            { }
            return kq;
        }

        private string DownloadFile(string url, string folderName)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpWebResponse resp;

                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    resp = (HttpWebResponse)req.GetResponse();
                }
                catch (Exception)
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.UserAgent = "Mozilla/5.0";
                    resp = (HttpWebResponse)req.GetResponse();
                }

                using (resp)
                {
                    string folderPath = HttpContext.Current.Server.MapPath("~/" + folderName);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filename = url.Substring(url.LastIndexOf("/") + 1).Replace("%20", "_");
                    if (filename.Contains("?"))
                    {
                        filename = filename.Replace(filename.Substring(filename.IndexOf("?"), filename.IndexOf("=") - filename.IndexOf("?") + 1), "_");
                        if (filename.Contains("&"))
                            filename = filename.Replace(filename.Substring(filename.IndexOf("&"), filename.IndexOf("=") - filename.IndexOf("&") + 1), "_");
                    }
                    if (!filename.Contains("."))
                        filename += ".jpg";
                    string filePath = HttpContext.Current.Server.MapPath("~/" + folderName + "/" + filename);

                    using (FileStream outputFileStream = new FileStream(filePath, FileMode.Create))
                    {
                        resp.GetResponseStream().CopyTo(outputFileStream);
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private bool LuuTapTin(string strSource, string strUploadFolder, string strFile)
        {
            try
            {
                bool redo = false;
                const int maxRetries = 5;
                int retries = 0;

                HttpWebRequest request;
                HttpWebResponse response = null;
                do
                {
                    try
                    {
                        redo = false;
                        DirectoryInfo dirInfo = new DirectoryInfo(strUploadFolder);
                        if (dirInfo.Exists == false)
                            dirInfo.Create();
                        FileInfo fileInfo = new FileInfo(dirInfo.FullName + strFile);
                        if (fileInfo.Exists)
                        {
                            return true;
                        }
                        request = (HttpWebRequest)WebRequest.Create(strSource);
                        response = (HttpWebResponse)request.GetResponse();
                        Stream stream = response.GetResponseStream();

                        // Write to disk
                        FileStream fs = new FileStream(dirInfo.FullName + strFile, FileMode.Create);
                        byte[] read = new byte[1024]; //1KB
                        int count = stream.Read(read, 0, read.Length);
                        while (count > 0)
                        {
                            fs.Write(read, 0, count);
                            count = stream.Read(read, 0, read.Length);
                        }
                        fs.Flush();
                        fs.Close();
                        stream.Flush();
                        stream.Close();

                        using (var fileStream = new FileStream(strUploadFolder + strFile, FileMode.Create, FileAccess.Write))
                        {
                            stream.CopyTo(fileStream);
                            fileStream.Flush();
                            fileStream.Close();
                        }
                        stream.Flush();
                        stream.Close();

                        return true;
                    }
                    catch
                    {
                        redo = true;
                        ++retries;
                    }
                    finally
                    {
                        if (response != null)
                            response.Close();
                    }
                } while (redo && retries < maxRetries);
            }
            catch
            {
            }
            return false;
        }
    }
}