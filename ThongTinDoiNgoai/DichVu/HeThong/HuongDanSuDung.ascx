<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HuongDanSuDung.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.HuongDanSuDung" %>
<table cellspacing="1" border="0" cellpadding="0" width="100%">
    <tr>
        <td valign="top" width="350px">
            <div class="Vien_Khung" style="height: 500px; overflow: auto">
                <asp:TreeView ID="treePhanMuc" runat="server" OnSelectedNodeChanged="treePhanMuc_OnSelectedNodeChanged"
                    ImageSet="XPFileExplorer" LineImagesFolder="~/images/TreeLineImages" NodeIndent="25"
                    NodeWrap="True" ShowLines="True" SkipLinkText="">
                    <ParentNodeStyle HorizontalPadding="5px" ImageUrl="~/Images/Cat.gif" />
                    <RootNodeStyle HorizontalPadding="5px" ImageUrl="~/Images/Cat.gif" />
                    <NodeStyle HorizontalPadding="5px" ImageUrl="~/Images/Cat.gif" VerticalPadding="2px"
                        Font-Names="Tahoma" NodeSpacing="0px" />
                    <SelectedNodeStyle CssClass="Tree_Nut_Chon" />
                    <HoverNodeStyle CssClass="Tree_Nut_Hover" />
                </asp:TreeView>
            </div>
        </td>
        <td valign="top">
        </td>
    </tr>
</table>
