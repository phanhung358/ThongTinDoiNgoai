using System.Data;
using System.Configuration;
using System.Web;
using System.Net;
using FITC.Web.Component;
using System.IO;
using System.Web.Script.Serialization;
using System;
using ThongTinDoiNgoai;

public static class TUONGTAC
{
    public enum LoaiQuyen
    {
        Quyen1 = 0,
        Quyen2,
        Quyen3,
        Quyen4,
        Quyen5,
        Quyen6
    }
    public static string DonViID
    {
        get
        {
            if (HttpContext.Current.Session["CPXD_DonViID"] != null)
                return HttpContext.Current.Session["CPXD_DonViID"].ToString();
            else
                return null;
        }
        set
        {
            HttpContext.Current.Session["CPXD_DonViID"] = value;
        }
    }
    public static string MaDinhDanh
    {
        get
        {
            if (HttpContext.Current.Session["CPXD_MaDinhDanh"] != null)
                return HttpContext.Current.Session["CPXD_MaDinhDanh"].ToString();
            else
                return null;
        }
        set
        {
            HttpContext.Current.Session["CPXD_MaDinhDanh"] = value;
        }
    }
    public static string BoPhanID
    {
        get
        {
            if (HttpContext.Current.Session["CPXD_BoPhanID"] != null)
                return HttpContext.Current.Session["CPXD_BoPhanID"].ToString();
            else
                return null;
        }
        set
        {
            HttpContext.Current.Session["CPXD_BoPhanID"] = value;
        }
    }
    public static string NhomID
    {
        get
        {
            if (HttpContext.Current.Session["CPXD_NhomID"] != null)
                return HttpContext.Current.Session["CPXD_NhomID"].ToString();
            else
                return null;
        }
        set
        {
            HttpContext.Current.Session["CPXD_NhomID"] = value;
        }
    }
    public static string TenTaiKhoan
    {
        get
        {
            if (HttpContext.Current.Session["CPXD_TenTaiKhoan"] != null)
                return HttpContext.Current.Session["CPXD_TenTaiKhoan"].ToString();
            else
                return null;
        }
        set
        {
            HttpContext.Current.Session["CPXD_TenTaiKhoan"] = value;
        }
    }
    public static string TenDangNhap
    {
        get
        {
            if (HttpContext.Current.Session["CPXD_TenDangNhap"] != null)
                return HttpContext.Current.Session["CPXD_TenDangNhap"].ToString();
            else
                return null;
        }
        set
        {
            HttpContext.Current.Session["CPXD_TenDangNhap"] = value;
        }
    }
    public static string TaiKhoanID
    {
        get
        {
            if (HttpContext.Current.Session["CPXD_TaiKhoanID"] != null)
                return HttpContext.Current.Session["CPXD_TaiKhoanID"].ToString();
            else
                return null;
        }
        set
        {
            HttpContext.Current.Session["CPXD_TaiKhoanID"] = value;
        }
    }

    public static string getTenDonVi(string MaDinhDanh)
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        System.Data.DataSet dsDonVi = db.GetDataSet("TTDN_DM_DONVI_SELECT", 3, 0, MaDinhDanh);
        if (dsDonVi != null && dsDonVi.Tables.Count > 0 && dsDonVi.Tables[0].Rows.Count > 0)
        {
            return dsDonVi.Tables[0].Rows[0]["TenDonVi"].ToString();
        }

