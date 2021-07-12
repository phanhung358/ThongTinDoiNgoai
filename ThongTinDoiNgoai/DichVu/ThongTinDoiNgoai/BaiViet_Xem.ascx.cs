using FITC.Web.Component;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyVanBan.DichVu.ThongTinDoiNgoai
{
    public partial class BaiViet_Ct : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sBaiVietID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["BaiVietID"] != null)
            {
                sBaiVietID = Request.QueryString["BaiVietID"];
            }
            if (!IsPostBack)
            {
                addData();
            }
        }

        private void addData()
        {
            StringBuilder str = new StringBuilder();
            DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 0, sBaiVietID);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                str.AppendFormat("<h2 class='title margin-bottom-lg'>{0}</h2>", row["TieuDe"].ToString());
                if (!string.IsNullOrEmpty(row["ThoiGian"].ToString()))
                    str.AppendFormat("<div class='row margin-bottom-lg'><span>{0}</span></div>", DateTime.Parse(row["ThoiGian"].ToString()).ToString("dd/MM/yyyy - HH:mm"));
                if (!string.IsNullOrEmpty(row["TieuDePhu"].ToString()))
                    str.AppendFormat("<h4 class='m-bottom'>{0}</h4>", row["TieuDePhu"].ToString());
                if (!string.IsNullOrEmpty(row["TomTat"].ToString()))
                    str.AppendFormat("<div class='hometext m-bottom'>{0}</div>", row["TomTat"].ToString());

                try
                {
                    string DiaChiWeb = "";
                    DataSet ds1 = db.GetDataSet("TTDN_TRANGWEB_SELECT", 1, row["WebID"].ToString());
                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow rowWeb = ds1.Tables[0].Rows[0];
                        DiaChiWeb = rowWeb["DiaChiWeb"].ToString();
                    }

                    HtmlDocument NoiDung = new HtmlDocument();
                    NoiDung.LoadHtml(row["NoiDung"].ToString());
                    if (NoiDung != null)
                    {
                        string DirUpload = Static.GetPath() + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DiaChiWeb.Remove(0, DiaChiWeb.IndexOf("/") + 2) + "/";
                        var dsFile = NoiDung.DocumentNode.SelectNodes(".//img");
                        if (dsFile != null)
                        {
                            foreach (var file in dsFile)
                            {
                                string strSource = HttpUtility.UrlDecode(file.Attributes["url-img-full"] == null ? (file.Attributes["data-src"] == null ? file.Attributes["src"].Value : file.Attributes["data-src"].Value) : file.Attributes["url-img-full"].Value, Encoding.UTF8);
                                //string fileName = "";
                                //if (strSource.ToLower().Contains(".jpg") || strSource.ToLower().Contains(".jpeg") || strSource.ToLower().Contains(".png") || strSource.ToLower().Contains(".gif") || strSource.ToLower().Contains(".tiff") || strSource.ToLower().Contains(".pdf"))
                                //{
                                //    fileName = Path.GetFileName(new Uri(strSource).AbsolutePath).Replace(" ", "_");
                                //}
                                //else
                                //{
                                //    fileName = strSource.Substring(strSource.LastIndexOf("/") + 1).Replace(" ", "_").Replace("&amp;", "&").Replace("&#x3a;", ":").Replace("&#x2f;", "/").Replace("&#x2e;", ".");
                                //}
                                //string strSourceRep = DirUpload.Substring(DirUpload.IndexOf("/UploadFiles/")) + ChuyenTuCoDauSangKoDau(HttpUtility.UrlDecode(fileName));
                                string img = file.OuterHtml;

                                if (File.Exists(Server.MapPath(strSource)))
                                {
                                    System.Drawing.Image image = LoadImage(Server.MapPath(strSource));

                                    if (file.Attributes["style"] != null && (file.Attributes["style"].Value.Contains("width") || file.Attributes["style"].Value.Contains("height")))
                                    {
                                        string[] st = file.Attributes["style"].Value.Split(';');
                                        string newstyle = "";
                                        foreach (var css in st)
                                        {
                                            if (css.Contains("width") && Convert.ToInt32(new string(css.Where(x => char.IsDigit(x)).ToArray())) > 920)
                                                newstyle += "width: 920px;";
                                            else if (css.Contains("height"))
                                                newstyle += "height: auto;";
                                            else
                                                newstyle += css == "" ? "" : css + ";";
                                        }
                                        img = img.Replace(file.Attributes["style"].Value, newstyle);
                                    }
                                    else
                                    {
                                        if (file.Attributes["width"] != null && !string.IsNullOrEmpty(file.Attributes["width"].Value))
                                        {
                                            if (Convert.ToInt32(file.Attributes["width"].Value) > 920)
                                                img = img.Replace(file.Attributes["width"].Value, "920");
                                        }
                                        else if (image.Width > 920)
                                            img = img.Replace(">", " width=\"920\">");
                                        if (file.Attributes["height"] != null && !string.IsNullOrEmpty(file.Attributes["height"].Value))
                                            img = img.Replace(file.Attributes["height"].Value, "auto");
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

                str.AppendFormat("<div class='bodytext margin-bottom-lg'>{0}</div>", row["NoiDung"].ToString());
                str.AppendFormat("<div class='margin-bottom-lg'><p class='h5 text-right'>{0}</p></div>", row["TacGia"].ToString());

                divDanhSach.InnerHtml = str.ToString();
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
            return str.Replace(" ", "-").Replace("~", "").Replace("`", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "-").Replace("=", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(",", "").Replace(">", "").Replace("<", "").Replace("'", "").Replace("đ", "d").Replace("á", "a").Replace("à", "a").Replace("ạ", "a").Replace("ả", "a").Replace("ã", "a").Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ặ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ậ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ạ", "a").Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e").Replace("e", "e").Replace("é", "e").Replace("è", "e").Replace("ẹ", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("i", "i").Replace("í", "i").Replace("ì", "i").Replace("ị", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("o", "o").Replace("ó", "o").Replace("ò", "o").Replace("ọ", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ộ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ợ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("u", "u").Replace("ú", "u").Replace("ù", "u").Replace("ụ", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ự", "u").Replace("ử", "u").Replace("ữ", "u").Replace("y", "y").Replace("ý", "y").Replace("ỳ", "y").Replace("ỵ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("/", "-").Replace("?", "-").Replace("\"", "");
        }
    }
}