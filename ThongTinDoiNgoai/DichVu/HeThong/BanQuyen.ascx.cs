using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using FITC.Web.Component;
using System.Data;

namespace ThongTinDoiNgoai.DichVu.HeThong
{
    public partial class BanQuyen : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder();
            DataSet ds = db.GetDataSet("TTDN_DM_DONVI_SELECT", 1, TUONGTAC.DonViID);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                //if (Convert.ToBoolean(row["bBanQuyen"].ToString()))
                //    str.AppendFormat("<div class='BanQuyen_SuDung'><strong>Bản quyền sử dụng:</strong>&nbsp;{0}</div>", "");
                //else
                //    str.AppendFormat("<div class='BanQuyen_SuDung'><strong>Phiên bản thử nghiệm:</strong>&nbsp;{0}</div>", "");
                //str.AppendFormat("<div class='BanQuyen_DonVi'><strong>Đơn vị:</strong>&nbsp;{0}</div>",TUONGTAC.getTenDonVi());
                //str.AppendFormat("<div class='BanQuyen_NgayHoatDong'><strong>Ngày hoạt động:</strong>&nbsp;{0}</div>", Convert.ToDateTime(row["NgayHoatDong"].ToString()).ToString("dd/MM/yyyy"));
            }

            divBanQuyen.InnerHtml = str.ToString();
        }
    }
}