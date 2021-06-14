using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FITC.Web.Component;

namespace ThongTinDoiNgoai.DichVu.HeThong
{
    public partial class SoDienThoai : System.Web.UI.UserControl
    {
        CacHamChung ham = new CacHamChung();
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataSet ds = db.GetDataSet("TTDN_DM_SDT_SELECT", 1, "baochi");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    txtSoDienThoai.Text = row["SoDienThoai"].ToString();
                }
            }
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            db.ExcuteSP("TTDN_DM_SDT_UPDATE", txtSoDienThoai.Text,"baochi");
        }
    }
}