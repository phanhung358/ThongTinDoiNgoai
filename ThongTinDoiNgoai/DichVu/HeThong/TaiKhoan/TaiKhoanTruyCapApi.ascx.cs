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

namespace PhanAnhKienNghi.DichVu.Reputa
{
    public partial class TaiKhoanTruyCapApi : System.Web.UI.UserControl
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
            tblCell.Text = "Tài khoản";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Tên đơn vị";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Token";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 100;
            tblCell.Text = "Lần gọi cuối";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Lượt gọi";
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

            DataSet ds = db.GetDataSet("TTDN_DM_TAIKHOAN_TRUYCAP_SELECT", 2);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
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
                    tblCell.Text = row["TaiKhoan"].ToString().Trim();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    tblCell.Text = row["TenDonVi"].ToString().Trim();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    tblCell.Text = row["Token"].ToString().Trim();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    tblCell.Text = Convert.ToDateTime(row["ThoiGian"].ToString().Trim()).ToString("dd/MM/yyyy HH:mm");
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    tblCell.HorizontalAlign = HorizontalAlign.Center;
                    tblCell.Text = ham.getDinhDang(row["SoLuot"].ToString().Trim());
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    ImageButton imgSua = new ImageButton();
                    imgSua.ID = "Sua_" + row["TaiKhoanID"].ToString();
                    imgSua.ToolTip = "Click để sửa";
                    imgSua.ImageUrl = Static.AppPath() + "/images/edit.gif";
                    imgSua.Click += new ImageClickEventHandler(imgSua_Click);
                    tblCell.Controls.Add(imgSua);
                    tblCell.HorizontalAlign = HorizontalAlign.Center;
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    ImageButton imgXoa = new ImageButton();
                    imgXoa.ID = "Xoa_" + row["TaiKhoanID"].ToString().Trim();
                    imgXoa.ImageUrl = Static.AppPath() + "/images/delete.gif";
                    imgXoa.ToolTip = "Click xóa ";
                    imgXoa.CausesValidation = false;
                    imgXoa.Attributes["onclick"] = string.Format("return confirm('Bạn có chắc chắn xóa tài khoản \"{0}\" này không ?');", row["TaiKhoan"]);
                    imgXoa.Click += new ImageClickEventHandler(imgXoa_Click);
                    tblCell.Controls.Add(imgXoa);
                    tblCell.HorizontalAlign = HorizontalAlign.Center;
                    tblRow.Controls.Add(tblCell);

                    tbl.Controls.Add(tblRow);
                }
            }
            divDanhSach.Controls.Add(tbl);
        }
        private string hopLe()
        {
            if (txtTaiKhoan.Text == "")
                return "Vui lòng nhập tên tài khoản!";
            if (txtTenDonVi.Text == "")
                return "Vui lòng nhập tên đơn vị!";
            return "";
        }
        protected void btnThemMoi_Click(object sender, EventArgs e)
        {
            string strLoi = "";
            strLoi = this.hopLe();
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                return;
            }
            FITC_CSecurity pass = new FITC_CSecurity();

            object[] obj = new object[4];
            obj[0] = 0;
            obj[1] = txtTaiKhoan.Text.Trim();
            obj[2] = txtTenDonVi.Text;
            obj[3] = pass.EncodePassword(txtTaiKhoan.Text.Trim() + DateTime.Now.ToString(), "SHA1");

            strLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_TRUYCAP_INSERT", obj);
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                return;
            }
            addData();
            ham.Alert(this, "Thêm mới thành công !", "btnThemMoi");
            btnHuyBo_Click(null, null);
        }
        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            string strLoi = "";
            strLoi = this.hopLe();
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                return;
            }
            FITC_CSecurity pass = new FITC_CSecurity();
            object[] obj = new object[4];
            obj[0] = hidID.Value;
            obj[1] = txtTaiKhoan.Text.Trim();
            obj[2] = txtTenDonVi.Text;
            obj[3] = pass.EncodePassword(txtTaiKhoan.Text.Trim() + DateTime.Now.ToString(), "SHA1");
            strLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_TRUYCAP_INSERT", obj);

            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                return;
            }
            addData();
            ham.Alert(this, "Cập nhật thành công !", "btnThemMoi");
            btnHuyBo_Click(null, null);
        }
        protected void btnHuyBo_Click(object sender, EventArgs e)
        {
            txtTenDonVi.Text = "";
            txtTaiKhoan.Text = "";
            txtTaiKhoan.Focus();
            btnThemMoi.Visible = true;
            btnCapNhat.Visible = false;
        }

        protected void imgSua_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnImg = (ImageButton)sender;
                string strID = btnImg.ID.Remove(0, btnImg.ID.LastIndexOf('_') + 1);
                using (DataSet ds = db.GetDataSet("TTDN_DM_TAIKHOAN_TRUYCAP_SELECT", 1, strID))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        hidID.Value = ds.Tables[0].Rows[0]["TaiKhoanID"].ToString().Trim();
                        txtTaiKhoan.Text = ds.Tables[0].Rows[0]["TaiKhoan"].ToString().Trim();
                        txtTenDonVi.Text = ds.Tables[0].Rows[0]["TenDonVi"].ToString().Trim();
                        btnThemMoi.Visible = false;
                        btnCapNhat.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message.Replace("'", "\\\""), "btnThemMoi");
            }
        }
        protected void imgXoa_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnImg = (ImageButton)sender;
                string strID = btnImg.ID.Remove(0, btnImg.ID.LastIndexOf('_') + 1);
                string strLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_TRUYCAP_DELETE", strID);
                if (strLoi != "")
                {
                    ham.Alert(this, strLoi.Replace("'", "\\\""), "btnThemMoi");
                    return;
                }
                ham.Alert(this, "Xóa thành công !", "btnThemMoi");
                addData();
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message.Replace("'", "\\\""), "btnThemMoi");
            }
        }
    }
}