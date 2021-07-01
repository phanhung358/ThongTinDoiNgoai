using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Configuration;
using FITC.Web.Component;
using System.Threading;
using System.Data;
using HtmlAgilityPack;
using System.Text;
using System.Globalization;

namespace ThongTinDoiNgoai
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            Static.sConnectString = ConfigurationManager.AppSettings["ConnectDb"];
            // Code that runs on application startup
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //DichVuLayTin();

        }

        private void DichVuLayTin()
        {
            FITC_CDataBase db = new FITC_CDataBase(Static.sConnectString);
            
            while (true)
            {
                try
                {
                    LayTinTuDong tin = new LayTinTuDong();
                    Thread t = new Thread(() => tin.ThucHien("0"));
                    t.Start();

                    Thread.Sleep(1000 * 60 * 5);//5 phut
                    t.Abort();
                }
                catch (Exception)
                {
                    Thread.Sleep(1000 * 60 * 5);//5 phut
                    continue;
                }
            }
        }
    }
}