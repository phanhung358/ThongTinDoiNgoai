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
                dangky.Text = "<link href='/Css/ttdn_mobie.css?v=1' rel='stylesheet' type='text/css'/>";
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

            if (bMobie)
            {
                str.Append("<script type=\"text/javascript\">");
                str.Append("jQuery(document).ready(function($) {");
                str.AppendFormat("$(\"#navigation-list{0}\").hide();", "");
                str.AppendFormat("$(\".menuBtn{0}\").click(function()", "");
                str.Append("{");
                str.AppendFormat("$(\"#navigation-list{0}\").slideToggle(300);", "");
                str.Append(" });");
                str.Append("});</script>");

                str.AppendFormat("<header>");
                str.AppendFormat("<div><span class=\"menuBtn{0}\"><img alt=\"\" src=\"\" /></span></div>", "");
                str.AppendFormat("<nav id=\"navigation-list{0}\">", "");
            }
            else
                str.Append("<div class='menu-trai-danhmuc'>Danh mục</div>");
            str.Append("<ul>");
            DataSet ds = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0, 0, "", 1);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                str.AppendFormat("<li class='trangchu'><a href='{0}'>Trang chủ</a></li>", Static.AppPath() + "/thongtindoingoai/");
                str.AppendFormat("<li class='cocon'>Tin từ Sở, Ban, Ngành");
                str.Append("<ul>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    str.AppendFormat("<li><a href='{1}.html'>{0}</a></li>", row["TenWeb"].ToString(), "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row["TenWeb"].ToString()) + "-a" + row["WebID"].ToString().Trim());
                }
                str.Append("</ul>");
                str.Append("</li>");
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
                DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 1, 0, 0, 0, 1);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int count = ds.Tables[0].Rows.Count < 20 ? ds.Tables[0].Rows.Count : 20;
                    for (int i = 0; i < count; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        str.Append("<div class='tinmoi-mautin'>");
                        str.AppendFormat("<a href='{0}.html'>{1}</a>", "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row["TieuDe"].ToString()) + "-b" + row["BaiVietID"].ToString().Trim(), row["TieuDe"].ToString().Trim());
                        str.Append("</div>");
                    }
                }
                str.Append("</div>");
                str.Append("</div>");
                str.Append("<div class='tintungtrang'>");
                str.Append("<div class='tintungtrang-tieude'>Tin từ Báo Thừa Thiên Huế</div>");
                str.Append("<div class='tintungtrang-danhsach'>");
                DataSet ds1 = db.GetDataSet("TTDN_BAIVIET_SELECT", 3, 0, 0, 0, 0, "https://baothuathienhue.vn");
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    int count = ds1.Tables[0].Rows.Count < 3 ? ds1.Tables[0].Rows.Count : 3;
                    for (int i = 0; i < count; i++)
                    {
                        DataRow row = ds1.Tables[0].Rows[i];
                        str.Append("<div class='tintungtrang-mautin'>");
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
                str.Append("<div class='tintungtrang'>");
                str.Append("<div class='tintungtrang-tieude'>Tin từ TRT</div>");
                str.Append("<div class='tintungtrang-danhsach'>");
                DataSet ds2 = db.GetDataSet("TTDN_BAIVIET_SELECT", 3, 0, 0, 0, 0, "http://www.trt.com.vn");
                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    int count = ds2.Tables[0].Rows.Count < 3 ? ds2.Tables[0].Rows.Count : 3;
                    for (int i = 0; i < count; i++)
                    {
                        DataRow row = ds2.Tables[0].Rows[i];
                        str.Append("<div class='tintungtrang-mautin'>");
                        str.AppendFormat("<a href='{0}.html'>{1}</a>", "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row["TieuDe"].ToString()) + "-b" + row["BaiVietID"].ToString().Trim(), row["TieuDe"].ToString().Trim());
                        str.Append("</div>");
                    }
                    DataRow row1 = ds2.Tables[0].Rows[0];
                    str.Append("<div class='tintungtrang-xemtiep'>");
                    str.AppendFormat("<a href='{0}.html'>Xem tiếp</a>", "/thongtindoingoai/" + ChuyenTuCoDauSangKoDau(row1["TenWeb"].ToString()) + "-a" + row1["WebID"].ToString().Trim());
                    str.Append("</div>");
                }
                str.Append("</div>");
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
            string[] arr = Request.QueryString["loai"].ToLower().Split('-');
            string sLoai = arr[arr.Length - 1];
            if (sLoai.Length > 1)
            {
                string sBaiVietID = sLoai.Substring(1);
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

                                        if (file.Attributes["style"] != null)
                                        {
                                            string[] st = file.Attributes["style"].Value.Split(';');
                                            string newstyle = "";
                                            int c = 0;
                                            foreach (var css in st)
                                            {
                                                if (css.Contains("width") && !css.Contains("max-width"))
                                                {
                                                    if (Convert.ToInt32(new string(css.Where(x => char.IsDigit(x)).ToArray())) > 850)
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
                                                if (Convert.ToInt32(file.Attributes["width"].Value) > 850)
                                                    img = img.Replace(string.Format("width=\"{0}\"", file.Attributes["width"].Value), "width=\"100%\"");
                                            }
                                            else if (image.Width > 850)
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

                    str.AppendFormat("<div class='chitiet-noidung'>{0}</div>", NoiDung.DocumentNode.InnerHtml);
                    str.AppendFormat("<div class='chitiet-tacgia'><p>{0}</p></div>", row["TacGia"].ToString());
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
            return str.Replace(" ", "-").Replace("~", "").Replace("`", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "-").Replace("=", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(",", "").Replace(">", "").Replace("<", "").Replace("'", "").Replace("đ", "d").Replace("á", "a").Replace("à", "a").Replace("ạ", "a").Replace("ả", "a").Replace("ã", "a").Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ặ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ậ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e").Replace("e", "e").Replace("é", "e").Replace("è", "e").Replace("ẹ", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("i", "i").Replace("í", "i").Replace("ì", "i").Replace("ị", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("o", "o").Replace("ó", "o").Replace("ò", "o").Replace("ọ", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ộ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ợ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("u", "u").Replace("ú", "u").Replace("ù", "u").Replace("ụ", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ự", "u").Replace("ử", "u").Replace("ữ", "u").Replace("y", "y").Replace("ý", "y").Replace("ỳ", "y").Replace("ỵ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("/", "-").Replace("?", "-").Replace("\'", "").Replace("\"", "").Replace(":", "-").Replace(";", "-").Replace("--", "-");
        }
    }
}