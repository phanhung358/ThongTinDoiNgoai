<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaoMenu.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.TaoMenu.TaoMenu" %>
<table border="0" style="width: 100%">
    <tr>
        <td style="vertical-align: top; width: 350px">
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
        <td style="vertical-align: top">
            <table class="Vien_Khung" style="width: 100%">
                <tr>
                    <td>
                        &nbsp;</td>
                    <td style="text-align: right">
                        <asp:ImageButton ID="imgThemMoi" runat="server" ImageUrl="~/Images/ImgThemMoi.png" />
                    </td>
                </tr>
            </table>
            <div id="divDanhSach" runat="server">
            </div>
        </td>
    </tr>
</table>
<asp:Button ID="btnSuKien" runat="server" Text="Nạp lại" Style="display: none;" OnClick="btnSuKien_Click" />
