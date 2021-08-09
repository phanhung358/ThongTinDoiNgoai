<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrangWeb.ascx.cs" Inherits="QuanLyVanBan.DichVu.DuLieu.TrangWeb" %>

<div class="Vien_Khung">
    <table border="0" style="width: 100%">
        <tr>
            <td style="width: 100px">Nhóm thông tin:
            </td>
            <td style="width: 150px">
                <asp:DropDownList ID="drpNhom" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpNhom_SelectedIndexChanged" Width="100%">
                    <asp:ListItem Value="0">[Tất cả]</asp:ListItem>
                    <asp:ListItem Value="1" Selected="True">Thông tin đối ngoại</asp:ListItem>
                    <asp:ListItem Value="2">Thông tin báo chí</asp:ListItem>
                    <asp:ListItem Value="3">Sở, ban, ngành</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="text-align: right">
                <asp:ImageButton ID="imgThemMoi" runat="server" ImageUrl="~/Images/imgThemMoi.png" />
            </td>
        </tr>
    </table>
</div>
<div id="divDanhSach" runat="server"></div>
<table style="width: 100%" class="Vien_Khung" id="tblPhanTrang" runat="server" border="0">
    <tr>
        <td class="PhanTrang_Cot1">Số dòng trên 1 trang:
            <asp:DropDownList ID="drpSoTin" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSoTin_SelectedIndexChanged">
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem Selected="True">20</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            <asp:Label ID="lblPhanTrang" runat="server"></asp:Label>
        </td>
    </tr>
</table>
<asp:Button ID="btnSuKien" runat="server" CssClass="butotn" Text="btnSuKien" />