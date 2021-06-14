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

namespace ThongTinDoiNgoai.DichVu.HeThong.TaiKhoan
{
    public partial class BoPhan : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

            addData();
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

            object[] obj = new object[2];
            obj[0] = TUONGTAC.DonViID;
            obj[1] = txtBoPhan.Text.Trim();

            strLoi = db.ExcuteSP("TTDN_DM_BOPHAN_INSERT", obj);
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                return;
            }
            else
            {
                txtBoPhan.Enabled = false;
                btnThemMoi.Visible = false;
                btnHuyBo.Visible = true;
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

            object[] obj = new object[3];
            obj[0] = hidID.Value;
            obj[1] = TUONGTAC.DonViID;
            obj[2] = txtBoPhan.Text.Trim();


            strLoi = db.ExcuteSP("TTDN_DM_BOPHAN_UPDATE", obj);
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                return;
            }
            else
            {
                txtBoPhan.Enabled = false;
                btnCapNhat.Visible = false;
                btnHuyBo.Visible = true;
            }
            addData();
        }
        protected void btnHuyBo_Click(object sender, EventArgs e)
        {
            txtBoPhan.Enabled = true;
            btnHuyBo.Visible = false;
            btnThemMoi.Visible = true;
            btnCapNhat.Visible = false;

            txtBoPhan.Text = "";
            hidID.Value = "";

        }
        protected void imgSua_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnImg = (ImageButton)sender;
                string strID = btnImg.ID.Remove(0, btnImg.ID.LastIndexOf('_') + 1);
                using (DataSet ds = db.GetDataSet("TTDN_DM_BOPHAN_SELECT", 1, strID))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        txtBoPhan.Enabled = true;
                        btnHuyBo.Visible = false;
                        btnCapNhat.Visible = true;
                        btnThemMoi.Visible = false;

                        hidID.Value = ds.Tables[0].Rows[0]["BoPhanID"].ToString().Trim();
                        txtBoPhan.Text = ds.Tables[0].Rows[0]["TenBoPhan"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message.Replace("'", "\\\""), "btnSuKien");
            }
        }
        protected void imgXoa_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnImg = (ImageButton)sender;
                string strID = btnImg.ID.Remove(0, btnImg.ID.LastIndexOf('_') + 1);
                string strLoi = db.ExcuteSP("TTDN_DM_BOPHAN_DELETE", strID);
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

        private string hopLe()
        {
            string str = "";
            str = txtBoPhan.Text.Trim();
            if (str.Length == 0)
            {
                txtBoPhan.Focus();
                return "Vui lòng nhập tên nhóm !";
            }

            return "";
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
            tblCell.Width = 30;
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Tên nhóm";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 30;
            Label lbl = new Label();
            lbl.Text = "STT<br/>";
            tblCell.Controls.Add(lbl);

            ImageButton imgThuTu = new ImageButton();
            imgThuTu.ID = "thutu_";
            imgThuTu.ImageUrl = Static.AppPath() + "/Images/save.gif";
            imgThuTu.ToolTip = "Click lưu thứ tự ";
            imgThuTu.Click += new ImageClickEventHandler(luuThuTu_Click);
            tblCell.Controls.Add(imgThuTu);
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
            using (DataSet ds = db.GetDataSet("TTDN_DM_BOPHAN_SELECT", 0, TUONGTAC.DonViID))
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

                    int k = 0;
                    for (int i = TuBanGhi; i < DenBanGhi; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        string css = "Dong_Chan";
                        if (k % 2 == 1)
                            css = "Dong_Le";
                        k++;
                        tblRow = new TableRow();
                        tblRow.CssClass = css;

                        tblCell = new TableCell();
                        tblCell.Text = (i + 1).ToString();
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["TenBoPhan"].ToString().Trim();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        TextBox txtSTT = new TextBox();
                        txtSTT.CssClass = "textbox";
                        txtSTT.Style.Add("text-align", "center");
                        ham.setTextBox_Number(txtSTT, false);
                        txtSTT.ID = "ThuTu_" + row["BoPhanID"].ToString().Trim();
                        txtSTT.Text = (i + 1).ToString().Trim();
                        txtSTT.Width = 25;
                        tblCell.Controls.Add(txtSTT);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        ImageButton imgSua = new ImageButton();
                        imgSua.ID = "Sua_" + row["BoPhanID"].ToString();
                        imgSua.ToolTip = "Click để sửa";
                        imgSua.ImageUrl = Static.AppPath() + "/images/edit.gif";
                        imgSua.Click += new ImageClickEventHandler(imgSua_Click);
                        tblCell.Controls.Add(imgSua);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        ImageButton imgXoa = new ImageButton();
                        imgXoa.ID = "Xoa_" + row["BoPhanID"].ToString().Trim();
                        imgXoa.ImageUrl = Static.AppPath() + "/images/delete.gif";
                        imgXoa.ToolTip = "Click xóa ";
                        imgXoa.CausesValidation = false;
                        imgXoa.Attributes["onclick"] = string.Format("return confirm('Bạn có chắc chắn xóa nhóm \"{0}\" này không ?');", row["TenBoPhan"]);
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
            }
            divDanhSach.Controls.Add(tbl);
        }

        void lnk_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            string strID = lnk.ID.Remove(0, lnk.ID.LastIndexOf('_') + 1);

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

        protected void luuThuTu_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string sLoi = "";
                DataSet ds = db.GetDataSet("TTDN_DM_BOPHAN_SELECT", 0, TUONGTAC.DonViID);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TextBox txtSTT = (TextBox)this.FindControl("ThuTu_" + row["BoPhanID"].ToString().Trim());
                        if (txtSTT != null)
                        {
                            string stt = ham.getXoaDinhDang(txtSTT.Text);
                            if (stt == "")
                                stt = "1";
                            sLoi = db.ExcuteSP("TTDN_DM_BOPHAN_UPDATE_STT", row["BoPhanID"].ToString().Trim(), stt);
                            if (sLoi != "")
                                break;
                        }
                    }

                }
                if (sLoi == "")
                {
                    addData();
                }
                else
                    ham.Alert(this, sLoi, "btnThemMoi");
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnThemMoi");
            }
        }
    }
}