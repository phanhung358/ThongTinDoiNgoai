using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using FITC.Web.Component;
using System.Data;
using System.Configuration;

namespace ThongTinDoiNgoai.DichVu.HeThong.PhanQuyen
{
    public partial class ThietLapQuyTrinh : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["control"] == null)
                    Session["control"] = "tonghopphananh";
                if (ConfigurationManager.AppSettings["TaoTaiKhoanRieng"] != null)
                {
                    btnCanhBao.Visible = false;
                    btnXemQuetThe.Visible = false;
                }
            }
            switch (Session["control"].ToString())
            {
                case "tonghopphananh_donvi":
                    btnTongHopPhanAnh.CssClass = "tabChuan_Select";
                    pl.Controls.Add(LoadControl(Static.AppPath() + "/dichvu/hethong/phanquyen/QT_TongHopPhanAnh.ascx"));
                    break;
                case "tonghopphananh_linhvuc":
                    btnTongHopPhanAnhTheoLinhVuc.CssClass = "tabChuan_Select";
                    pl.Controls.Add(LoadControl(Static.AppPath() + "/dichvu/hethong/phanquyen/QT_TongHopPhanAnh_LV.ascx"));
                    break;

                case "canhbao":
                    btnCanhBao.CssClass = "tabChuan_Select";
                    pl.Controls.Add(LoadControl(Static.AppPath() + "/dichvu/hethong/phanquyen/QT_CanhBao_ChuyenMuc.ascx"));
                    break;
                case "xemquetthe":
                    btnXemQuetThe.CssClass = "tabChuan_Select";
                    pl.Controls.Add(LoadControl(Static.AppPath() + "/dichvu/hethong/phanquyen/QuyTrinhXemChamCong.ascx"));
                    break;
                default:
                    btnTongHopPhanAnh.CssClass = "tabChuan_Select";
                    pl.Controls.Add(LoadControl(Static.AppPath() + "/dichvu/hethong/phanquyen/QT_TongHopPhanAnh.ascx"));
                    break;
            }
        }

        protected void btnTongHopPhanAnh_Click(object sender, EventArgs e)
        {
            Session["control"] = "tonghopphananh_donvi";
            Response.Redirect(Request.RawUrl.ToString());
        }

        protected void btnXemQuetThe_Click(object sender, EventArgs e)
        {
            Session["control"] = "xemquetthe";
            Response.Redirect(Request.RawUrl.ToString());
        }

        protected void btnTongHopPhanAnhTheoLinhVuc_Click(object sender, EventArgs e)
        {
            Session["control"] = "tonghopphananh_linhvuc";
            Response.Redirect(Request.RawUrl.ToString());
        }

        protected void btnCanhBao_Click(object sender, EventArgs e)
        {
            Session["control"] = "canhbao";
            Response.Redirect(Request.RawUrl.ToString());
        }


    }
}