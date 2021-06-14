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
    public partial class PhanQuyen_KiemSoat : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["QuyenID"] = "0";
                Session["MenuID"] = "0";
            }
            addDataQuyen();
            addDanhSach();
        }
        private void addDataQuyen()
        {
            divDanhSach.Controls.Clear();

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
            tblCell.Width = 200;
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
                        DataSet dsCap1 = db.GetDataSet("TTDN_DM_MENU_SELECT", 4, row["MenuID"].ToString());
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
                                        RadioButton chk = new RadioButton();
                                        chk.ClientIDMode = ClientIDMode.Static;
                                        chk.Checked = false;
                                        if (rowCap1["MenuID"].ToString() == Session["menuID"].ToString() && (k+1).ToString()== Session["QuyenID"].ToString())
                                            chk.Checked = true;
                                        chk.ID = "quyen_" + rowCap1["MenuID"].ToString().Trim() + "_" + (k + 1).ToString().Trim();
                                        chk.AutoPostBack = true;
                                        chk.CheckedChanged += Chk_CheckedChanged;
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

        private void Chk_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton opt = (RadioButton)sender;
            string[] arr = opt.ID.Split('_');
            string MenuID = arr[arr.Length - 2].Trim();
            string QuyenID = arr[arr.Length - 1].Trim();
            Session["QuyenID"] = QuyenID;
            Session["MenuID"] = MenuID;
            addDataQuyen();
            addDanhSach();
        }

        private void addDanhSach()
        {
            divDanhSachNhanVien.Controls.Clear();

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
            tblCell.Width = 30;
            tblCell.Text = "STT";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 150;
            tblCell.Text = "Họ và tên";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Đơn vị";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Xóa";
            tblCell.Width = 30;
            tblRow.Controls.Add(tblCell);

            tbl.Controls.Add(tblRow);

            tblRow = new TableRow();
            tblRow.CssClass = "Dong_Le Dong_Dam";

            tblCell = new TableCell();
            tblCell.Text = "Quyền ngoài nhóm";
            tblCell.ColumnSpan = 4;
            tblRow.Controls.Add(tblCell);
            tbl.Controls.Add(tblRow);
            using (DataSet ds = db.GetDataSet("TTDN_QUYENCHUCNANG_SELECT", 5, Session["QuyenID"], Session["MenuID"]))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        tblRow = new TableRow();
                        tblRow.CssClass = "Dong_Chan";

                        tblCell = new TableCell();
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblCell.Text = (i + 1).ToString().Trim();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["TenTaiKhoan"].ToString().Trim();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["TenDonVi"].ToString().Trim();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        ImageButton imgXoa = new ImageButton();
                        imgXoa.ID = "TaiKhoanID_" + row["TaiKhoanID"].ToString();
                        imgXoa.ImageUrl = Static.AppPath() + "/images/delete.gif";
                        imgXoa.ToolTip = "Click xóa ";
                        imgXoa.CausesValidation = false;
                        imgXoa.Attributes["onclick"] = string.Format("return confirm('Bạn có chắc chắn xóa quyền của \"{0}\" này không ?');", row["TenTaiKhoan"]);
                        imgXoa.Click += ImgXoaTaiKhoan_Click;
                        tblCell.Controls.Add(imgXoa);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tbl.Controls.Add(tblRow);

                    }
                }
            }
            tblRow = new TableRow();
            tblRow.CssClass = "Dong_Le Dong_Dam";

            tblCell = new TableCell();
            tblCell.Text = "Quyền trong nhóm";
            tblCell.ColumnSpan = 4;
            tblRow.Controls.Add(tblCell);
            tbl.Controls.Add(tblRow);
            using (DataSet ds = db.GetDataSet("TTDN_QUYENCHUCNANG_SELECT", 6, Session["QuyenID"], Session["MenuID"]))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        tblRow = new TableRow();
                        tblRow.CssClass = "Dong_Chan";

                        tblCell = new TableCell();
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblCell.Text = (i + 1).ToString().Trim();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["TenNhom"].ToString().Trim();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        ImageButton imgXoa = new ImageButton();
                        imgXoa.ID = "NhomID_" + row["NhomID"].ToString();
                        imgXoa.ImageUrl = Static.AppPath() + "/images/delete.gif";
                        imgXoa.ToolTip = "Click xóa ";
                        imgXoa.CausesValidation = false;
                        imgXoa.Attributes["onclick"] = string.Format("return confirm('Bạn có chắc chắn xóa quyền trong nhóm {0} này không ?');", row["TenNhom"]);
                        imgXoa.Click += ImgXoaNhom_Click;
                        tblCell.Controls.Add(imgXoa);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tbl.Controls.Add(tblRow);

                    }
                }
            }
            divDanhSachNhanVien.Controls.Add(tbl);
        }
        private void ImgXoaTaiKhoan_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btnImg = (ImageButton)sender;
            string TaiKhoanID = btnImg.ID.Remove(0, btnImg.ID.LastIndexOf('_') + 1);
            db.ExcuteSP("TTDN_QUYENCHUCNANG_DELETE", 0, TaiKhoanID, Session["MenuID"], Session["QuyenID"]);
            addDanhSach();
        }
        private void ImgXoaNhom_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btnImg = (ImageButton)sender;
            string NhomID = btnImg.ID.Remove(0, btnImg.ID.LastIndexOf('_') + 1);

            db.ExcuteSP("TTDN_QUYENCHUCNANG_DELETE", 1, NhomID, Session["MenuID"], Session["QuyenID"]);
            addDanhSach();
        }
    }
}