using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using System.Text;
namespace ThongTinDoiNgoai
{
    public class CommonFunctions : IDisposable
    {
        public void ResponseFile(MemoryStream stream, string strFileName)
        {
            try
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.AddHeader("Accept-Ranges", "bytes");
                    HttpContext.Current.Response.Buffer = false;
                    long fileLength = stream.Length;
                    long startBytes = 0;
                    long speed = 100 * 1024; //100K bytes
                    int pack = 10 * 1024; //10K bytes
                    int sleep = (int)Math.Floor((double)(1000 * pack / speed)) + 1;
                    if (HttpContext.Current.Request.Headers["Range"] != null)
                    {
                        HttpContext.Current.Response.StatusCode = 206;
                        string[] range = HttpContext.Current.Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    HttpContext.Current.Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                        HttpContext.Current.Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));

                    HttpContext.Current.Response.AddHeader("Connection", "Keep-Alive");
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + strFileName + "\"");
                    HttpContext.Current.Response.ContentType = "application/octet-stream";

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (HttpContext.Current.Response.IsClientConnected)
                        {
                            HttpContext.Current.Response.BinaryWrite(br.ReadBytes(pack));
                            System.Threading.Thread.Sleep(sleep);
                        }
                        else
                            i = maxCount;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
                HttpContext.Current.Response.End();
            }
        }
        public bool ExtensionIsSupported(string strExtInput)
        {
            string[] arrExtSupported = { ".txt", ".rtf", ".pdf", ".doc", ".odt", ".zip", ".gz", ".rar" };
            foreach (string strExtSupported in arrExtSupported)
                if (strExtInput.Trim().ToLower() == strExtSupported)
                    return true;

            return false;
        }
        public string ExtensionToMIMEType(string strExt)
        {
            string strMIMEType = "text/plain";
            switch (strExt.Trim().ToLower())
            {
                case ".txt":
                    strMIMEType = "text/plain";
                    break;
                case ".rtf":
                    strMIMEType = "application/rtf";
                    break;
                case ".pdf":
                    strMIMEType = "application/pdf";
                    break;
                case ".doc":
                    strMIMEType = "application/msword";
                    break;
                case ".odt":
                    strMIMEType = "application/vnd.oasis.opendocument.text";
                    break;
                case ".zip":
                    strMIMEType = "application/zip";
                    break;
                case ".gz":
                    strMIMEType = "application/x-gzip";
                    break;
                case ".rar":
                    strMIMEType = "application/x-rar-compressed";
                    break;
            }
            return strMIMEType;
        }
        public string MIMETypeToExtension(string strMIMEType)
        {
            string strExt = ".txt";
            switch (strMIMEType.Trim().ToLower())
            {
                case "text/plain":
                    strExt = ".txt";
                    break;
                case "application/rtf":
                    strExt = ".rtf";
                    break;
                case "application/pdf":
                    strExt = ".pdf";
                    break;
                case "application/msword":
                    strExt = ".doc";
                    break;
                case "application/vnd.oasis.opendocument.text":
                case "application/x-vnd.oasis.opendocument.text":
                    strExt = ".odt";
                    break;
                case "application/zip":
                    strExt = ".zip";
                    break;
                case "application/x-gzip":
                    strExt = ".gz";
                    break;
                case "application/x-rar-compressed":
                    strExt = ".rar";
                    break;
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    strExt = ".docx";
                    break;
            }
            return strExt;
        }
        public string EncodeBase64File(string srcFile)
        {
            string strData = "";
            using (FileStream sr = new FileStream(srcFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] srcBT = new byte[sr.Length];
                sr.Read(srcBT, 0, srcBT.Length);
                strData = Convert.ToBase64String(srcBT, Base64FormattingOptions.InsertLineBreaks);
            }
            return strData;
        }
        public MemoryStream DecodeBase64File(string srcSource)
        {
            MemoryStream ms = new MemoryStream();
            byte[] bt64 = Convert.FromBase64String(srcSource);
            ms.Write(bt64, 0, bt64.Length);
            return ms;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //dispose of managed resources
            }
            //dispose of unmanaged resources
        }
        ~CommonFunctions()
        {
            Dispose(false);
        }
    }
    public class DynamicWebService
    {
        #region Attributes
        private string _strURL;
        private string _strMethodName;
        private string _strPostValues;
        #endregion

        #region Properties
        public string URL
        {
            get { return _strURL; }
            set { _strURL = value; }
        }
        public string MethodName
        {
            get { return _strMethodName; }
            set { _strMethodName = value; }
        }
        public string PostValues
        {
            get { return _strPostValues; }
            set { _strPostValues = value; }
        }
        public string ResultString;
        public XmlDocument ResultXML;
        #endregion

        public DynamicWebService() { }
        public DynamicWebService(string strUrl, string strMethodName, string strPostValues)
        {
            URL = strUrl;
            MethodName = strMethodName;
            PostValues = strPostValues;
        }

        public void Invoke()
        {
            try
            {

                string SOAP =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
               xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <{0} xmlns=""http://tempuri.org/"">
                  {1}
                </{0}>
              </soap:Body>
            </soap:Envelope>";

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
                req.Headers.Add("SOAPAction", "\"http://tempuri.org/" + MethodName + "\"");
                req.ContentType = "text/xml;charset=\"utf-8\"";
                req.Accept = "text/xml";
                req.Method = "POST";
                req.KeepAlive = false;

                using (Stream stm = req.GetRequestStream())
                {
                    SOAP = string.Format(SOAP, MethodName, PostValues);
                    using (StreamWriter stmw = new StreamWriter(stm))
                    {
                        stmw.Write(SOAP);
                    }
                }

                using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    string strResult = responseReader.ReadToEnd();
                    ResultXML = new XmlDocument();
                    ResultXML.LoadXml(strResult);
                    ResultString = Regex.Replace(strResult, "<.*?>", string.Empty);
                }
            }
            catch(Exception ex) {
                string s = ex.Message;
            }
        }
    }

    public class CheckVersion
    {
        #region Attributes
        private string _strVersion;
        private string _strTuKhoa;
        #endregion

        #region Properties
        public string Version
        {
            get { return _strVersion; }
            set { _strVersion = value; }
        }
        public string TuKhoa
        {
            get { return _strTuKhoa; }
            set { _strTuKhoa = value; }
        }
        #endregion

        public CheckVersion() { }

        public string GetVersion()
        {
            string strXML = string.Format(
    @"<sVersion>{0}</sVersion>
<sTuKhoa>{1}</sTuKhoa>",
          this.Version,
          this.TuKhoa);

            return strXML;
        }
    }

    public class MessageObject : IDisposable
    {
        #region Attributes
        private string _strContentID;
        private string _strContentType;
        private string _strContentBody;
        #endregion

        #region Properties
        public string strContentID
        {
            set { _strContentID = value; }
            get { return _strContentID; }
        }
        public string strContentType
        {
            set { _strContentType = value; }
            get { return _strContentType; }
        }
        public string strContentBody
        {
            set { _strContentBody = value; }
            get { return _strContentBody; }
        }
        #endregion

        public MessageObject() { }
        public List<MessageObject> ExtractMessageObject(string strBoundary, string strSOAPContent)
        {
            List<MessageObject> lstMsgObj = null;
            try
            {
                string[] arrSOAPContent = strSOAPContent.Trim().Split(new string[] { strBoundary }, StringSplitOptions.RemoveEmptyEntries);
                if (arrSOAPContent.Length > 0)
                    if (arrSOAPContent[arrSOAPContent.Length - 1] == "--")
                        arrSOAPContent[arrSOAPContent.Length - 1] = null;

                for (int i = 0; i < arrSOAPContent.Length; i++)
                {
                    if (arrSOAPContent[i] == null)
                        continue;
                    MessageObject msgObj = GetMessageObject(arrSOAPContent[i]);
                    if (msgObj != null)
                    {
                        if (lstMsgObj == null)
                            lstMsgObj = new List<MessageObject>();

                        lstMsgObj.Add(msgObj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstMsgObj;
        }
        private MessageObject GetMessageObject(string strContent)
        {
            MessageObject msgObj = null;
            try
            {
                strContent = strContent.Trim();
                string strContentID = "";
                string strContentType = "";
                //byte[] byteArray = Encoding.UTF8.GetBytes(strContent);
                //Stream stream = new MemoryStream(byteArray);
                //TextReader textReader = new StreamReader(stream);
                //strContentID = textReader.ReadLine().Trim();
                //strContentID = strContentID.Split(':')[1].Trim();

                //strContentType = textReader.ReadLine().Trim();
                //strContentType = strContentType.Split(':')[1].Trim();

                //strContentBody = textReader.ReadToEnd().Trim();

                msgObj = new MessageObject();
                msgObj.strContentID = strContentID;
                msgObj.strContentType = strContentType;
                msgObj.strContentBody = strContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return msgObj;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //dispose of managed resources
            }
            //dispose of unmanaged resources
        }
        ~MessageObject()
        {
            Dispose(false);
        }
    }
}
