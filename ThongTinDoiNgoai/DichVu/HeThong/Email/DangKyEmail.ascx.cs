using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FITC.Web.Component;
using System.Data;

namespace ThongTinDoiNgoai.DichVu.HeThong.Email
{
    public partial class DangKyEmail : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadDanhSach();
            imgThemMoi.Attributes["onclick"] = string.Format("return thickboxPopup('Thêm mới cấu hình thư điện tử','{0}?btn={1}&control={2}', 750, '100%');",
               ResolveUrl("~/home/popup.aspx"), btnSuKien.ClientID, "/DichVu/hethong/Email/DangKyEmail_TM.ascx");
        }

        protected void imgDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btnImg = (ImageButton)sender;
            string id = btnImg.ID.Remove(0, 4);



            FITC_MailFunctions mf = new FITC_MailFunctions();
            string sLoi = "";

            sLoi = mf.DeleteEmailSettings(id, "TTDN_DM_EMAIL_DELETE");
            if (sLoi != "")
            {
                ham.Alert(this, "Lỗi: " + sLoi.Replace("'", "\\'"), "btnSuKien");
                return;
            }
            ham.Alert(this, "Xóa thành công", "btnSuKien");
            LoadDanhSach();
        }
        private void LoadDanhSach()
        {
            divDanhSach.Controls.Clear();

            Table tbl = new Table();
            tbl.Width = Unit.Percentage(100);
            tbl.CssClass = "Vien_Bang";
            tbl.CellPadding = 5;
            tbl.CellSpacing = 1;

            TableRow tblRow;
            TableCell tblCell;

            tblRow = new TableRow();
            tblRow.CssClass = "Dong_TieuDe";

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "STT";
            tblCell.Width = 30;
            tblCell.HorizontalAlign = HorizontalAlign.Center;
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Thư điện tử";
            tblCell.HorizontalAlign = HorizontalAlign.Center;
            tblRow.Controls.Add(tblCell);

            //tblCell = new TableCell();
            //tblCell.CssClass = "Cot_TieuDe";
            //tblCell.Text = "TK mặc định";
            //tblCell.Width = 80;
            //tblCell.HorizontalAlign = HorizontalAlign.Center;
            //tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Sửa";
            tblCell.Width = 40;
            tblCell.HorizontalAlign = HorizontalAlign.Center;
            tblRow.Controls.Add(tblCell);

            //tblCell = new TableCell();
            //tblCell.CssClass = "Cot_TieuDe";
            //tblCell.Text = "Xóa";
            //tblCell.Width = 40;
            //tblCell.HorizontalAlign = HorizontalAlign.Center;
            //tblRow.Controls.Add(tblCell);

            tbl.Controls.Add(tblRow);

            DataSet ds = db.GetDataSet("TTDN_DM_EMAIL_SELECT", 0, TUONGTAC.TaiKhoanID);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                imgThemMoi.Visible = false;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    DataRow row = ds.Tables[0].Rows[i];
                    string css = "Dong_Le";
                    if (i % 2 == 0)
                    {
                        css = "Dong_Chan";
                    }

                    tblRow = new TableRow();
                    tblRow.CssClass = css;

                    tblCell = new TableCell();
                    tblCell.HorizontalAlign = HorizontalAlign.Center;
                    tblCell.Text = (i + 1).ToString();
                    tblRow.Controls.Add(tblCell);

                    tblCell = new TableCell();
                    tblCell.Text = row["DiaChiEmail"].ToString().Trim();
                    tblCell.HorizontalAlign = HorizontalAlign.Left;
                    tblRow.Controls.Add(tblCell);

                    //tblCell = new TableCell();
                    //if ((bool)row["bMacDinh"])
                    //{
                    //    Image imgDefault = new Image();
                    //    imgDefault.ImageUrl = Static.AppPath() + "/images/HoatDong.gif";
                    //    imgDefault.ToolTip = "Tài khoản mặc định";
                    //    tblCell.Controls.Add(imgDefault);
                    //}
                    //tblCell.HorizontalAlign = HorizontalAlign.Center;
                    //tblRow.Controls.Add(tblCell);

                    //----------------------------------------------------------------

                    tblCell = new TableCell();
                    ImageButton imgEdit = new ImageButton();
                    imgEdit.ID = "edit_" + row["CauHinhID"].ToString().Trim();
                    imgEdit.ImageUrl = Static.AppPath() + "/images/edit.gif";
                    imgEdit.ToolTip = "Click để sửa";
                    imgEdit.CausesValidation = false;
                    imgEdit.Attributes["onclick"] = string.Format("return thickboxPopup('Thêm mới cấu hình thư điện tử','{0}?btn={1}&control={2}&SuaID={3}', 750, '100%');",
               ResolveUrl("~/home/popup.aspx"), btnSuKien.ClientID, "/DichVu/hethong/Email/DangKyEmail_TM.ascx", row["CauHinhID"].ToString().Trim());

                    tblCell.Controls.Add(imgEdit);
                    tblCell.HorizontalAlign = HorizontalAlign.Center;
                    tblRow.Controls.Add(tblCell);

                    //tblCell = new TableCell();
                    //ImageButton imgDelete = new ImageButton();
                    //imgDelete.ID = "del_" + row["CauHinhID"].ToString().Trim();
                    //imgDelete.ImageUrl = Static.AppPath() + "/images/delete.gif";
                    //imgDelete.ToolTip = "Click xoá";
                    //imgDelete.CausesValidation = false;
                    //imgDelete.Attributes["onclick"] = "return confirm('Bạn có chắc chắn xóa cấu hình này không?');";
                    //imgDelete.Click += new ImageClickEventHandler(imgDelete_Click);
                    //tblCell.Controls.Add(imgDelete);
                    //tblCell.HorizontalAlign = HorizontalAlign.Center;
                    //tblRow.Controls.Add(tblCell);

                    tbl.Controls.Add(tblRow);
                }

                ds.Clear();
                ds.Dispose();
            }
            else
            {
                tblRow = new TableRow();
                tblRow.CssClass = "Dong_Chan";

                tblCell = new TableCell();
                tblCell.ColumnSpan = 5;
                tblCell.Text = "Thông tin chưa được cập nhật";
                tblCell.HorizontalAlign = HorizontalAlign.Center;
                tblRow.Controls.Add(tblCell);

                tbl.Controls.Add(tblRow);
            }

            divDanhSach.Controls.Add(tbl);
        }
    }
}