using FITC.Web.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ThongTinDoiNgoai
{
    public partial class _Default : Page
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            divMain.Controls.Add(LoadControl(Static.AppPath() + "/dichvu/thongtindoingoai/trangchu.ascx"));
        }
    }
}