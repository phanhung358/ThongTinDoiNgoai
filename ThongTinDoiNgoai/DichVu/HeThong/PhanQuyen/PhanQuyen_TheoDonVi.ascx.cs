using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using FITC.Web.Component;
using System.Data;

namespace ThongTinDoiNgoai.DichVu.HeThong.PhanQuyen
{
    public partial class PhanQuyen_TheoDonVi : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            chkTatCa.Attributes["onclick"] = "checkall(this,'Cha_')";
            if (!IsPostBack)
            {
                addDonVi();
                db.AddToCombo(db.ExcutePro("TTDN_DM_TAIKHOAN_NHOM_SELECT", 0), drpNhomQuyen, "TenNhom", "NhomID");
                napChucNang();
            }
            loadPhongBan();
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
        private void napChucNang()
        {
            optChucNang.Items.Clear();
            DataSet ds = db.GetDataSet("TTDN_DM_MENU_SELECT", 6, 371);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    optChucNang.Items.Add(new ListItem(row["TenMenu"].ToString(), row["MenuID"].ToString()));
                }
            }
            if (optChucNang.Items.Count > 0)
            {
                optChucNang.SelectedIndex = 0;
                optChucNang_SelectedIndexChanged(null, null);
            }
        }
        private void loadPhongBan()
        {
            string sPath = Static.AppPath();
            string sMenuID = optChucNang.SelectedValue;
            lblDs_QuyenTT.Controls.Clear();
            string sTenThongTin = "TenDonVi", sThongTinID = "MaDinhDanh";
            DataSet dsCha = db.GetDataSet("TTDN_DM_DONVI_SELECT", 4, 1);
            if (dsCha != null && dsCha.Tables.Count > 0 && dsCha.Tables[0].Rows.Count > 0)
            {
                int countRow = dsCha.Tables[0].Rows.Count;
                Table tblTableCha = new Table();
                tblTableCha.Attributes["border"] = "0";
                tblTableCha.Width = Unit.Percentage(100);
                tblTableCha.Attributes["cellpadding"] = "0";
                tblTableCha.Attributes["cellspacing"] = "1";
                TableRow tblRowCha;
                TableCell tblCellCha;

                for (int i = 0; i < countRow; i++)
                {
                    DataRow rowCha = dsCha.Tables[0].Rows[i];
                    tblRowCha = new TableRow();
                    tblRowCha.Attributes["class"] = "Dong_Chan";

                    string sNhanVienID = optQuyenTT.SelectedValue;
                    tblCellCha = new TableCell();
                    CheckBox ckbCha = new CheckBox();
                    ckbCha.ID = "Cha_" + i.ToString().Trim();
                    ckbCha.Attributes["onclick"] = "IsCheck('" + this.ClientID + "_" + ckbCha.ID + "')";
                    if (CheckPermission(sMenuID, sNhanVienID, rowCha[sThongTinID].ToString(), "dv"))
                        ckbCha.Checked = true;
                    ckbCha.Text = rowCha[sTenThongTin].ToString();
                    tblCellCha.Controls.Add(ckbCha);
                    tblRowCha.Controls.Add(tblCellCha);
                    tblTableCha.Controls.Add(tblRowCha);

                    DataSet dsCon = db.GetDataSet("TTDN_DM_DONVI_SELECT", 4, rowCha["DonViID"].ToString());
                    if (dsCon != null && dsCon.Tables.Count > 0 && dsCon.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dsCon.Tables[0].Rows.Count; j++)
                        {
                            DataRow rowCon = dsCon.Tables[0].Rows[j];
                            tblRowCha = new TableRow();
                            tblRowCha.Attributes["class"] = "Dong_Chan";

                            tblCellCha = new TableCell();
                            tblCellCha.Style.Add("padding-left", "20px");
                            ckbCha = new CheckBox();
                            ckbCha.ID = "Cha_" + i.ToString().Trim() + "_Con_" + j.ToString();
                            ckbCha.Attributes["onclick"] = "IsCheck('" + this.ClientID + "_" + ckbCha.ID + "')";
                            if (CheckPermission(sMenuID, sNhanVienID, rowCon[sThongTinID].ToString(), "dv"))
                                ckbCha.Checked = true;
                            ckbCha.Text = rowCon[sTenThongTin].ToString();
                            tblCellCha.Controls.Add(ckbCha);
                            tblRowCha.Controls.Add(tblCellCha);
                            tblTableCha.Controls.Add(tblRowCha);
                        }
                    }
                }
                lblDs_QuyenTT.Controls.Add(tblTableCha);
            }
        }
        private bool CheckPermission(string sMenuID, string sNhanVienID, string ThongTinID, string tukhoa)
        {
            DataSet ds = db.GetDataSet("TTDN_QUYTRINHXULY_SELECT", 0, sNhanVienID, ThongTinID, sMenuID, tukhoa);
            if (ds == null)
                return false;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                return false;
            return true;

        }
        private void LuuQuyen_TT()
        {
            try
            {
                for (int ii = 0; ii < optQuyenTT.Items.Count; ii++)
                {
                    if (!optQuyenTT.Items[ii].Selected)
                        continue;

                    string sNhanVienID = optQuyenTT.Items[ii].Value;
                    string sMenuID = optChucNang.SelectedValue;
                    string spName = "";
                    string sThongTinID = "MaDinhDanh";
                    DataSet dsCha = db.GetDataSet("TTDN_DM_DONVI_SELECT", 4, 1);
                    if (dsCha != null && dsCha.Tables.Count > 0 && dsCha.Tables[0].Rows.Count > 0)
                    {
                        int countRow = dsCha.Tables[0].Rows.Count;
                        for (int i = 0; i < countRow; i++)
                        {
                            DataRow rowCha = dsCha.Tables[0].Rows[i];
                            CheckBox ckb = (CheckBox)this.FindControl("Cha_" + i.ToString().Trim());
                            if (ckb != null)
                            {
                                object[] obj = new object[4];
                                obj[0] = sNhanVienID;
                                obj[1] = sMenuID;
                                obj[2] = rowCha[sThongTinID].ToString();
                                obj[3] = "dv";
                                if (!ckb.Checked)
                                    spName = "TTDN_QUYTRINHXULY_DELETE";
                                else
                                    spName = "TTDN_QUYTRINHXULY_INSERT";
                                db.ExcutePro(spName, obj);
                            }

                            DataSet dsCon = db.GetDataSet("TTDN_DM_DONVI_SELECT", 4, rowCha["DonViID"].ToString());
                            if (dsCon != null && dsCon.Tables.Count > 0 && dsCon.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsCon.Tables[0].Rows.Count; j++)
                                {
                                    DataRow rowCon = dsCon.Tables[0].Rows[j];

                                    ckb = (CheckBox)this.FindControl("Cha_" + i.ToString().Trim() + "_Con_" + j.ToString());
                                    if (ckb != null)
                                    {
                                        object[] obj = new object[4];
                                        obj[0] = sNhanVienID;
                                        obj[1] = sMenuID;
                                        obj[2] = rowCon[sThongTinID].ToString();
                                        obj[3] = "dv";
                                        if (!ckb.Checked)
                                            spName = "TTDN_QUYTRINHXULY_DELETE";
                                        else
                                            spName = "TTDN_QUYTRINHXULY_INSERT";
                                        db.ExcutePro(spName, obj);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            ham.Alert(this, "Phân quyền thành công !", "btnPhanQuyen");
        }

        protected void drpNhomQuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            optChucNang_SelectedIndexChanged(null, null);
        }
        protected void optQuyenTT_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadPhongBan();
        }
        protected void btnPhanQuyen_Click(object sender, EventArgs e)
        {
            if (optQuyenTT.SelectedIndex == -1)
            {
                ham.Alert(this, "Bạn chưa chọn nhân viên để phân quyền", "btnPhanQuyen");
                return;
            }
            LuuQuyen_TT();
        }
        protected void optChucNang_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sTenHam = "";
            DataSet ds = db.GetDataSet("TTDN_DM_MENU_SELECT", 1, optChucNang.SelectedValue);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                sTenHam = ds.Tables[0].Rows[0]["FileLienKet"].ToString().ToLower().Trim().ToLower();
            }
            DataSet dsNhanVien = null;
            switch (sTenHam)
            {
                case "phananh/thongke/tonghop.ascx":
                    dsNhanVien = db.GetDataSet("TTDN_DONVIBOPHAN_SELECT", 0, drpNhomQuyen.SelectedValue, optChucNang.SelectedValue,"", drpDonVi.SelectedValue);
                    break;
                case "phananh/thongke/thongkexuly.ascx":
                    dsNhanVien = db.GetDataSet("TTDN_DONVIBOPHAN_SELECT", 1, drpNhomQuyen.SelectedValue, optChucNang.SelectedValue,"", drpDonVi.SelectedValue);
                    break;
            }
            optQuyenTT.Items.Clear();
            if (dsNhanVien != null && dsNhanVien.Tables.Count > 0 && dsNhanVien.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsNhanVien.Tables[0].Rows.Count; i++)
                {
                    optQuyenTT.Items.Add(new ListItem(dsNhanVien.Tables[0].Rows[i]["TenTaiKhoan"].ToString(), dsNhanVien.Tables[0].Rows[i]["TaiKhoanID"].ToString()));
                }
            }
        }

        protected void chkTatCa1_CheckedChanged(object sender, EventArgs e)
        {
            for (int ii = 0; ii < optQuyenTT.Items.Count; ii++)
            {
                optQuyenTT.Items[ii].Selected = chkTatCa1.Checked;
            }
        }
    }
}