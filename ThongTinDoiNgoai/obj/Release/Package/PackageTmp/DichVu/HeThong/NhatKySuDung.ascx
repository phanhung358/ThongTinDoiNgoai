<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NhatKySuDung.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.NhatKySuDung" %>
<table cellspacing="1" width="100%" class="Vien_Khung">
    <tr>
        <td width="60" align="right">
            Từ ngày
        </td>
        <td width="70">
            <asp:TextBox ID="txtTuNgay" runat="server" CssClass="textbox" Width="65px"></asp:TextBox>
        </td>
        <td width="30">
            <asp:Image ID="imgTuNgay" runat="server" CssClass="IconCalendar" ImageAlign="AbsMiddle"
                ImageUrl="~/Images/showCalander.gif" />
        </td>
        <td width="60" align="right">
            Đến ngày
        </td>
        <td width="70">
            <asp:TextBox ID="txtDenNgay" runat="server" CssClass="textbox" Width="65px"></asp:TextBox>
        </td>
        <td width="30">
            <asp:Image ID="imgDenNgay" runat="server" CssClass="IconCalendar" ImageAlign="AbsMiddle"
                ImageUrl="~/Images/showCalander.gif" />
        </td>
        <td>
            <asp:Button ID="btnXem" runat="server" CssClass="button" Text="Xem" OnClick="btnXem_Click" />
        </td>
    </tr>
</table>
<div id="divDanhSach" runat="server">
</div>
<table style="width: 100%" class="Vien_Khung" id="tblPhanTrang" runat="server" border="0">
    <tr>
        <td class="PhanTrang_Cot1" style="width:180px">
            Số dòng trên 1 trang:
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
