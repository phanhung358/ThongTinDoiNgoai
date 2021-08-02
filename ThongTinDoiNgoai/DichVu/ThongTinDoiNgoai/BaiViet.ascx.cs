using FITC.Web.Component;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
using System.Threading;
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
            drpWeb.Items.Clear();
            drpWeb.Items.Add(new ListItem("[Tất cả]", "0"));
            DataSet dsWeb = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0, 0, "", drpNhom.SelectedValue.ToString());
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
            imgThemMoi.Attributes["onclick"] = string.Format("return thickboxPopup('Tạo tin mới', '{0}?control={1}&btn={2}','900','700');", Static.AppPath() + "/home/popup.aspx", "/DichVu/ThongTinDoiNgoai/BaiViet_Tm.ascx", btnSuKien.ClientID);

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
            using (DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 1, 0, drpWeb.SelectedValue.ToString(), drpChuyenMuc.SelectedValue.ToString(), drpNhom.SelectedValue.ToString()))
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
                DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 0, drpWeb.SelectedValue, 0, drpNhom.SelectedValue.ToString());
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
                string regex = @"((?<ngay>[0|1|2]?[0-9]|3[01])[/\-\.](?<thang>0?[1-9]|1[012])[/\-\.](?<nam>[1-9][0-9][0-9][0-9])[\s]?(?<gio>[0|1]?[0-9]|2[0-4])?[:]?(?<phut>[0-5][0-9])?[:]?(?<giay>[0-5][0-9])?[\s]?(?<buoi>am|pm|sa|ch)?)|((?<gio1>[0|1]?[0-9]|2[0-4])[:]?(?<phut1>[0-5][0-9])[:]?(?<giay1>[0-5][0-9])[\s]?(?<buoi1>am|pm|sa|ch)?[\s]?(?<ngay1>[0|1|2]?[0-9]|3[01])[/\-\.](?<thang1>0?[1-9]|1[012])[/\-\.](?<nam1>[1-9][0-9][0-9][0-9]))";
                MatchCollection matchCollection = Regex.Matches(inputText.ToLower(), regex);
                if (matchCollection.Count > 0)
                {
                    string ngayThang = matchCollection[0].Value; // 07/9/2021 14:00:00
                }
                CacHamChung ham = new CacHamChung();
                foreach (Match match in matchCollection)
                {
                    if (match.Groups["thang"].Value != "")
                    {
                        kq = kq + match.Groups["thang"].Value;
                        kq = kq + "/" + match.Groups["ngay"].Value;
                        kq = kq + "/" + match.Groups["nam"].Value;
                        kq = kq + " " + (match.Groups["gio"].Value == "" ? "00" : match.Groups["gio"].Value);
                        kq = kq + ":" + (match.Groups["phut"].Value == "" ? "00" : match.Groups["phut"].Value);
                        kq = kq + ":" + (match.Groups["giay"].Value == "" ? "00" : match.Groups["giay"].Value);
                        kq = kq + " " + match.Groups["buoi"].Value;
                    }
                    else
                    {
                        kq = kq + match.Groups["thang1"].Value;
                        kq = kq + "/" + match.Groups["ngay1"].Value;
                        kq = kq + "/" + match.Groups["nam1"].Value;
                        kq = kq + " " + (match.Groups["gio1"].Value == "" ? "00" : match.Groups["gio"].Value);
                        kq = kq + ":" + (match.Groups["phut1"].Value == "" ? "00" : match.Groups["phut"].Value);
                        kq = kq + ":" + (match.Groups["giay1"].Value == "" ? "00" : match.Groups["giay"].Value);
                        kq = kq + " " + match.Groups["buoi1"].Value;
                    }
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

                    string filename = "";
                    if (url.ToLower().Contains(".jpg") || url.ToLower().Contains(".jpeg") || url.ToLower().Contains(".png") || url.ToLower().Contains(".gif") || url.ToLower().Contains(".tiff") || url.ToLower().Contains(".pdf"))
                    {
                        filename = Path.GetFileName(new Uri(url).AbsolutePath).Replace("%20", "_").Replace(" ", "_");
                    }
                    else
                    {
                        filename = url.Substring(url.LastIndexOf("/") + 1).Replace("%20", "_").Replace(" ", "_");
                    }

                    if (filename.Contains("?"))
                    {
                        filename = filename.Replace(filename.Substring(filename.IndexOf("?"), filename.IndexOf("=") - filename.IndexOf("?") + 1), "_");
                        if (filename.Contains("&"))
                            filename = filename.Replace(filename.Substring(filename.IndexOf("&"), filename.IndexOf("=") - filename.IndexOf("&") + 1), "_");
                    }
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

        protected void drpNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            addDanhMuc();
            addData();
        }
    }
}