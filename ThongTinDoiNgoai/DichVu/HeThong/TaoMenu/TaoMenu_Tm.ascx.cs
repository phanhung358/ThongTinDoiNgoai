using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FITC.Web.Component;
using System.IO;

namespace ThongTinDoiNgoai.DichVu.HeThong.TaoMenu
{
    public partial class TaoMenu_Tm : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sMenuID = "0";
        string sMenuChaID = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            btnXoa.Attributes["onclick"] = "return confirm('Bạn có chắc chắn xóa menu này không');";
            if (Request.QueryString["MenuID"] != null)
                sMenuID = Request.QueryString["MenuID"].ToString();
            if (Request.QueryString["MenuChaID"] != null)
                sMenuChaID = Request.QueryString["MenuChaID"].ToString();
            if (!IsPostBack)
            {
                MenuCha();
                drpMenuCha.SelectedValue = sMenuChaID;
                btnXoa.Visible = false;
                btnCapNhat.Visible = false;
                if (Request.QueryString["MenuID"] != null)
                {
                    napMenu();
                    btnThemMoi.Visible = false;
                    btnCapNhat.Visible = true;
                    btnXoa.Visible = true;
                }
            }
            if (Request.QueryString["cv"]!=null && Request.QueryString["cv"].ToString()=="7")
            {
                chkCoBaoCao.Enabled = false;
                chkHoatDong.Enabled = false;
                txtPath.Enabled = false;
                txtQuyen.Enabled = false;
                drpMenuCha.Enabled = false;
                btnXoa.Visible = false;
                if (Request.QueryString["MenuID"] == "244")
                    txtQuyen.Enabled = true;
            }
        }
        private void MenuCha()
        {
            DataSet ds = db.GetDataSet("TTDN_DM_MENU_SELECT", 0, 0, 0);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                drpMenuCha.Items.Add(new ListItem("[Chọn]", "0"));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    drpMenuCha.Items.Add(new ListItem(row["TenMenu"].ToString(), row["MenuID"].ToString()));
                    DataSet dsCon = db.GetDataSet("TTDN_DM_MENU_SELECT", 0, row["MenuID"].ToString(), 0);
                    if (dsCon != null && dsCon.Tables.Count > 0 && dsCon.Tables[0].Rows.Count > 0)
                    {
                        for (int ii = 0; ii < dsCon.Tables[0].Rows.Count; ii++)
                        {
                            DataRow rowCon = dsCon.Tables[0].Rows[ii];
                            drpMenuCha.Items.Add(new ListItem("----" + rowCon["TenMenu"].ToString(), rowCon["MenuID"].ToString()));
                        }
                    }
                }
            }
        }
        private void napMenu()
        {

            DataSet ds = db.GetDataSet("TTDN_DM_MENU_SELECT", 6, sMenuID);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txtTenMenu.Text = ds.Tables[0].Rows[0]["TenMenu"].ToString().Trim();
                txtPath.Text = ds.Tables[0].Rows[0]["FileLienKet"].ToString().Trim();
                txtThuTu.Text = ds.Tables[0].Rows[0]["STT"].ToString().Trim();

                if (ds.Tables[0].Rows[0]["bHoatDong"].ToString().Trim() != "")
                    chkHoatDong.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["bHoatDong"].ToString().Trim());
                txtQuyen.Text = ds.Tables[0].Rows[0]["dsQuyen"].ToString().Trim();
                db.GetItem(drpMenuCha, ds.Tables[0].Rows[0]["MenuChaID"].ToString().Trim());

            }
        }
        protected void lnkTaoMoi_Click(object sender, EventArgs e)
        {
            btnThemMoi.Visible = true;
            btnCapNhat.Visible = false;
            txtTenMenu.Text = "";
            txtPath.Text = "";
            txtThuTu.Text = "";
        }
        protected void btnThemMoi_Click(object sender, EventArgs e)
        {
            if (txtTenMenu.Text.Trim() == "")
            {
                ham.Alert(this, "Chưa nhập tên menu", "btnThemMoi");
                return;
            }
            //uploadFile(upload);
            object[] obj = new object[8];
            obj[0] = drpMenuCha.SelectedValue;
            obj[1] = txtTenMenu.Text.Trim();
            obj[2] = txtPath.Text.Trim();
            obj[3] = txtThuTu.Text.Trim();
            obj[4] = txtQuyen.Text;
            obj[5] = chkHoatDong.Checked;
            obj[6] = chkCoBaoCao.Checked;
            obj[7] = chkMacDinh.Checked;
            string sLoi = db.ExcuteSP("TTDN_DM_MENU_INSERT", obj);
            if (sLoi == "")
            {
                ham.Alert(this, "Thêm mới thành công !", "btnThemMoi");
                txtTenMenu.Text = "";
                txtPath.Text = "";
                txtThuTu.Text = "";
            }
            else
                ham.Alert(this, sLoi, "btnThemMoi");

        }
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (txtTenMenu.Text.Trim() == "")
            {
                ham.Alert(this, "Chưa nhập tên menu", "btnThemMoi");
                return;
            }
            object[] obj = new object[9];
            obj[0] = sMenuID;
            obj[1] = txtTenMenu.Text.Trim();
            obj[2] = txtPath.Text.Trim();
            obj[3] = txtThuTu.Text.Trim();
            obj[4] = txtQuyen.Text;
            obj[5] = chkHoatDong.Checked;
            obj[6] = chkCoBaoCao.Checked;
            obj[7] = chkMacDinh.Checked;
            obj[8] = drpMenuCha.SelectedValue;
            string sLoi = db.ExcuteSP("TTDN_DM_MENU_UPDATE", obj);
            if (sLoi == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.FindControl("btnThemMoi"), this.GetType(), "Message_Close", "alert('Cập nhật thành công !'); self.parent.tb_remove();", true);
            }
            else
                ham.Alert(this, sLoi, "btnThemMoi");

        }
        protected void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string sLoi = db.ExcuteSP("TTDN_DM_MENU_DELETE", sMenuID, sMenuChaID);
                if (sLoi == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.FindControl("btnThemMoi"), this.GetType(), "Message_Close", "alert('Xóa thành công !'); self.parent.tb_remove();", true);
                }
                else
                    ham.Alert(this, sLoi, "btnThemMoi");
            }
            catch { }
        }

    }
}