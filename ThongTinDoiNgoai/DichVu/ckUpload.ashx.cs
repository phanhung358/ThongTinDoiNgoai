using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ThongTinDoiNgoai.DichVu
{
    /// <summary>
    /// Summary description for ckUpload
    /// </summary>
    public class ckUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var upload = context.Request.Form["upload"];
            var file = context.Request.Files[0];

            context.Response.ContentType = "json";
            context.Response.Write(JsonConvert.SerializeObject(upload));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}