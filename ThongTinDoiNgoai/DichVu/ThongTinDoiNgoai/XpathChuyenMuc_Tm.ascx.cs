using FITC.Web.Component;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyVanBan.DichVu.DuLieu
{
    public partial class XpathChuyenMuc_Tm : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sWebID = "0";
        string sChuyenMucID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSuKien.Style.Add("display", "none");
            if (Request.QueryString["WebID"] != null && Request.QueryString["ChuyenMucID"] != null)
            {
                sWebID = Request.QueryString["WebID"];
                sChuyenMucID = Request.QueryString["ChuyenMucID"];
            }
            if (!IsPostBack)
            {
                addDanhMuc();
                addSua();
            }
            btnLayTuTrangKhac.Attributes["onclick"] = string.Format("return thickboxPopup('Copy Xpath từ chuyên mục khác', '{0}?control={1}&btn={2}&WebID={3}&ChuyenMucID={4}','100%','100%');", Static.AppPath() + "/home/popup.aspx", "/DichVu/ThongTinDoiNgoai/XpathChuyenMuc_Tm_Copy.ascx", btnSuKien.ClientID, sWebID, sChuyenMucID);
        }

        private void addDanhMuc()
        {
            drpWeb.Items.Add(new ListItem("[Chọn]", "0"));
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
            drpChuyenMuc.Items.Add(new ListItem("[Chọn]", "0"));
        }

        private void addSua()
        {
            try
            {
                db.GetItem(drpWeb, sWebID);
                drpWeb_SelectedIndexChanged(null, null);
                db.GetItem(drpChuyenMuc, sChuyenMucID);

                DataSet ds = db.GetDataSet("TTDN_XPATH_CHUYENMUC_SELECT", 0, sWebID, sChuyenMucID);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    btnLayTuTrangKhac.Visible = false;
                    txtDanhSach.Text = row["DanhSach"].ToString();
                    txtBaiViet_Url1.Text = row["BaiViet_Url"].ToString();
                    txtBaiViet_Url2.Text = row["BaiViet_Url1"].ToString();
                    txtBaiViet_Url3.Text = row["BaiViet_Url2"].ToString();
                }
                else
                {
                    btnLayTuTrangKhac.Visible = true;
                }
            }
            catch { }
        }

        private string KiemTra()
        {
            FITC_CDataTime dt = new FITC_CDataTime();
            string sLoi = "";
            if (drpWeb.SelectedValue == "0")
                sLoi = "Chưa chọn trang web!";
            if (drpChuyenMuc.SelectedValue == "0")
                sLoi = "Chưa chọn chuyên mục!";
            if (txtDanhSach.Text.Trim() == "")
                sLoi = "Chưa nhập Xpath danh sách!";
            if (txtBaiViet_Url1.Text.Trim() == "")
                sLoi = "Chưa nhập Xpath URL bài viết!";

            return sLoi;
        }

        protected void btnThemMoi_Click(object sender, EventArgs e)
        {
            try
            {
                string strLoi = KiemTra();
                if (strLoi != "")
                {
                    ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                    return;
                }
                object[] obj = new object[7];


                obj[0] = txtDanhSach.Text.Trim();
                obj[1] = txtBaiViet_Url1.Text.Trim();
                obj[2] = txtBaiViet_Url2.Text.Trim();
                obj[3] = txtBaiViet_Url3.Text.Trim();
                obj[4] = drpChuyenMuc.SelectedValue;
                obj[5] = drpWeb.SelectedValue;
                obj[6] = TUONGTAC.TenTaiKhoan;

                string sLoi = db.ExcuteSP("TTDN_XPATH_CHUYENMUC_INSERT", obj);
                if (sLoi == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.FindControl("btnThemMoi"), this.GetType(), "Message_Close", "alert('Cập nhật thành công !'); self.parent.tb_remove();", true);
                }
                else
                {
                    ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                    return;
                }
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnThemMoi");
            }
        }

        protected void drpWeb_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpChuyenMuc.Items.Clear();
            if (drpWeb.SelectedValue == "0")
                drpChuyenMuc.Items.Add(new ListItem("[Chọn]", "0"));
            else
            {
                drpChuyenMuc.Items.Add(new ListItem("[Chọn]", "0"));
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
        }

        protected void btnConnect_Click(object sender, EventArgs e)
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8
            };

            try
            {
                DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 3, drpWeb.SelectedValue);
                if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsChuyenMuc.Tables[0].Rows[0];

                    ChromeOptions options = new ChromeOptions() { Proxy = null };
                    options.AddArgument("--headless");
                    options.AddArgument("--no-sandbox");
                    options.AddArgument("--ignore-certificate-errors");
                    ChromeDriver driver = new ChromeDriver(HttpContext.Current.Server.MapPath(Static.AppPath() + "/ChromeDriver"), options, TimeSpan.FromMinutes(3));
                    driver.Navigate().GoToUrl(row["UrlChuyenMuc"].ToString());

                    for (int i = 1; i <= 10; i++)
                    {
                        string jsCode = "window.scrollTo({top: document.body.scrollHeight / " + 10 + " * " + i + ", behavior: \"smooth\"});";
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript(jsCode);
                        Thread.Sleep(1000);
                    }

                    HtmlDocument html = new HtmlDocument();
                    html.LoadHtml(driver.PageSource);
                    driver.Quit();

                    if (html == null)
                    {
                        ham.Alert("Không lấy được dữ liệu của chuyên mục!");
                        return;
                    }
                    html.DocumentNode.InnerHtml = html.DocumentNode.InnerHtml.Replace("<TABLE", "<table").Replace("<TR", "<tr").Replace("<TD", "<td").Replace("<DIV", "<div").Replace("<A", "<a").Replace("<P", "<p").Replace("<SPAN", "<span").Replace("<STRONG", "<strong").Replace("<EM", "<em").Replace("<TITLE", "<title").Replace("<SCRIPT", "<script").Replace("</TABLE", "</table").Replace("</TR", "</tr").Replace("</TD", "</td").Replace("</DIV", "</div").Replace("</A", "</a").Replace("</P", "</p").Replace("</SPAN", "</span").Replace("</STRONG", "</strong").Replace("</EM", "</em").Replace("</TITLE", "</title").Replace("</SCRIPT", "</script").Replace("<TBODY>", "").Replace("</TBODY>", "").Replace("<tbody>", "").Replace("</tbody>", "");

                    string xds = txtDanhSach.Text.Replace("tbody/", "");
                    string XDanhSach = xds.LastIndexOf(']') == xds.Length - 1 ? xds.Remove(xds.LastIndexOf('['), xds.Length - xds.LastIndexOf('[')) : xds;
                    string xbv = txtBaiViet_Url1.Text.Replace("tbody/", "").Replace(XDanhSach, ".");
                    string xbv1 = txtBaiViet_Url2.Text.Replace("tbody/", "").Replace(XDanhSach, ".");
                    string xbv2 = txtBaiViet_Url3.Text.Replace("tbody/", "").Replace(XDanhSach, ".");
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

                            if (BaiViet_Url == null)
                            {
                                ham.Alert("Lỗi: Xpath không đúng!");
                                return;
                            }
                        }
                        ham.Alert("Thông báo: Kiểm tra hoàn tất, Xpath này có thể sử dụng được.");
                        return;
                    }
                    else
                    {
                        ham.Alert("Lỗi: Xpath không đúng!");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ham.Alert("Lỗi: " + ex.Message);
            }
        }

        protected void btnSuKien_Click(object sender, EventArgs e)
        {
            addSua();
        }
    }
}