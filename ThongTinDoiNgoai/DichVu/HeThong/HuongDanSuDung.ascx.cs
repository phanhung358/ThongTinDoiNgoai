using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FITC.Web.Component;
using System.Data;


namespace ThongTinDoiNgoai.DichVu.HeThong
{
    public partial class HuongDanSuDung : System.Web.UI.UserControl
    {
        FITC_CDataBase db = new FITC_CDataBase(Static.GetConnect());
        CacHamChung ham = new CacHamChung();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Static.PhanTrangThu = 1;
                Session["node_select"] = "0";
                addTree_();
                string sPathFile = Static.getUrl() + Static.GetPath() + "/NhanSu/HuongDanSuDung/HuongDan.doc";
            }
        }

        private void addTree_()
        {
            treePhanMuc.Nodes.Clear();
            treePhanMuc.CollapseAll();
            TreeNode nodeCon = new TreeNode("Hướng dẫn sử dụng", "0");
            nodeCon.Expand();
            treePhanMuc.Nodes.Add(nodeCon);
            addTree(nodeCon.ChildNodes, "0");
            if (Session["node_select"].ToString().Trim() == "0")
            {
                nodeCon.Select();
                //nodeCon.ExpandAll();
            }
        }
        private void addTree(TreeNodeCollection root, string sMenuChaID)
        {
           
        }
        protected void treePhanMuc_OnSelectedNodeChanged(object sender, EventArgs e)
        {
           
        }
    }
}