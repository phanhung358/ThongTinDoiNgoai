using FITC.Web.Component;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

            List<object[]> dsTin = new List<object[]>();
            try
            {
                DataSet dsWeb = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0, webID);
                if (dsWeb != null && dsWeb.Tables.Count > 0 && dsWeb.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsWeb.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = dsWeb.Tables[0].Rows[i];
                        DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 3, row["WebID"].ToString());
                        if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsChuyenMuc.Tables[0].Rows.Count; j++)
                            {
                                DataRow rowCM = dsChuyenMuc.Tables[0].Rows[j];
                                DataSet dsXpathCM = db.GetDataSet("TTDN_XPATH_CHUYENMUC_SELECT", 0, rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString());
                                if (dsXpathCM != null && dsXpathCM.Tables.Count > 0 && dsXpathCM.Tables[0].Rows.Count > 0)
                                {
                                    DataRow rowXpathCM = dsXpathCM.Tables[0].Rows[0];

                                    HtmlDocument html = htmlWeb.Load(rowCM["UrlChuyenMuc"].ToString());

                                    if (html == null)
                                    {
                                        string Loi = "Không lấy được dữ liệu của chuyên mục!";
                                        string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), Loi, rowCM["UrlChuyenMuc"].ToString());
                                        continue;
                                    }
                                    html.Text = html.Text.Replace("<TABLE", "<table").Replace("<TR", "<tr").Replace("<TD", "<td").Replace("<DIV", "<div").Replace("<A", "<a").Replace("<P", "<p").Replace("<SPAN", "<span").Replace("<STRONG", "<strong").Replace("<EM", "<em").Replace("<TITLE", "<title").Replace("</TABLE", "</table").Replace("</TR", "</tr").Replace("</TD", "</td").Replace("</DIV", "</div").Replace("</A", "</a").Replace("</P", "</p").Replace("</SPAN", "</span").Replace("</STRONG", "</strong").Replace("</EM", "</em").Replace("</TITLE", "</title");

                                    string xds = rowXpathCM["DanhSach"].ToString();
                                    string xbv = rowXpathCM["BaiViet_Url"].ToString();
                                    string xbv1 = rowXpathCM["BaiViet_Url1"].ToString();
                                    string xbv2 = rowXpathCM["BaiViet_Url2"].ToString();
                                    if (!html.Text.Contains("<tbody>"))
                                    {
                                        xds = xds.Replace("tbody/", "");
                                        xbv = xbv.Replace("tbody/", "");
                                        xbv1 = xbv1.Replace("tbody/", "");
                                        xbv2 = xbv2.Replace("tbody/", "");
                                    }
                                    string XDanhSach = xds.LastIndexOf(']') == xds.Length - 1 ? xds.Remove(xds.LastIndexOf('['), xds.Length - xds.LastIndexOf('[')) : xds;
                                    xbv = xbv.Replace(XDanhSach, ".");
                                    xbv1 = xbv1.Replace(XDanhSach, ".");
                                    xbv2 = xbv2.Replace(XDanhSach, ".");
                                    string XBaiViet_Url = xbv.IndexOf('[') == 1 ? xbv.Remove(1, xbv.IndexOf(']')) : xbv;
                                    string XBaiViet_Url1 = xbv1.IndexOf('[') == 1 ? xbv1.Remove(1, xbv1.IndexOf(']')) : xbv1;
                                    string XBaiViet_Url2 = xbv2.IndexOf('[') == 1 ? xbv2.Remove(1, xbv2.IndexOf(']')) : xbv2;

                                    var DanhSach = html.DocumentNode.SelectNodes(XDanhSach) != null ? html.DocumentNode.SelectNodes(XDanhSach).ToList() : null;
                                    if (DanhSach != null)
                                    {
                                        foreach (var item in DanhSach)
                                        {
                                            string BaiViet_Url = null;
                                            if (!string.IsNullOrEmpty(XBaiViet_Url) && item.SelectSingleNode(XBaiViet_Url) != null)
                                                BaiViet_Url = item.SelectSingleNode(XBaiViet_Url).Attributes["href"].Value.Replace("&amp;", "&");
                                            else if (!string.IsNullOrEmpty(XBaiViet_Url1) && item.SelectSingleNode(XBaiViet_Url1) != null)
                                                BaiViet_Url = item.SelectSingleNode(XBaiViet_Url1).Attributes["href"].Value.Replace("&amp;", "&");
                                            else if (!string.IsNullOrEmpty(XBaiViet_Url2) && item.SelectSingleNode(XBaiViet_Url2) != null)
                                                BaiViet_Url = item.SelectSingleNode(XBaiViet_Url2).Attributes["href"].Value.Replace("&amp;", "&");
                                            else
                                                BaiViet_Url = null;

                                            if (BaiViet_Url != null)
                                            {
                                                if (BaiViet_Url.Contains("http"))
                                                {
                                                    if (row["DiaChiWeb"].ToString().Substring(0, 5) == "https" && BaiViet_Url.Substring(0, 5) !="https")
                                                        BaiViet_Url = BaiViet_Url.Replace("http", "https");
                                                    if (row["DiaChiWeb"].ToString().Substring(0, 5) != "https" && BaiViet_Url.Substring(0, 5) == "https")
                                                        BaiViet_Url = BaiViet_Url.Replace("https", "http");
                                                }
                                                else
                                                {
                                                    if (BaiViet_Url.LastIndexOf(row["DiaChiWeb"].ToString()) == -1)
                                                    {
                                                        BaiViet_Url = row["DiaChiWeb"].ToString() + (BaiViet_Url.IndexOf('/') == 0 ? BaiViet_Url : "/" + BaiViet_Url);
                                                    }
                                                }

                                                object[] obj = new object[3];
                                                obj[0] = BaiViet_Url;
                                                obj[1] = rowXpathCM["ChuyenMucID"].ToString();
                                                obj[2] = rowXpathCM["WebID"].ToString();
                                                dsTin.Add(obj);
                                            }
                                            else
                                            {
                                                string Loi = "Không lấy được đường dẫn (URL) của bài viết trong chuyên mục!";
                                                string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), Loi, rowCM["UrlChuyenMuc"].ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string Loi = "Không lấy được danh sách tin của chuyên mục!";
                                        string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString(), Loi, rowCM["UrlChuyenMuc"].ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            try
            {
                foreach (var item in dsTin)
                {
                    DataSet dsXpathCT = db.GetDataSet("TTDN_XPATH_CHITIET_SELECT", 0, item[2].ToString(), item[1].ToString());
                    if (dsXpathCT != null && dsXpathCT.Tables.Count > 0 && dsXpathCT.Tables[0].Rows.Count > 0)
                    {
                        DataRow rowXpathCT = dsXpathCT.Tables[0].Rows[0];

                        HtmlDocument html = htmlWeb.Load(item[0].ToString());

                        if (html == null)
                        {
                            string Loi = "Không lấy được chi tiết bài viết!";
                            string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCT["WebID"].ToString(), rowXpathCT["ChuyenMucID"].ToString(), Loi, item[0].ToString());
                            continue;
                        }
                        html.Text = html.Text.Replace("<TABLE", "<table").Replace("<TR", "<tr").Replace("<TD", "<td").Replace("<DIV", "<div").Replace("<A", "<a").Replace("<P", "<p").Replace("<SPAN", "<span").Replace("<STRONG", "<strong").Replace("<EM", "<em").Replace("<TITLE", "<title").Replace("</TABLE", "</table").Replace("</TR", "</tr").Replace("</TD", "</td").Replace("</DIV", "</div").Replace("</A", "</a").Replace("</P", "</p").Replace("</SPAN", "</span").Replace("</STRONG", "</strong").Replace("</EM", "</em").Replace("</TITLE", "</title");

                        string XTieuDe = rowXpathCT["TieuDe"].ToString();
                        string XTieuDePhu = rowXpathCT["TieuDePhu"].ToString();
                        string XTomTat = rowXpathCT["TomTat"].ToString();
                        string XNoiDung = rowXpathCT["NoiDung"].ToString();
                        string XThoiGian = rowXpathCT["ThoiGian"].ToString();
                        string XTacGia = rowXpathCT["TacGia"].ToString();
                        if (!html.Text.Contains("<tbody>"))
                        {
                            XTieuDe = XTieuDe.Replace("tbody/", "");
                            XTieuDePhu = XTieuDePhu.Replace("tbody/", "");
                            XTomTat = XTomTat.Replace("tbody/", "");
                            XNoiDung = XNoiDung.Replace("tbody/", "");
                            XThoiGian = XThoiGian.Replace("tbody/", "");
                            XTacGia = XTacGia.Replace("tbody/", "");
                        }    

                        var TieuDe = html.DocumentNode.SelectSingleNode(XTieuDe) != null ? html.DocumentNode.SelectSingleNode(XTieuDe) : null;
                        var TieuDePhu = XTieuDePhu != "" ? (html.DocumentNode.SelectSingleNode(XTieuDePhu) != null ? html.DocumentNode.SelectSingleNode(XTieuDePhu) : null) : null;
                        var TomTat = XTomTat != "" ? (html.DocumentNode.SelectSingleNode(XTomTat) != null ? html.DocumentNode.SelectSingleNode(XTomTat) : null) : null;
                        var NoiDung = html.DocumentNode.SelectSingleNode(XNoiDung) != null ? html.DocumentNode.SelectSingleNode(XNoiDung) : null;
                        var ThoiGian = XThoiGian != "" ? (html.DocumentNode.SelectSingleNode(XThoiGian) != null ? html.DocumentNode.SelectSingleNode(XThoiGian) : null) : null;
                        var TacGia = XTacGia != "" ? (html.DocumentNode.SelectSingleNode(XTacGia) != null ? html.DocumentNode.SelectSingleNode(XTacGia) : null) : null;


                        string tgian = null;
                        string strNewsDatePosted = null;
                        if (ThoiGian != null)
                        {
                            strNewsDatePosted = ThoiGian.InnerText.Trim();
                            tgian = LayNgay(strNewsDatePosted);
                        }

                        if (!string.IsNullOrEmpty(XTieuDe) && TieuDe == null)
                        {
                            string Loi = "Không lấy được tiêu đề của bài viết!";
                            string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCT["WebID"].ToString(), rowXpathCT["ChuyenMucID"].ToString(), Loi, item[0].ToString());
                        }
                        if (!string.IsNullOrEmpty(XTieuDePhu) && TieuDePhu == null)
                        {
                            string Loi = "Không lấy được tiêu đề phụ của bài viết!";
                            string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCT["WebID"].ToString(), rowXpathCT["ChuyenMucID"].ToString(), Loi, item[0].ToString());
                        }
                        if (!string.IsNullOrEmpty(XTomTat) && TomTat == null)
                        {
                            string Loi = "Không lấy được tóm tắt bài viết!";
                            string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCT["WebID"].ToString(), rowXpathCT["ChuyenMucID"].ToString(), Loi, item[0].ToString());
                        }
                        if (!string.IsNullOrEmpty(XNoiDung) && NoiDung == null)
                        {
                            string Loi = "Không lấy được nội dung của bài viết!";
                            string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCT["WebID"].ToString(), rowXpathCT["ChuyenMucID"].ToString(), Loi, item[0].ToString());
                        }
                        if (!string.IsNullOrEmpty(XThoiGian) && ThoiGian == null)
                        {
                            string Loi = "Không lấy được thời gian đăng bài!";
                            string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCT["WebID"].ToString(), rowXpathCT["ChuyenMucID"].ToString(), Loi, item[0].ToString());
                        }
                        if (!string.IsNullOrEmpty(XTacGia) && TacGia == null)
                        {
                            string Loi = "Không lấy được tác giả của bài viết!";
                            string s = db.ExcuteSP("TTDN_CHUYENMUC_LOI_INSERT", rowXpathCT["WebID"].ToString(), rowXpathCT["ChuyenMucID"].ToString(), Loi, item[0].ToString());
                        }

                        string DiaChiWeb = "";
                        DataSet ds = db.GetDataSet("TTDN_TRANGWEB_SELECT", 1, item[2].ToString());
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow row = ds.Tables[0].Rows[0];
                            DiaChiWeb = row["DiaChiWeb"].ToString();
                        }

                        object[] obj = new object[10];
                        obj[0] = TieuDe != null ? TieuDe.InnerText : null;
                        obj[1] = TieuDePhu != null ? TieuDePhu.InnerText : null;
                        obj[2] = TomTat != null ? TomTat.InnerText : null;
                        obj[3] = NoiDung != null ? NoiDung.InnerHtml.Replace("src=\"/uploads", "src=\"" + DiaChiWeb + "/uploads") : null;
                        obj[4] = ThoiGian != null ? tgian : null;
                        obj[5] = TacGia != null ? TacGia.InnerText : null;
                        obj[6] = item[0].ToString();
                        obj[7] = NoiDung?.InnerHtml.Length;
                        obj[8] = item[1].ToString();
                        obj[9] = item[2].ToString();
                        string sLoi = db.ExcuteSP("TTDN_BAIVIET_INSERT", obj);
                        if (sLoi != "")
                        {

                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        private string LayNgay(string inputText)
        {
            inputText = inputText.ToLower();
            string kq = "";
            try
            {
                //string inputText = " gg gd ngay cap nhat 07/9/2021 2:00:23 AM gdf dgd gdg";
                string regex = @"(?<ngay>[0|1|2]?[0-9]|3[01])[/](?<thang>0?[1-9]|1[012])[/](?<nam>[1-9][0-9][0-9][0-9])[\s]?(?<gio>[0|1]?[0-9]|2[0-4])?[:]?(?<phut>[0-5][0-9])?[:]?(?<giay>[0-5][0-9])?[\s]?(?<buoi>am|pm|sa|ch)?";
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