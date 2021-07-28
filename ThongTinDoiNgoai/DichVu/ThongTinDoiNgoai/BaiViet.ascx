<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BaiViet.ascx.cs" Inherits="QuanLyVanBan.DichVu.DuLieu.BaiViet" %>

<style>
    .text-ellipsis {
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
        width: 400px;
        display: block;
    }
</style>

<table style="width: 100%" class="Vien_Khung">
    <tr>
        <td style="width: 100px">Nhóm thông tin:
        </td>
        <td style="width: 150px">
            <asp:DropDownList ID="drpNhom" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpNhom_SelectedIndexChanged" Width="100%">
                <asp:ListItem Value="0">[Tất cả]</asp:ListItem>
                <asp:ListItem Value="1" Selected="True">Thông tin đối ngoại</asp:ListItem>
                <asp:ListItem Value="2">Thông tin báo chí</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="width: 65px;">Trang web:</td>
        <td style="width:300px">
            <asp:DropDownList ID="drpWeb" CssClass="js-example-basic-single" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="drpWeb_SelectedIndexChanged"></asp:DropDownList>
        </td>
        <td style="width: 70px; text-align: right">Chuyên mục:</td>
        <td style="width:300px">
            <asp:DropDownList ID="drpChuyenMuc" CssClass="js-example-basic-single" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="drpChuyenMuc_SelectedIndexChanged"></asp:DropDownList>
        </td>
        <td style="text-align: right">
            <asp:Button ID="btnXoa" runat="server" Text="Xóa" CssClass="button" OnClick="btnXoa_Click" />
            <asp:Button runat="server" CssClass="button" ID="btnCapNhat" Text="Lấy dữ liệu" OnClick="btnCapNhat_Click" />
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