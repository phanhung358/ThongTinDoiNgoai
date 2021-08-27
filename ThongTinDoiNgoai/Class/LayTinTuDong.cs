using FITC.Web.Component;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace ThongTinDoiNgoai
{
    public class LayTinTuDong
    {
        public LayTinTuDong()
        { }
        public void ThucHien(string webID)
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8
            };
            FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
            try
            {
                string ThuMuc = ConfigurationManager.AppSettings["ThuMuc"].Replace("\\", "/") + "/ChromeDriver";
                ChromeDriverService DeviceDriver = ChromeDriverService.CreateDefaultService(ThuMuc);
                ChromeOptions options = new ChromeOptions() { Proxy = null };
                options.PageLoadStrategy = PageLoadStrategy.Normal;
                options.AddArgument("--headless");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--ignore-certificate-errors");
                ChromeDriver driver = new ChromeDriver(DeviceDriver, options, TimeSpan.FromMinutes(1));
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState == \"complete\" && window.jQuery != \"undefined\" && jQuery.active == 0"));

                string[] dsTag = { "<TABLE", "<TR", "<TD", "<DIV", "<A", "<P", "<SPAN", "<STRONG", "<EM", "<TITLE", "<SCRIPT", "</TABLE>", "</TR>", "</TD>", "</DIV>", "</A>", "</P>", "</SPAN>", "</STRONG>", "</EM>", "</TITLE>", "</SCRIPT>" };
                DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 3, webID);
                if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
                {
                    string CheDoDacBiet = "False";
                    for (int j = 0; j < dsChuyenMuc.Tables[0].Rows.Count; j++)
                    {
                        List<object[]> dsTin = new List<object[]>();
                        DataRow rowCM = dsChuyenMuc.Tables[0].Rows[j];
                        try
                        {
                            DataSet dsXpathCM = db.GetDataSet("TTDN_XPATH_CHUYENMUC_SELECT", 0, rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString());
                            if (dsXpathCM != null && dsXpathCM.Tables.Count > 0 && dsXpathCM.Tables[0].Rows.Count > 0)
                            {
                                DataRow rowXpathCM = dsXpathCM.Tables[0].Rows[0];
                                CheDoDacBiet = rowXpathCM["CheDoDacBiet"].ToString();
                                HtmlDocument html = new HtmlDocument();
                                if (CheDoDacBiet == "False")
                                {
                                    driver.Navigate().GoToUrl(rowCM["UrlChuyenMuc"].ToString());
                                    html.LoadHtml(driver.PageSource);
                                }
                                else
                                {
                                    html = htmlWeb.Load(rowCM["UrlChuyenMuc"].ToString());
                                }

                                if (html == null)
                                {
                                    string Loi = "Không lấy được dữ liệu của chuyên mục!";
                                    string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), Loi, rowCM["UrlChuyenMuc"].ToString());
                                    continue;
                                }
                                foreach (var tag in dsTag)
                                {
                                    html.DocumentNode.InnerHtml = html.DocumentNode.InnerHtml.Replace(tag, tag.ToLower());
                                }
                                html.DocumentNode.InnerHtml = html.DocumentNode.InnerHtml.Replace("<TBODY>", "").Replace("</TBODY>", "").Replace("<tbody>", "").Replace("</tbody>", "");

                                string xds = rowXpathCM["DanhSach"].ToString().Replace("tbody/", "");
                                string XDanhSach = xds.LastIndexOf(']') == xds.Length - 1 ? xds.Remove(xds.LastIndexOf('['), xds.Length - xds.LastIndexOf('[')) : xds;
                                string xbv = rowXpathCM["BaiViet_Url"].ToString().Replace("tbody/", "").Replace(XDanhSach, ".");
                                string xbv1 = rowXpathCM["BaiViet_Url1"].ToString().Replace("tbody/", "").Replace(XDanhSach, ".");
                                string xbv2 = rowXpathCM["BaiViet_Url2"].ToString().Replace("tbody/", "").Replace(XDanhSach, ".");
                                string xadd = rowXpathCM["AnhDaiDien"].ToString().Replace("tbody/", "").Replace(XDanhSach, ".");
                                string xtg = rowXpathCM["ThoiGian"].ToString().Replace("tbody/", "").Replace(XDanhSach, ".");
                                string XBaiViet_Url = xbv.IndexOf('[') == 1 ? xbv.Remove(1, xbv.IndexOf(']')) : xbv;
                                string XBaiViet_Url1 = xbv1.IndexOf('[') == 1 ? xbv1.Remove(1, xbv1.IndexOf(']')) : xbv1;
                                string XBaiViet_Url2 = xbv2.IndexOf('[') == 1 ? xbv2.Remove(1, xbv2.IndexOf(']')) : xbv2;
                                string XAnhDaiDien = xadd.IndexOf('[') == 1 ? xadd.Remove(1, xadd.IndexOf(']')) : xadd;
                                string XThoiGian = xtg.IndexOf('[') == 1 ? xtg.Remove(1, xtg.IndexOf(']')) : xtg;

                                var DanhSach = html.DocumentNode.SelectNodes(XDanhSach) != null ? html.DocumentNode.SelectNodes(XDanhSach).ToList() : null;
                                if (DanhSach != null)
                                {
                                    int count = 0;
                                    foreach (var item in DanhSach)
                                    {
                                        string BaiViet_Url = null;
                                        string TieuDe = null;
                                        string AnhDaiDien = "";
                                        string ThoiGian = "";
                                        if (!string.IsNullOrEmpty(XBaiViet_Url) && item.SelectSingleNode(XBaiViet_Url) != null && !string.IsNullOrEmpty(item.SelectSingleNode(XBaiViet_Url).InnerText.Replace("&nbsp;", " ").Trim()))
                                        {
                                            BaiViet_Url = item.SelectSingleNode(XBaiViet_Url).Attributes["href"].Value.Replace("&amp;", "&").Replace("&#x3a;", ":").Replace("&#x2f;", "/").Replace("&#x2e;", ".");
                                            TieuDe = item.SelectSingleNode(XBaiViet_Url).InnerText.Replace("&nbsp;", " ").Trim();
                                        }
                                        else if (!string.IsNullOrEmpty(XBaiViet_Url1) && item.SelectSingleNode(XBaiViet_Url1) != null && !string.IsNullOrEmpty(item.SelectSingleNode(XBaiViet_Url1).InnerText.Replace("&nbsp;", " ").Trim()))
                                        {
                                            BaiViet_Url = item.SelectSingleNode(XBaiViet_Url1).Attributes["href"].Value.Replace("&amp;", "&").Replace("&#x3a;", ":").Replace("&#x2f;", "/").Replace("&#x2e;", ".");
                                            TieuDe = item.SelectSingleNode(XBaiViet_Url1).InnerText.Replace("&nbsp;", " ").Trim();
                                        }
                                        else if (!string.IsNullOrEmpty(XBaiViet_Url2) && item.SelectSingleNode(XBaiViet_Url2) != null && !string.IsNullOrEmpty(item.SelectSingleNode(XBaiViet_Url2).InnerText.Replace("&nbsp;", " ").Trim()))
                                        {
                                            BaiViet_Url = item.SelectSingleNode(XBaiViet_Url2).Attributes["href"].Value.Replace("&amp;", "&").Replace("&#x3a;", ":").Replace("&#x2f;", "/").Replace("&#x2e;", ".");
                                            TieuDe = item.SelectSingleNode(XBaiViet_Url2).InnerText.Replace("&nbsp;", " ").Trim();
                                        }
                                        if (!string.IsNullOrEmpty(XAnhDaiDien) && item.SelectSingleNode(XAnhDaiDien) != null)
                                        {
                                            HtmlNode anh = item.SelectSingleNode(XAnhDaiDien);
                                            string strSource = anh.Attributes["url-img-full"] == null ? (anh.Attributes["data-src"] == null ? anh.Attributes["src"].Value : anh.Attributes["data-src"].Value) : anh.Attributes["url-img-full"].Value;
                                            if (!strSource.Contains(rowCM["DiaChiWeb"].ToString()))
                                            {
                                                if (strSource.IndexOf("//") == 0)
                                                    strSource = new Uri(rowCM["DiaChiWeb"].ToString()).Scheme + ":" + strSource;
                                                else if (strSource.IndexOf("/") == 0)
                                                    strSource = rowCM["DiaChiWeb"].ToString() + strSource;
                                                else if (strSource.IndexOf("../") == 0)
                                                    strSource = rowCM["DiaChiWeb"].ToString() + "/" + strSource.Replace("../", "");
                                                else if (strSource.IndexOf("~/") == 0)
                                                    strSource = rowCM["DiaChiWeb"].ToString() + strSource.Replace("~/", "/");
                                                else if (strSource.IndexOf("./") == 0)
                                                    strSource = rowCM["DiaChiWeb"].ToString() + strSource.Replace("./", "/");
                                            }
                                            strSource = strSource.Replace("&amp;", "&").Replace("&#x3a;", ":").Replace("&#x2f;", "/").Replace("&#x2e;", ".");
                                            if (!string.IsNullOrEmpty(strSource))
                                            {
                                                string DirUpload = ConfigurationManager.AppSettings["ThuMuc"].Replace("\\", "/") + "/UploadFiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + rowCM["DiaChiWeb"].ToString().Remove(0, rowCM["DiaChiWeb"].ToString().IndexOf("/") + 2) + "/";
                                                string fileName = "";
                                                if (strSource.ToLower().Contains(".jpg") || strSource.ToLower().Contains(".jpeg") || strSource.ToLower().Contains(".png") || strSource.ToLower().Contains(".gif") || strSource.ToLower().Contains(".tiff") || strSource.ToLower().Contains(".pdf"))
                                                {
                                                    fileName = Path.GetFileName(new Uri(HttpUtility.UrlDecode(strSource, Encoding.UTF8)).AbsolutePath);
                                                }
                                                else
                                                {
                                                    fileName = HttpUtility.UrlDecode(strSource, Encoding.UTF8).Substring(strSource.LastIndexOf("/") + 1);
                                                }

                                                string err = DownloadFile(HttpUtility.UrlDecode(strSource, Encoding.UTF8), DirUpload, fileName);
                                                if (err != "")
                                                {
                                                    string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCM["WebID"].ToString(), rowXpathCM["ChuyenMucID"].ToString(), err, strSource);
                                                }
                                                else
                                                {
                                                    AnhDaiDien = DirUpload.Substring(DirUpload.IndexOf("/UploadFiles/")) + ChuyenTuCoDauSangKoDau(HttpUtility.UrlDecode(fileName, Encoding.UTF8));
                                                }
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(XThoiGian) && item.SelectSingleNode(XThoiGian) != null)
                                        {
                                            ThoiGian = LayNgay(item.SelectSingleNode(XThoiGian).InnerText.Trim());
                                        }

                                        if (BaiViet_Url != null && !string.IsNullOrEmpty(TieuDe))
                                        {
                                            string DiaChiWeb = rowCM["DiaChiWeb"].ToString();
                                            if (BaiViet_Url.Contains("http"))
                                            {
                                                if (DiaChiWeb.Substring(0, 5) == "https" && BaiViet_Url.Substring(0, 5) != "https")
                                                    BaiViet_Url = BaiViet_Url.Replace("http", "https");
                                                else if (DiaChiWeb.Substring(0, 5) != "https" && BaiViet_Url.Substring(0, 5) == "https")
                                                    BaiViet_Url = BaiViet_Url.Replace("https", "http");
                                                if (!DiaChiWeb.Contains("www") && BaiViet_Url.Contains("www"))
                                                    BaiViet_Url = BaiViet_Url.Replace("www.", "");
                                                else if (DiaChiWeb.Contains("www") && !BaiViet_Url.Contains("www"))
                                                    BaiViet_Url = BaiViet_Url.Replace("://", "://www.");
                                                if (!BaiViet_Url.Contains(DiaChiWeb))
                                                    continue;
                                            }
                                            else if (BaiViet_Url.LastIndexOf(DiaChiWeb) == -1)
                                            {
                                                if (BaiViet_Url.IndexOf("/") == 0)
                                                    BaiViet_Url = DiaChiWeb + BaiViet_Url;
                                                else if (BaiViet_Url.IndexOf("../") == 0)
                                                    BaiViet_Url = DiaChiWeb + "/" + BaiViet_Url.Replace("../", "");
                                                else if (BaiViet_Url.IndexOf("~/") == 0)
                                                    BaiViet_Url = DiaChiWeb + BaiViet_Url.Replace("~/", "/");
                                                else if (BaiViet_Url.IndexOf("./") == 0)
                                                    BaiViet_Url = DiaChiWeb + BaiViet_Url.Replace("./", "/");
                                                else
                                                    BaiViet_Url = DiaChiWeb + "/" + BaiViet_Url;
                                            }

                                            object[] obj = new object[6];
                                            obj[0] = HttpUtility.UrlDecode(BaiViet_Url);
                                            obj[1] = rowXpathCM["ChuyenMucID"].ToString();
                                            obj[2] = rowXpathCM["WebID"].ToString();
                                            obj[3] = TieuDe;
                                            obj[4] = AnhDaiDien;
                                            obj[5] = ThoiGian;
                                            dsTin.Add(obj);
                                        }
                                        else
                                        {
                                            count++;
                                        }
                                    }
                                    if (count == DanhSach.Count)
                                    {
                                        string Loi = "Không lấy được đường dẫn (URL) của bài viết trong chuyên mục!";
                                        string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), Loi, rowCM["UrlChuyenMuc"].ToString());
                                        continue;
                                    }
                                }
                                else
                                {
                                    string Loi = "Không lấy được danh sách tin của chuyên mục!";
                                    string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), Loi, rowCM["UrlChuyenMuc"].ToString());
                                    continue;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), ex.Message, rowCM["UrlChuyenMuc"].ToString());
                        }

                        int count1 = 0;
                        DataSet dsXpathCT = db.GetDataSet("TTDN_XPATH_CHITIET_SELECT", 0, rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString());
                        foreach (var item in dsTin)
                        {
                            try
                            {
                                HtmlDocument html = new HtmlDocument();
                                if (CheDoDacBiet == "False")
                                {
                                    driver.Navigate().GoToUrl(item[0].ToString());
                                    html.LoadHtml(driver.PageSource);
                                }
                                else
                                {
                                    html = htmlWeb.Load(item[0].ToString());
                                }

                                if (html == null)
                                {
                                    string Loi = "Không lấy được chi tiết bài viết!";
                                    string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), Loi, item[0].ToString());
                                    continue;
                                }
                                foreach (var tag in dsTag)
                                {
                                    html.DocumentNode.InnerHtml = html.DocumentNode.InnerHtml.Replace(tag, tag.ToLower());
                                }
                                html.DocumentNode.InnerHtml = html.DocumentNode.InnerHtml.Replace("<TBODY>", "").Replace("</TBODY>", "").Replace("<tbody>", "").Replace("</tbody>", "");

                                if (dsXpathCT != null && dsXpathCT.Tables.Count > 0 && dsXpathCT.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsXpathCT.Tables[0].Rows.Count; i++)
                                    {
                                        DataRow rowXpathCT = dsXpathCT.Tables[0].Rows[i];

                                        string XTomTat = rowXpathCT["TomTat"].ToString().Replace("tbody/", "");
                                        string XNoiDung = rowXpathCT["NoiDung"].ToString().Replace("tbody/", "");
                                        string XThoiGian = rowXpathCT["ThoiGian"].ToString().Replace("tbody/", "");
                                        string XTacGia = rowXpathCT["TacGia"].ToString().Replace("tbody/", "");
                                        string XTieuDe = rowXpathCT["TieuDe"].ToString().Replace("tbody/", "");

                                        var TomTat = XTomTat != "" ? (html.DocumentNode.SelectSingleNode(XTomTat) != null ? html.DocumentNode.SelectSingleNode(XTomTat) : null) : null;
                                        var NoiDung = html.DocumentNode.SelectSingleNode(XNoiDung) != null ? html.DocumentNode.SelectSingleNode(XNoiDung) : null;
                                        var ThoiGian = XThoiGian != "" ? (html.DocumentNode.SelectSingleNode(XThoiGian) != null ? html.DocumentNode.SelectSingleNode(XThoiGian) : null) : null;
                                        var TacGia = XTacGia != "" ? (html.DocumentNode.SelectSingleNode(XTacGia) != null ? html.DocumentNode.SelectSingleNode(XTacGia) : null) : null;
                                        var TieuDe = XTieuDe != "" ? (html.DocumentNode.SelectSingleNode(XTieuDe) != null ? html.DocumentNode.SelectSingleNode(XTieuDe) : null) : null;


                                        string tgian = null;
                                        string strNewsDatePosted = null;
                                        if (ThoiGian != null)
                                        {
                                            strNewsDatePosted = ThoiGian.InnerText.Trim();
                                            tgian = LayNgay(strNewsDatePosted);
                                        }

                                        string scheck = null;
                                        Regex regex = new Regex(@"^(0?[1-9]|1[012])[\/](0?[1-9]|[12]\d|3[01])[\/]([1-9]\d\d\d)[\s](2[0-4]|[01]?\d[:][0-5]\d[:][0-5]\d)[\s]?(am|pm)?");
                                        if (!string.IsNullOrEmpty(tgian))
                                            scheck = regex.Replace(tgian, "");
                                        
                                        if (NoiDung != null && !string.IsNullOrEmpty(NoiDung.InnerHtml) && (!string.IsNullOrEmpty(item[5].ToString()) || scheck == ""))
                                        {
                                            if (rowCM["DiaChiWeb"].ToString() == "https://vietnam.vn" && NoiDung.InnerHtml.IndexOf("<div class=\"clean\"></div>") != -1)
                                                NoiDung.InnerHtml = NoiDung.InnerHtml.Remove(NoiDung.InnerHtml.IndexOf("<div class=\"clean\"></div>"));

                                            string DirUpload = ConfigurationManager.AppSettings["ThuMuc"].Replace("\\", "/") + "/UploadFiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + rowCM["DiaChiWeb"].ToString().Remove(0, rowCM["DiaChiWeb"].ToString().IndexOf("/") + 2) + "/";
                                            var dsFile = NoiDung.SelectNodes(".//img");
                                            if (dsFile != null)
                                            {
                                                foreach (var file in dsFile)
                                                {
                                                    string strSource = file.Attributes["url-img-full"] == null ? (file.Attributes["data-src"] == null ? (file.Attributes["src"] == null ? "" : file.Attributes["src"].Value) : file.Attributes["data-src"].Value) : file.Attributes["url-img-full"].Value;
                                                    if (!strSource.Contains(rowCM["DiaChiWeb"].ToString()))
                                                    {
                                                        if (strSource.IndexOf("//") == 0)
                                                            strSource = new Uri(rowCM["DiaChiWeb"].ToString()).Scheme + ":" + strSource;
                                                        else if (strSource.IndexOf("/") == 0)
                                                            strSource = rowCM["DiaChiWeb"].ToString() + strSource;
                                                        else if (strSource.IndexOf("../") == 0)
                                                            strSource = rowCM["DiaChiWeb"].ToString() + "/" + strSource.Replace("../", "");
                                                        else if (strSource.IndexOf("~/") == 0)
                                                            strSource = rowCM["DiaChiWeb"].ToString() + strSource.Replace("~/", "/");
                                                        else if (strSource.IndexOf("./") == 0)
                                                            strSource = rowCM["DiaChiWeb"].ToString() + strSource.Replace("./", "/");
                                                    }
                                                    strSource = strSource.Replace("&amp;", "&").Replace("&#x3a;", ":").Replace("&#x2f;", "/").Replace("&#x2e;", ".");
                                                    if (!string.IsNullOrEmpty(strSource))
                                                    {
                                                        string fileName = "";
                                                        if (strSource.Contains("data:image/png;base64"))
                                                        {
                                                            var anh = Base64ToImage(strSource.Replace("data:image/png;base64,", ""));
                                                            fileName = ChuyenTuCoDauSangKoDau(HttpUtility.UrlDecode(item[3].ToString() + (dsFile.IndexOf(file) + 1).ToString() + ".png", Encoding.UTF8));
                                                            
                                                            //if (File.Exists(DirUpload + fileName))
                                                            //    goto ThayDoiDuongDan;

                                                            try
                                                            {
                                                                using (var m = new MemoryStream())
                                                                {
                                                                    anh.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                                                                    var png = System.Drawing.Image.FromStream(m);
                                                                    png.Save(DirUpload + fileName);
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", item[2].ToString(), item[1].ToString(), ex.Message, strSource);
                                                                goto TiepTuc;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (strSource.ToLower().Contains(".jpg") || strSource.ToLower().Contains(".jpeg") || strSource.ToLower().Contains(".png") || strSource.ToLower().Contains(".gif") || strSource.ToLower().Contains(".tiff") || strSource.ToLower().Contains(".pdf"))
                                                            {
                                                                fileName = Path.GetFileName(new Uri(HttpUtility.UrlDecode(strSource, Encoding.UTF8)).AbsolutePath);
                                                            }
                                                            else
                                                            {
                                                                fileName = HttpUtility.UrlDecode(strSource, Encoding.UTF8).Substring(strSource.LastIndexOf("/") + 1);
                                                            }

                                                            //if (File.Exists(DirUpload + fileName))
                                                            //    goto ThayDoiDuongDan;

                                                            string err = DownloadFile(HttpUtility.UrlDecode(strSource, Encoding.UTF8), DirUpload, fileName);
                                                            if (err != "")
                                                            {
                                                                string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", item[2].ToString(), item[1].ToString(), err, strSource);
                                                                goto TiepTuc;
                                                            }
                                                        }

                                                        //ThayDoiDuongDan:;
                                                        string img = file.OuterHtml;
                                                        if (file.Attributes["src"] != null)
                                                        {
                                                            img = img.Replace(file.Attributes["src"].Value, DirUpload.Substring(DirUpload.IndexOf("/UploadFiles/")) + ChuyenTuCoDauSangKoDau(HttpUtility.UrlDecode(fileName, Encoding.UTF8)));
                                                        }
                                                        else
                                                        {
                                                            img = img.Replace(">", " src='" + DirUpload.Substring(DirUpload.IndexOf("/UploadFiles/")) + ChuyenTuCoDauSangKoDau(HttpUtility.UrlDecode(fileName, Encoding.UTF8)) + "'>");
                                                        }
                                                        NoiDung.InnerHtml = NoiDung.InnerHtml.Replace(file.OuterHtml, img);
                                                        TiepTuc:;
                                                    }
                                                }
                                            }

                                            string sTieuDe = item[3].ToString();
                                            if (TieuDe != null && !string.IsNullOrEmpty(TieuDe.InnerText) && !TieuDe.InnerText.Trim().ToLower().Equals(item[3].ToString().Trim().ToLower()))
                                                sTieuDe = TieuDe.InnerText.Trim();

                                            object[] obj = new object[10];   
                                            obj[0] = sTieuDe;
                                            obj[1] = TomTat != null ? TomTat.InnerText : null;
                                            obj[2] = NoiDung != null ? NoiDung.InnerHtml : null;
                                            obj[3] = !string.IsNullOrEmpty(item[5].ToString()) ? item[5].ToString() : (ThoiGian != null ? tgian : null);
                                            obj[4] = TacGia != null ? TacGia.InnerText : null;
                                            obj[5] = item[0].ToString();
                                            obj[6] = NoiDung?.InnerHtml.Length;
                                            obj[7] = item[1].ToString();
                                            obj[8] = item[2].ToString();
                                            obj[9] = item[4].ToString();

                                            string sLoi = db.ExcuteSP("TTDN_BAIVIET_INSERT", obj);
                                            if (sLoi != "")
                                            {
                                                string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", item[2].ToString(), item[1].ToString(), "Lỗi lưu dữ liệu: " + sLoi, item[0].ToString());
                                            }
                                        }
                                        else
                                        {
                                            count1++;
                                        }
                                    }
                                }
                                db.ExcuteSP("TTDN_CHUYENMUC_UPDATE_THOIGIANDONGBO", rowCM["ChuyenMucID"].ToString());
                            }
                            catch (Exception ex)
                            {
                                string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", item[2].ToString(), item[1].ToString(), ex.Message, item[0].ToString());
                            }
                        }
                        
                        if (count1 == dsTin.Count * dsXpathCT.Tables[0].Rows.Count && dsTin.Count != 0)
                        {
                            string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), "Không lấy được thông tin của từng bài viết!", rowCM["UrlChuyenMuc"].ToString());
                        }
                    }
                }
                driver.Quit();
            }
            catch(Exception ex)
            {
                string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", 0, 0, ex.Message, "");
            }
        }

        public System.Drawing.Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }

        public string LayNgay(string inputText)
        {
            inputText = inputText.Replace(",", "").Replace("|", "").Replace(" - ", " ").Replace("h", ":").Trim();
            Regex trimmer = new Regex(@"\s\s+"); // Xóa khoảng trắng thừa trong chuỗi
            inputText = trimmer.Replace(inputText, " ");
            string kq = "";
            try
            {
                //string inputText = " gg gd ngay cap nhat 07/9/2021 2:00:23 AM gdf dgd gdg";
                string regex = @"((?<ngay>[0|1|2]?[0-9]|3[01])[/\-\.](?<thang>0?[1-9]|1[012])[/\-\.](?<nam>[1-9][0-9][0-9][0-9])[\s]?(?<gio>2[0-4]|[0|1]?[0-9])?[:]?(?<phut>[0-5][0-9])?[:]?(?<giay>[0-5][0-9])?[\s]?(?<buoi>am|pm|sa|ch)?)|((?<gio1>2[0-4]|[0|1]?[0-9])[:]?(?<phut1>[0-5][0-9])[:]?(?<giay1>[0-5][0-9])?[\s]?(?<buoi1>am|pm|sa|ch)?[\s]?(?<ngay1>[0|1|2]?[0-9]|3[01])[/\-\.](?<thang1>0?[1-9]|1[012])[/\-\.](?<nam1>[1-9][0-9][0-9][0-9]))";
                MatchCollection matchCollection = Regex.Matches(inputText.ToLower(), regex);
                if (matchCollection.Count > 0)
                {
                    string ngayThang = matchCollection[0].Value; // 07/9/2021 14:00:00
                }

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
                        kq = kq + " " + (match.Groups["gio1"].Value == "" ? "00" : match.Groups["gio1"].Value);
                        kq = kq + ":" + (match.Groups["phut1"].Value == "" ? "00" : match.Groups["phut1"].Value);
                        kq = kq + ":" + (match.Groups["giay1"].Value == "" ? "00" : match.Groups["giay1"].Value);
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

        private string DownloadFile(string url, string folderName, string filename)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpWebResponse resp;

                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.Proxy = null;
                    resp = (HttpWebResponse)req.GetResponse();
                }
                catch (Exception)
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.Proxy = null;
                    req.UserAgent = "Mozilla/5.0";
                    resp = (HttpWebResponse)req.GetResponse();
                }

                using (resp)
                {
                    string folderPath = folderName;
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filePath = folderName + ChuyenTuCoDauSangKoDau(HttpUtility.UrlDecode(filename, Encoding.UTF8));
                    
                    using (FileStream outputFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
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

        public string ChuyenTuCoDauSangKoDau(string strUrl)
        {
            string str = strUrl.Trim().ToLower();
            while (str.LastIndexOf("  ") > 0)
                str = str.Replace("  ", "");
            return str.Replace(" ", "-").Replace("~", "").Replace("`", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "-").Replace("=", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(",", "").Replace(">", "").Replace("<", "").Replace("'", "").Replace("đ", "d").Replace("á", "a").Replace("à", "a").Replace("ạ", "a").Replace("ả", "a").Replace("ã", "a").Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ặ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ậ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ạ", "a").Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e").Replace("e", "e").Replace("é", "e").Replace("è", "e").Replace("ẹ", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("i", "i").Replace("í", "i").Replace("ì", "i").Replace("ị", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("o", "o").Replace("ó", "o").Replace("ò", "o").Replace("ọ", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ộ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ợ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("u", "u").Replace("ú", "u").Replace("ù", "u").Replace("ụ", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ự", "u").Replace("ử", "u").Replace("ữ", "u").Replace("y", "y").Replace("ý", "y").Replace("ỳ", "y").Replace("ỵ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("/", "-").Replace("?", "-").Replace("\"", "").Replace(":", "-").Replace(";", "-").Replace("--", "-");
        }
    }
}