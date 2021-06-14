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

using System.IO;


namespace ThongTinDoiNgoai.DichVu.HeThong.TaiKhoan
{
    public partial class DonVi : System.Web.UI.UserControl
    {
        CacHamChung ham = new CacHamChung();
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSuKien.Style.Add("display", "none");
            if (!IsPostBack)
            {
                Static.PhanTrangThu = 1;
                Session["node_select"] = "1";
                Session["Cap"] = 1;
                addTree_();
            }
            object[] objQuyen = TUONGTAC.getQuyen();
            if ((bool)objQuyen[(int)TUONGTAC.LoaiQuyen.Quyen2])
                btnDongBo.Visible = true;
            btnThemMoi.OnClientClick = string.Format("return thickboxPopup('Thêm mới đơn vị','{0}?control={1}&btn={2}&DonViChaID={3}&Cap={4}', 550, '100%');",
                 ResolveUrl("~/home/popup.aspx"), "/DichVu/HeThong/TaiKhoan/DonVi_Tm.ascx", btnSuKien.ClientID, Session["node_select"], Session["Cap"]);
            LoadData();
        }

        private void addTree_()
        {
            DataSet ds = db.GetDataSet("TTDN_DM_DONVI_SELECT", 1, 1);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow rowDV = ds.Tables[0].Rows[0];
                treeDonVi.Nodes.Clear();
                treeDonVi.CollapseAll();
                TreeNode nodeCon = new TreeNode(rowDV["TenDonVi"].ToString().Trim() + " - " + rowDV["MaDinhDanh"].ToString().Trim(), "1");
                nodeCon.ExpandAll();
                treeDonVi.Nodes.Add(nodeCon);

                ds = db.GetDataSet("TTDN_DM_DONVI_SELECT", 5, 0);
                addTree(ds, nodeCon.ChildNodes, "00.00.H57", 0);
                if (Session["node_select"].ToString().Trim() == "1")
                {
                    nodeCon.Select();
                }
            }
        }
        private void addTree(DataSet ds, TreeNodeCollection root, string DonViChaID, int iCap)
        {
            DataView dataView = ds.Tables[0].DefaultView;
            dataView.RowFilter = String.Format("MaDinhDanhCha= '{0}'", DonViChaID);
            dataView.Sort = "ThuTu";
            DataTable dt = dataView.ToTable();

            foreach (DataRow row in dt.Rows)
            {
                TreeNode nodeCon = new TreeNode(row["TenDonVi"].ToString().Trim() + " - " + row["MaDinhDanh"].ToString().Trim(), row["DonViID"].ToString().Trim());
                nodeCon.Collapse();
                if (row["DonViID"].ToString().Trim() == Session["node_select"].ToString().Trim())
                {
                    nodeCon.Expand();
                    nodeCon.Select();
                }
                root.Add(nodeCon);
                addTree(ds, nodeCon.ChildNodes, row["MaDinhDanh"].ToString().Trim(), iCap + 1);
            }
        }

        private void LoadData()
        {
            divDanhSach.Controls.Clear();
            lblPhanTrang.Text = "";
            Table tbl = new Table();
            tbl.CellPadding = 3;
            tbl.CellSpacing = 1;
            tbl.CssClass = "Vien_Bang";
            tbl.BorderWidth = Unit.Pixel(0);
            TableRow tblRow = new TableRow();
            TableCell tblCell = new TableCell();

            #region tạo bảng
            tbl.Width = Unit.Percentage(100);
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
            imgThuTu.Click += new ImageClickEventHandler(LuuThuTu_Click);
            tblCell.Controls.Add(imgThuTu);

            ImageButton imgXoaSTT = new ImageButton();
            imgXoaSTT.ID = "xoatt_";
            imgXoaSTT.ImageUrl = Static.AppPath() + "/Images/delete.gif";
            imgXoaSTT.ToolTip = "Click xóa thứ tự";
            imgXoaSTT.Click += new ImageClickEventHandler(XoaThuTu_Click);
            tblCell.Controls.Add(imgXoaSTT);
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.Text = "Mã định danh";
            tblCell.Width = 60;
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.HorizontalAlign = HorizontalAlign.Center;
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.Text = "Tên đơn vị";
            tblCell.CssClass = "Cot_TieuDe";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.Text = "ĐVHC";
            tblCell.CssClass = "Cot_TieuDe";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.Text = "Cấp";
            tblCell.CssClass = "Cot_TieuDe";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.Text = "Nhóm";
            tblCell.CssClass = "Cot_TieuDe";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.Text = "Sửa";
            tblCell.Width = 30;
            tblCell.CssClass = "Cot_TieuDe";
            tblRow.Controls.Add(tblCell);

            tblCell = new TableCell();
            tblCell.CssClass = "Cot_TieuDe";
            tblCell.Width = 30;
            tblCell.Text = "Xóa";
            tblRow.Controls.Add(tblCell);
            tbl.Controls.Add(tblRow);

            #endregion
            try
            {
                DataSet ds = db.GetDataSet("TTDN_DM_DONVI_SELECT", 4, Session["node_select"]);
                if (ds != null && ds.Tables[0].Rows.Count <= int.Parse(drpSoTin.Items[0].Text))
                    tblPhanTrang.Visible = false;
                else
                    tblPhanTrang.Visible = true;
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    //Phân trang===============================================
                    int TrangHienTai = Static.PhanTrangThu;
                    int TongSoTin = ds.Tables[0].Rows.Count;
                    int SoTinTrenTrang = Convert.ToInt16(drpSoTin.SelectedValue);
                    PhanTrang(TongSoTin, SoTinTrenTrang, TrangHienTai, lblPhanTrang);
                    int TuBanGhi = (TrangHienTai - 1) * SoTinTrenTrang;
                    int DenBanGhi = (TrangHienTai * SoTinTrenTrang) > TongSoTin ? TongSoTin : TrangHienTai * SoTinTrenTrang;
                    //End phân trang==========================================
                    //-----------------------------------------------
                    for (int i = TuBanGhi; i < DenBanGhi; i++)
                    {
                        DataRow row = ds.Tables[0].Rows[i];
                        tblRow = new TableRow();
                        string sCss = "";
                        if (i % 2 == 0)
                            sCss = "Dong_Chan";
                        else
                            sCss = "Dong_Le"; ;
                        tblRow.Attributes["class"] = sCss;
                        tblCell = new TableCell();
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        TextBox txt = new TextBox();
                        txt.Width = Unit.Pixel(30);
                        txt.Text = (i + 1).ToString();
                        txt.CssClass = "textbox";
                        txt.Style.Add("text-align", "center");
                        txt.ID = "STT_" + row["DonViID"].ToString();
                        tblCell.Controls.Add(txt);
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["MaDinhDanh"].ToString();
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["TenDonVi"].ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["TenPhuongXa"].ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        string sCap = "";
                        switch (row["Cap"].ToString())
                        {
                            case "1":
                                sCap = "Sở, ban, ngành";
                                break;
                            case "2":
                                sCap = "Huyện, thị xã, thành phố";
                                break;
                            case "3":
                                sCap = "Xã, phường";
                                break;
                            case "4":
                                sCap = "Cơ quan TW đóng trên địa bàn";
                                break;
                            case "5":
                                sCap = "Sự nghiệp";
                                break;
                            case "6":
                                sCap = "Doanh nghiệp";
                                break;
                        }
                        tblCell.Text = sCap;
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        tblCell.Text = row["TenNhom"].ToString();
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        ImageButton imgSua = new ImageButton();
                        imgSua.ImageUrl = Static.AppPath() + "/Images/edit.gif";
                        imgSua.ID = "Sua_" + row["DonViID"].ToString();
                        imgSua.OnClientClick = string.Format("return thickboxPopup('Cập nhật đơn vị','{0}?control={1}&btn={2}&DonViID={3}&DonViChaID={4}&Cap={5}', 550, '100%');",
                            ResolveUrl("~/home/popup.aspx"), "/DichVu/HeThong/TaiKhoan/DonVi_Tm.ascx", btnSuKien.ClientID, row["DonViID"].ToString(), row["DonViChaID"].ToString(), Session["Cap"]);
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblCell.Controls.Add(imgSua);
                        tblRow.Controls.Add(tblCell);

                        tblCell = new TableCell();
                        ImageButton imgXoa = new ImageButton();
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        imgXoa.ID = "Xoa_" + row["DonViID"].ToString();
                        imgXoa.ImageUrl = Static.AppPath() + "/images/delete.gif";
                        imgXoa.ToolTip = "Click xóa ";
                        imgXoa.CausesValidation = false;
                        imgXoa.Attributes["onclick"] = string.Format("return confirm('Bạn có chắc chắn xóa đơn vị\"{0}\" này không ?');", row["TenDonVi"].ToString());
                        imgXoa.Click += new ImageClickEventHandler(imgXoa_Click);
                        tblCell.CssClass = "Tin_O_CanhTrai";
                        tblCell.Controls.Add(imgXoa);
                        tblRow.Controls.Add(tblCell);
                        tbl.Controls.Add(tblRow);
                    }
                }
                else
                {
                    tblRow = new TableRow(); tblCell = new TableCell();
                    tblCell.ColumnSpan = 9;
                    tblCell.CssClass = "Dong_Chan";
                    tblCell.Text = "Thông tin chưa được cập nhật";
                    tblCell.HorizontalAlign = HorizontalAlign.Center;
                    tblRow.Controls.Add(tblCell);
                    tbl.Controls.Add(tblRow);
                }
                divDanhSach.Controls.Add(tbl);
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnSuKien");
            }
        }
        protected void imgXoa_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton btImg = (ImageButton)sender;
                string sID = btImg.ID.Remove(0, btImg.ID.LastIndexOf('_') + 1);
                string sLoi = db.ExcuteSP("TTDN_DM_DONVI_DELETE", sID);
                if (sLoi == "")
                {
                    ham.Alert(this, "Thực hiện xóa thành công", "btnSuKien");
                    LoadData();
                    addTree_();
                }
                else
                    ham.Alert(this, sLoi, "btnSuKien");
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnSuKien");
            }
        }
        protected void btnSuKien_Click(object sender, EventArgs e)
        {
            LoadData();
            addTree_();
        }
        protected void LuuThuTu_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataSet ds = db.GetDataSet("MADINHDANH_SELECT", 0, Session["node_select"]);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string sLoi = "";
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TextBox txtThuTu = (TextBox)this.FindControl("STT_" + row["DonViID"].ToString());
                        if (txtThuTu != null)
                        {
                            string stt = ham.getXoaDinhDang(txtThuTu.Text);
                            if (stt == "")
                                stt = "1";
                            sLoi = db.ExcuteSP("MADINHDANH_UPDATE_STT", row["DonViID"].ToString(), stt);
                        }
                    }
                    if (sLoi == "")
                    {
                        ham.Alert(this, "Lưu thứ tự thành công", "btnSuKien");
                        LoadData();
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
        protected void XoaThuTu_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataSet ds = db.GetDataSet("MADINHDANH_SELECT", 0, Session["node_select"]);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string sLoi = "";
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        sLoi = db.ExcuteSP("MADINHDANH_UPDATE_STT", row["DonViID"].ToString(), 1);
                    }
                    if (sLoi == "")
                    {
                        ham.Alert(this, "Xóa thứ tự thành công", "btnSuKien");
                        LoadData();
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
        protected void treeDonVi_OnSelectedNodeChanged(object sender, EventArgs e)
        {
            int iCap = 1;
            TreeNode node = treeDonVi.SelectedNode;
            Session["node_select"] = node.Value;
            while (node.Parent != null)
            {
                iCap = iCap + 1;
                node = node.Parent;
            }
            Session["Cap"] = iCap;
            node.ExpandAll();
            LoadData();
            btnThemMoi.OnClientClick = string.Format("return thickboxPopup('Thêm mới đơn vị','{0}?control={1}&btn={2}&DonViChaID={3}&Cap={4}', 550, '100%');",
               ResolveUrl("~/home/popup.aspx"), "/DichVu/HeThong/TaiKhoan/DonVi_Tm.ascx", btnSuKien.ClientID, Session["node_select"], Session["Cap"]);
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
                LoadData();
            }
            catch (Exception ex)
            {
                ham.Alert(this, ex.Message, "btnSuKien");
            }
        }
        protected void drpSoTin_SelectedIndexChanged(object sender, EventArgs e)
        {
            Static.PhanTrangThu = 1;
            LoadData();
        }


        #endregion

        protected void btnTim_Click(object sender, EventArgs e)
        {
            if (txtTuKhoa.Text != "")
            {
                treeDonVi.Nodes.Clear();
                treeDonVi.CollapseAll();
                TreeNode nodeCon = new TreeNode("Cây đơn vị", "0");
                nodeCon.ExpandAll();
                treeDonVi.Nodes.Add(nodeCon);
                DataSet ds = db.GetDataSet("TTDN_DM_DONVI_SELECT", 6, 0, "", txtTuKhoa.Text);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    TreeNode nodeCon1 = new TreeNode(row["TenDonVi"].ToString().Trim() + " - " + row["MaDinhDanh"].ToString().Trim(), row["DonViID"].ToString().Trim());
                    nodeCon.ChildNodes.Add(nodeCon1);
                }
            }
            else
                addTree_();
        }

        protected void btnDongBo_Click(object sender, EventArgs e)
        {
            object[] obj = new object[2];
            obj[0] = 0;
            DataSet ds = db.GetDataSet("TTDN_DM_DONVI_SELECT", 7);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                obj[1] = Convert.ToDateTime(row["NgayCapNhat"].ToString());
            }
            DichVuChay dichvu = new DichVuChay("https://esb.thuathienhue.gov.vn/service/httg.asmx", true);
            ds = dichvu.getDataSet("", "MADINHDANH_SELECT_THEONGAY", obj);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    obj = new object[14];
                    obj[0] = row["DonViID"].ToString();
                    obj[1] = row["DonViChaID"].ToString();
                    obj[2] = row["TenDonVi"].ToString();
                    obj[3] = row["MaDinhDanh"].ToString();
                    obj[4] = row["MaDinhDanhCha"].ToString();
                    obj[5] = row["SDT"].ToString();
                    obj[6] = row["Fax"].ToString();
                    obj[7] = row["DiaChi"].ToString();
                    obj[8] = row["Email"].ToString();
                    obj[9] = row["website"].ToString();
                    obj[10] = row["TenVietTat"].ToString();
                    obj[11] = row["Cap"].ToString();
                    obj[12] = row["MaQuanHuyen"].ToString();
                    obj[13] = row["MaPhuongXa"].ToString();
                    string sLoi = db.ExcuteSP("TTDN_DM_DONVI_INSERT_DB", obj);
                    if (sLoi != "")
                        Response.Write(sLoi);
                }
            }
            LoadData();
        }

    }
}