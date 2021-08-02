using FITC.Web.Component;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyVanBan.DichVu.DuLieu
{
    public partial class XpathChiTiet_Tm : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sWebID = "0";
        string sChuyenMucID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            divDanhSach.Style.Add("display", "none");
            if (Request.QueryString["WebID"] != null && Request.QueryString["ChuyenMucID"] != null)
            {
                sWebID = Request.QueryString["WebID"];
                sChuyenMucID = Request.QueryString["ChuyenMucID"];
            }
            if (!IsPostBack)
            {
                addDanhMuc();
            }
                addSua();
        }

        private void addDanhMuc()
        {
            drpWeb.Items.Add(new ListItem("[Chọn]", "0"));
            DataSet dsWeb = db.GetDataSet("TTDN_TRANGWEB_SELECT", 0);
            if (dsWeb != null && dsWeb.Tables.Count > 0 && dsWeb.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsWeb.Tables[0].Rows.Count; i++)
                {
                    DataRow row = dsWeb.Tables[0].Rows[i];
                    drpWeb.Items.Add(new ListItem(row["TenWeb"].ToString() + " (" + row["DiaChiWeb"].ToString() + ")", row["WebID"].ToString()));
                }
            }
            drpChuyenMuc.Items.Clear();
            drpChuyenMuc.Items.Add(new ListItem("[Chọn]", "0"));
        }

        private void addSua()
        {
            try
            {
                db.GetItem(drpWeb, sWebID);
                drpWeb_SelectedIndexChanged(null, null);
                db.GetItem(drpChuyenMuc, sChuyenMucID);

                DataSet ds = db.GetDataSet("TTDN_XPATH_CHITIET_SELECT", 0, sWebID, sChuyenMucID);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    txtXpathID.Value = row["XpathID"].ToString();
                    txtTieuDe.Text = row["TieuDe"].ToString();
                    txtTomTat.Text = row["TomTat"].ToString();
                    txtNoiDung.Text = row["NoiDung"].ToString();
                    txtThoiGian.Text = row["ThoiGian"].ToString();
                    txtDinhDangThoiGian.Text = row["DinhDangThoiGian"].ToString();
                    txtTacGia.Text = row["TacGia"].ToString();
                }

                divDanhSach.Controls.Clear();

                Table tbl = new Table();
                tbl.Width = Unit.Percentage(100);
                tbl.CellPadding = 0;
                tbl.CellSpacing = 0;
                tbl.BorderWidth = 0;

                TableCell tblCell = new TableCell();
                TableRow tblRow = new TableRow();

                int k = 1;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 1)
                {
                    chkKhac.Checked = true;
                    chkKhac_CheckedChanged(null, null);
                    for (k = 1; k < ds.Tables[0].Rows.Count; k++)
                    {
                        DataRow row = ds.Tables[0].Rows[k];

                        tblRow = new TableRow();

                        tblCell = new TableCell();
                        HiddenField hidden = new HiddenField();
                        hidden.ID = "XpathID_" + k.ToString();
                        hidden.Value = row["XpathID"].ToString();
                        tblCell.Controls.Add(hidden);
                        tblRow.Controls.Add(tblCell);
                        tbl.Controls.Add(tblRow);

                        tblRow = new TableRow();

                        tblCell = new TableCell();
                        tblCell.Width = 115;
                        tblCell.Text = "Xpath Tiêu đề " + k.ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        TextBox txtTieuDe = new TextBox();
                        txtTieuDe.Width = Unit.Percentage(100);
                        txtTieuDe.ID = "TieuDe_" + k.ToString();
                        txtTieuDe.Text = row["TieuDe"].ToString();
                        tblCell.Controls.Add(txtTieuDe);
                        tblRow.Controls.Add(tblCell);
                        tbl.Controls.Add(tblRow);

                        tblRow = new TableRow();

                        tblCell = new TableCell();
                        tblCell.Width = 115;
                        tblCell.Text = "Xpath Tóm tắt " + k.ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        TextBox txtTomTat = new TextBox();
                        txtTomTat.Width = Unit.Percentage(100);
                        txtTomTat.ID = "TomTat_" + k.ToString();
                        txtTomTat.Text = row["TomTat"].ToString();
                        tblCell.Controls.Add(txtTomTat);
                        tblRow.Controls.Add(tblCell);
                        tbl.Controls.Add(tblRow);

                        tblRow = new TableRow();

                        tblCell = new TableCell();
                        tblCell.Width = 115;
                        tblCell.Text = "Xpath Nội dung " + k.ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        TextBox txtNoiDung = new TextBox();
                        txtNoiDung.Width = Unit.Percentage(100);
                        txtNoiDung.ID = "NoiDung_" + k.ToString();
                        txtNoiDung.Text = row["NoiDung"].ToString();
                        tblCell.Controls.Add(txtNoiDung);
                        tblRow.Controls.Add(tblCell);
                        tbl.Controls.Add(tblRow);

                        tblRow = new TableRow();

                        tblCell = new TableCell();
                        tblCell.Width = 115;
                        tblCell.Text = "Xpath Thời gian " + k.ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        TextBox txtThoiGian = new TextBox();
                        txtThoiGian.Width = Unit.Percentage(100);
                        txtThoiGian.ID = "ThoiGian_" + k.ToString();
                        txtThoiGian.Text = row["ThoiGian"].ToString();
                        tblCell.Controls.Add(txtThoiGian);
                        tblRow.Controls.Add(tblCell);
                        tbl.Controls.Add(tblRow);

                        tblRow = new TableRow();

                        tblCell = new TableCell();
                        tblCell.Width = 115;
                        tblCell.Text = "Xpath Tác giả " + k.ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        TextBox txtTacGia = new TextBox();
                        txtTacGia.Width = Unit.Percentage(100);
                        txtTacGia.ID = "TacGia_" + k.ToString();
                        txtTacGia.Text = row["TacGia"].ToString();
                        tblCell.Controls.Add(txtTacGia);
                        tblRow.Controls.Add(tblCell);
                        tbl.Controls.Add(tblRow);
                    }
                }

                for (int i = k; i < 4; i++)
                {
                    tblRow = new TableRow();

                    tblCell = new TableCell();
                    HiddenField hidden = new HiddenField();
                    hidden.ID = "XpathID_" + i.ToString();
                    hidden.Value = "0";
                    tblCell.Controls.Add(hidden);
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);

                    tblRow = new TableRow();

                    tblCell = new TableCell();
                    tblCell.Width = 115;
                    tblCell.Text = "Xpath Tiêu đề " + i.ToString();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    TextBox txtTieuDe = new TextBox();
                    txtTieuDe.Width = Unit.Percentage(100);
                    txtTieuDe.ID = "TieuDe_" + i.ToString();
                    tblCell.Controls.Add(txtTieuDe);
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);

                    tblRow = new TableRow();

                    tblCell = new TableCell();
                    tblCell.Width = 115;
                    tblCell.Text = "Xpath Tóm tắt " + i.ToString();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    TextBox txtTomTat = new TextBox();
                    txtTomTat.Width = Unit.Percentage(100);
                    txtTomTat.ID = "TomTat_" + i.ToString();
                    tblCell.Controls.Add(txtTomTat);
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);

                    tblRow = new TableRow();

                    tblCell = new TableCell();
                    tblCell.Width = 115;
                    tblCell.Text = "Xpath Nội dung " + i.ToString();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    TextBox txtNoiDung = new TextBox();
                    txtNoiDung.Width = Unit.Percentage(100);
                    txtNoiDung.ID = "NoiDung_" + i.ToString();
                    tblCell.Controls.Add(txtNoiDung);
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);

                    tblRow = new TableRow();

                    tblCell = new TableCell();
                    tblCell.Width = 115;
                    tblCell.Text = "Xpath Thời gian " + i.ToString();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    TextBox txtThoiGian = new TextBox();
                    txtThoiGian.Width = Unit.Percentage(100);
                    txtThoiGian.ID = "ThoiGian_" + i.ToString();
                    tblCell.Controls.Add(txtThoiGian);
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);

                    tblRow = new TableRow();

                    tblCell = new TableCell();
                    tblCell.Width = 115;
                    tblCell.Text = "Xpath Tác giả " + i.ToString();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    TextBox txtTacGia = new TextBox();
                    txtTacGia.Width = Unit.Percentage(100);
                    txtTacGia.ID = "TacGia_" + i.ToString();
                    tblCell.Controls.Add(txtTacGia);
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);
                }
                divDanhSach.Controls.Add(tbl);
            }
            catch { }
        }

        private string KiemTra()
        {
            FITC_CDataTime dt = new FITC_CDataTime();
            string sLoi = "";
            if (drpWeb.SelectedValue == "0")
                sLoi = "Chưa chọn trang web!";
            if (drpChuyenMuc.SelectedValue == "0")
                sLoi = "Chưa chọn chuyên mục!";
            if (txtNoiDung.Text.Trim() == "")
                sLoi = "Chưa nhập Xpath nội dung bài viết!";
            return sLoi;
        }

        protected void btnThemMoi_Click(object sender, EventArgs e)
        {
            try
            {
                string strLoi = KiemTra();
                if (strLoi != "")
                {
                    ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                    return;
                }
                object[] obj = new object[9];

                obj[0] = txtXpathID.Value;
                obj[1] = txtTomTat.Text.Trim();
                obj[2] = txtNoiDung.Text.Trim();
                obj[3] = txtThoiGian.Text.Trim();
                obj[4] = txtTacGia.Text.Trim();
                obj[5] = drpChuyenMuc.SelectedValue;
                obj[6] = drpWeb.SelectedValue;
                obj[7] = TUONGTAC.TenTaiKhoan;
                obj[8] = txtTieuDe.Text.Trim();
                string sLoi = db.ExcuteSP("TTDN_XPATH_CHITIET_INSERT", obj);
                if (sLoi == "")
                {
                    if (chkKhac.Checked)
                    {
                        sLoi = Luu(drpWeb.SelectedValue, drpChuyenMuc.SelectedValue);
                    }
                    if (sLoi == "")
                        ScriptManager.RegisterClientScriptBlock(this.FindControl("btnThemMoi"), this.GetType(), "Message_Close", "alert('Cập nhật thành công !'); self.parent.tb_remove();", true);
                    else
                    {
                        ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                        return;
                    }
                }
                else
                {
                    ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                    return;
                }
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnThemMoi");
            }
        }

        protected void drpWeb_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpChuyenMuc.Items.Clear();
            if (drpWeb.SelectedValue == "0")
                drpChuyenMuc.Items.Add(new ListItem("[Chọn]", "0"));
            else
            {
                drpChuyenMuc.Items.Add(new ListItem("[Chọn]", "0"));
                DataSet dsChuyenMuc = db.GetDataSet("TTDN_CHUYENMUC_SELECT", 0, drpWeb.SelectedValue);
                if (dsChuyenMuc != null && dsChuyenMuc.Tables.Count > 0 && dsChuyenMuc.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsChuyenMuc.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = dsChuyenMuc.Tables[0].Rows[i];
                        drpChuyenMuc.Items.Add(new ListItem(row["TenChuyenMuc"].ToString() + " (" + row["UrlChuyenMuc"].ToString() + ")", row["ChuyenMucID"].ToString()));
                    }
                }
            }
        }

        protected void chkKhac_CheckedChanged(object sender, EventArgs e)
        {
            divDanhSach.Style.Add("display", "none");
            if (chkKhac.Checked)
            {
                divDanhSach.Style.Remove("display");
            }
        }

        private string Luu(string WebID, string ChuyenMucID)
        {
            string sLoi = "";

            for (int i = 1; i < 4; i++)
            {
                HiddenField txtXpathID = (HiddenField)FindControl("XpathID_" + i.ToString());
                TextBox txtTomTat = (TextBox)FindControl("TomTat_" + i.ToString());
                TextBox txtNoiDung = (TextBox)FindControl("NoiDung_" + i.ToString());
                TextBox txtThoiGian = (TextBox)FindControl("ThoiGian_" + i.ToString());
                TextBox txtTacGia = (TextBox)FindControl("TacGia_" + i.ToString());
                TextBox txtTieuDe = (TextBox)FindControl("TieuDe_" + i.ToString());

                object[] obj = new object[9];

                obj[0] = txtXpathID.Value;
                obj[1] = txtTomTat.Text.Trim();
                obj[2] = txtNoiDung.Text.Trim();
                obj[3] = txtThoiGian.Text.Trim();
                obj[4] = txtTacGia.Text.Trim();
                obj[5] = drpChuyenMuc.SelectedValue;
                obj[6] = drpWeb.SelectedValue;
                obj[7] = TUONGTAC.TenTaiKhoan;
                obj[7] = txtTieuDe.Text.Trim();

                if (!string.IsNullOrEmpty(txtTomTat.Text) || !string.IsNullOrEmpty(txtNoiDung.Text) || !string.IsNullOrEmpty(txtThoiGian.Text) || !string.IsNullOrEmpty(txtTacGia.Text) || !string.IsNullOrEmpty(txtTieuDe.Text))
                {
                    sLoi = db.ExcuteSP("TTDN_XPATH_CHITIET_INSERT", obj);
                    if (sLoi != "")
                        return sLoi;
                }
                else
                {
                    if (txtXpathID.Value != "0")
                        sLoi = db.ExcuteSP("TTDN_XPATH_CHITIET_DELETE", txtXpathID.Value);
                    if (sLoi != "")
                        return sLoi;
                }
            }

            return sLoi;
        }
    }
}