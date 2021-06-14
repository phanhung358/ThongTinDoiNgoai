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

namespace ThongTinDoiNgoai.DichVu.HeThong.PhanQuyen
{
    public partial class PhanQuyen_TaiKhoan : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                txtTuKhoa.Attributes["onfocus"] = "NhanEnter('" + btnTim.ClientID + "','" + txtTuKhoa.ClientID + "');";
                if (!IsPostBack)
                {
                    addDonVi();
                    db.AddToCombo(db.ExcutePro("TTDN_DM_TAIKHOAN_NHOM_SELECT", 0), drpNhomQuyen, "TenNhom", "NhomID");
                    drpNhomQuyen_SelectedIndexChanged(null, null);
                }
                addDataQuyen();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void addDonVi()
        {
            drpDonVi.Items.Clear();
            drpDonVi.Items.Add(new ListItem("[Tất cả]", ""));
            DataSet ds = db.GetDataSet("TTDN_DM_DONVI_SELECT", 0);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    drpDonVi.Items.Add(new ListItem(row["MaDinhDanh"].ToString() + " - " + row["TenDonVi"].ToString(), row["MaDinhDanh"].ToString()));
                }
            }

        }
        protected void drpNhomQuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            optNhanVien.Items.Clear();
            DataSet dsNhanVien = db.GetDataSet("TTDN_DM_TAIKHOAN_SELECT", 3, drpNhomQuyen.SelectedValue, 0, drpDonVi.SelectedValue, txtTuKhoa.Text);
            if (dsNhanVien != null && dsNhanVien.Tables.Count > 0 && dsNhanVien.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsNhanVien.Tables[0].Rows.Count; i++)
                {
                    optNhanVien.Items.Add(new ListItem(dsNhanVien.Tables[0].Rows[i]["TenTaiKhoan"].ToString(), dsNhanVien.Tables[0].Rows[i]["TaiKhoanID"].ToString()));
                }
            }
        }
        private void addDataQuyen()
        {
            try
            {
                divDanhSach.Controls.Clear();
                string sNhomID = "0";
                string sTaiKhoanID = "0";
                DataSet dsTK = db.GetDataSet("[TTDN_DM_TAIKHOAN_SELECT]", 1, optNhanVien.SelectedValue);
                if (dsTK != null && dsTK.Tables.Count > 0 && dsTK.Tables[0].Rows.Count > 0)
                {
                    sNhomID = dsTK.Tables[0].Rows[0]["NhomID"].ToString();
                    sTaiKhoanID = dsTK.Tables[0].Rows[0]["TaiKhoanID"].ToString();
                }

                Table tbl = new Table();
                tbl.Width = Unit.Percentage(100);
                tbl.CssClass = "Vien_Bang";
                tbl.CellPadding = 3;
                tbl.CellSpacing = 1;
                tbl.BorderWidth = 0;

                TableRow tblRow;
                TableCell tblCell;

                tblRow = new TableRow();
                tblRow.CssClass = "Dong_TieuDe";

                tblCell = new TableCell();
                tblCell.CssClass = "Cot_TieuDe";
                tblCell.Width = 250;
                tblCell.Text = "Tên chức năng";
                tblRow.Controls.Add(tblCell);

                tblCell = new TableCell();
                tblCell.CssClass = "Cot_TieuDe";
                tblCell.Text = "Quyền";
                tblRow.Controls.Add(tblCell);

                tbl.Controls.Add(tblRow);

                using (DataSet ds = db.GetDataSet("TTDN_DM_MENU_SELECT", 4, 0, 0, 1))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow row = ds.Tables[0].Rows[i];
                            tblRow = new TableRow();
                            tblRow.CssClass = "Dong_Le Dong_Dam";

                            tblCell = new TableCell();
                            tblCell.Text = row["TenMenu"].ToString().Trim();
                            tblRow.Controls.Add(tblCell);

                            tblCell = new TableCell();
                            tblRow.Controls.Add(tblCell);
                            tbl.Controls.Add(tblRow);
                            bool bCoChucNang = false;
                            DataSet dsCap1 = db.GetDataSet("TTDN_DM_MENU_SELECT", 3, row["MenuID"].ToString(), TUONGTAC.TaiKhoanID, TUONGTAC.NhomID);
                            if (dsCap1 != null && dsCap1.Tables.Count > 0 && dsCap1.Tables[0].Rows.Count > 0)
                            {
                                for (int ii = 0; ii < dsCap1.Tables[0].Rows.Count; ii++)
                                {
                                    DataRow rowCap1 = dsCap1.Tables[0].Rows[ii];
                                    if (rowCap1["dsquyen"].ToString() == "" && rowCap1["filelienket"].ToString() != "")
                                        continue;
                                    bCoChucNang = true;
                                    tblRow = new TableRow();
                                    tblRow.CssClass = "Dong_Chan";

                                    tblCell = new TableCell();
                                    tblCell.Style.Add("padding-left", "20px");
                                    tblCell.Text = rowCap1["TenMenu"].ToString().Trim();
                                    tblRow.Controls.Add(tblCell);

                                    tblCell = new TableCell();
                                    if (rowCap1["filelienket"].ToString() != "")
                                    {
                                        string[] arrQuyen = rowCap1["dsQuyen"].ToString().Trim().Split('|');
                                        for (int k = 0; k < arrQuyen.Length; k++)
                                        {
                                            CheckBox chk = new CheckBox();
                                            chk.ID = "quyen_" + rowCap1["MenuID"].ToString() + "_" + (k + 1).ToString();
                                            chk.Checked = kiemTraCoQuyen(chk, rowCap1["MenuID"].ToString(), k + 1, sNhomID);
                                            chk.Text = arrQuyen[k];
                                            tblCell.Controls.Add(chk);
                                        }
                                    }
                                    tblRow.Controls.Add(tblCell);
                                    tbl.Controls.Add(tblRow);

                                    DataSet dsCap2 = db.GetDataSet("TTDN_DM_MENU_SELECT", 3, rowCap1["MenuID"].ToString(), TUONGTAC.TaiKhoanID, TUONGTAC.NhomID);
                                    if (dsCap2 != null && dsCap2.Tables.Count > 0 && dsCap2.Tables[0].Rows.Count > 0)
                                    {
                                        for (int iii = 0; iii < dsCap2.Tables[0].Rows.Count; iii++)
                                        {
                                            DataRow rowCap2 = dsCap2.Tables[0].Rows[iii];
                                            if (rowCap2["dsquyen"].ToString() == "")
                                                continue;
                                            tblRow = new TableRow();
                                            tblRow.CssClass = "Dong_Chan";

                                            tblCell = new TableCell();
                                            tblCell.Style.Add("padding-left", "40px");
                                            tblCell.Text = rowCap2["TenMenu"].ToString().Trim();
                                            tblRow.Controls.Add(tblCell);

                                            tblCell = new TableCell();
                                            if (rowCap2["filelienket"].ToString() != "")
                                            {
                                                string[] arrQuyen = rowCap2["dsQuyen"].ToString().Trim().Split('|');
                                                for (int k = 0; k < arrQuyen.Length; k++)
                                                {
                                                    CheckBox chk = new CheckBox();
                                                    chk.ID = "quyen_" + rowCap2["MenuID"].ToString() + "_" + (k + 1).ToString();
                                                    chk.Checked = kiemTraCoQuyen(chk, rowCap2["MenuID"].ToString(), k + 1, sNhomID);
                                                    chk.Text = arrQuyen[k];
                                                    tblCell.Controls.Add(chk);
                                                }
                                            }
                                            tblRow.Controls.Add(tblCell);
                                            tbl.Controls.Add(tblRow);
                                        }
                                    }
                                }
                            }
                            tblRow.Visible = bCoChucNang;
                        }
                    }

                }
                divDanhSach.Controls.Add(tbl);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        private bool kiemTraCoQuyen(CheckBox chk, string MenuID, int QuyenID, string NhomID)
        {
            bool bCoQuyen = false;
            if (optNhanVien.SelectedIndex != -1)
            {
                DataSet dsCap1 = db.GetDataSet("[PHANANH_QUYENCHUCNANG_SELECT]", 0, optNhanVien.SelectedValue, MenuID, QuyenID, NhomID);
                if (dsCap1 != null && dsCap1.Tables.Count > 0 && dsCap1.Tables[0].Rows.Count > 0)
                {
                    if (dsCap1.Tables[0].Rows[0]["QuyenNhom"].ToString() == "1")
                        chk.Enabled = false;
                    bCoQuyen = true;
                }
            }
            return bCoQuyen;
        }
        protected void btnPhanQuyen_Click(object sender, EventArgs e)
        {
            try
            {
                if (optNhanVien.SelectedIndex == -1)
                {
                    ham.Alert(this, "Bạn chưa chọn người phân quyền !", "btnPhanQuyen");
                    return;
                }
                using (DataSet ds = db.GetDataSet("TTDN_DM_MENU_SELECT", 4, 0, 0, null))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow row = ds.Tables[0].Rows[i];

                            DataSet dsCap1 = db.GetDataSet("TTDN_DM_MENU_SELECT", 3, row["MenuID"].ToString(), TUONGTAC.TaiKhoanID, TUONGTAC.NhomID);
                            if (dsCap1 != null && dsCap1.Tables.Count > 0 && dsCap1.Tables[0].Rows.Count > 0)
                            {
                                for (int ii = 0; ii < dsCap1.Tables[0].Rows.Count; ii++)
                                {
                                    DataRow rowCap1 = dsCap1.Tables[0].Rows[ii];
                                    if (rowCap1["dsquyen"].ToString() == "" && rowCap1["filelienket"].ToString() != "")
                                        continue;
                                    if (rowCap1["filelienket"].ToString() != "")
                                    {
                                        string[] arrQuyen = rowCap1["dsQuyen"].ToString().Trim().Split('|');
                                        for (int k = 0; k < arrQuyen.Length; k++)
                                        {
                                            CheckBox chk = (CheckBox)this.FindControl("quyen_" + rowCap1["MenuID"].ToString() + "_" + (k + 1).ToString());
                                            if (chk != null && chk.Enabled)
                                            {
                                                object[] obj = new object[5];
                                                obj[0] = optNhanVien.SelectedValue;
                                                obj[1] = 0;//NhomID
                                                obj[2] = rowCap1["MenuID"].ToString();
                                                obj[3] = k + 1;
                                                obj[4] = chk.Checked;
                                                db.ExcuteSP("TTDN_QUYENCHUCNANG_UPDATE", obj);
                                            }
                                        }
                                    }

                                    DataSet dsCap2 = db.GetDataSet("TTDN_DM_MENU_SELECT", 3, rowCap1["MenuID"].ToString(), TUONGTAC.TaiKhoanID, TUONGTAC.NhomID);
                                    if (dsCap2 != null && dsCap2.Tables.Count > 0 && dsCap2.Tables[0].Rows.Count > 0)
                                    {
                                        for (int iii = 0; iii < dsCap2.Tables[0].Rows.Count; iii++)
                                        {
                                            DataRow rowCap2 = dsCap2.Tables[0].Rows[iii];
                                            if (rowCap2["dsquyen"].ToString() == "")
                                                continue;

                                            if (rowCap2["filelienket"].ToString() != "")
                                            {
                                                string[] arrQuyen = rowCap2["dsQuyen"].ToString().Trim().Split('|');
                                                for (int k = 0; k < arrQuyen.Length; k++)
                                                {
                                                    CheckBox chk = (CheckBox)this.FindControl("quyen_" + rowCap2["MenuID"].ToString() + "_" + (k + 1).ToString());
                                                    if (chk != null && chk.Enabled)
                                                    {
                                                        object[] obj = new object[5];
                                                        obj[0] = optNhanVien.SelectedValue;
                                                        obj[1] = 0;//NhomID
                                                        obj[2] = rowCap2["MenuID"].ToString();
                                                        obj[3] = k + 1;
                                                        obj[4] = chk.Checked;
                                                        db.ExcuteSP("TTDN_QUYENCHUCNANG_UPDATE", obj);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ham.Alert(this, "Phân quyền thành công !", "btnPhanQuyen");
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnPhanQuyen");
            }
        }
        protected void optNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            addDataQuyen();
        }

        protected void btnTim_Click(object sender, EventArgs e)
        {
            drpNhomQuyen_SelectedIndexChanged(null, null);
        }
    }
}