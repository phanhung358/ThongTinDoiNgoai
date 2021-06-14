using FITC.Web.Component;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyVanBan.DichVu.ThongTinDoiNgoai
{
    public partial class BaiViet_Ct : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        string sBaiVietID = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["BaiVietID"] != null)
            {
                sBaiVietID = Request.QueryString["BaiVietID"];
            }
            if (!IsPostBack)
            {
                addData();
            }
        }

        private void addData()
        {
            StringBuilder str = new StringBuilder();
            DataSet ds = db.GetDataSet("TTDN_BAIVIET_SELECT", 0, sBaiVietID);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                str.AppendFormat("<h2 class='title margin-bottom-lg'>{0}</h2>", row["TieuDe"].ToString());
                if (!string.IsNullOrEmpty(row["ThoiGian"].ToString()))
                    str.AppendFormat("<div class='row margin-bottom-lg'><span>{0}</span></div>", DateTime.Parse(row["ThoiGian"].ToString()).ToString("dd/MM/yyyy - HH:mm"));
                if (!string.IsNullOrEmpty(row["TieuDePhu"].ToString()))
                    str.AppendFormat("<h4 class='m-bottom'>{0}</h4>", row["TieuDePhu"].ToString());
                if (!string.IsNullOrEmpty(row["TomTat"].ToString()))
                    str.AppendFormat("<div class='hometext m-bottom'>{0}</div>", row["TomTat"].ToString());
                str.AppendFormat("<div class='bodytext margin-bottom-lg'>{0}</div>", row["NoiDung"].ToString());
                str.AppendFormat("<div class='margin-bottom-lg'><p class='h5 text-right'>{0}</p></div>", row["TacGia"].ToString());

                divDanhSach.InnerHtml = str.ToString();
            }
        }
    }
}