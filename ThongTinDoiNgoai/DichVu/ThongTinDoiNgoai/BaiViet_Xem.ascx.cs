﻿using FITC.Web.Component;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
                            string strSource = file.Attributes["src"].Value;
                            string fileName = strSource.Substring(strSource.LastIndexOf("/") + 1).Replace("%20", "_").Replace(" ", "_");
                            if (fileName.Contains("?"))
                            {
                                fileName = fileName.Replace(fileName.Substring(fileName.IndexOf("?"), fileName.IndexOf("=") - fileName.IndexOf("?") + 1), "_");
                                if (fileName.Contains("&"))
                                    fileName = fileName.Replace(fileName.Substring(fileName.IndexOf("&"), fileName.IndexOf("=") - fileName.IndexOf("&") + 1), "_");
                            }
                            if (!fileName.Contains("."))
                                fileName += ".jpg";
                            string strSourceRep = DirUpload + fileName;
                            string img = file.OuterHtml.Replace(file.Attributes["src"].Value, strSourceRep);
                            Bitmap image = new Bitmap(Server.MapPath(strSourceRep), true);
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
                                if (file.Attributes["width"] != null && Convert.ToInt32(file.Attributes["width"].Value) > 920)
                                    img = img.Replace(file.Attributes["width"].Value, "920");
                                else if(image.Width > 500)
                                    img = img.Replace(">", " width=\"920\">");
                                if (file.Attributes["height"] != null)
                                    img = img.Replace(file.Attributes["height"].Value, "auto");
                            }
                            

                            NoiDung.DocumentNode.InnerHtml = NoiDung.DocumentNode.InnerHtml.Replace(file.OuterHtml, img);
                        }
                    }
                }

                str.AppendFormat("<div class='bodytext margin-bottom-lg'>{0}</div>", NoiDung.DocumentNode.InnerHtml);
                str.AppendFormat("<div class='margin-bottom-lg'><p class='h5 text-right'>{0}</p></div>", row["TacGia"].ToString());

                divDanhSach.InnerHtml = str.ToString();
            }
        }
    }
}