<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DonVi.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.TaiKhoan.DonVi" %>

<table style="width: 100%">
    <tr>
        <td style="width: 300px; vertical-align: top">
            <div class="Vien_Khung">
                <table>
                    <tr>
                        <td>
                            <div>
                                Từ khóa: &nbsp;<asp:TextBox ID="txtTuKhoa" Width="180px" runat="server"></asp:TextBox>
                                <asp:Button ID="btnTim" Text="Tìm" CssClass="button" runat="server" OnClick="btnTim_Click" />
                            </div>
                            <asp:TreeView ID="treeDonVi" runat="server" OnSelectedNodeChanged="treeDonVi_OnSelectedNodeChanged"
                                ImageSet="XPFileExplorer" LineImagesFolder="~/images/TreeLineImages" NodeIndent="25"
                                NodeWrap="True" ShowLines="True" SkipLinkText="">
                                <ParentNodeStyle HorizontalPadding="5px" ImageUrl="~/Images/Cat.gif" />
                                <RootNodeStyle HorizontalPadding="5px" ImageUrl="~/Images/Cat.gif" />
                                <NodeStyle HorizontalPadding="2px" ImageUrl="~/Images/Cat.gif" VerticalPadding="2px"
                                    Font-Names="Tahoma" NodeSpacing="0px" />
                                <SelectedNodeStyle CssClass="Tree_Nut_Chon" />
                                <HoverNodeStyle CssClass="Tree_Nut_Hover" />
                            </asp:TreeView>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
        <td style="vertical-align: top">
            <table style="width: 100%" class="Vien_Khung" border="0">
                <tr>
                    <td style="width: 250px">&nbsp;</td>
                    <td style="text-align: right">
                        <asp:Button ID="btnDongBo" runat="server" Text="Đồng bộ" CssClass="button" OnClick="btnDongBo_Click" Visible="False" />
                        <asp:Button ID="btnThemMoi" runat="server" Text="Thêm mới" CssClass="button" />
                    </td>
                </tr>
            </table>
            <div id="divDanhSach" runat="server">
            </div>
            <table style="width: 100%" runat="server" id="tblPhanTrang" class="Vien_Khung" border="0">
                <tr>
                    <td id="td_SoTin" runat="server" class="Tin_SoTin_Combobox" style="width: 200px">Số dòng trên 1 trang:
                        <asp:DropDownList ID="drpSoTin" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSoTin_SelectedIndexChanged">
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem Selected="true">20</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>35</asp:ListItem>
                            <asp:ListItem>40</asp:ListItem>
                            <asp:ListItem>45</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>55</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="Tin_SoTin_PhanTrang">
                        <asp:Label ID="lblPhanTrang" runat="server" Text="lblPhanTrang"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Button ID="btnSuKien" runat="server" OnClick="btnSuKien_Click" Text="btnSuKien" />
