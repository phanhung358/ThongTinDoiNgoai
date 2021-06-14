using FITC.Web.Component;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyVanBan.DichVu.DuLieu
{
    public partial class XpathChuyenMuc_Tm_Copy : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sWebID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["WebID"] != null)
                sWebID = Request.QueryString["WebID"];

            if (!IsPostBack)
            {
                db.AddToComboChon(db.ExcutePro("TTDN_TRANGWEB_SELECT"), drpWeb, "TenWeb", "WebID");
                db.GetItem(drpWeb, sWebID);
            }
        }
        private string KiemTra()
        {
            FITC_CDataTime dt = new FITC_CDataTime();
            string sLoi = "";
            if (drpWeb.SelectedValue == "0")
                sLoi = "Chưa chọn trang web!";
            if (drpChuyenMuc.SelectedValue == "0")
                sLoi = "Chưa chọn chuyên mục!";
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
                object[] obj = new object[4];
                obj[0] = drpChuyenMuc.SelectedValue;
                obj[1] = Request.QueryString["ChuyenMucID"];
                obj[2] =  Request.QueryString["WebID"];
                obj[3] = TUONGTAC.TenTaiKhoan;
                Response.Write(drpChuyenMuc.SelectedValue);

                string sLoi = db.ExcuteSP("TTDN_XPATH_CHUYENMUC_INSERT_COPY", obj);
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
            db.AddToComboChon(db.ExcutePro("TTDN_CHUYENMUC_SELECT", 2, drpWeb.SelectedValue), drpChuyenMuc, "TenChuyenMuc", "ChuyenMucID");
        }

    }
}