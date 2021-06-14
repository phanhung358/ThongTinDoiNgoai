using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FITC.Web.Component;
using System.Data;
using System.Web.Script.Serialization;

namespace ThongTinDoiNgoai.DichVu.HeThong.TaoMenu
{
    public partial class TaoMenu : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        object[] objQuyen = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            objQuyen = TUONGTAC.getQuyen();
            if (!IsPostBack)
            {
                Static.PhanTrangThu = 1;
                Session["node_select"] = "0";
                addTree_();
            }
            imgThemMoi.Attributes["onclick"] = string.Format("return thickboxPopup('Thêm mới chức năng','{0}?btn={1}&control={2}', 500, 370);",
               ResolveUrl("~/home/popup.aspx"), btnSuKien.ClientID, "/DichVu/hethong/TaoMenu/TaoMenu_TM.ascx");
            addData();
        }

        private void ExxpanAll(TreeNode nodeCon)
        {
            //nodeCon.ExpandAll();
            //if (nodeCon.Parent != null)
            //ExxpanAll(nodeCon.Parent);
        }
        private void addTree_()
        {
            treePhanMuc.Nodes.Clear();
            treePhanMuc.CollapseAll();
            TreeNode nodeCon = new TreeNode("Hệ thống chức năng", "0");
            nodeCon.Expand();
            treePhanMuc.Nodes.Add(nodeCon);
            addTree(nodeCon.ChildNodes, "0");
            if (Session["node_select"].ToString().Trim() == "0")
            {
                nodeCon.Select();
                //nodeCon.ExpandAll();
            }
        }
        private void addTree(TreeNodeCollection root, string sMenuChaID)
        {
            int iCongViec = 4;
            if (!(bool)objQuyen[(int)TUONGTAC.LoaiQuyen.Quyen2])
                iCongViec = 7;
            DataSet ds = db.GetDataSet("TTDN_DM_MENU_SELECT", iCongViec, sMenuChaID, 0, null);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    TreeNode nodeCon = new TreeNode(row["Tenmenu"].ToString().Trim(), row["menuid"].ToString().Trim());
                    nodeCon.Collapse();
                    root.Add(nodeCon);
                    if (Session["node_select"].ToString().Trim() == row["MenuID"].ToString().Trim())
                    {
                        nodeCon.Select();
                        nodeCon.Expand();
                        if (nodeCon.Parent != null)
                            nodeCon.Parent.Expand();
                    }
                    addTree(nodeCon.ChildNodes, row["MenuID"].ToString().Trim());
                }
            }
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

            TableRow tblRow;
            TableCell tblCell;

            tblRow = new TableRow();
            tblRow.CssClass = "Dong_TieuDe";

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
            tblCell.Text = "Tên chức năng";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "File liên kết";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Quyền";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Text = "Hoạt động";
            tblCell.Width = 60;
            tblRow.Controls.Add(tblCell);

            tbl.Controls.Add(tblRow);

            int iCongViec = 4;

            if (!(bool)objQuyen[(int)TUONGTAC.LoaiQuyen.Quyen2])
                iCongViec = 7;

            using (DataSet ds = db.GetDataSet("TTDN_DM_MENU_SELECT", iCongViec, Session["node_select"], 0, null))
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
                        TextBox txtSTT = new TextBox();
                        txtSTT.CssClass = "textbox";
                        txtSTT.Style.Add("text-align", "center");
                        ham.setTextBox_Number(txtSTT, false);
                        txtSTT.ID = "ThuTu_" + row["MenuID"].ToString().Trim();
                        txtSTT.Text = (i + 1).ToString().Trim();
                        txtSTT.Width = 25;
                        tblCell.Controls.Add(txtSTT);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        LinkButton lnk = new LinkButton();
                        lnk.ID = "Sua_" + row["MenuID"].ToString();
                        lnk.Attributes["onclick"] = string.Format("return thickboxPopup('Thêm mới chức năng','{0}?btn={1}&control={2}&MenuID={3}&cv={4}', 500, 350);",
               ResolveUrl("~/home/popup.aspx"), btnSuKien.ClientID, "/DichVu/hethong/TaoMenu/TaoMenu_TM.ascx", row["MenuID"], iCongViec);
                        lnk.Text = row["TenMenu"].ToString().Trim();
                        tblCell.Controls.Add(lnk);
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["FileLienKet"].ToString().Trim();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["dsQuyen"].ToString().Trim();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        if (row["bHoatDong"].ToString().Trim() == "True")
                            tblCell.Text = "x";
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

        protected void treePhanMuc_OnSelectedNodeChanged(object sender, EventArgs e)
        {
            TreeNode node = treePhanMuc.SelectedNode;
            Session["node_select"] = node.Value;
            node.ExpandAll();
            addData();
            imgThemMoi.Attributes["onclick"] = string.Format("return thickboxPopup('Thêm mới chức năng','{0}?btn={1}&control={2}&MenuChaID={3}', 500, 370);",
              ResolveUrl("~/home/popup.aspx"), btnSuKien.ClientID, "/DichVu/hethong/TaoMenu/TaoMenu_TM.ascx", treePhanMuc.SelectedNode.Value);

        }
        protected void luuThuTu_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataSet ds = db.GetDataSet("TTDN_DM_MENU_SELECT", 4, Session["node_select"]);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string sLoi = "";
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TextBox txtSTT = (TextBox)this.FindControl("ThuTu_" + row["MenuID"].ToString().Trim());
                        if (txtSTT != null)
                        {
                            string stt = ham.getXoaDinhDang(txtSTT.Text);
                            if (stt == "")
                                stt = "1";
                            sLoi = db.ExcuteSP("TTDN_DM_MENU_UPDATE_STT", row["MenuID"].ToString().Trim(), stt);
                            if (sLoi != "")
                                break;
                        }
                    }
                    if (sLoi == "")
                    {
                        ham.Alert(this, "Lưu thứ tự thành công !", "btnSuKien");
                        addData();
                        addTree_();
                    }
                    else
                        ham.Alert(this, sLoi, "btnSuKien");
                }
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnSuKien");
            }
        }
        protected void imgXoa_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btnImg = (ImageButton)sender;
            string strID = btnImg.ID.Remove(0, btnImg.ID.LastIndexOf('_') + 1);
            string strLoi = db.ExcuteSP("TTDN_DM_MENU_DELETE", strID);
            if (strLoi != "")
            {
                ham.Alert(this, strLoi.Replace("'", "\\\""), "btnSuKien");
                return;
            }
            addData();
            ham.Alert(this, "Xóa thành công !", "btnSuKien");
            btnSuKien_Click(null, null);
        }
        protected void btnSuKien_Click(object sender, EventArgs e)
        {
            addTree_();
        }
        protected void drpLoaiHinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            addData();
        }

    }

    public class PhanAnhPush
    {
        public string id { get; set; }
        public string nguonid { get; set; }
        public string linhvucid { get; set; }

        public string tenlinhvuc { get; set; }
        public string taikhoan { get; set; }
        public string nguoiphananh { get; set; }

        public string dienthoai { get; set; }
        public string diachi { get; set; }
        public string email { get; set; }
        public string tieude { get; set; }
        public string noidung_phananh { get; set; }
        public string filephananh { get; set; }
        public string ngayphananh { get; set; }
        public string kinhdo { get; set; }

        public string vido { get; set; }

        public string noidung_traloi { get; set; }

        public string filetraloi { get; set; }

        public string ngaytraloi { get; set; }
        public string diachi_sukien { get; set; }
        public bool cong_khai { get; set; }
        public string tungay { get; set; }

        public string denngay { get; set; }
        public string don_vi_thu_ly { get; set; }
        public string trangthai { get; set; }
        public string danhgia { get; set; }

        public IList<FileDinhKemPush> danh_sach_filedinhkem { get; set; }
        public IList<FileDinhKemPush> danh_sach_filedinhkem_kq { get; set; }
    }
    public class FileDinhKemPush
    {
        public string FileName { get; set; }
    }
}