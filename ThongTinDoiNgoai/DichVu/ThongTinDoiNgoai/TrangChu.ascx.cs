using FITC.Web.Component;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ThongTinDoiNgoai.DichVu.ThongTinDoiNgoai
{
    public partial class TrangChu : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
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
            if (Request.QueryString["id"] == null)
            {
                drpSoTin1.Visible = false;
                StringBuilder str = new StringBuilder();
                str.Append("<div class='ttdn-nen'>");
                //=============================================
                using (DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 1))
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

                            if (i % 2 == 0)
                                str.Append("<div class='ttdn-dong-chan'>");
                            else
                                str.Append("<div class='ttdn-dong-le'>");
                            str.Append("<div class='ttdn-anhdaidien-trai'>");
                            str.AppendFormat("<img src='{0}' />", string.IsNullOrEmpty(row["AnhDaiDien"].ToString()) ? Static.AppPath() + "/Images/no_image.png" : row["AnhDaiDien"].ToString());
                            str.Append("</div>");
                            str.Append("<div class='ttdn-noidung-phai'>");
                            str.Append("<div class='ttdn-dongtieude'>");
                            str.AppendFormat("<div class='ttdn-tieude'>{0}</div>", row["TieuDe"].ToString());
                            str.AppendFormat("<div class='ttdn-thoigian'>{0}</div>", DateTime.Parse(row["ThoiGian"].ToString()).ToString("dd/MM/yyyy - HH:mm"));
                           
                            str.Append("</div>");
                            str.Append("<div class='ttdn-dongnoidung'>");
                            str.AppendFormat("<div class='ttdn-tomtat'>{0}</div>", row["TomTat"].ToString());
                            str.AppendFormat("<div class='ttdn-xemchitiet'><a href='/?id={0}'>Xem chi tiết</a></div>", row["BaiVietID"].ToString());
                            str.Append("</div>");
                            str.Append("</div>");
                            str.Append("</div>");
                        }
                    }
                    else
                    {
                        str.Append("<div class='ttdn-dong-chan'>");
                        str.Append("<div class='ttdn-rong'>Thông tin chưa được cập nhật</div>");
                        str.Append("</div>");
                    }
                }
                str.Append("</div>");
                divDanhSach.InnerHtml = str.ToString();
            }
            else
                addData_ChiTiet();
        }

        private void addData_ChiTiet()
        {
            drpSoTin.Visible = false;
            string id = Request.QueryString["id"];
            string ChuyenMucID = "0";

            StringBuilder str = new StringBuilder();
            str.Append("<div class='ttdn-chitiet'>");
            DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 0, id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                ChuyenMucID = row["ChuyenMucID"].ToString();
                str.AppendFormat("<h2 class='ttdn-chitiet-tieude'>{0}</h2>", row["TieuDe"].ToString());
                if (!string.IsNullOrEmpty(row["ThoiGian"].ToString()))
                    str.AppendFormat("<div class='ttdn-chitiet-thoigian'><span>{0}</span></div>", DateTime.Parse(row["ThoiGian"].ToString()).ToString("dd/MM/yyyy - HH:mm"));
                if (!string.IsNullOrEmpty(row["TomTat"].ToString()))
                    str.AppendFormat("<div class='ttdn-chitiet-tomtat'>{0}</div>", row["TomTat"].ToString());

                string DiaChiWeb = "";
                DataSet ds1 = db.GetDataSet("TTDN_TRANGWEB_SELECT", 1, row["WebID"].ToString());
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow rowWeb = ds1.Tables[0].Rows[0];
                    DiaChiWeb = rowWeb["DiaChiWeb"].ToString();
                }
                HtmlDocument NoiDung = new HtmlDocument();
                NoiDung.LoadHtml(row["NoiDung"].ToString());
                try
                {
                    if (NoiDung != null)
                    {
                        var dsFile = NoiDung.DocumentNode.SelectNodes(".//img");
                        if (dsFile != null)
                        {
                            foreach (var file in dsFile)
                            {
                                string strSource = HttpUtility.UrlDecode(file.Attributes["src"].Value, Encoding.UTF8);
                                string img = file.OuterHtml;

                                if (File.Exists(Server.MapPath(strSource)))
                                {
                                    System.Drawing.Image image = LoadImage(Server.MapPath(strSource));

                                    if (file.Attributes["style"] != null)
                                    {
                                        string[] st = file.Attributes["style"].Value.Split(';');
                                        string newstyle = "";
                                        int c = 0;
                                        foreach (var css in st)
                                        {
                                            if (css.Contains("width") && !css.Contains("max-width"))
                                            {
                                                if (Convert.ToInt32(new string(css.Where(x => char.IsDigit(x)).ToArray())) > 920)
                                                    newstyle += "width: 100%; ";
                                                else
                                                    newstyle += css.Trim() + "; ";
                                            }
                                            else
                                            {

                                                if (css.Contains("height"))
                                                {
                                                    newstyle += "height: auto; ";
                                                    c++;
                                                }
                                                else
                                                {
                                                    newstyle += css.Trim() == "" ? "" : css.Trim() + "; ";
                                                    c++;
                                                }
                                            }  
                                        }
                                        if (c == st.Length)
                                            newstyle += "width: 100%; ";
                                        img = img.Replace(file.Attributes["style"].Value, newstyle);
                                    }
                                    else
                                    {
                                        if (file.Attributes["width"] != null && !string.IsNullOrEmpty(file.Attributes["width"].Value))
                                        {
                                            if (Convert.ToInt32(file.Attributes["width"].Value) > 920)
                                                img = img.Replace(string.Format("width=\"{0}\"", file.Attributes["width"].Value), "width=\"100%\"");
                                        }
                                        else if (image.Width > 920)
                                            img = img.Replace(">", " width=\"100%\">");
                                        if (file.Attributes["height"] != null && !string.IsNullOrEmpty(file.Attributes["height"].Value))
                                            img = img.Replace(string.Format("height=\"{0}\"", file.Attributes["height"].Value), "height=\"auto\"");
                                    }
                                }

                                NoiDung.DocumentNode.InnerHtml = NoiDung.DocumentNode.InnerHtml.Replace(file.OuterHtml, img);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                str.AppendFormat("<div class='ttdn-chitiet-noidung'>{0}</div>", NoiDung.DocumentNode.InnerHtml);
                str.AppendFormat("<div class='ttdn-chitiet-tacgia'><p>{0}</p></div>", row["TacGia"].ToString());
            }
            str.Append("</div>");
            str.Append("<div class='ttdn-tinkhac'>");
            DataSet dsKhac = db.GetDataSet("TTDN_BAIVIET_SELECT", 2, id, 0, ChuyenMucID);
            if (dsKhac != null && dsKhac.Tables.Count > 0 && dsKhac.Tables[0].Rows.Count > 0)
            {
                //Phân trang===============================================
                int iSoTin = int.Parse(drpSoTin1.SelectedValue) * (Static.PhanTrangThu - 1);
                if (dsKhac.Tables[0].Rows.Count <= iSoTin && Static.PhanTrangThu != 1)
                    Static.PhanTrangThu = Static.PhanTrangThu - 1;

                int TrangHienTai = Static.PhanTrangThu;
                int TongSoTin = dsKhac.Tables[0].Rows.Count;
                int SoTinTrenTrang = Convert.ToInt32(drpSoTin1.SelectedValue);
                PhanTrang(TongSoTin, SoTinTrenTrang, TrangHienTai, lblPhanTrang);
                int TuBanGhi = (TrangHienTai - 1) * SoTinTrenTrang;
                int DenBanGhi = (TrangHienTai * SoTinTrenTrang) > TongSoTin ? TongSoTin : TrangHienTai * SoTinTrenTrang;
                if (TongSoTin > SoTinTrenTrang)
                    tblPhanTrang.Visible = true;
                //End phân trang==========================================

                str.Append("<div class='ttdn-tinkhac-demuc'>Tin khác:</div>");
                str.Append("<ul class='danhsach'>");
                for (int i = TuBanGhi; i < DenBanGhi; i++)
                {
                    DataRow row = dsKhac.Tables[0].Rows[i];

                    str.AppendFormat("<li class='tin'><a href='/?id={0}'>{1}</a></li>", row["BaiVietID"].ToString(), row["TieuDe"].ToString());
                }
                str.Append("</ul>");
            }
            str.Append("</div>");

            divDanhSach.InnerHtml = str.ToString();
        }

        public byte[] ReadAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            return buffer;
        }

        public System.Drawing.Image LoadImage(string url)
        {
            byte[] bytes = ReadAllBytes(url);

            System.Drawing.Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = System.Drawing.Image.FromStream(ms);
            }

            return image;
        }
    }
}