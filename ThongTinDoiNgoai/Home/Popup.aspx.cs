using System;
using System.Configuration;
using System.Web.UI;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FITC.Web.Component;

namespace ThongTinDoiNgoai
{
    public partial class Popup : System.Web.UI.Page
    {
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (TUONGTAC.TaiKhoanID == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this.FindControl("btnCapNhat"), this.GetType(), "Message_Close", "self.parent.tb_remove();", true);
                }
                if (!KiemTraLienKet())
                    Response.End();

                var ctl = Request.Params["control"];
                if (!ham.ChuoiHopLe(ctl))
                    Response.End();
                LoadUserControl(ctl);
                ctl = ctl.ToLower();
                if (ctl.EndsWith("canhbao_tm.ascx") || ctl.EndsWith("tiepnhan_tm.ascx") || ctl.EndsWith("lapkehoach_tm.ascx") || ctl.EndsWith("quanlyvideo_tm.ascx") || (Session["control"] != null && Session["control"].ToString().Contains("chinhsua")))
                {
                    StringBuilder str = new StringBuilder();
                    str.AppendFormat("<div id=\"map_vien\" style=\"display: none;z-index: 3;\">");

                    str.AppendFormat("<input id=\"pac-input\" class=\"pac-input controls\" type=\"text\" placeholder=\"Nhập địa chỉ\" />");

                    str.AppendFormat("<div class=\"map_close\">");
                    str.AppendFormat("<img src=\"../Images/close.gif\" title=\"Đóng bản đồ\" onclick=\"HienBanDo(-1);\"/>");
                    str.AppendFormat("</div>");

                    str.AppendFormat("<div id=\"map\" style=\"width: 97.6%; height:200px; position: absolute;z-index: 2; \"></div>");
                    str.AppendFormat("</div>");
                    lblMap.Text = str.ToString();
                    if (ctl.EndsWith("tiepnhan_tm.ascx") || ctl.EndsWith("quanlyvideo_tm.ascx"))
                        Popbody.Attributes["onload"] = string.Format("initialize({0},{1});", ConfigurationManager.AppSettings["ViDo"], ConfigurationManager.AppSettings["KinhDo"]);
                    else
                        Popbody.Attributes["onload"] = "initialize();";
                }
            }
            catch {}
        }
        private void LoadUserControl(string sDuongDan, params object[] objThamSo)
        {
            try
            {
                phMain.Controls.Clear();
                Control ctr = LoadControl(Request.ApplicationPath + "/" + sDuongDan, objThamSo);
                phMain.Controls.Add(ctr);
            }catch(Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        private UserControl LoadControl(string userControlPath, params object[] constructorParameters)
        {
            List<Type> constParamTypes = new List<Type>();
            foreach (object constParam in constructorParameters)
            {
                constParamTypes.Add(constParam.GetType());
            }

            UserControl ctl = Page.LoadControl(userControlPath) as UserControl;

            // Find the relevant constructor
            ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

            // And then call the relevant constructor
            if (constructor == null)
            {
                throw new MemberAccessException("The requested constructor was not found on: " + ctl.GetType().BaseType.ToString());
            }
            else
            {
                constructor.Invoke(ctl, constructorParameters);
            }

            //Finally return the fully initialized UC
            return ctl;
        }

        private bool KiemTraLienKet()
        {
            return true;
            FITC_CNumber num = new FITC_CNumber();
            FITC_CDataTime dt = new FITC_CDataTime();
            string[] arr_int = new string[] { "bl", "pa", "article_id" };
            for (int i = 0; i < arr_int.Length; i++)
            {
                if (Request.QueryString[arr_int[i]] != null && (Request.QueryString[arr_int[i]].ToString().Length > 20 || !num.isNumber(Request.QueryString[arr_int[i]].ToString())))
                    return false;
            }
            return true;
        }
    }
}