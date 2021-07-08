using FITC.Web.Component;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ThongTinDoiNgoai.DichVu.ThongTinDoiNgoai
{
    public partial class TrangChu : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                addDanhMuc();
            }
            addData();
        }

        private void addDanhMuc()
        {
            drpTrangWeb.Items.Add(new ListItem("[Tất cả]", "0"));
            DataSet dsLoaiVanBan = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0);
            if (dsLoaiVanBan != null && dsLoaiVanBan.Tables.Count > 0 && dsLoaiVanBan.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsLoaiVanBan.Tables[0].Rows.Count; i++)
                {
                    DataRow row = dsLoaiVanBan.Tables[0].Rows[i];
                    drpTrangWeb.Items.Add(new ListItem(row["TenWeb"].ToString() + " (" + row["DiaChiWeb"].ToString() + ")", row["WebID"].ToString()));
                }
            }
            drpChuyenMuc.Items.Add(new ListItem("[Tất cả]", "0"));
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
            if (Request.QueryString["id"] == null)
            {
                StringBuilder str = new StringBuilder();
                str.Append("<div class='ttdn-nen'>");
                //=============================================
                using (DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 1, 0, drpTrangWeb.SelectedValue, drpChuyenMuc.SelectedValue))
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

                            if (i % 2 == 0)
                                str.Append("<div class='ttdn-dong-chan'>");
                            else
                                str.Append("<div class='ttdn-dong-le'>");
                            str.Append("<div class='ttdn-dongtieude'>");
                            str.AppendFormat("<div class='ttdn-tieude'>{0}</div>", DateTime.Parse(row["ThoiGian"].ToString()).ToString("dd/MM/yyyy - HH:mm"));
                            str.AppendFormat("<div class='ttdn-thoigian'>{0}</div>", row["TieuDe"].ToString());
                            str.Append("</div>");
                            str.Append("<div class='ttdn-dongnoidung'>");
                            str.AppendFormat("<div class='ttdn-tomtat'>{0}</div>", row["TomTat"].ToString());
                            str.AppendFormat("<div class='ttdn-xemchitiet'><a href='/?id={0}'>Xem chi tiết</a></div>", row["BaiVietID"].ToString());
                            str.Append("</div>");
                            str.Append("</div>");
                        }
                    }
                    else
                    {
                        str.Append("<div class='ttdn-dong-chan'>");
                        str.Append("<div class='ttdn-rong'>Thông tin chưa được cập nhật</div>");
                        str.Append("</div>");
                    }
                }
                str.Append("</div>");
                divDanhSach.InnerHtml = str.ToString();
            }
            else
                addData_ChiTiet();
        }

        private void addData_ChiTiet()
        {
            tblPhanTrang.Visible = false;
            divControl.Visible = false;
            string id = Request.QueryString["id"];
            string ChuyenMucID = "0";

            StringBuilder str = new StringBuilder();
            str.Append("<div class='ttdn-chitiet'>");
            DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 0, id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                ChuyenMucID = row["ChuyenMucID"].ToString();
                str.AppendFormat("<h2 class='ttdn-chitiet-tieude'>{0}</h2>", row["TieuDe"].ToString());
                if (!string.IsNullOrEmpty(row["ThoiGian"].ToString()))
                    str.AppendFormat("<div class='ttdn-chitiet-thoigian'><span>{0}</span></div>", DateTime.Parse(row["ThoiGian"].ToString()).ToString("dd/MM/yyyy - HH:mm"));
                if (!string.IsNullOrEmpty(row["TomTat"].ToString()))
                    str.AppendFormat("<div class='ttdn-chitiet-tomtat'>{0}</div>", row["TomTat"].ToString());

                str.AppendFormat("<div class='ttdn-chitiet-noidung'>{0}</div>", row["NoiDung"].ToString());
                str.AppendFormat("<div class='ttdn-chitiet-tacgia'><p>{0}</p></div>", row["TacGia"].ToString());
            }
            str.Append("</div>");
            str.Append("<div class='ttdn-tinkhac'>");
            DataSet dsKhac = db.GetDataSet("TTDN_BAIVIET_SELECT", 2, id, 0, ChuyenMucID);
            if (dsKhac != null && dsKhac.Tables.Count > 0 && dsKhac.Tables[0].Rows.Count > 0)
            {
                str.Append("<div class='ttdn-tinkhac-demuc'>Tin khác:</div>");
                str.Append("<ul class='danhsach'>");
                for (int i = 0; i < dsKhac.Tables[0].Rows.Count; i++)
                {
                    DataRow row = dsKhac.Tables[0].Rows[i];

                    str.AppendFormat("<li class='tin'><a href='/?id={0}'>{1}</a></li>", row["BaiVietID"].ToString(), row["TieuDe"].ToString());
                }
                str.Append("</ul>");
            }
            str.Append("</div>");

            divDanhSach.InnerHtml = str.ToString();
        }

        protected void drpTrangWeb_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpChuyenMuc.Items.Clear();
            drpChuyenMuc.Items.Add(new ListItem("[Tất cả]", "0"));
            DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 2, drpTrangWeb.SelectedValue);
            if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsChuyenMuc.Tables[0].Rows.Count; i++)
                {
                    DataRow row = dsChuyenMuc.Tables[0].Rows[i];
                    drpChuyenMuc.Items.Add(new ListItem(row["TenChuyenMuc"].ToString() + " (" + row["UrlChuyenMuc"].ToString() + ")", row["ChuyenMucID"].ToString()));
                }
            }
            addData();
        }

        protected void drpChuyenMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            addData();
        }
    }
}