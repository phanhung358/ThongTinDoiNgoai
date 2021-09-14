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

namespace ThongTinDoiNgoai
{
    public partial class ttdn : System.Web.UI.Page
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        bool bHome;
        protected void Page_Load(object sender, EventArgs e)
        {
            bool bMobie = false;
            string sLoai = Request.UserAgent.ToLowerInvariant();
            if ((sLoai.Contains("mobile") || sLoai.Contains("blackberry") || sLoai.Contains("iphone")))
            {
                bMobie = true;
            }

            if (!IsPostBack)
            {
                bHome = true;
            }

            if (!bMobie)
                dangky.Text = "<link href='/Css/ttdn.css?v=1' rel='stylesheet' type='text/css'/>";
            else
                dangky.Text = "<link href='/Css/ttdn_mobile.css?v=1' rel='stylesheet' type='text/css'/>";
            addMenuTrai();
            addVungChinh();
        }

        private void addMenuTrai()
        {
            StringBuilder str = new StringBuilder();
            bool bMobie = false;
            string sLoai = Request.UserAgent.ToLowerInvariant();
            if ((sLoai.Contains("mobile") || sLoai.Contains("blackberry") || sLoai.Contains("iphone")))
            {
                bMobie = true;
            }

            str.Append("<div class='menu-trai-vien'>");
            if (bMobie)
            {
                str.Append("<script type=\"text/javascript\">");
                str.Append("jQuery(document).ready(function($) {");
                str.Append("$(\"#navigation-list\").hide();");
                str.Append("$(\".menuBtn\").click(function()");
                str.Append("{");
                str.Append("$(\"#navigation-list\").slideToggle(300);");
                str.Append(" });");
                str.Append("});</script>");

                str.AppendFormat("<header>");
                str.Append("<div class=\"danhmuc-vien\">");
                str.Append("<div class=\"danhmuc\">Danh mục</div>");
                str.Append("<span class=\"menuBtn\"></span>");
                str.Append("</div>");
                str.AppendFormat("<nav id=\"navigation-list{0}\">", "");
            }
            else
                str.Append("<div class='menu-trai-danhmuc'>Danh mục</div>");
            str.Append("<ul>");
            str.AppendFormat("<li class='trangchu'><a href='{0}'>Trang chủ</a></li>", Static.AppPath() + "/thongtindoingoai/");
            DataSet ds = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0, 0, "", 3);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                str.Append("<script type=\"text/javascript\">");
                str.Append("$(document).ready(function($) {");
                str.Append("$(\"#sbn-list\").hide();");
                str.Append("$(\"#so-ban-nganh\").click(function()");
                str.Append("{");
                str.Append("$(\"#sbn-list\").slideToggle(300);");
                str.Append("$(\"#so-ban-nganh\").toggleClass('so-ban-nganh');");
                str.Append(" });");
                str.Append("});</script>");

