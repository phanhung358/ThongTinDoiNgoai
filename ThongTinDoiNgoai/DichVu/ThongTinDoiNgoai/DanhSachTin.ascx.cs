using FITC.Web.Component;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ThongTinDoiNgoai.DichVu.ThongTinDoiNgoai
{
    public partial class DanhSachTin : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sWebID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["loai"] != null)
            {
                string[] arr = Request.QueryString["loai"].ToLower().Split('-');
                string sLoai = arr[arr.Length - 1];
                if (sLoai.Length > 1)
                {
                    sWebID = sLoai.Substring(1);
                }
            }
            if (!IsPostBack)
            {

            }
            addData();
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
            bool bMobie = false;
            string sLoai = Request.UserAgent.ToLowerInvariant();
            if ((sLoai.Contains("mobile") || sLoai.Contains("blackberry") || sLoai.Contains("iphone")))
            {
                bMobie = true;
            }

            if (bMobie)
                addData_Mobile();
            else
                addData_Web();
        }

        private void addData_Web(){
            if (sWebID != "0")
            {
                StringBuilder str = new StringBuilder();
                //=============================================
                using (DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 1, 0, sWebID))
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

                        string TenWeb = "";
                        DataSet ds1 = db.GetDataSet("TTDN_TRANGWEB_SELECT", 1, sWebID);
                        if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            TenWeb = ds1.Tables[0].Rows[0]["TenWeb"].ToString().ToUpper();
                        }

                        str.AppendFormat("<div class='demuc'>{0}</div>", TenWeb);
                        str.Append("<div class='danhsachtin-nen'>");
                        for (int i = TuBanGhi; i < DenBanGhi; i++)
                        {
                            DataRow row = ds.Tables[0].Rows[i];

                            if (i % 2 == 0)
                                str.Append("<div class='dong-chan'>");
                            else
                                str.Append("<div class='dong-le'>");
                            str.Append("<div class='anhdaidien-trai'>");
                            str.AppendFormat("<img src='{0}' />", string.IsNullOrEmpty(row["AnhDaiDien"].ToString()) ? Static.AppPath() + "/Images/no_image.png" : row["AnhDaiDien"].ToString());
                            str.Append("</div>");
                            str.Append("<div class='noidung-phai'>");
                            str.Append("<div class='dongtieude'>");
                            str.AppendFormat("<div class='tieude'><a href='{1}.html'>{0}</a></div>", row["TieuDe"].ToString(), "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row["TieuDe"].ToString()) + "-b" + row["BaiVietID"].ToString().Trim());
                            str.AppendFormat("<div class='thoigian'>{0}</div>", DateTime.Parse(row["ThoiGian"].ToString()).ToString("dd/MM/yyyy - HH:mm"));

                            str.Append("</div>");
                            str.Append("<div class='dongnoidung'>");
                            str.AppendFormat("<div class='tomtat'>{0}</div>", row["TomTat"].ToString());
                            str.AppendFormat("<div class='xemchitiet'><a href='{0}.html'>Xem chi tiết</a></div>", "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row["TieuDe"].ToString()) + "-b" + row["BaiVietID"].ToString().Trim());
                            str.Append("</div>");
                            str.Append("</div>");
                            str.Append("</div>");
                        }
                        str.Append("</div>");
                    }
                    else
                    {
                        str.Append("<div class='dong-chan'>");
                        str.Append("<div class='rong'>Thông tin chưa được cập nhật</div>");
                        str.Append("</div>");
                    }
                }
                divDanhSach.InnerHtml = str.ToString();
            }
        }

        private void addData_Mobile()
        {
            tblPhanTrang.Visible = false;
            WebID.Value = sWebID;
            LoadMore.Text = "<script type='text/javascript' src='/js/LoadMore.js'></script>";
            if (sWebID != "0")
            {
                StringBuilder str = new StringBuilder();
                //=============================================
                using (DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT_MOBILE", 0, 0, sWebID))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string TenWeb = "";
                        DataSet ds1 = db.GetDataSet("TTDN_TRANGWEB_SELECT", 1, sWebID);
                        if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            TenWeb = ds1.Tables[0].Rows[0]["TenWeb"].ToString().ToUpper();
                        }

                        str.AppendFormat("<div class='demuc'>{0}</div>", TenWeb);
                        str.Append("<div class='danhsachtin-nen' id='DanhSachTin'>");
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow row = ds.Tables[0].Rows[i];

                            str.AppendFormat("<a href='{0}.html'>", "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row["TieuDe"].ToString()) + "-b" + row["BaiVietID"].ToString().Trim());
                            if (i % 2 == 0)
                                str.Append("<div class='dong-chan'>");
                            else
                                str.Append("<div class='dong-le'>");
                            str.Append("<div class='anhdaidien-trai'>");
                            str.AppendFormat("<img src='{0}' />", string.IsNullOrEmpty(row["AnhDaiDien"].ToString()) ? Static.AppPath() + "/Images/no_image.png" : row["AnhDaiDien"].ToString());
                            str.Append("</div>");
                            str.Append("<div class='noidung-phai'>");
                            str.Append("<div class='dongtieude'>");
                            str.AppendFormat("<div class='tieude'>{0}</div>", row["TieuDe"].ToString());
                            str.AppendFormat("<div class='thoigian'>{0}</div>", DateTime.Parse(row["ThoiGian"].ToString()).ToString("dd/MM/yyyy - HH:mm"));
                            str.Append("</div>");
                            str.Append("</div>");
                            str.Append("</div>");
                            str.Append("</a>");
                        }
                        str.Append("</div>");
                    }
                    else
                    {
                        str.Append("<div class='dong-chan'>");
                        str.Append("<div class='rong'>Thông tin chưa được cập nhật</div>");
                        str.Append("</div>");
                    }
                }
                divDanhSach.InnerHtml = str.ToString();
            }
        }

        public string ChuyenTuCoDauSangKoDau(string strUrl)
        {
            string str = strUrl.Trim().ToLower();
            while (str.LastIndexOf("  ") > 0)
                str = str.Replace("  ", "");
            return str.Replace(" ", "-").Replace("~", "").Replace("`", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "-").Replace("=", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(",", "").Replace(">", "").Replace("<", "").Replace("'", "").Replace("đ", "d").Replace("á", "a").Replace("à", "a").Replace("ạ", "a").Replace("ả", "a").Replace("ã", "a").Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ặ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ậ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e").Replace("e", "e").Replace("é", "e").Replace("è", "e").Replace("ẹ", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("i", "i").Replace("í", "i").Replace("ì", "i").Replace("ị", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("o", "o").Replace("ó", "o").Replace("ò", "o").Replace("ọ", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ộ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ợ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("u", "u").Replace("ú", "u").Replace("ù", "u").Replace("ụ", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ự", "u").Replace("ử", "u").Replace("ữ", "u").Replace("y", "y").Replace("ý", "y").Replace("ỳ", "y").Replace("ỵ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("/", "-").Replace("?", "-").Replace("\'", "").Replace("\"", "").Replace(":", "-").Replace(";", "-").Replace("--", "-");
        }
    }
}