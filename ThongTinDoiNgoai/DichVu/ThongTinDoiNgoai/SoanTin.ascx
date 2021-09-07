<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SoanTin.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.ThongTinDoiNgoai.SoanTin" %>

<table style="width: 100%" class="Vien_Khung">
    <tr>
        <td style="text-align: right">
            <asp:ImageButton ID="imgThemMoi" runat="server" ImageUrl="~/Images/imgThemMoi.png" />
        </td>
    </tr>
</table>
<div id="divDanhSach" runat="server"></div>
<table style="width: 100%" class="Vien_Khung" id="tblPhanTrang" runat="server" border="0">
    <tr>
        <td class="PhanTrang_Cot1" style="width:200px">Số dòng trên 1 trang:
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
<asp:Button ID="btnSuKien" runat="server" CssClass="button" Text="btnSuKien" />