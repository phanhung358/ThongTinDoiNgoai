using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Limilabs.Client.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.MIME;
using FITC.Web.Component;

/// <summary>
/// Summary description for FITC_SmtpController
/// </summary>
public class FITC_SmtpController
{
    private Smtp smtpClient;
    public Smtp SMTPClient
    {
        get { return smtpClient; }
        set { smtpClient = value; }
    }

    FITC_CDataBase db = new FITC_CDataBase(Static.sConnectString);

    public FITC_SmtpController()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public void Connect(FITC_EmailSettings.FITC_EmailInfo emailInfo)
    {
        if (this.SMTPClient == null || this.SMTPClient.Connected == false)
        {
            if (emailInfo != null)
            {
                this.SMTPClient = new Smtp();

                string server = emailInfo.OutgoingServer;
                string user = emailInfo.EmailAddress;
                string password = new EncryptDescript().CriptDescript(emailInfo.Password);
                int port = emailInfo.OutgoingServerPort;
                bool ssl = emailInfo.IsOutgoingSecureConnection;
                bool usePort = emailInfo.IsUsingOutgoingServerPort;

                SMTPClient.Connect(server);

                if (ssl)
                    SMTPClient.StartTLS();

                SMTPClient.UseBestLogin(user, password);
            }
        }
    }

    public void Disconnect()
    {
        if (this.SMTPClient != null && this.SMTPClient.Connected)
            this.SMTPClient.Close();
    }

    public string SendMail(string cauHinhID, IMail email,bool bLuuMailGui)
    {

        try
        {
            string sLoi = "";
            SMTPClient.SendMessage(email);
            if (bLuuMailGui)
            sLoi = StoreMessage(cauHinhID, email, 2);
            if (sLoi != "")
                return sLoi.Replace("'", "\\'");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "";
    }
    public string StoreMessage(string cauHinhID, IMail email, int flag)
    {
        try
        {
            if (email != null)
            {
                //Xử lý subject
                string subject = email.Document.Root.Headers["subject"];
                if (subject == null || subject.Length == 0)
                    subject = "(Không tiêu đề)";

                //Xử lý nội dung text
                string text = email.Text;
                if (string.IsNullOrEmpty(text) && email.IsHtml)
                    text = email.GetTextFromHtml();

                //Xử lý ngày gởi
                DateTime sendDate = DateTime.Now;

                //Xử lý tập tin đính kèm                            
                string attachments = "";
                if (email.Attachments.Count > 0)
                {
                    foreach (MimeData mime in email.Attachments)
                    {
                        attachments += mime.SafeFileName + "|";
                    }
                    attachments = attachments.Remove(attachments.Length - 1);
                }

                object[] obj = new object[11];
                obj[0] = cauHinhID;
                obj[1] = email.MessageID;
                obj[2] = email.To.ToString().Trim();
                obj[3] = email.Cc.ToString().Trim();
                obj[4] = email.Bcc.ToString().Trim();
                obj[5] = subject;
                obj[6] = HttpUtility.HtmlEncode(text);
                obj[7] = email.Html;
                obj[8] = attachments;
                obj[9] = email.Date.GetValueOrDefault(sendDate);
                obj[10] = flag; //EmailFlag
                string sLoi = db.ExcuteSP("FITC_EMAIL_GOIEMAIL_INSERT", obj);
                if (sLoi != "")
                    return "Lỗi: Trong quá trình xử lý email !";
            }
            else
                return "Lỗi: Không tìm thấy email !";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "";
    }
}
