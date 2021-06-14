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
    public partial class ChuyenMuc : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSuKien.Style.Add("display", "none");
            if (!IsPostBack)
            {
                addDanhMuc();
            }
            addData();
        }

        private void addDanhMuc()
        {
            drpWeb.Items.Add(new ListItem("[Tất cả]", "0"));
            DataSet dsWeb = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0);
            if (dsWeb != null && dsWeb.Tables.Count > 0 && dsWeb.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsWeb.Tables[0].Rows.Count; i++)
                {
                    DataRow row = dsWeb.Tables[0].Rows[i];
                    drpWeb.Items.Add(new ListItem(row["TenWeb"].ToString(), row["WebID"].ToString()));
                }
            }
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
            imgThemMoi.Attributes["onclick"] = string.Format("return thickboxPopup('Thêm mới chuyên mục', '{0}?control={1}&btn={2}&WebID={3}','650','500');", Static.AppPath() + "/home/popup.aspx", "/DichVu/ThongTinDoiNgoai/ChuyenMuc_Tm.ascx", btnSuKien.ClientID, drpWeb.SelectedValue);

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
            tblCell.Width = 30;
            tblCell.Text = "STT";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Tên website";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Tên chuyên mục";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "URL chuyên mục";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 75;
            tblCell.Text = "Trạng thái";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 75;
            tblCell.Text = "XPath chuyên mục";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 65;
            tblCell.Text = "XPath <br/> chi tiết";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 65;
            tblCell.Text = "Thời gian <br/> đồng bộ";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Tổng tin";
            tblCell.Width = 50;
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

            tblPhanTrang.Visible = false;
            using (DataSet ds = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 0, drpWeb.SelectedValue.ToString()))
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
                        tblCell.Text = row["TenChuyenMuc"].ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = string.Format("<a href='{0}' target='_blank'>{1}</a>", row["UrlChuyenMuc"].ToString(), row["UrlChuyenMuc"].ToString());
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        Button btnChange = new Button();
                        btnChange.Text = row["TrangThai"].ToString() == "1" ? "Hoạt động" : "Tạm dừng";
                        btnChange.CssClass = row["TrangThai"].ToString() == "1" ? "play" : "pause";
                        btnChange.ID = "Change_" + row["ChuyenMucID"].ToString() + "_" + row["TrangThai"].ToString();
                        btnChange.ToolTip = "Click để thay đổi trạng thái";
                        btnChange.Click += new EventHandler(btnChange_Click);
                        tblCell.Controls.Add(btnChange);
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        ImageButton xpathCM = new ImageButton();
                        xpathCM.ToolTip = "Click để thiết lập Xpath chuyên mục";
                        xpathCM.ImageUrl = Static.AppPath() + "/images/view.gif";
                        xpathCM.Attributes["onclick"] = string.Format("return thickboxPopup('Thiết lập Xpath chuyên mục', '{0}?control={1}&btn={2}&ChuyenMucID={3}&WebID={4}','650','500');", Static.AppPath() + "/home/popup.aspx", "/DichVu/ThongTinDoiNgoai/XpathChuyenMuc_Tm.ascx", btnSuKien.ClientID, row["ChuyenMucID"].ToString(), row["WebID"].ToString());
                        tblCell.Controls.Add(xpathCM);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        ImageButton xpathCT = new ImageButton();
                        xpathCT.ToolTip = "Click để thiết lập Xpath chi tiết";
                        xpathCT.ImageUrl = Static.AppPath() + "/images/view.gif";
                        xpathCT.Attributes["onclick"] = string.Format("return thickboxPopup('Thiết lập Xpath chi tiết', '{0}?control={1}&btn={2}&ChuyenMucID={3}&WebID={4}','650','500');", Static.AppPath() + "/home/popup.aspx", "/DichVu/ThongTinDoiNgoai/XpathChiTiet_Tm.ascx", btnSuKien.ClientID, row["ChuyenMucID"].ToString(), row["WebID"].ToString());
                        tblCell.Controls.Add(xpathCT);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        if (row["ThoiGianDongBo"].ToString() != "")
                            tblCell.Text = Convert.ToDateTime(row["ThoiGianDongBo"].ToString()).ToString("dd/MM/yyyy HH:mm");
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblCell.Text = row["TongSoTin"].ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        ImageButton imgSua = new ImageButton();
                        imgSua.ToolTip = "Click để sửa";
                        imgSua.ImageUrl = Static.AppPath() + "/images/edit.gif";
                        imgSua.Attributes["onclick"] = string.Format("return thickboxPopup('Chỉnh sửa chuyên mục', '{0}?control={1}&btn={2}&ChuyenMucID={3}','650','500');", Static.AppPath() + "/home/popup.aspx", "/DichVu/ThongTinDoiNgoai/ChuyenMuc_Tm.ascx", btnSuKien.ClientID, row["ChuyenMucID"].ToString());
                        tblCell.Controls.Add(imgSua);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        ImageButton imgXoa = new ImageButton();
                        imgXoa.ID = "Xoa_" + row["ChuyenMucID"].ToString().Trim();
                        imgXoa.ImageUrl = Static.AppPath() + "/images/delete.gif";
                        imgXoa.ToolTip = "Click xóa ";
                        imgXoa.CausesValidation = false;
                        imgXoa.Attributes["onclick"] = string.Format("return confirm('Bạn có chắc chắn xóa chuyên mục này không ?');");
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

        private void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                string[] str = btn.ID.Split('_');
                str[2] = str[2] == "0" ? "1" : "0";
                string strLoi = db.ExcuteSP("TTDN_CHUYENMUC_UPDATE_TRANGTHAI", str[1], str[2]);
                if (strLoi != "")
                {
                    ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                    return;
                }
                addData();
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message.Replace("'", "\\\""), "btnSuKien");
            }
        }

        private void imgXoa_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnImg = (ImageButton)sender;
                string strID = btnImg.ID.Remove(0, btnImg.ID.LastIndexOf('_') + 1);
                string strLoi = db.ExcuteSP("TTDN_CHUYENMUC_DELETE", strID);
                if (strLoi != "")
                {
                    ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                    return;
                }
                addData();
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message.Replace("'", "\\\""), "btnSuKien");
            }
        }

        protected void drpWeb_SelectedIndexChanged(object sender, EventArgs e)
        {
            addData();
        }
    }
}