using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using FITC.Web.Component;

/// <summary>
/// Summary description for FITC_EmailSettings
/// </summary>
/// 

public enum EmailType
{
    POP3, IMAP4
}

[Serializable]
public class FITC_EmailSettings
{
    [Serializable]
    public class FITC_EmailInfo
    {
        private string emailAddress;
        private string password;
        private string displayName;
        private string incomingServer;
        private string outgoingServer;
        private int incomingServerPort;
        private int outgoingServerPort;
        private bool isDefaultAccount;
        private bool isDeleteFromServer;
        private bool isUsingIncomingServerPort;
        private bool isUsingOutgoingServerPort;
        private bool isIncomingSecureConnection;
        private bool isOutgoingSecureConnection;
        private bool isOutgoingWithAuthentication;
        private EmailType emailType;

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        public string IncomingServer
        {
            get { return incomingServer; }
            set { incomingServer = value; }
        }
        public string OutgoingServer
        {
            get { return outgoingServer; }
            set { outgoingServer = value; }
        }
        public int IncomingServerPort
        {
            get { return incomingServerPort; }
            set { incomingServerPort = value; }
        }
        public int OutgoingServerPort
        {
            get { return outgoingServerPort; }
            set { outgoingServerPort = value; }
        }
        public bool IsDefaultAccount
        {
            get { return isDefaultAccount; }
            set { isDefaultAccount = value; }
        }
        public bool IsDeleteFromServer
        {
            get { return isDeleteFromServer; }
            set { isDeleteFromServer = value; }
        }
        public bool IsUsingIncomingServerPort
        {
            get { return isUsingIncomingServerPort; }
            set { isUsingIncomingServerPort = value; }
        }
        public bool IsUsingOutgoingServerPort
        {
            get { return isUsingOutgoingServerPort; }
            set { isUsingOutgoingServerPort = value; }
        }
        public bool IsIncomingSecureConnection
        {
            get { return isIncomingSecureConnection; }
            set { isIncomingSecureConnection = value; }
        }
        public bool IsOutgoingSecureConnection
        {
            get { return isOutgoingSecureConnection; }
            set { isOutgoingSecureConnection = value; }
        }
        public bool IsOutgoingWithAuthentication
        {
            get { return isOutgoingWithAuthentication; }
            set { isOutgoingWithAuthentication = value; }
        }
        public EmailType EmailType
        {
            get { return emailType; }
            set { emailType = value; }
        }
    }

    private FITC_EmailInfo emailInfo;
    public FITC_EmailInfo EmailInfo
    {
        get { return emailInfo; }
        set { emailInfo = value; }
    }

    FITC_CDataBase db = new FITC_CDataBase(Static.sConnectString);

    public FITC_EmailSettings()
    {
        this.EmailInfo = new FITC_EmailInfo();
    }

    public string Save(string nhanVienID, FITC_EmailSettings emailSettings, string sTenThuTuc)
    {
        string sLoi = "";
        object[] obj = new object[16];
        obj[0] = nhanVienID;
        obj[1] = emailSettings.EmailInfo.EmailAddress;
        obj[2] = emailSettings.EmailInfo.Password;
        obj[3] = emailSettings.EmailInfo.DisplayName;
        obj[4] = emailSettings.EmailInfo.EmailType.ToString();
        obj[5] = emailSettings.EmailInfo.IncomingServer;
        obj[6] = emailSettings.EmailInfo.IsUsingIncomingServerPort;
        obj[7] = emailSettings.EmailInfo.IncomingServerPort;
        obj[8] = emailSettings.EmailInfo.IsIncomingSecureConnection;
        obj[9] = emailSettings.EmailInfo.OutgoingServer;
        obj[10] = emailSettings.EmailInfo.IsUsingOutgoingServerPort;
        obj[11] = emailSettings.EmailInfo.OutgoingServerPort;
        obj[12] = emailSettings.EmailInfo.IsOutgoingSecureConnection;
        obj[13] = emailSettings.EmailInfo.IsOutgoingWithAuthentication;
        obj[14] = emailSettings.EmailInfo.IsDefaultAccount;
        obj[15] = emailSettings.EmailInfo.IsDeleteFromServer;

        sLoi = db.ExcuteSP(sTenThuTuc, obj);

        return sLoi;
    }

    public string Update(string nhanVienID, string cauHinhID, FITC_EmailSettings emailSettings, string sTenThuTuc)
    {
        string sLoi = "";
        object[] obj = new object[17];
        obj[0] = nhanVienID;
        obj[1] = cauHinhID;
        obj[2] = emailSettings.EmailInfo.EmailAddress;
        obj[3] = emailSettings.EmailInfo.Password;
        obj[4] = emailSettings.EmailInfo.DisplayName;
        obj[5] = emailSettings.EmailInfo.EmailType.ToString();
        obj[6] = emailSettings.EmailInfo.IncomingServer;
        obj[7] = emailSettings.EmailInfo.IsUsingIncomingServerPort;
        obj[8] = emailSettings.EmailInfo.IncomingServerPort;
        obj[9] = emailSettings.EmailInfo.IsIncomingSecureConnection;
        obj[10] = emailSettings.EmailInfo.OutgoingServer;
        obj[11] = emailSettings.EmailInfo.IsUsingOutgoingServerPort;
        obj[12] = emailSettings.EmailInfo.OutgoingServerPort;
        obj[13] = emailSettings.EmailInfo.IsOutgoingSecureConnection;
        obj[14] = emailSettings.EmailInfo.IsOutgoingWithAuthentication;
        obj[15] = emailSettings.EmailInfo.IsDefaultAccount;
        obj[16] = emailSettings.EmailInfo.IsDeleteFromServer;

        sLoi = db.ExcuteSP(sTenThuTuc, obj);

        return sLoi;
    }

