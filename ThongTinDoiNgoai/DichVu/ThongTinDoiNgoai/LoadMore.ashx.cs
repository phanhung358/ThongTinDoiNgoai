using FITC.Web.Component;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace ThongTinDoiNgoai.DichVu.ThongTinDoiNgoai
{
    /// <summary>
    /// Summary description for LoadMore
    /// </summary>
    public class LoadMore : IHttpHandler
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string WebID = context.Request.QueryString["WebID"];
            string page = context.Request.QueryString["page"];

            StringBuilder str = new StringBuilder();
            using (DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT_MOBILE", 0, 0, WebID, page))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
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
                }
            }

            context.Response.Write(str.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
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