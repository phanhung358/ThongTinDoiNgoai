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
    public partial class PhanQuyen : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["control"] == null)
                        Session["control"] = "taikhoan";
                }
                
                switch (Session["control"].ToString())
                {
                    case "taikhoan":
                        btnTheoTaiKhoan.CssClass = "tabChuan_Select";
                        pl.Controls.Add(LoadControl(Static.AppPath() + "/dichvu/hethong/phanquyen/PhanQuyen_TaiKhoan.ascx"));
                        break;
                    case "nhom":
                        btnTheoNhom.CssClass = "tabChuan_Select";
                        pl.Controls.Add(LoadControl(Static.AppPath() + "/dichvu/hethong/phanquyen/PhanQuyen_Nhom.ascx"));
                        break;
                    case "tonghopphananh_donvi":
                        btnTongHopPhanAnh.CssClass = "tabChuan_Select";
                        pl.Controls.Add(LoadControl(Static.AppPath() + "/dichvu/hethong/phanquyen/PhanQuyen_TheoDonVi.ascx"));
                        break;              
                    case "kiemsoatquyen":
                        btnKiemSoatQuyen.CssClass = "tabChuan_Select";
                        pl.Controls.Add(LoadControl(Static.AppPath() + "/dichvu/hethong/phanquyen/PhanQuyen_KiemSoat.ascx"));
                        break;

                    default:
                        btnTheoTaiKhoan.CssClass = "tabChuan_Select";
                        pl.Controls.Add(LoadControl(Static.AppPath() + "/dichvu/hethong/phanquyen/PhanQuyen_TaiKhoan.ascx"));
                        break;
                }
            }catch(Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnTheoTaiKhoan_Click(object sender, EventArgs e)
        {
            Session["control"] = "taikhoan";
            Response.Redirect(Request.RawUrl.ToString());
        }

        protected void btnTheoNhom_Click(object sender, EventArgs e)
        {
            Session["control"] = "nhom";
            Response.Redirect(Request.RawUrl.ToString());
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

        protected void btnTheoCanhBao_Click(object sender, EventArgs e)
        {
            Session["control"] = "theocanhbao";
            Response.Redirect(Request.RawUrl.ToString());
        }
        protected void btnTheoDichVu_Click(object sender, EventArgs e)
        {
            Session["control"] = "theodichvu";
            Response.Redirect(Request.RawUrl.ToString());
        }

        protected void btnKiemSoatQuyen_Click(object sender, EventArgs e)
        {
            Session["control"] = "kiemsoatquyen";
            Response.Redirect(Request.RawUrl.ToString());
        }
    }
}