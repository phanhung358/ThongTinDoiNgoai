using FITC.Web.Component;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyVanBan.DichVu.DuLieu
{
    public partial class TrangWeb : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSuKien.Style.Add("display", "none");
            if (!IsPostBack)
            {

            }
            addData();
        }

        private string hopLe()
        {
            if (txtTenWeb.Text.Trim().Length == 0)
            {
                txtTenWeb.Focus();
                return "Vui lòng nhập tên trang web!";
            }
            if (txtDiaChiWeb.Text.Trim().Length == 0)
            {
                txtDiaChiWeb.Focus();
                return "Vui lòng nhập địa chỉ trang web(url)!";
            }

            return "";
        }

        #region Các hàm phân trang
        private void PhanTrang(int TongSoTin, int SoTinTrenTrang, int TrangHienTai, Label lblPhanTrang)
        {
            try
            {
                lblPhanTrang.Controls.Clear();
                Table tbl = new Table();
                TableRow tblRow = new TableRow();
                TableCell tblCell = new TableCell();
                int TongSoTrang = TongSoTin / SoTinTrenTrang;
                if (TongSoTin % SoTinTrenTrang != 0)
                    TongSoTrang = TongSoTrang + 1;
                lblPhanTrang.Visible = true;
                if (TongSoTin <= SoTinTrenTrang)
                {
                    lblPhanTrang.Visible = false;
                    return;
                }

                tblCell = new TableCell();
                tblCell.Text = "Trang:";
                tblCell.Width = 15;
                tblRow.Controls.Add(tblCell);

                int k = TrangHienTai / 10;
                k = TrangHienTai % 10 != 0 ? k + 1 : k;
                int TrangBatDau = (k - 1) * 10 + 1;
                int TrangKetThuc = TongSoTrang > 10 * k ? 10 * k : TongSoTrang;
                if (TrangBatDau > 10)
                {
                    int index = TrangBatDau - 1;
                    tblCell = new TableCell();
                    LinkButton link = new LinkButton();
                    link.Text = "<<";
                    link.ID = "btnLink_" + index.ToString();
                    link.CssClass = "Tin_PhanTrang";
                    link.Click += new EventHandler(LinkTrangTiep_Click);
                    tblCell.Controls.Add(link);
                    tblRow.Controls.Add(tblCell);
                }
                for (int i = TrangBatDau; i <= TrangKetThuc; i++)
                {
                    if (i == TrangHienTai)
                    {
                        tblCell = new TableCell();
                        tblCell.Text = "[" + i.ToString() + "]";
                        tblCell.CssClass = "Tin_PhanTrang_HienTai";
                        tblCell.Width = 2;
                        tblRow.Controls.Add(tblCell);
                    }
                    else
                    {
                        tblCell = new TableCell();
                        LinkButton link = new LinkButton();
                        link.Text = i.ToString();
                        link.ID = "btnLink_" + i.ToString();
                        link.CssClass = "Tin_PhanTrang";
                        tblCell.Width = 2;
                        link.Click += new EventHandler(LinkTrangTiep_Click);
                        tblCell.Controls.Add(link);
                        tblRow.Controls.Add(tblCell);
                    }
                }
                if (TongSoTrang > 10 * k)
                {
                    int index = 10 * k + 1;
                    tblCell = new TableCell();
                    LinkButton link = new LinkButton();
                    link.Text = ">>";
                    link.ID = "btnLink_" + index.ToString();
                    link.CssClass = "Tin_PhanTrang";
                    tblCell.Width = 100;
                    link.Click += new EventHandler(LinkTrangTiep_Click);
                    tblCell.Controls.Add(link);
                    tblRow.Controls.Add(tblCell);
                }
                tbl.Controls.Add(tblRow);
                lblPhanTrang.Controls.Add(tbl);
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnSuKien");
            }
        }

        protected void LinkTrangTiep_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnLink = (LinkButton)sender;
                Static.PhanTrangThu = Int16.Parse(btnLink.ID.Substring(8));
                addData();
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnSuKien");
            }
        }
        protected void drpSoTin_SelectedIndexChanged(object sender, EventArgs e)
        {
            Static.PhanTrangThu = 1;
            addData();
        }


        #endregion

        private void addData()
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
            tblCell.Width = 40;
            tblCell.Text = "STT";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Tên trang web";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Địa chỉ";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 110;
            tblCell.Text = "Ngày cập nhật";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 30;
            tblCell.Text = "Sửa";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 30;
            tblCell.Text = "Xóa";
            tblRow.Controls.Add(tblCell);

            tbl.Controls.Add(tblRow);

            tblPhanTrang.Visible = false;
            using (DataSet ds = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //Phân trang===============================================
                    int iSoTin = int.Parse(drpSoTin.SelectedValue) * (Static.PhanTrangThu - 1);
                    if (ds.Tables[0].Rows.Count <= iSoTin && Static.PhanTrangThu != 1)
                        Static.PhanTrangThu = Static.PhanTrangThu - 1;

                    int TrangHienTai = Static.PhanTrangThu;
                    int TongSoTin = ds.Tables[0].Rows.Count;
                    int SoTinTrenTrang = Convert.ToInt32(drpSoTin.SelectedValue);
                    PhanTrang(TongSoTin, SoTinTrenTrang, TrangHienTai, lblPhanTrang);
                    int TuBanGhi = (TrangHienTai - 1) * SoTinTrenTrang;
                    int DenBanGhi = (TrangHienTai * SoTinTrenTrang) > TongSoTin ? TongSoTin : TrangHienTai * SoTinTrenTrang;
                    if (TongSoTin > SoTinTrenTrang)
                        tblPhanTrang.Visible = true;
                    //End phân trang==========================================

                    for (int i = TuBanGhi; i < DenBanGhi; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        string css = "Dong_Chan";
                        if (i % 2 == 1)
                            css = "Dong_Le";
                        tblRow = new TableRow();
                        tblRow.CssClass = css;

                        tblCell = new TableCell();
                        tblCell.Text = (i + 1).ToString();
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["TenWeb"].ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["DiaChiWeb"].ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = DateTime.Parse(row["NgayCapNhat"].ToString()).ToString("dd/MM/yyyy HH:mm");
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        ImageButton imgSua = new ImageButton();
                        imgSua.ID = "Sua_" + row["WebID"].ToString();
                        imgSua.ToolTip = "Click để sửa";
                        imgSua.ImageUrl = Static.AppPath() + "/images/edit.gif";
                        imgSua.Click += new ImageClickEventHandler(imgSua_Click);
                        tblCell.Controls.Add(imgSua);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        ImageButton imgXoa = new ImageButton();
                        imgXoa.ID = "Xoa_" + row["WebID"].ToString().Trim();
                        imgXoa.ImageUrl = Static.AppPath() + "/images/delete.gif";
                        imgXoa.ToolTip = "Click xóa ";
                        imgXoa.CausesValidation = false;
                        imgXoa.Attributes["onclick"] = string.Format("return confirm('Bạn có chắc chắn xóa trang web \"{0}\" này không ?');", row["DiaChiWeb"]);
                        imgXoa.Click += new ImageClickEventHandler(imgXoa_Click);
                        tblCell.Controls.Add(imgXoa);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tbl.Controls.Add(tblRow);
                    }
                }
                else
                {
                    int intCell = tblRow.Cells.Count;
                    tblRow = new TableRow();
                    tblRow.CssClass = "Dong_Chan";
                    tblCell = new TableCell();
                    tblCell.ColumnSpan = intCell;
                    tblCell.Text = "Thông tin chưa được cập nhật";
                    tblCell.HorizontalAlign = HorizontalAlign.Center;
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);
                }
                divDanhSach.Controls.Add(tbl);
            }
        }

        private void imgXoa_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnImg = (ImageButton)sender;
                string strID = btnImg.ID.Remove(0, btnImg.ID.LastIndexOf('_') + 1);
                string strLoi = db.ExcuteSP("TTDN_TRANGWEB_DELETE", strID);
                if (strLoi != "")
                {
                    ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                    return;
                }
                btnHuyBo_Click(null, null);
                addData();
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message.Replace("'", "\\\""), "btnSuKien");
            }
        }

        private void imgSua_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnImg = (ImageButton)sender;
                string strID = btnImg.ID.Remove(0, btnImg.ID.LastIndexOf('_') + 1);
                using (DataSet ds = db.GetDataSet("TTDN_TRANGWEB_SELECT", 1, strID))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        btnCapNhat.Visible = true;
                        btnHuyBo.Visible = true;
                        btnThemMoi.Visible = false;

                        hidID.Value = ds.Tables[0].Rows[0]["WebID"].ToString().Trim();
                        txtTenWeb.Text = ds.Tables[0].Rows[0]["TenWeb"].ToString().Trim();
                        txtDiaChiWeb.Text = ds.Tables[0].Rows[0]["DiaChiWeb"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message.Replace("'", "\\\""), "btnSuKien");
            }
        }

        protected void btnThemMoi_Click(object sender, EventArgs e)
        {

            string strLoi = "";
            strLoi = this.hopLe();
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                return;
            }
            else
            {
                DataSet ds = db.GetDataSet("TTDN_TRANGWEB_SELECT", 2, hidID.Value, txtDiaChiWeb.Text.Trim());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ham.Alert(this, string.Format("Địa chỉ trang web(url) \"{0}\" đã tồn tại!", txtDiaChiWeb.Text.Trim()), "btnSuKien");
                    return;
                }
            }

            strLoi = db.ExcuteSP("TTDN_TRANGWEB_INSERT", txtTenWeb.Text.Trim(), txtDiaChiWeb.Text.Trim(), "Phan Hùng");
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                return;
            }
            else
            {
                ham.Alert(this, "Cập nhật thành công!", "btnSuKien");
                txtTenWeb.Text = "";
                txtDiaChiWeb.Text = "";
            }
            addData();
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            string strLoi = "";
            strLoi = this.hopLe();
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                return;
            }
            else
            {
                DataSet ds = db.GetDataSet("TTDN_TRANGWEB_SELECT", 2, hidID.Value, txtDiaChiWeb.Text.Trim());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ham.Alert(this, string.Format("Địa chỉ trang web(url) \"{0}\" đã tồn tại!", txtDiaChiWeb.Text.Trim()), "btnSuKien");
                    return;
                }
            }

            strLoi = db.ExcuteSP("TTDN_TRANGWEB_UPDATE", hidID.Value, txtTenWeb.Text.Trim(), txtDiaChiWeb.Text.Trim(), "Phan Hùng");
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                return;
            }
            else
            {
                ham.Alert(this, "Cập nhật thành công!", "btnSuKien");
                txtTenWeb.Text = "";
                txtDiaChiWeb.Text = "";
                btnThemMoi.Visible = true;
                btnCapNhat.Visible = false;
                btnHuyBo.Visible = false;
            }
            addData();
        }

        protected void btnHuyBo_Click(object sender, EventArgs e)
        {
            txtTenWeb.Text = "";
            txtDiaChiWeb.Text = "";
            btnThemMoi.Visible = true;
            btnCapNhat.Visible = false;
            btnHuyBo.Visible = false;
        }
    }
}