using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Limilabs.Client.POP3;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Mail;

/// <summary>
/// Summary description for FITC_MailFunctions
/// </summary>
public class FITC_MailFunctions
{
    private FITC_Pop3Controller pop3Controller;
    private FITC_Imap4Controller imap4Controller;
    private FITC_SmtpController smtpController;
    private FITC_EmailSettings emailSettings;

    public FITC_EmailSettings EmailSettings
    {
        get { return emailSettings; }
        set { emailSettings = value; }
    }

    public FITC_MailFunctions()
    {
        this.pop3Controller = new FITC_Pop3Controller();
        this.imap4Controller = new FITC_Imap4Controller();
        this.smtpController = new FITC_SmtpController();
    }

    public void SetEmailInfo(FITC_EmailSettings.FITC_EmailInfo emailInfo)
    {
        if (this.EmailSettings != null)
            this.EmailSettings.EmailInfo = emailInfo;
        else
        {
            this.EmailSettings = new FITC_EmailSettings();
            this.EmailSettings.EmailInfo = emailInfo;
        }
    }

    public FITC_EmailSettings.FITC_EmailInfo GetDefaultEmailInfo()
    {
        if (this.EmailSettings != null)
            return this.EmailSettings.EmailInfo;
        else return null;
    }

    public void Connect()
    {
        FITC_EmailSettings.FITC_EmailInfo emailInfo = this.GetDefaultEmailInfo();
        if (emailInfo != null)
        {
            if (emailInfo.EmailType == EmailType.POP3)
            {
                if (pop3Controller.Pop3Client == null || pop3Controller.Pop3Client.Connected == false)
                    pop3Controller.Connect(emailInfo);
                else
                {
                    pop3Controller.Disconnect();
                    pop3Controller.Connect(emailInfo);
                }
            }
            else
            {
                if (imap4Controller.Imap4Client == null || imap4Controller.Imap4Client.Connected == false)
                    imap4Controller.Connect(emailInfo);
                else
                {
                    imap4Controller.Disconnect();
                    imap4Controller.Connect(emailInfo);
                }
            }
        }
    }

    public void Disconnect()
    {
        FITC_EmailSettings.FITC_EmailInfo emailInfo = this.GetDefaultEmailInfo();
        if (emailInfo != null)
        {
            if (emailInfo.EmailType == EmailType.POP3 && (pop3Controller.Pop3Client != null && pop3Controller.Pop3Client.Connected))
                pop3Controller.Disconnect();
            else if (imap4Controller.Imap4Client != null && imap4Controller.Imap4Client.Connected)
                imap4Controller.Disconnect();
        }
    }

    public void Reconnect()
    {
        Disconnect();
        Connect();
    }

    public void ConnectSMTP()
    {
        FITC_EmailSettings.FITC_EmailInfo emailInfo = this.GetDefaultEmailInfo();
        if (emailInfo != null)
        {
            if (smtpController.SMTPClient == null || smtpController.SMTPClient.Connected == false)
                smtpController.Connect(emailInfo);
            else
            {
                smtpController.Disconnect();
                smtpController.Connect(emailInfo);
            }
        }
    }

    public void DisconnectSMTP()
    {
        FITC_EmailSettings.FITC_EmailInfo emailInfo = this.GetDefaultEmailInfo();
        if (emailInfo != null)
        {
            if (smtpController.SMTPClient != null && smtpController.SMTPClient.Connected)
                smtpController.Disconnect();
        }
    }

    public void ReconnectSMTP()
    {
        DisconnectSMTP();
        ConnectSMTP();
    }

    public string CheckEmailSettings()
    {
        try
        {
            Reconnect();
            Disconnect();
            ReconnectSMTP();
            DisconnectSMTP();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "";
    }

    public string SaveEmailSettings(string nhanVienID, string sTenThuTuc)
    {
        return new FITC_EmailSettings().Save(nhanVienID, this.EmailSettings, sTenThuTuc);
    }

    public string UpdateEmailSettings(string nhanVienID, string cauHinhID, string sTenThuTuc)
    {
        return new FITC_EmailSettings().Update(nhanVienID, cauHinhID, this.EmailSettings, sTenThuTuc);
    }

    public string DeleteEmailSettings(string cauHinhID, string sTenThuTuc)
    {
        return new FITC_EmailSettings().Delete(cauHinhID, sTenThuTuc);
    }

    public FITC_EmailSettings LoadEmailSettings(string cauHinhID, string sTenThuTuc)
    {
        return new FITC_EmailSettings().Load(cauHinhID, sTenThuTuc, "");
    }
    public FITC_EmailSettings LoadEmailSettings(string cauHinhID, string sTenThuTuc, string sTenHienThi)
    {
        return new FITC_EmailSettings().Load(cauHinhID, sTenThuTuc,sTenHienThi);
    }

    public List<string> GetAllPop3()
    {
        return this.pop3Controller.GetAll();
    }

    public List<long> GetAllImap4()
    {
        return this.imap4Controller.GetAll();
    }

    public IMail GetEmail(long index)
    {
        FITC_EmailSettings.FITC_EmailInfo emailInfo = this.GetDefaultEmailInfo();
        if (emailInfo.EmailType == EmailType.POP3)
            return this.pop3Controller.GetEmail(index);
        else
            return this.imap4Controller.GetEmail(index);
    }

    public IMail GetEmailPop3ByUID(string uID)
    {
        return this.pop3Controller.GetEmailByUID(uID);
    }

    public IMail GetEmailImap4ByUID(long uID)
    {
        return this.imap4Controller.GetEmailByUID(uID);
    }

    public long GetEmailCount()
    {
        FITC_EmailSettings.FITC_EmailInfo emailInfo = this.GetDefaultEmailInfo();
        if (emailInfo.EmailType == EmailType.POP3)
            return this.pop3Controller.GetEmailCount();
        else
            return this.imap4Controller.GetEmailCount();
    }

    public string SendMail(string cauHinhID, IMail email, bool bLuuMailGui)
    {
        return this.smtpController.SendMail(cauHinhID, email, bLuuMailGui);
    }

    public string StoreMessage(string cauHinhID, IMail email, int flag)
    {
        return this.smtpController.StoreMessage(cauHinhID, email, flag);
    }

    public string DeleteMailFromServer(long index)
    {
        FITC_EmailSettings.FITC_EmailInfo emailInfo = this.GetDefaultEmailInfo();
        if (emailInfo.EmailType == EmailType.POP3)
            return this.pop3Controller.DeleteEmailFromServer(index);
        else
            return this.imap4Controller.DeleteEmailFromServer(index);
    }

    public string DeleteMailFromServerByUID(object uID)
    {
        FITC_EmailSettings.FITC_EmailInfo emailInfo = this.GetDefaultEmailInfo();
        if (emailInfo.EmailType == EmailType.POP3)
            return this.pop3Controller.DeleteEmailFromServerByUID((string)uID);
        else
            return this.imap4Controller.DeleteEmailFromServerByUID((long)uID);
    }

    public string SubjectDecode(string strInput)
    {
        string strOutput = "";
        string[] arrayInput = strInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string str in arrayInput)
        {
            if (Regex.IsMatch(str, @"=?.*?.*?.*?=", RegexOptions.IgnoreCase))
            {
                Attachment attachment = Attachment.CreateAttachmentFromString("", string.Format("=?{0}?{1}?{2}?=", str.Split('?')[1], str.Split('?')[2], str.Split('?')[3]));
                strOutput += attachment.Name + " ";
            }
            else
                strOutput += str + " ";
        }
        return strOutput.Trim();
    }
}
