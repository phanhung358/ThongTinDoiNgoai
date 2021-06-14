using System;
using System.Web.UI.WebControls;
using System.Data;
using FITC.Web.Component;
using System.Text;
using EsbUsers.Sso;
using System.Configuration;

namespace ThongTinDoiNgoai
{
    public partial class Default : System.Web.UI.Page
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                btnSuKien.Style.Add("display", "none");

                Session.Timeout = 100;

                if (Session["DangNhap_BT"] != null && Session["DangNhap_BT"].ToString() == "true")
                {
                    goto TiepTuc;
                }

                ClientSso.Ins.XacThucNguoiDung();
                if (Session["DangNhap_BT"] == null && ClientSso.Ins.CurrentSessionLoginInfo == null)
                {
                    Response.Redirect(Static.AppPath() + "/home/dangnhap.aspx", false);
                }
                else
                {
                    if (Session["DangNhap_BT"] == null)
                    {
                        Response.Redirect(Static.AppPath() + "/home/dangnhap.aspx", false);
                    }
                    if (Session["DangNhap_BT"] != null && Session["DangNhap_BT"].ToString() == "false" && ClientSso.Ins.CurrentSessionLoginInfo == null)
                    {
                        Response.Redirect(Static.AppPath() + "/home/dangnhap.aspx", false);
                    }
                }


            TiepTuc:
                if (!KiemTraLienKet())
                {
                    Response.Redirect(Static.AppPath() + "/home");
                    return;
                }
                if (!IsPostBack)
                    addDichVu();


                addMenu();
                //======================================
                string sThoatID = "";
                DataSet dsMenu = db.GetDataSet("TTDN_DM_MENU_SELECT", 2, 0, 0, 0, "hethong\\dangxuat.ascx");
                if (dsMenu != null && dsMenu.Tables.Count > 0 && dsMenu.Tables[0].Rows.Count > 0)
                {
                    sThoatID = dsMenu.Tables[0].Rows[0]["MenuID"].ToString().Trim();
                }
                if (TUONGTAC.getTenDonVi() != "")
                    divTenDonVi.InnerHtml = string.Format("<a href='?'>Trang chủ</a>") + " :: " + string.Format("{0}", TUONGTAC.TenTaiKhoan) + string.Format(" :: <a href='?id={0}'>Thoát</a>", sThoatID);
                else
                    divTenDonVi.InnerHtml = TUONGTAC.TenDangNhap + string.Format(" :: <a href='?id={0}'>Thoát</a>", sThoatID);
                string sCatID = "0";
                if (Request.QueryString["id"] != null)
                {
                    sCatID = Request.QueryString["id"].ToString();
                    if (sCatID != "0")
                    {
                        if (!kiemTraQuyen())
                            Response.End();
                        DataSet ds = db.GetDataSet("TTDN_DM_MENU_SELECT", 1, sCatID);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            string s = Static.AppPath() + "/dichvu/" + ds.Tables[0].Rows[0]["FileLienKet"].ToString().Trim().ToLower();
                            divMain.Controls.Add(LoadControl(s));
                        }
                    }
                    divMain.Attributes["class"] = "Vung_Chinh";
                }
                else
                {
                    drpMenuCha_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        private void addDichVu()
        {
            drpMenuCha.Items.Clear();
            DataSet dsMenuCap1 = db.GetDataSet("TTDN_DM_MENU_SELECT", 5, 0, TUONGTAC.TaiKhoanID, TUONGTAC.NhomID);
            if (dsMenuCap1 != null && dsMenuCap1.Tables.Count > 0 && dsMenuCap1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow rowCap1 in dsMenuCap1.Tables[0].Rows)
                {
                    drpMenuCha.Items.Add(new ListItem(rowCap1["TenMenu"].ToString(), rowCap1["MenuID"].ToString()));
                }
            }
            if (Session["MenuCha"] != null)
                drpMenuCha.SelectedValue = Session["MenuCha"].ToString();
            else
                drpMenuCha.SelectedIndex = 1;
        }
        private void addMenu()
        {
            StringBuilder str = new StringBuilder();
            DataSet dsMenuCap2 = db.GetDataSet("TTDN_DM_MENU_SELECT", 3, drpMenuCha.SelectedValue, TUONGTAC.TaiKhoanID, TUONGTAC.NhomID);
            if (dsMenuCap2 != null && dsMenuCap2.Tables.Count > 0 && dsMenuCap2.Tables[0].Rows.Count > 0)
            {
                str.Append("<ul>");
                foreach (DataRow rowCap2 in dsMenuCap2.Tables[0].Rows)
                {
                    string cssActive = "";
                    if (rowCap2["Menuid"].ToString() == Request.QueryString["id"])
                        cssActive = "active";

                    str.AppendFormat("<li><a href='?id={1}' class='{2}'>{0}</a></li>", rowCap2["Tenmenu"].ToString(), rowCap2["MenuID"].ToString(), cssActive);

                }
                str.Append("</ul>");
            }

            divMenuMoi.InnerHtml = str.ToString();
        }
        private bool kiemTraQuyen()
        {
            object[] objQuyen = null;
            objQuyen = TUONGTAC.getQuyen();

            bool bCoQuyen = false;

            if ((bool)objQuyen[(int)TUONGTAC.LoaiQuyen.Quyen1])
                bCoQuyen = true;
            if ((bool)objQuyen[(int)TUONGTAC.LoaiQuyen.Quyen2])
                bCoQuyen = true;
            if ((bool)objQuyen[(int)TUONGTAC.LoaiQuyen.Quyen3])
                bCoQuyen = true;
            if ((bool)objQuyen[(int)TUONGTAC.LoaiQuyen.Quyen4])
                bCoQuyen = true;
            if ((bool)objQuyen[(int)TUONGTAC.LoaiQuyen.Quyen5])
                bCoQuyen = true;
            return bCoQuyen;
        }
        private bool KiemTraLienKet()
        {
            FITC_CNumber num = new FITC_CNumber();
            string[] arr_int = new string[] { "id" };
            for (int i = 0; i < arr_int.Length; i++)
            {
                if (Request.QueryString[arr_int[i]] != null && (Request.QueryString[arr_int[i]].ToString().Length > 10 || !num.isNumber(Request.QueryString[arr_int[i]].ToString())))
                    return false;
            }
            return true;
        }
        protected void drpMenuCha_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["LoaiDanhMuc"] = null;
                Session["MenuCha"] = drpMenuCha.SelectedValue;
                db.ExcuteSP("TTDN_DM_TAIKHOAN_UPDATE_DICHVU", TUONGTAC.TaiKhoanID, drpMenuCha.SelectedValue);
                addMenu();
                string id = "";
                if (Request.QueryString["id"] != null)
                    Response.Redirect(Static.AppPath() + "/home/", false);
                else
                {
                    DataSet dsMenuCap2 = db.GetDataSet("TTDN_DM_MENU_SELECT", 3, drpMenuCha.SelectedValue, TUONGTAC.TaiKhoanID, TUONGTAC.NhomID);
                    if (dsMenuCap2 != null && dsMenuCap2.Tables.Count > 0 && dsMenuCap2.Tables[0].Rows.Count > 0)
                    {
                        id = dsMenuCap2.Tables[0].Rows[0]["MenuID"].ToString();
                    }

                    if (id != "")
                        Response.Redirect(Static.AppPath() + "/home/?id=" + id, false);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        protected void btnSuKien_Click(object sender, EventArgs e)
        {
            //addDanhSachCanhBao();
        }
    }
}