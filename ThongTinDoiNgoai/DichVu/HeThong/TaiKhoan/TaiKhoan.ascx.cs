using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FITC.Web.Component;
using System.Data;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Configuration;

namespace ThongTinDoiNgoai.DichVu.HeThong.TaiKhoan
{
    public partial class TaiKhoan : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                addDonVi();
                Static.PhanTrangThu = 1;
                Session["TuKhoa"] = "";
            }
            imgThemMoi.Attributes["onclick"] = string.Format("return thickboxPopup('Thêm mới tài khoản','{0}?btn={1}&control={2}', 650, '100%');",
                ResolveUrl("~/home/popup.aspx"), btnSuKien.ClientID, "/DichVu/hethong/TaiKhoan/TaiKhoan_TM.ascx");
            addData();
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
        private void addData()
        {
            try
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
                tblCell.Text = "STT";
                tblCell.Width = 30;
                tblRow.Controls.Add(tblCell);

                tblCell = new TableCell();
                tblCell.CssClass = "Cot_TieuDe";
                tblCell.Text = "Tên tài khoản";
                tblCell.Width = 150;
                tblRow.Controls.Add(tblCell);

                tblCell = new TableCell();
                tblCell.CssClass = "Cot_TieuDe";
                tblCell.Text = "Tên đăng nhập";
                tblCell.Width = 100;
                tblRow.Controls.Add(tblCell);

                tblCell = new TableCell();
                tblCell.CssClass = "Cot_TieuDe";
                tblCell.Text = "Chức vụ";
                tblCell.Width = 100;
                tblRow.Controls.Add(tblCell);

                tblCell = new TableCell();
                tblCell.CssClass = "Cot_TieuDe";
                tblCell.Text = "Số điện thoại";
                tblCell.Width = 80;
                tblRow.Controls.Add(tblCell);

                tblCell = new TableCell();
                tblCell.CssClass = "Cot_TieuDe";
                tblCell.Text = "Quyền thêm";
                tblCell.Width = 80;
                tblRow.Controls.Add(tblCell);

                tblCell = new TableCell();
                tblCell.CssClass = "Cot_TieuDe";
                tblCell.Text = "Tên đơn vị";
                tblRow.Controls.Add(tblCell);

                tblCell = new TableCell();
                tblCell.CssClass = "Cot_TieuDe";
                tblCell.Text = "Sửa";

                tblCell.Width = 30;
                tblRow.Controls.Add(tblCell);

                tblCell = new TableCell();
                tblCell.CssClass = "Cot_TieuDe";
                tblCell.Text = "Xóa";
                tblCell.Width = 30;
                tblRow.Controls.Add(tblCell);
                tbl.Controls.Add(tblRow);

                using (DataSet dsNhomQuyen = db.GetDataSet("TTDN_DM_TAIKHOAN_NHOM_SELECT", 4))
                {
                    if (dsNhomQuyen != null && dsNhomQuyen.Tables.Count > 0 && dsNhomQuyen.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < dsNhomQuyen.Tables[0].Rows.Count; k++)
                        {
                            DataRow rowNQ = dsNhomQuyen.Tables[0].Rows[k];
                            DataSet ds = db.GetDataSet("TTDN_DM_TAIKHOAN_SELECT", 7, rowNQ["NhomID"].ToString(), 0, drpDonVi.SelectedValue, Session["TuKhoa"]);
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                tblRow = new TableRow();
                                tblRow.CssClass = "Dong_Le Dong_Dam";

                                tblCell = new TableCell();
                                tblCell.ColumnSpan = 9;
                                tblCell.Text = rowNQ["TenNhom"].ToString().Trim();
                                tblRow.Controls.Add(tblCell);

                                tbl.Controls.Add(tblRow);

                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {

                                    DataRow row = ds.Tables[0].Rows[i];
                                    tblRow = new TableRow();
                                    tblRow.CssClass = "Dong_Chan";

                                    tblCell = new TableCell();
                                    tblCell.Text = (i + 1).ToString();
                                    tblCell.HorizontalAlign = HorizontalAlign.Center;
                                    tblRow.Controls.Add(tblCell);

                                    tblCell = new TableCell();
                                    tblCell.Text = row["TenTaiKhoan"].ToString().Trim();
                                    tblRow.Controls.Add(tblCell);

                                    tblCell = new TableCell();
                                    tblCell.Text = row["TenDangNhap"].ToString().Trim();
                                    tblRow.Controls.Add(tblCell);

                                    tblCell = new TableCell();
                                    tblCell.Text = row["ChucVu"].ToString().Trim();
                                    tblRow.Controls.Add(tblCell);

                                    tblCell = new TableCell();
                                    tblCell.Text = row["SoDienThoai"].ToString().Trim();
                                    tblRow.Controls.Add(tblCell);

                                    tblCell = new TableCell();
                                    tblCell.HorizontalAlign = HorizontalAlign.Center;
                                    if (row["SoQuyenThem"].ToString() != "0")
                                    {
                                        LinkButton lnk = new LinkButton();
                                        lnk.Text = row["SoQuyenThem"].ToString().Trim();
                                        lnk.Attributes["onclick"] = string.Format("return thickboxPopup('Danh sách quyền phân thêm ngoài nhóm','{0}?control={1}&TaiKhoanID={2}', 650, '100%');",
                                          ResolveUrl("~/home/popup.aspx"), "/DichVu/hethong/TaiKhoan/QuyenNgoaiNhom.ascx", row["TaiKhoanID"].ToString());
                                        tblCell.Controls.Add(lnk);
                                    }
                                    tblRow.Controls.Add(tblCell);

                                    tblCell = new TableCell();
                                    tblCell.Text = row["TenDonVi"].ToString().Trim();
                                    tblRow.Controls.Add(tblCell);

                                    tblCell = new TableCell();
                                    ImageButton imgSua = new ImageButton();
                                    imgSua.ID = "Sua_" + row["TaiKhoanID"].ToString();
                                    imgSua.ToolTip = "Click để sửa";
                                    imgSua.ImageUrl = Static.AppPath() + "/images/edit.gif";
                                    imgSua.Attributes["onclick"] = string.Format("return thickboxPopup('Sửa thông tin tài khoản','{0}?btn={1}&control={2}&TaiKhoanID={3}', 650, '100%');",
                                        ResolveUrl("~/home/popup.aspx"), btnSuKien.ClientID, "/DichVu/hethong/TaiKhoan/TaiKhoan_TM.ascx", row["TaiKhoanID"].ToString());
                                    tblCell.Controls.Add(imgSua);
                                    tblCell.HorizontalAlign = HorizontalAlign.Center;
                                    tblRow.Controls.Add(tblCell);

                                    tblCell = new TableCell();
                                    ImageButton imgXoa = new ImageButton();
                                    imgXoa.ID = "Xoa_" + row["TaiKhoanID"].ToString();
                                    imgXoa.ImageUrl = Static.AppPath() + "/images/delete.gif";
                                    imgXoa.ToolTip = "Click xóa ";
                                    imgXoa.CausesValidation = false;
                                    imgXoa.Attributes["onclick"] = string.Format("return confirm('Bạn có chắc chắn xóa tài khoản \"{0}\" này không ?');", row["TenTaiKhoan"]);
                                    imgXoa.Click += new ImageClickEventHandler(imgXoa_Click);
                                    tblCell.Controls.Add(imgXoa);
                                    tblCell.HorizontalAlign = HorizontalAlign.Center;
                                    tblRow.Controls.Add(tblCell);

                                    tbl.Controls.Add(tblRow);
                                }
                            }
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

        protected void imgXoa_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btnImg = (ImageButton)sender;
            string strID = btnImg.ID.Remove(0, btnImg.ID.LastIndexOf('_') + 1);
            string strLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_DELETE", strID);
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                return;
            }
            addData();
            ham.Alert(this, "Xóa thành công !", "btnSuKien");
        }
        protected void btnSuKien_Click(object sender, EventArgs e)
        {
            addData();
        }

        protected void btnTim_Click(object sender, EventArgs e)
        {
            Session["TuKhoa"] = txtTuKhoa.Text;
            addData();
        }

        protected void drpDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {
            addData();
        }
    }
}