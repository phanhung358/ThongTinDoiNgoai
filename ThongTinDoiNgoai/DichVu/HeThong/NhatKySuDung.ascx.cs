using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FITC.Web.Component;
using System.Data;

namespace ThongTinDoiNgoai.DichVu.HeThong
{
    public partial class NhatKySuDung : System.Web.UI.UserControl
    {
        CacHamChung ham = new CacHamChung();
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtTuNgay.Text = "1/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Session["TuNgay"] = txtTuNgay.Text;
                Session["DenNgay"] = txtDenNgay.Text;
            }
            imgTuNgay.Attributes.Add("onclick", "popUpCalendar(this,this,'dd/mm/yyyy','" + txtTuNgay.ClientID + "');");
            imgDenNgay.Attributes.Add("onclick", "popUpCalendar(this,this,'dd/mm/yyyy','" + txtDenNgay.ClientID + "');");
            addData();
        }

        private void addData()
        {


            divDanhSach.Controls.Clear();
            Table tbl = new Table();
            tbl.Width = Unit.Percentage(100);
            tbl.CssClass = "Vien_Bang";
            tbl.CellPadding = 3;
            tbl.CellSpacing = 1;
            tbl.BorderWidth = 0;

            TableCell tblCell = new TableCell();
            TableRow tblRow = new TableRow();

            tblRow = new TableRow();
            tblRow.CssClass = "Dong_TieuDe";
            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "STT";
            tblCell.Width = 10;
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.Width = 120;
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Thời gian";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Tên tài khoản";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Tên đơn vị";
            tblRow.Controls.Add(tblCell);

            //tblCell = new TableCell();
            //tblCell.CssClass = "Cot_TieuDe";
            //tblCell.Text = "Chức năng";
            //tblRow.Controls.Add(tblCell);

            //tblCell = new TableCell();
            //tblCell.CssClass = "Cot_TieuDe";
            //tblCell.Text = "Thao tác";
            //tblRow.Controls.Add(tblCell);

            tbl.Controls.Add(tblRow);
            tblPhanTrang.Visible = false;
            FITC_CDataTime dt = new FITC_CDataTime();
            if ((Session["TuNgay"] != null && Session["TuNgay"].ToString() == "") || !dt.IsDateFormat(Session["TuNgay"].ToString()))
            {
                divDanhSach.Controls.Add(tbl);
                ham.Alert(this, "Từ ngày không hợp lệ", "btnXem");
                return;
            }

            if ((Session["TuNgay"] != null && Session["TuNgay"].ToString() == "") || !dt.IsDateFormat(Session["DenNgay"].ToString()))
            {
                divDanhSach.Controls.Add(tbl);
                ham.Alert(this, "Đến ngày không hợp lệ", "btnXem");
                return;
            }
            try
            {
                int SoTinTrenTrang = Convert.ToInt32(drpSoTin.SelectedValue);
                object[] obj = new object[6];
                obj[0] = 0;
                obj[1] = 0;
                obj[2] = ham.GetDateEnglish(Session["TuNgay"].ToString());
                obj[3] = ham.GetDateEnglish(Session["DenNgay"].ToString()) + " 23:59:59";
                obj[4] = Static.PhanTrangThu;
                obj[5] = SoTinTrenTrang;

                using (DataSet ds = db.GetDataSet("TTDN_NHATKY_SUDUNG_SELECT", obj))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //Phân trang===============================================
                        int TrangHienTai = Convert.ToInt32(Static.PhanTrangThu);
                        int TongSoTin = int.Parse(ds.Tables[0].Rows[0]["SoDong"].ToString());
                        PhanTrang(TongSoTin, SoTinTrenTrang, TrangHienTai, lblPhanTrang);
                        int TuBanGhi = (TrangHienTai - 1) * SoTinTrenTrang;
                        int stt = TuBanGhi + 1;
                        if (TongSoTin > SoTinTrenTrang)
                            tblPhanTrang.Visible = true;
                        //End phân trang==========================================
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow row = ds.Tables[0].Rows[i];
                            string css = "Dong_Chan";
                            if (i % 2 == 1)
                                css = "Dong_Le";
                            tblRow = new TableRow();
                            tblRow.CssClass = css;

                            tblCell = new TableCell();
                            tblCell.Text = (stt++).ToString();
                            tblCell.HorizontalAlign = HorizontalAlign.Center;
                            tblRow.Controls.Add(tblCell);

                            tblCell = new TableCell();
                            tblCell.HorizontalAlign = HorizontalAlign.Center;
                            tblCell.Text = Convert.ToDateTime(row["ThoiGian"].ToString().Trim()).ToString("dd/MM/yyyy HH:mm");
                            tblRow.Controls.Add(tblCell);

                            tblCell = new TableCell();
                            tblCell.Text = row["TenTaiKhoan"].ToString().Trim();
                            tblRow.Controls.Add(tblCell);

                            tblCell = new TableCell();
                            tblCell.Text = row["TenDonVi"].ToString().Trim();
                            tblRow.Controls.Add(tblCell);

                            //tblCell = new TableCell();
                            //tblCell.Text = row["TenChucNang"].ToString().Trim();
                            //tblRow.Controls.Add(tblCell);

                            //tblCell = new TableCell();
                            //switch (row["ThaoTac"].ToString().Trim())
                            //{
                            //    case "0":
                            //        tblCell.Text = "Vào hệ thống";
                            //        break;
                            //    case "1":
                            //        tblCell.Text = "Thêm dữ liệu";
                            //        break;
                            //    case "2":
                            //        tblCell.Text = "Sửa dữ liệu";
                            //        break;
                            //    case "3":
                            //        tblCell.Text = "Xóa dữ liệu";
                            //        break;
                            //}
                            //tblRow.Controls.Add(tblCell);

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
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            divDanhSach.Controls.Add(tbl);
        }
        protected void btnXem_Click(object sender, EventArgs e)
        {
            Session["TuNgay"] = txtTuNgay.Text;
            Session["DenNgay"] = txtDenNgay.Text;
            addData();
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
    }
}