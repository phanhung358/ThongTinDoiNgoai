using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using System.Collections.Generic;

/// <summary>
/// Summary description for FITC_ImapController
/// </summary>
public class FITC_Imap4Controller
{
    private Imap imap4Client;
    private List<IMail> listEmail;

    public Imap Imap4Client
    {
        get { return imap4Client; }
        set { imap4Client = value; }
    }
    public List<IMail> ListEmail
    {
        get { return listEmail; }
        set { listEmail = value; }
    }

    public FITC_Imap4Controller()
    {
        this.ListEmail = new List<IMail>();
    }

    public void Connect(FITC_EmailSettings.FITC_EmailInfo emailInfo)
    {
        if (this.Imap4Client == null || this.Imap4Client.Connected == false)
        {
            if (emailInfo != null && emailInfo.EmailType == EmailType.IMAP4)
            {
                this.Imap4Client = new Imap();

                string server = emailInfo.IncomingServer;
                string user = emailInfo.EmailAddress;
                string password = new EncryptDescript().CriptDescript(emailInfo.Password);
                int port = emailInfo.IncomingServerPort;
                bool ssl = emailInfo.IsIncomingSecureConnection;
                bool usePort = emailInfo.IsUsingIncomingServerPort;

                if (ssl)
                {
                    if (usePort)
                        Imap4Client.ConnectSSL(server, port);
                    else
                        Imap4Client.ConnectSSL(server);
                }
                else
                {
                    if (usePort)
                        Imap4Client.Connect(server, port);
                    else
                        Imap4Client.Connect(server);
                }
                Imap4Client.UseBestLogin(user, password);
            }
        }
    }

    public void Disconnect()
    {
        if (this.Imap4Client != null && this.Imap4Client.Connected)
            this.Imap4Client.Close();
    }

    public List<long> GetAll()
    {
        Imap4Client.SelectInbox();
        return Imap4Client.SearchFlag(Flag.All);
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
        return this.Imap4Client.SelectInbox().MessageCount;
    }

    public IMail GetEmail(long index)
    {
        IMail email = null;
        email = new MailBuilder().CreateFromEml(Imap4Client.GetMessage(index));
        return email;
    }

    public IMail GetEmailByUID(long uID)
    {
        IMail email = null;
        email = new MailBuilder().CreateFromEml(Imap4Client.GetMessageByUID(uID));
        return email;
    }

    public string DeleteEmailFromServer(long index)
    {
        try
        {
            Imap4Client.DeleteMessage(index);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "";
    }

    public string DeleteEmailFromServerByUID(long uID)
    {
        try
        {
            Imap4Client.DeleteMessageByUID(uID);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "";
    }
}