    public string Delete(string cauHinhID, string sTenThuTuc)
    {
        string sLoi = "";
        //Xoa noi dung email
        sLoi = db.ExcuteSP(sTenThuTuc, 0, cauHinhID);
        //Xoa cau hinh
        sLoi = db.ExcuteSP(sTenThuTuc, cauHinhID);
        return sLoi;
    }

    public FITC_EmailSettings Load(string cauHinhID, string sTenThuTuc)
    {
        FITC_EmailSettings emailSettings = new FITC_EmailSettings();
        DataSet ds = db.GetDataSet(sTenThuTuc, 1, cauHinhID);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            emailSettings.EmailInfo.EmailAddress = row["DiaChiEmail"].ToString().Trim();
            emailSettings.EmailInfo.Password = row["MatKhau"].ToString().Trim();
            emailSettings.EmailInfo.DisplayName = row["TenHienThi"].ToString().Trim();
            if (row["GiaoThucNhan"].ToString().Trim() == "POP3")
                emailSettings.EmailInfo.EmailType = EmailType.POP3;
            else
                emailSettings.EmailInfo.EmailType = EmailType.IMAP4;
            emailSettings.EmailInfo.IncomingServer = row["MayChuNhan"].ToString().Trim();
            emailSettings.EmailInfo.IsUsingIncomingServerPort = (bool)row["SuDungCongNhan"];
            emailSettings.EmailInfo.IncomingServerPort = (int)row["CongNhan"];
            emailSettings.EmailInfo.IsIncomingSecureConnection = (bool)row["bBaoMatNhan"];
            emailSettings.EmailInfo.OutgoingServer = row["MayChuGoi"].ToString().Trim();
            emailSettings.EmailInfo.IsUsingOutgoingServerPort = (bool)row["SuDungCongGoi"];
            emailSettings.EmailInfo.OutgoingServerPort = (int)row["CongGoi"];
            emailSettings.EmailInfo.IsOutgoingSecureConnection = (bool)row["bBaoMatGoi"];
            emailSettings.EmailInfo.IsOutgoingWithAuthentication = (bool)row["bXacThucGoi"];
            emailSettings.EmailInfo.IsDefaultAccount = (bool)row["bMacDinh"];
            emailSettings.EmailInfo.IsDeleteFromServer = (bool)row["bXoaGoc"];

            ds.Clear();
            ds.Dispose();

            return emailSettings;
        }
        else
            return null;
    }

    public FITC_EmailSettings Load(string cauHinhID, string sTenThuTuc, string sTenHienThi)
    {
        FITC_EmailSettings emailSettings = new FITC_EmailSettings();
        DataSet ds = db.GetDataSet(sTenThuTuc, 1, cauHinhID);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            emailSettings.EmailInfo.EmailAddress = row["DiaChiEmail"].ToString().Trim();
            emailSettings.EmailInfo.Password = row["MatKhau"].ToString().Trim();
            if (sTenHienThi == "")
                emailSettings.EmailInfo.DisplayName = row["TenHienThi"].ToString().Trim();
            else
                emailSettings.EmailInfo.DisplayName = sTenHienThi;
            if (row["GiaoThucNhan"].ToString().Trim() == "POP3")
                emailSettings.EmailInfo.EmailType = EmailType.POP3;
            else
                emailSettings.EmailInfo.EmailType = EmailType.IMAP4;
            emailSettings.EmailInfo.IncomingServer = row["MayChuNhan"].ToString().Trim();
            emailSettings.EmailInfo.IsUsingIncomingServerPort = (bool)row["SuDungCongNhan"];
            emailSettings.EmailInfo.IncomingServerPort = (int)row["CongNhan"];
            emailSettings.EmailInfo.IsIncomingSecureConnection = (bool)row["bBaoMatNhan"];
            emailSettings.EmailInfo.OutgoingServer = row["MayChuGoi"].ToString().Trim();
            emailSettings.EmailInfo.IsUsingOutgoingServerPort = (bool)row["SuDungCongGoi"];
            emailSettings.EmailInfo.OutgoingServerPort = (int)row["CongGoi"];
            emailSettings.EmailInfo.IsOutgoingSecureConnection = (bool)row["bBaoMatGoi"];
            emailSettings.EmailInfo.IsOutgoingWithAuthentication = (bool)row["bXacThucGoi"];
            emailSettings.EmailInfo.IsDefaultAccount = (bool)row["bMacDinh"];
            emailSettings.EmailInfo.IsDeleteFromServer = (bool)row["bXoaGoc"];

            ds.Clear();
            ds.Dispose();

            return emailSettings;
        }
        else
            return null;
    }

}
