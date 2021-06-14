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
using Limilabs.Mail;
using System.Collections.Generic;

/// <summary>
/// Summary description for FITC_Pop3Controller
/// </summary>
public class FITC_Pop3Controller
{
    private Pop3 pop3Client;
    private List<IMail> listEmail;

    public Pop3 Pop3Client
    {
        get { return pop3Client; }
        set { pop3Client = value; }
    }
    public List<IMail> ListEmail
    {
        get { return listEmail; }
        set { listEmail = value; }
    }

    public FITC_Pop3Controller()
    {
        this.ListEmail = new List<IMail>();
    }

    public void Connect(FITC_EmailSettings.FITC_EmailInfo emailInfo)
    {
        if (this.Pop3Client == null || this.Pop3Client.Connected == false)
        {
            if (emailInfo != null && emailInfo.EmailType == EmailType.POP3)
            {
                this.Pop3Client = new Pop3();

                string server = emailInfo.IncomingServer;
                string user = emailInfo.EmailAddress;
                string password = new EncryptDescript().CriptDescript(emailInfo.Password);
                int port = emailInfo.IncomingServerPort;
                bool ssl = emailInfo.IsIncomingSecureConnection;
                bool usePort = emailInfo.IsUsingIncomingServerPort;

                if (ssl)
                {
                    if (usePort)
                        Pop3Client.ConnectSSL(server, port);
                    else
                        Pop3Client.ConnectSSL(server);
                }
                else
                {
                    if (usePort)
                        Pop3Client.Connect(server, port);
                    else
                        Pop3Client.Connect(server);
                }
                Pop3Client.UseBestLogin(user, password);
            }
        }
    }

    public void Disconnect()
    {
        if (this.Pop3Client != null && this.Pop3Client.Connected)
            this.Pop3Client.Close();
    }

    public List<string> GetAll()
    {
        return Pop3Client.GetAll();
    }

    public void AddList(IMail email)
    {
        this.ListEmail.Add(email);
    }

    public void ClearList()
    {
        this.ListEmail.Clear();
    }

    public long GetEmailCount()
    {
        return this.Pop3Client.GetAccountStat().MessageCount;
    }

    public IMail GetEmail(long index)
    {
        IMail email = null;
        email = new MailBuilder().CreateFromEml(Pop3Client.GetMessage(index));
        return email;
    }

    public IMail GetEmailByUID(string uID)
    {
        IMail email = null;
        email = new MailBuilder().CreateFromEml(Pop3Client.GetMessageByUID(uID));
        return email;
    }

    public string DeleteEmailFromServer(long index)
    {
        try
        {
            Pop3Client.DeleteMessage(index);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "";
    }

    public string DeleteEmailFromServerByUID(string uID)
    {
        try
        {
            Pop3Client.DeleteMessageByUID(uID);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "";
    }
}