                str.AppendFormat("<li class='cocon'><a href='/thongtindoingoai/so-ban-nganh-a00.html'>Tin từ Sở, Ban, Ngành</a>");
                str.Append("<span id='so-ban-nganh'></span>");
                str.Append("<ul id='sbn-list'>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    str.AppendFormat("<li><a href='{1}.html'>{0}</a></li>", row["TenWeb"].ToString(), "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row["TenWeb"].ToString()) + "-a" + row["WebID"].ToString().Trim());
                }
                str.Append("</ul>");
                str.Append("</li>");
            }
            ds = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0, 0, "", 1);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    str.AppendFormat("<li class='cocon'><a href='{1}.html'>{0}</a></li>", row["TenWeb"].ToString(), "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row["TenWeb"].ToString()) + "-a" + row["WebID"].ToString().Trim());
                }
            }
            //ds = db.GetDataSet("VANBAN_DM_LOAIVANBAN_SELECT", 0);
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            str.AppendFormat("<li class='cocon'>Văn bản");
            str.Append("<ul>");
            //foreach (DataRow row in ds.Tables[0].Rows)
            //{
            //    str.AppendFormat("<li><a href='{1}.html'>{0}</a></li>", row["TenLoaiVanBan"].ToString(), sChucNang + "/" + ChuyenTuCoDauSangKoDau_Url(row["TenLoaiVanBan"].ToString()) + "-a" + row["LoaiVanBanID"].ToString().Trim());
            //}
            for (int i = 0; i < 5; i++)
            {
                str.AppendFormat("<li><a href=''>{0}</a></li>", "Văn bản hành chính");
            }
            str.Append("</ul>");
            str.Append("</li>");
            //}

            str.Append("</ul>");
            if (bMobie)
            {
                str.Append("</nav>");
                str.Append("</header>");
            }
            str.Append("</div>");

            divMenuTrai.InnerHtml = str.ToString();
        }

        private void addVungChinh()
        {
            string sLoai = "";
            if (Request.QueryString["loai"] != null)
                sLoai = Request.QueryString["loai"].ToLower();
            if (sLoai != "")
            {
                string[] arr = sLoai.Split('-');
                sLoai = arr[arr.Length - 1];
                if (sLoai.Length > 1)
                {
                    if (!IsPostBack)
                    {
                        switch (sLoai.Substring(0, 1))
                        {
                            case "a":
                                bHome = false;
                                break;
                        }
                    }
                    sLoai = sLoai.Substring(0, 1);
                }
            }
            if (sLoai != "b")
            {
                addData();
            }
            else
            {
                addData_ChiTiet();
            }
        }

        private void addData()
        {
            StringBuilder str = new StringBuilder();
            if (bHome)
            {
                str.Append("<div class='vung-chinh-vien'>");
                str.Append("<div class='tinmoi'>");
                str.Append("<div class='tinmoi-tieude'>Tin mới</div>");
                str.Append("<div class='tinmoi-danhsach'>");
                DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 4);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int count = ds.Tables[0].Rows.Count < 10 ? ds.Tables[0].Rows.Count : 10;
                    for (int i = 0; i < count; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        str.Append("<div class='tinmoi-mautin'>");
                        str.AppendFormat("<img src='{0}'>", !string.IsNullOrEmpty(row["AnhDaiDien"].ToString()) ? row["AnhDaiDien"].ToString() : Static.AppPath() + "/Images/no_image.png");
                        str.AppendFormat("<a href='{0}.html'>{1}</a>", "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row["TieuDe"].ToString()) + "-b" + row["BaiVietID"].ToString().Trim(), row["TieuDe"].ToString().Trim());
                        str.Append("</div>");
                    }
                }
                str.Append("</div>");
                str.Append("</div>");

                str.Append("<div class='tintungtrang-vien'>");
                List<DataRow> listWeb = new List<DataRow>();
                DataSet dsSo = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0, 0, "", 1);
                if (dsSo != null && dsSo.Tables.Count > 0 && dsSo.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsSo.Tables[0].Rows.Count; j++)
                    {
                        DataRow rowSo = dsSo.Tables[0].Rows[j];
                        listWeb.Add(rowSo);
                    }
                }

                str.AppendFormat("<div class='tintungtrang'>");
                str.AppendFormat("<div class='tintungtrang-tieude'>Tin từ Sở, Ban, Ngành</div>");
                str.Append("<div class='tintungtrang-danhsach'>");
                ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 1, 0, 0, 0, 3);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int count = ds.Tables[0].Rows.Count < 3 ? ds.Tables[0].Rows.Count : 3;
                    for (int i = 0; i < count; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        str.Append("<div class='tintungtrang-mautin'>");
                        str.AppendFormat("<img src='{0}'>", !string.IsNullOrEmpty(row["AnhDaiDien"].ToString()) ? row["AnhDaiDien"].ToString() : Static.AppPath() + "/Images/no_image.png");
                        str.AppendFormat("<a href='{0}.html'>{1}</a>", "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row["TieuDe"].ToString()) + "-b" + row["BaiVietID"].ToString().Trim(), row["TieuDe"].ToString().Trim());
                        str.Append("</div>");
                    }
                    str.Append("<div class='tintungtrang-xemtiep'>");
                    str.AppendFormat("<a href='/thongtindoingoai/so-ban-nganh-a00.html'>Xem tiếp</a>");
                    str.Append("</div>");
                }
                str.Append("</div>");
                str.Append("</div>");

                if (listWeb.Count > 0)
                {
                    foreach (DataRow item in listWeb)
                    {
                        string sClass = "";
                        if (listWeb.IndexOf(item) == listWeb.Count - 1 && listWeb.Count % 2 == 0)
                            sClass = "width-100";
                        str.AppendFormat("<div class='tintungtrang {0}'>", sClass);
                        str.AppendFormat("<div class='tintungtrang-tieude'>Tin từ {0}</div>", item["DiaChiWeb"].ToString().Contains("trt.com.vn") ? "TRT" : item["TenWeb"].ToString());
                        str.Append("<div class='tintungtrang-danhsach'>");
                        DataSet ds1 = db.GetDataSet("TTDN_BAIVIET_SELECT", 3, 0, 0, 0, 0, item["DiaChiWeb"].ToString());
                        if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            int count = ds1.Tables[0].Rows.Count < 3 ? ds1.Tables[0].Rows.Count : 3;
                            for (int i = 0; i < count; i++)
                            {
                                DataRow row = ds1.Tables[0].Rows[i];
                                str.Append("<div class='tintungtrang-mautin'>");
                                str.AppendFormat("<img src='{0}'>", !string.IsNullOrEmpty(row["AnhDaiDien"].ToString()) ? row["AnhDaiDien"].ToString() : Static.AppPath() + "/Images/no_image.png");
                                str.AppendFormat("<a href='{0}.html'>{1}</a>", "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row["TieuDe"].ToString()) + "-b" + row["BaiVietID"].ToString().Trim(), row["TieuDe"].ToString().Trim());
                                str.Append("</div>");
                            }
                            DataRow row1 = ds1.Tables[0].Rows[0];
                            str.Append("<div class='tintungtrang-xemtiep'>");
                            str.AppendFormat("<a href='{0}.html'>Xem tiếp</a>", "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row1["TenWeb"].ToString()) + "-a" + row1["WebID"].ToString().Trim());
                            str.Append("</div>");
                        }
                        str.Append("</div>");
                        str.Append("</div>");
                    }
                }
                str.Append("</div>");
                str.Append("</div>");

                divMain.InnerHtml = str.ToString();
            }
            else
            {
                divMain.Controls.Clear();
                string[] arr = Request.QueryString["loai"].ToLower().Split('-');
                string sLoai = arr[arr.Length - 1];
                if (sLoai.Length > 1)
                {
                    string sWebID = sLoai.Substring(1);
                    divMain.Controls.Add(LoadControl(Static.AppPath() + "/dichvu/thongtindoingoai/danhsachtin.ascx"));
                }
            }
        }

        private void addData_ChiTiet()
        {
            bool bMobie = false;
            string sLoai = Request.UserAgent.ToLowerInvariant();
            if ((sLoai.Contains("mobile") || sLoai.Contains("blackberry") || sLoai.Contains("iphone")))
            {
                bMobie = true;
            }

            string[] arr = Request.QueryString["loai"].ToLower().Split('-');
            sLoai = arr[arr.Length - 1];
            if (sLoai.Length > 1)
            {
                string sBaiVietID = sLoai.Substring(1);
                string sChuyenMucID = "0";
                StringBuilder str = new StringBuilder();
                str.Append("<div class='vung-chinh-vien'>");
                DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 0, sBaiVietID);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    string TenWeb = "";
                    string DiaChiWeb = "";
                    DataSet ds1 = db.GetDataSet("TTDN_TRANGWEB_SELECT", 1, row["WebID"].ToString());
                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow rowWeb = ds1.Tables[0].Rows[0];
                        DiaChiWeb = rowWeb["DiaChiWeb"].ToString();
                        TenWeb = rowWeb["TenWeb"].ToString().ToUpper();
                    }
                    sChuyenMucID = row["ChuyenMucID"].ToString();

                    str.AppendFormat("<div class='demuc'>{0}</div>", TenWeb);
                    str.Append("<div class='chitiet'>");
                    str.AppendFormat("<h2 class='chitiet-tieude'>{0}</h2>", row["TieuDe"].ToString());
                    if (!string.IsNullOrEmpty(row["ThoiGian"].ToString()))
                        str.AppendFormat("<div class='chitiet-thoigian'><span>{0}</span></div>", DateTime.Parse(row["ThoiGian"].ToString()).ToString("dd/MM/yyyy - HH:mm"));
                    if (!string.IsNullOrEmpty(row["TomTat"].ToString()))
                        str.AppendFormat("<div class='chitiet-tomtat'>{0}</div>", row["TomTat"].ToString());

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

                                        int c = 0;
                                        if (file.Attributes["style"] != null)
                                        {
                                            string[] st = file.Attributes["style"].Value.Split(';');
                                            string newstyle = "";
                                            foreach (var css in st)
                                            {
                                                if (css.Contains("width") && !css.Contains("max-width"))
                                                {
                                                    if (Convert.ToInt32(new string(css.Where(x => char.IsDigit(x)).ToArray())) > 850 || bMobie)
                                                        newstyle += "width: 100%; ";
                                                    else
                                                        newstyle += css.Trim() + "; ";
                                                    c = 1;
                                                }
                                                else if (css.Contains("height"))
                                                    newstyle += "height: auto; ";
                                                else
                                                    newstyle += css.Trim() == "" ? "" : css.Trim() + "; ";
                                            }
                                            img = img.Replace(file.Attributes["style"].Value, newstyle);
                                        }
                                        if (c == 0)
                                        {
                                            if (file.Attributes["width"] != null && !string.IsNullOrEmpty(file.Attributes["width"].Value))
                                            {
                                                if (Convert.ToInt32(file.Attributes["width"].Value) > 850 || bMobie)
                                                    img = img.Replace(string.Format("width=\"{0}\"", file.Attributes["width"].Value), "width=\"100%\"");
                                            }
                                            else if (image.Width > 850 || bMobie)
                                                img = img.Replace(">", " width=\"100%\">");
                                            if (file.Attributes["height"] != null && !string.IsNullOrEmpty(file.Attributes["height"].Value))
                                                img = img.Replace(string.Format("height=\"{0}\"", file.Attributes["height"].Value), "height=\"auto\"");
                                        }
                                    }

                                    NoiDung.DocumentNode.InnerHtml = NoiDung.DocumentNode.InnerHtml.Replace(file.OuterHtml, img);
                                }
                            }

                            var dsIfame = NoiDung.DocumentNode.SelectNodes(".//iframe");
                            if (dsIfame != null && bMobie)
                            {
                                foreach (var frame in dsIfame)
                                {
                                    var iframe = frame.OuterHtml;
                                    if (frame.Attributes["style"] != null)
                                    {
                                        string[] st = frame.Attributes["style"].Value.Split(';');
                                        string newstyle = "";
                                        int c = 0;
                                        foreach (var css in st)
                                        {
                                            if (css.Contains("width") && !css.Contains("max-width"))
                                            {
                                                newstyle += "width: 100%; ";
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
                                        iframe = iframe.Replace(frame.Attributes["style"].Value, newstyle);
                                    }
                                    else
                                    {
                                        if (frame.Attributes["width"] != null && !string.IsNullOrEmpty(frame.Attributes["width"].Value))
                                        {
                                            iframe = iframe.Replace(string.Format("width=\"{0}\"", frame.Attributes["width"].Value), "width=\"100%\"");
                                        }
                                        if (frame.Attributes["height"] != null && !string.IsNullOrEmpty(frame.Attributes["height"].Value))
                                            iframe = iframe.Replace(string.Format("height=\"{0}\"", frame.Attributes["height"].Value), "height=\"auto\"");
                                    }

                                    NoiDung.DocumentNode.InnerHtml = NoiDung.DocumentNode.InnerHtml.Replace(frame.OuterHtml, iframe);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }

                    str.AppendFormat("<div class='chitiet-noidung'>{0}</div>", NoiDung.DocumentNode.InnerHtml);
                    str.AppendFormat("<div class='chitiet-tacgia'><p>{0}</p></div>", row["TacGia"].ToString());
                    if (row["BaiVietID"].ToString() != row["BaiViet_Url"].ToString())
                        str.AppendFormat("<div class='baivietgoc'><a href='{0}' target='_blank'>>>><i>Xem bài viết gốc</i></a></div>", row["BaiViet_Url"].ToString());
                    str.Append("</div>");

                    str.Append("<div class='tinkhac-demuc'>Tin khác:</div>");
                    str.Append("<div class='tinkhac-danhsach'>");
                    DataSet dsKhac = db.GetDataSet("TTDN_BAIVIET_SELECT", 2, sBaiVietID, 0, sChuyenMucID);
                    if (dsKhac != null && dsKhac.Tables.Count > 0 && dsKhac.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsKhac.Tables[0].Rows.Count; i++)
                        {
                            DataRow rowKhac = dsKhac.Tables[0].Rows[i];
                            str.Append("<div class='tinkhac-mautin'>");
                            str.AppendFormat("<a href='{0}.html'>{1}</a>", "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(rowKhac["TieuDe"].ToString()) + "-b" + rowKhac["BaiVietID"].ToString().Trim(), rowKhac["TieuDe"].ToString().Trim());
                            str.Append("</div>");
                        }
                    }
                    str.Append("</div>");
                }
                str.Append("</div>");

                divMain.InnerHtml = str.ToString();
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
            return str.Replace(" ", "-").Replace("~", "").Replace("`", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "-").Replace("=", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(",", "").Replace(">", "").Replace("<", "").Replace("'", "").Replace("đ", "d").Replace("á", "a").Replace("à", "a").Replace("ạ", "a").Replace("ả", "a").Replace("ã", "a").Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ặ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ậ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e").Replace("e", "e").Replace("é", "e").Replace("è", "e").Replace("ẹ", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("i", "i").Replace("í", "i").Replace("ì", "i").Replace("ị", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("o", "o").Replace("ó", "o").Replace("ò", "o").Replace("ọ", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ộ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ợ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("u", "u").Replace("ú", "u").Replace("ù", "u").Replace("ụ", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ự", "u").Replace("ử", "u").Replace("ữ", "u").Replace("y", "y").Replace("ý", "y").Replace("ỳ", "y").Replace("ỵ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("/", "-").Replace("?", "-").Replace("\'", "").Replace("\"", "").Replace(":", "-").Replace(";", "-").Replace("--", "-").Replace("“", "").Replace("”", "");
        }
    }
}