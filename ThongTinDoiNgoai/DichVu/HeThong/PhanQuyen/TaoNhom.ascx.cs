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

namespace ThongTinDoiNgoai.DichVu.HeThong.PhanQuyen
{
    public partial class TaoNhom : System.Web.UI.UserControl
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

            object[] obj = new object[1];
            obj[0] = txtNhom.Text.Trim();

            strLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_NHOM_INSERT", obj);
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                return;
            }

            ham.Alert(this, "Thêm mới thành công !", "btnSuKien");
            btnHuyBo_Click(null, null);
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

            object[] obj = new object[2];
            obj[0] = hidID.Value;
            obj[1] = txtNhom.Text.Trim();
            strLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_NHOM_UPDATE", obj);
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                return;
            }

            ham.Alert(this, "Cập nhật thành công !", "btnSuKien");
            btnHuyBo_Click(null, null);
            addData();
        }
        protected void btnHuyBo_Click(object sender, EventArgs e)
        {
            txtNhom.Text = "";
            hidID.Value = "";
            btnThemMoi.Visible = true;
            btnCapNhat.Visible = false;
        }
        protected void imgSua_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnImg = (ImageButton)sender;
                string strID = btnImg.ID.Remove(0, btnImg.ID.LastIndexOf('_') + 1);
                using (DataSet ds = db.GetDataSet("TTDN_DM_TAIKHOAN_NHOM_SELECT", 1, strID))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        hidID.Value = ds.Tables[0].Rows[0]["NhomID"].ToString().Trim();
                        txtNhom.Text = ds.Tables[0].Rows[0]["TenNhom"].ToString().Trim();
                        btnThemMoi.Visible = false;
                        btnCapNhat.Visible = true;
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
                string strLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_NHOM_DELETE", strID);
                if (strLoi != "")
                {
                    ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                    return;
                }
                ham.Alert(this, "Xóa thành công !", "btnSuKien");
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
            str = txtNhom.Text.Trim();
            if (str.Length == 0)
            {
                txtNhom.Focus();
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
            tblCell.Width = 10;
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

            using (DataSet ds = db.GetDataSet("TTDN_DM_TAIKHOAN_NHOM_SELECT", 0))
            {
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
                        tblCell.Text = row["TenNhom"].ToString().Trim();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        TextBox txtSTT = new TextBox();
                        txtSTT.CssClass = "textbox";
                        txtSTT.Style.Add("text-align", "center");
                        ham.setTextBox_Number(txtSTT, false);
                        txtSTT.ID = "ThuTu_" + row["NhomID"].ToString().Trim();
                        txtSTT.Text = (i + 1).ToString().Trim();
                        txtSTT.Width = 25;
                        tblCell.Controls.Add(txtSTT);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();

                        ImageButton imgSua = new ImageButton();
                        imgSua.ID = "Sua_" + row["NhomID"].ToString();
                        imgSua.ToolTip = "Click để sửa";
                        imgSua.ImageUrl = Static.AppPath() + "/images/edit.gif";
                        imgSua.Click += new ImageClickEventHandler(imgSua_Click);
                        tblCell.Controls.Add(imgSua);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        if (row["NhomID"].ToString().Trim() != "2")
                        {
                            ImageButton imgXoa = new ImageButton();
                            imgXoa.ID = "Xoa_" + row["NhomID"].ToString().Trim();
                            imgXoa.ImageUrl = Static.AppPath() + "/images/delete.gif";
                            imgXoa.ToolTip = "Click xóa ";
                            imgXoa.CausesValidation = false;
                            imgXoa.Attributes["onclick"] = string.Format("return confirm('Bạn có chắc chắn xóa nhóm \"{0}\" này không ?');", row["TenNhom"]);
                            imgXoa.Click += new ImageClickEventHandler(imgXoa_Click);
                            tblCell.Controls.Add(imgXoa);
                        }
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

        protected void luuThuTu_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string sLoi = "";
                DataSet ds = db.GetDataSet("TTDN_DM_TAIKHOAN_NHOM_SELECT", 0);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TextBox txtSTT = (TextBox)this.FindControl("ThuTu_" + row["NhomID"].ToString().Trim());
                        if (txtSTT != null)
                        {
                            string stt = ham.getXoaDinhDang(txtSTT.Text);
                            if (stt == "")
                                stt = "1";
                            sLoi = db.ExcuteSP("TTDN_DM_TAIKHOAN_NHOM_UPDATE_STT", row["NhomID"].ToString().Trim(), stt);
                            if (sLoi != "")
                                break;
                        }
                    }

                }
                if (sLoi == "")
                {
                    ham.Alert(this, "Lưu thứ tự thành công !", "btnThemMoi");
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