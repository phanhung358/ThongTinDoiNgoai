using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FITC.Web.Component;
using System.Data;

namespace ThongTinDoiNgoai.DichVu.HeThong.TaiKhoan
{
    public partial class QuyenNgoaiNhom : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string TaiKhoanID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["TaiKhoanID"] != null)
                TaiKhoanID = Request.QueryString["TaiKhoanID"];

            addDataQuyen();
        }
        private void addDataQuyen()
        {
            divDanhSach.Controls.Clear();
            string sNhomID = "0";
            string sTaiKhoanID = "0";
            DataSet dsTK = db.GetDataSet("[TTDN_DM_TAIKHOAN_SELECT]", 1, TaiKhoanID);
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
                        DataSet dsCap1 = db.GetDataSet("TTDN_DM_MENU_SELECT", 3, row["MenuID"].ToString(), TaiKhoanID, 0);
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
                            }
                        }
                        tblRow.Visible = bCoChucNang;
                    }
                }

            }
            divDanhSach.Controls.Add(tbl);
        }

        private bool kiemTraCoQuyen(CheckBox chk, string MenuID, int QuyenID, string NhomID)
        {
            bool bCoQuyen = false;

            DataSet dsCap1 = db.GetDataSet("[PHANANH_QUYENCHUCNANG_SELECT]", 0, TaiKhoanID, MenuID, QuyenID, NhomID);
            if (dsCap1 != null && dsCap1.Tables.Count > 0 && dsCap1.Tables[0].Rows.Count > 0)
            {
                if (dsCap1.Tables[0].Rows[0]["QuyenNhom"].ToString() == "1")
                    chk.Enabled = false;
                bCoQuyen = true;
            }
            return bCoQuyen;
        }
    }
}