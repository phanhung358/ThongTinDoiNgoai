using FITC.Web.Component;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ThongTinDoiNgoai.DichVu.ThongTinDoiNgoai
{
    public partial class BaiViet_Tm : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sBaiVietID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["BaiVietID"] != null)
                sBaiVietID = Request.QueryString["BaiVietID"];
            if (!IsPostBack)
            {
                addDanhMuc();
            }
            addData();
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

        private void addData()
        {
            if (!string.IsNullOrEmpty(sBaiVietID))
            {
                DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 0, sBaiVietID);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    drpWeb.SelectedValue = row["WebID"].ToString();
                    drpWeb_SelectedIndexChanged(null, null);
                    drpChuyenMuc.SelectedValue = row["ChuyenMucID"].ToString();
                    txtTieuDe.Text = row["TieuDe"].ToString();
                    txtTomTat.Text = row["TomTat"].ToString();
                    txtNoiDung.Text = row["NoiDung"].ToString();
                    txtTacGia.Text = row["TacGia"].ToString();
                    txtBaiViet_Url.Text = row["BaiViet_Url"].ToString();
                    AnhDaiDien.Src = string.IsNullOrEmpty(row["AnhDaiDien"].ToString()) ? "/Images/no_image.png" : row["AnhDaiDien"].ToString();
                }
            }
        }

        private string KiemTra()
        {
            FITC_CDataTime dt = new FITC_CDataTime();
            if (drpWeb.SelectedValue == "0")
            {
                drpWeb.Focus();
                return "Chưa chọn trang web!";
            }
            if (txtTieuDe.Text.Trim() == "")
            {
                txtTieuDe.Focus();
                return "Chưa nhập tiêu đề bài viết!";
            }
            if (txtTomTat.Text.Trim() == "")
            {
                txtTomTat.Focus();
                return "Chưa nhập tóm tắt bài viết!";
            }
            if (txtNoiDung.Text.Trim() == "")
            {
                txtNoiDung.Focus();
                return "Chưa nhập nội dung bài viết!";
            }
            return "";
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

                string ThoiGian = null;
                string sAnhDaiDien = null;
                if (!string.IsNullOrEmpty(sBaiVietID))
                {
                    DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 0, sBaiVietID);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];

                        ThoiGian = row["ThoiGian"].ToString();
                    }
                }
                string diachiweb = "";
                DataSet ds1 = db.GetDataSet("TTDN_TRANGWEB_SELECT", 1, drpWeb.SelectedValue);
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds1.Tables[0].Rows[0];

                    diachiweb = row["DiaChiWeb"].ToString();
                }
                string DirUpload = ConfigurationManager.AppSettings["ThuMuc"].Replace("\\", "/") + "/UploadFiles/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + diachiweb.Remove(0, diachiweb.IndexOf("/") + 2) + "/";
                sAnhDaiDien = Upload(DirUpload, UploadFile);
                if (sAnhDaiDien == null && AnhDaiDien.Src != "/Images/no_image.png")
                    sAnhDaiDien = AnhDaiDien.Src;

                object[] obj = new object[12];

                obj[0] = txtTieuDe.Text.Trim();
                obj[1] = txtTomTat.Text.Trim();
                obj[2] = txtNoiDung.Text.Trim();
                obj[3] = ThoiGian;
                obj[4] = txtTacGia.Text.Trim();
                obj[5] = txtBaiViet_Url.Text.Trim();
                obj[6] = txtNoiDung.Text.Trim().Length;
                obj[7] = drpChuyenMuc.SelectedValue;
                obj[8] = drpWeb.SelectedValue;
                obj[9] = sAnhDaiDien;
                obj[10] = 0;
                obj[11] = TUONGTAC.TaiKhoanID;
                string sLoi = db.ExcuteSP("TTDN_BAIVIET_INSERT", obj);
                if (sLoi == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this.FindControl("btnThemMoi"), this.GetType(), "Message_Close", "alert('Cập nhật thành công !'); self.parent.tb_remove();", true);
                }
                else
                {
                    ham.Alert(this, sLoi, "btnThemMoi");
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

        bool CheckFileType(string fileName)
        {

            string ext = Path.GetExtension(fileName);
            switch (ext.ToLower())
            {
                case ".gif":
                    return true;
                case ".png":
                    return true;
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                default:
                    return false;
            }
        }

        public string Upload(string folderName, FileUpload file)
        {
            string FilePath = folderName;
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            if (Page.IsValid && file.HasFile && CheckFileType(file.FileName))
            {
                string fullFileName = ChangeFileName(FilePath, file.FileName);
                string filePath = Path.Combine(FilePath, fullFileName);
                file.SaveAs(filePath);
                return filePath.Substring(filePath.IndexOf("/UploadFiles"));
            }
            else
                return null;
        }

        public string ChangeFileName(string folderName, string fullFileName)
        {
            string fileName = Path.GetFileNameWithoutExtension(fullFileName);
            string fileExtension = Path.GetExtension(fullFileName);
            if (File.Exists(Path.Combine(folderName, fullFileName)))
            {
                if ((fileName.LastIndexOf(")") == fileName.Length - 1) && (fileName.LastIndexOf(")") - fileName.LastIndexOf("(") >= 2))
                {
                    string tam = fileName.Substring(fileName.LastIndexOf("(") + 1, fileName.LastIndexOf(")") - fileName.LastIndexOf("(") - 1);
                    if (int.TryParse(tam, out int chiso))
                    {
                        chiso++;
                        fileName = fileName.Replace(tam, chiso.ToString());
                        return ChangeFileName(folderName, fileName + fileExtension);
                    }
                    else
                    {
                        fileName = fileName + "(1)";
                        return ChangeFileName(folderName, fileName + fileExtension);
                    }
                }
                else
                {
                    fileName = fileName + "(1)";
                    return ChangeFileName(folderName, fileName + fileExtension);
                }
            }
            return fileName + fileExtension;
        }
    }
}