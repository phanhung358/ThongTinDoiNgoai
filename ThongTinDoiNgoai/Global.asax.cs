using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Configuration;
using FITC.Web.Component;
using System.Threading;
using System.Data;
using HtmlAgilityPack;
using System.Text;
using System.Globalization;

namespace ThongTinDoiNgoai
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            Static.sConnectString = ConfigurationManager.AppSettings["ConnectDb"];
            // Code that runs on application startup
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Thread t = new Thread(new ThreadStart(DichVuLayTin));
            //t.Start();

        }

        private void DichVuLayTin()
        {
            FITC_CDataBase db = new FITC_CDataBase(Static.sConnectString);
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8
            };
            while (true)
            {

                try
                {
                    List<object[]> dsTin = new List<object[]>();
                    DataSet dsWeb = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0, 0);
                    if (dsWeb != null && dsWeb.Tables.Count > 0 && dsWeb.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsWeb.Tables[0].Rows.Count; i++)
                        {
                            DataRow row = dsWeb.Tables[0].Rows[i];
                            DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 0, row["WebID"].ToString());
                            if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsChuyenMuc.Tables[0].Rows.Count; j++)
                                {
                                    DataRow rowCM = dsChuyenMuc.Tables[0].Rows[j];
                                    DataSet dsXpathCM = db.GetDataSet("TTDN_XPATH_CHUYENMUC_SELECT", 0, rowCM["WebID"].ToString(), rowCM["ChuyenMucID"].ToString());
                                    if (dsXpathCM != null && dsXpathCM.Tables.Count > 0 && dsXpathCM.Tables[0].Rows.Count > 0)
                                    {
                                        DataRow rowXpathCM = dsXpathCM.Tables[0].Rows[0];

                                        HtmlDocument html = htmlWeb.Load(row["DiaChiWeb"].ToString() + rowCM["UrlChuyenMuc"].ToString());
                                        var DanhSach = html.DocumentNode.SelectNodes(rowXpathCM["DanhSach"].ToString()) != null ? html.DocumentNode.SelectNodes(rowXpathCM["DanhSach"].ToString()).ToList() : null;
                                        if (DanhSach != null)
                                        {
                                            foreach (var item in DanhSach)
                                            {
                                                var TieuDe = item.SelectSingleNode(rowXpathCM["TieuDe"].ToString()) != null ? item.SelectSingleNode(rowXpathCM["TieuDe"].ToString()) : null;
                                                var TieuDePhu = rowXpathCM["TieuDePhu"].ToString() != "" ? (item.SelectSingleNode(rowXpathCM["TieuDePhu"].ToString()) != null ? item.SelectSingleNode(rowXpathCM["TieuDePhu"].ToString()) : null) : null;
                                                var TomTat = item.SelectSingleNode(rowXpathCM["TomTat"].ToString()) != null ? item.SelectSingleNode(rowXpathCM["TomTat"].ToString()) : null;
                                                var BaiViet_Url = item.SelectSingleNode(rowXpathCM["BaiViet_Url"].ToString()) != null ? item.SelectSingleNode(rowXpathCM["BaiViet_Url"].ToString()).Attributes["href"].Value.Replace("&amp;", "&") : null;

                                                if (BaiViet_Url != null)
                                                {
                                                    BaiViet_Url = (BaiViet_Url.LastIndexOf(row["DiaChiWeb"].ToString()) > -1) ? BaiViet_Url : (row["DiaChiWeb"].ToString() + BaiViet_Url);
                                                    object[] obj = new object[6];
                                                    obj[0] = TieuDe != null ? TieuDe.InnerText : null;
                                                    obj[1] = TieuDePhu != null ? TieuDePhu.InnerText : null;
                                                    obj[2] = TomTat != null ? TomTat.InnerText : null;
                                                    obj[3] = BaiViet_Url;
                                                    obj[4] = rowXpathCM["ChuyenMucID"].ToString();
                                                    obj[5] = rowXpathCM["WebID"].ToString();
                                                    dsTin.Add(obj);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (var item in dsTin)
                    {
                        DataSet dsXpathCT = db.GetDataSet("TTDN_XPATH_CHITIET_SELECT", 0, item[5].ToString(), item[4].ToString());
                        if (dsXpathCT != null && dsXpathCT.Tables.Count > 0 && dsXpathCT.Tables[0].Rows.Count > 0)
                        {
                            DataRow rowXpathCT = dsXpathCT.Tables[0].Rows[0];

                            HtmlDocument html = htmlWeb.Load(item[3].ToString());
                            var TieuDePhu = rowXpathCT["TieuDePhu"].ToString() != "" ? (html.DocumentNode.SelectSingleNode(rowXpathCT["TieuDePhu"].ToString()) != null ? html.DocumentNode.SelectSingleNode(rowXpathCT["TieuDePhu"].ToString()) : null) : null;
                            var TomTat = rowXpathCT["TomTat"].ToString() != "" ? (html.DocumentNode.SelectSingleNode(rowXpathCT["TomTat"].ToString()) != null ? html.DocumentNode.SelectSingleNode(rowXpathCT["TomTat"].ToString()) : null) : null;
                            var NoiDung = html.DocumentNode.SelectSingleNode(rowXpathCT["NoiDung"].ToString()) != null ? html.DocumentNode.SelectSingleNode(rowXpathCT["NoiDung"].ToString()) : null;
                            var ThoiGian = rowXpathCT["ThoiGian"].ToString() != "" ? (html.DocumentNode.SelectSingleNode(rowXpathCT["ThoiGian"].ToString()) != null ? html.DocumentNode.SelectSingleNode(rowXpathCT["ThoiGian"].ToString()) : null) : null;
                            var TacGia = rowXpathCT["TacGia"].ToString() != "" ? (html.DocumentNode.SelectSingleNode(rowXpathCT["TacGia"].ToString()) != null ? html.DocumentNode.SelectSingleNode(rowXpathCT["TacGia"].ToString()) : null) : null;

                            string tgian = null;
                            DateTime parsedDate;
                            if (ThoiGian != null)
                            {
                                if (DateTime.TryParseExact(ThoiGian.InnerText, rowXpathCT["DinhDangThoiGian"].ToString(), CultureInfo.GetCultureInfo("vi-VN"), DateTimeStyles.None, out parsedDate))
                                    tgian = parsedDate.ToString("yyyy-MM-dd HH:mm");
                            }

                            object[] obj = new object[10];
                            obj[0] = item[0].ToString();
                            obj[1] = TieuDePhu != null ? TieuDePhu.InnerText : null;
                            obj[2] = TomTat != null ? TomTat.InnerText : item[2].ToString();
                            obj[3] = NoiDung != null ? NoiDung.InnerHtml : null;
                            obj[4] = ThoiGian != null ? tgian : null;
                            obj[5] = TacGia != null ? TacGia.InnerText : null;
                            obj[6] = item[3].ToString();
                            obj[7] = NoiDung?.InnerHtml.Length;
                            obj[8] = item[4].ToString();
                            obj[9] = item[5].ToString();
                            string sLoi = db.ExcuteSP("TTDN_BAIVIET_INSERT", obj);
                            if (sLoi != "")
                            {
                                return;
                            }
                        }
                    }
                }
                catch  
                {
                }
                Thread.Sleep(1000 * 60 * 4);//5 phut
            }
        }
    }
}