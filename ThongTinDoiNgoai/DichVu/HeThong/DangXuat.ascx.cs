using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
namespace ThongTinDoiNgoai.DichVu.HeThong
{
    public partial class DangXuat : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl.Text = string.Format("<script>messaging.deleteToken('{0}');</script>", Session["token"]);

            Session.Clear();
            Response.Redirect("default.aspx");

            //if (Request.Cookies["myCookie"] != null)
            //{           
            //    HttpCookie myCookie = new HttpCookie("myCookie");
            //    Response.Cookies.Add(myCookie);
            //}
        }
    }
}