        return "";
    }
    public static string getTenDonVi()
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        System.Data.DataSet dsDonVi = db.GetDataSet("TTDN_DM_DONVI_SELECT", 3, 0, TUONGTAC.MaDinhDanh);
        if (dsDonVi != null && dsDonVi.Tables.Count > 0 && dsDonVi.Tables[0].Rows.Count > 0)
        {
            return dsDonVi.Tables[0].Rows[0]["TenDonVi"].ToString();
        }
        else
            return "&nbsp;";
    }
    public static void luuNhatKy1(string sThaoTac)
    {
        try
        {
            FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
            string sMenuID = "0";
            if (HttpContext.Current.Request.QueryString["id"] != null)
                sMenuID = HttpContext.Current.Request.QueryString["id"];
            object[] obj = new object[5];
            obj[0] = DonViID;
            obj[1] = TaiKhoanID;
            obj[2] = TenDangNhap;
            obj[3] = sMenuID;
            obj[4] = sThaoTac;
            db.ExcuteSP("TTDN_NHATKY_SUDUNG_INSERT", obj);
        }
        catch { }
    }

    public static object[] getQuyen()
    {
        string sMenuID = HttpContext.Current.Request.QueryString["id"];
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        object[] obj = new object[6];
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i] = false;
        }
        DataSet ds = db.GetDataSet("TTDN_QUYENCHUCNANG_SELECT", 0, TUONGTAC.TaiKhoanID, sMenuID, 0, TUONGTAC.NhomID, TUONGTAC.DonViID);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                switch (ds.Tables[0].Rows[i]["QuyenID"].ToString().Trim().ToLower())
                {
                    case "1":
                        obj[(int)LoaiQuyen.Quyen1] = true;
                        break;
                    case "2":
                        obj[(int)LoaiQuyen.Quyen2] = true;
                        break;
                    case "3":
                        obj[(int)LoaiQuyen.Quyen3] = true;
                        break;
                    case "4":
                        obj[(int)LoaiQuyen.Quyen4] = true;
                        break;
                    case "5":
                        obj[(int)LoaiQuyen.Quyen5] = true;
                        break;
                    case "6":
                        obj[(int)LoaiQuyen.Quyen6] = true;
                        break;
                }
            }
        }
        return obj;
    }

    public static object[] getQuyen(int sMenuID)
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        object[] obj = new object[6];
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i] = false;
        }

        //DataSet ds = db.GetDataSet("TTDN_QUYENCHUCNANG_SELECT", 2, TUONGTAC.TaiKhoanID, sMenuID, 0, TUONGTAC.NhomID);
        DataSet ds = db.GetDataSet("TTDN_QUYENCHUCNANG_SELECT", 0, TUONGTAC.TaiKhoanID, sMenuID, 0, TUONGTAC.NhomID, TUONGTAC.DonViID);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                switch (ds.Tables[0].Rows[i]["QuyenID"].ToString().Trim().ToLower())
                {
                    case "1":
                        obj[(int)LoaiQuyen.Quyen1] = true;
                        break;
                    case "2":
                        obj[(int)LoaiQuyen.Quyen2] = true;
                        break;
                    case "3":
                        obj[(int)LoaiQuyen.Quyen3] = true;
                        break;
                    case "4":
                        obj[(int)LoaiQuyen.Quyen4] = true;
                        break;
                    case "5":
                        obj[(int)LoaiQuyen.Quyen5] = true;
                        break;
                    case "6":
                        obj[(int)LoaiQuyen.Quyen6] = true;
                        break;
                }
            }
        }
        return obj;
    }
    public static object[] getQuyen(int sMenuID, string sNhanVienID, string NhomID)
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        object[] obj = new object[6];
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i] = false;
        }

        DataSet ds = db.GetDataSet("TTDN_QUYENCHUCNANG_SELECT", 0, sNhanVienID, sMenuID, 0, NhomID, 0);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                switch (ds.Tables[0].Rows[i]["QuyenID"].ToString().Trim().ToLower())
                {
                    case "1":
                        obj[(int)LoaiQuyen.Quyen1] = true;
                        break;
                    case "2":
                        obj[(int)LoaiQuyen.Quyen2] = true;
                        break;
                    case "3":
                        obj[(int)LoaiQuyen.Quyen3] = true;
                        break;
                    case "4":
                        obj[(int)LoaiQuyen.Quyen4] = true;
                        break;
                    case "5":
                        obj[(int)LoaiQuyen.Quyen5] = true;
                        break;
                    case "6":
                        obj[(int)LoaiQuyen.Quyen6] = true;
                        break;
                }
            }
        }
        return obj;
    }

    public static string getToken(string token, bool CongDan)
    {
        string TenDangNhap = "";
        try
        {
            if (ConfigurationManager.AppSettings["TaoTaiKhoanRieng"] == null && token.Substring(0, 1) != "#")
            {
                string sUrl = "https://user.thuathienhue.gov.vn/api/AuthenToken/Validate";
                JavaScriptSerializer js = new JavaScriptSerializer();
                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(sUrl);
                request1.Method = "GET";
                request1.ContentType = "application/json";
                request1.Headers.Add("token", token);
                WebResponse webResponse = request1.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();

                ThongTinTaiKhoan tk = js.Deserialize<ThongTinTaiKhoan>(response);
                if (tk != null && tk.Success)
                    TenDangNhap = tk.Token;
                else
                    TenDangNhap = "";
            }
            else
            {
                FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
                if (CongDan)
                {
                    DataSet ds = db.GetDataSet("TTDN_TAIKHOAN_CONGDAN_SELECT_TK", 4, token);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        TenDangNhap = row["TenDangNhap"].ToString().Trim();
                    }
                }
                else
                {
                    DataSet ds = db.GetDataSet("TTDN_DM_TAIKHOAN_SELECT_KT", 4, token);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        TenDangNhap = row["TenDangNhap"].ToString().Trim();
                    }
                }
            }
        }
        catch
        {
            TenDangNhap = "";
        }
        return TenDangNhap;
    }
    public static string getToken_HueG(string token, bool CongDan)
    {
        string TenDangNhap = "";
        try
        {
            if (ConfigurationManager.AppSettings["TaoTaiKhoanRieng"] == null && token.Substring(0, 1) != "#")
            {
                string sUrl = "https://user.thuathienhue.gov.vn/api/AuthenToken/Validate";
                JavaScriptSerializer js = new JavaScriptSerializer();
                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(sUrl);
                request1.Method = "GET";
                request1.ContentType = "application/json";
                request1.Headers.Add("token", token);
                WebResponse webResponse = request1.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();

                ThongTinTaiKhoan tk = js.Deserialize<ThongTinTaiKhoan>(response);
                FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
                DataSet ds = db.GetDataSet("TICHHOPDIDONG_TAIKHOAN_MAPCC_SELECT", 1, tk.Token);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    tk.OwnerCode = row["MaDinhDanh"].ToString().Trim();
                    tk.Token = row["TaiKhoanCongChuc"].ToString().Trim();
                }

                if (tk != null && tk.Success)
                    TenDangNhap = tk.Token;
                else
                    TenDangNhap = "";
            }
            else
            {
                FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
                if (CongDan)
                {
                    DataSet ds = db.GetDataSet("TTDN_TAIKHOAN_CONGDAN_SELECT_TK", 4, token);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        TenDangNhap = row["TenDangNhap"].ToString().Trim();
                    }
                }
                else
                {
                    DataSet ds = db.GetDataSet("TTDN_DM_TAIKHOAN_SELECT_KT", 4, token);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        TenDangNhap = row["TenDangNhap"].ToString().Trim();
                    }
                }
            }
        }
        catch
        {
            TenDangNhap = "";
        }
        return TenDangNhap;
    }
    public static ThongTinTaiKhoan Token(string token, bool CongDan)
    {
        ThongTinTaiKhoan tk = null;
        try
        {
            if (ConfigurationManager.AppSettings["TaoTaiKhoanRieng"] == null && token.Substring(0, 1) != "#")
            {
                string sUrl = "https://user.thuathienhue.gov.vn/api/AuthenToken/Validate";
                JavaScriptSerializer js = new JavaScriptSerializer();
                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(sUrl);
                request1.Method = "GET";
                request1.ContentType = "application/json";
                //request1.ContentLength = 0;
                request1.Headers.Add("token", token);
                WebResponse webResponse = request1.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                tk = js.Deserialize<ThongTinTaiKhoan>(response);
            }
            else
            {
                FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
                if (CongDan)
                {
                    DataSet ds = db.GetDataSet("TTDN_TAIKHOAN_CONGDAN_SELECT_TK", 4, token);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        tk = new ThongTinTaiKhoan();
                        tk.Success = true;
                        tk.Token = row["TenDangNhap"].ToString().Trim();
                        tk.FullName = row["HoVaTen"].ToString().Trim();
                        tk.Email = row["Email"].ToString().Trim();
                        tk.Address = "";
                        tk.CellPhone = row["DienThoai"].ToString().Trim();
                        tk.ErrCode = 0;
                    }
                    else
                    {
                        tk.Success = false;
                        tk.Message = "Không tồn tại token";
                    }
                }
                else
                {
                    DataSet ds = db.GetDataSet("TTDN_DM_TAIKHOAN_SELECT_KT", 4, token);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        tk = new ThongTinTaiKhoan();
                        tk.Success = true;
                        tk.Token = row["TenDangNhap"].ToString().Trim();
                        tk.FullName = row["TenTaiKhoan"].ToString().Trim();
                        tk.Email = "";
                        tk.Address = "";
                        tk.OwnerCode = row["MaDinhDanh"].ToString().Trim();
                        tk.CellPhone = row["SoDienThoai"].ToString().Trim();
                        tk.ErrCode = 0;
                    }
                    else
                    {
                        tk.Success = false;
                        tk.Message = "Không tồn tại token";
                    }
                }
            }
        }
        catch
        {

        }
        return tk;
    }
    public static ThongTinTaiKhoan Token_HueG(string token, bool CongDan)
    {
        ThongTinTaiKhoan tk = null;
        try
        {
            if (ConfigurationManager.AppSettings["TaoTaiKhoanRieng"] == null && token.Substring(0, 1) != "#")
            {
                string sUrl = "https://user.thuathienhue.gov.vn/api/AuthenToken/Validate";
                JavaScriptSerializer js = new JavaScriptSerializer();
                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(sUrl);
                request1.Method = "GET";
                request1.ContentType = "application/json";
                //request1.ContentLength = 0;
                request1.Headers.Add("token", token);
                WebResponse webResponse = request1.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                tk = js.Deserialize<ThongTinTaiKhoan>(response);
                FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
                DataSet ds = db.GetDataSet("TICHHOPDIDONG_TAIKHOAN_MAPCC_SELECT", 1, tk.Token);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    tk.OwnerCode = row["MaDinhDanh"].ToString().Trim();
                    tk.Token = row["TaiKhoanCongChuc"].ToString().Trim();
                }
            }
            else
            {
                FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
                if (CongDan)
                {
                    DataSet ds = db.GetDataSet("TTDN_TAIKHOAN_CONGDAN_SELECT_TK", 4, token);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        tk = new ThongTinTaiKhoan();
                        tk.Success = true;
                        tk.Token = row["TenDangNhap"].ToString().Trim();
                        tk.FullName = row["HoVaTen"].ToString().Trim();
                        tk.Email = row["Email"].ToString().Trim();
                        tk.Address = "";
                        tk.CellPhone = row["DienThoai"].ToString().Trim();
                        tk.ErrCode = 0;
                    }
                    else
                    {
                        tk.Success = false;
                        tk.Message = "Không tồn tại token";
                    }
                }
                else
                {
                    DataSet ds = db.GetDataSet("TTDN_DM_TAIKHOAN_SELECT_KT", 4, token);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        tk = new ThongTinTaiKhoan();
                        tk.Success = true;
                        tk.Token = row["TenDangNhap"].ToString().Trim();
                        tk.FullName = row["TenTaiKhoan"].ToString().Trim();
                        tk.Email = "";
                        tk.Address = "";
                        tk.OwnerCode = row["MaDinhDanh"].ToString().Trim();
                        tk.CellPhone = row["SoDienThoai"].ToString().Trim();
                        tk.ErrCode = 0;
                    }
                    else
                    {
                        tk.Success = false;
                        tk.Message = "Không tồn tại token";
                    }
                }
            }
        }
        catch
        {

        }
        return tk;
    }
    public static ThongTinTaiKhoan Token_DoanhNghiep(string token)
    {
        ThongTinTaiKhoan tk = null;
        try
        {

            string sUrl = "https://user.thuathienhue.gov.vn/api/AuthenToken/Validate";
            JavaScriptSerializer js = new JavaScriptSerializer();
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(sUrl);
            request1.Method = "GET";
            request1.ContentType = "application/json";
            //request1.ContentLength = 0;
            request1.Headers.Add("token", token);
            WebResponse webResponse = request1.GetResponse();
            Stream webStream = webResponse.GetResponseStream();
            StreamReader responseReader = new StreamReader(webStream);
            string response = responseReader.ReadToEnd();
            tk = js.Deserialize<ThongTinTaiKhoan>(response);
            FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
            DataSet ds = db.GetDataSet("TTDN_DM_COQUANBAOCHI_TAIKHOAN_SELECT", 2, 0, tk.Token);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                tk.OwnerCode = row["CoQuanID"].ToString().Trim();
                tk.Token = row["TaiKhoanID"].ToString().Trim();
            }
        }
        catch
        {

        }
        return tk;
    }
    public static string getTomTat(string sSQL, int soKyTu)
    {
        if (sSQL.IndexOf("</a>") > 0)
            return sSQL;
        sSQL = System.Text.RegularExpressions.Regex.Replace(sSQL, @"<(.|\n)*?>", string.Empty);
        sSQL = sSQL.Trim();
        if (soKyTu == 0)
            return sSQL;
        if (sSQL.Length < soKyTu)
            return sSQL;

        try
        {
            if (sSQL.Length > soKyTu)
            {
                int k = soKyTu;
                while (!sSQL.Substring(k, 1).Trim().Equals(""))
                {
                    k = k + 1;
                }
                sSQL = sSQL.Substring(0, k) + "...";
            }
        }
        catch
        {
            return sSQL;
        }
        return sSQL;
    }
    public static string TenPhuongXa(string MaXa)
    {

        if (MaXa == "")
            return "";
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        DataSet ds = db.GetDataSet("TTDN_DM_PHUONGXA_SELECT", 1, MaXa);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            return row["TenPhuongXa"].ToString();
        }
        return "";
    }
    public static string TenQuyenHuyen(string MaHuyen)
    {
        if (MaHuyen == "")
            return "";
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        DataSet ds = db.GetDataSet("TTDN_DM_QUANHUYEN_SELECT", 1, MaHuyen);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            return row["TenQuanHuyen"].ToString();
        }
        return "";
    }
    public static string TenTinhThanh(string MaTinh)
    {
        if (MaTinh == "")
            return "";
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        DataSet ds = db.GetDataSet("TTDN_DM_TINHTHANH_SELECT", 1, MaTinh);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            return row["TenTinhThanh"].ToString();
        }
        return "";
    }
    public static string HtmlEncode(string Chuoi)
    {
        //return Chuoi;
        return HttpUtility.HtmlEncode(Chuoi);
    }

}
