<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaoMenu_Tm.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.TaoMenu.TaoMenu_Tm" %>
<table style="width: 100%">
    <tr>
        <td style="width: 80px;">Menu cha</td>
        <td style="height: 26px">
            <asp:DropDownList ID="drpMenuCha" CssClass="js-example-basic-single" runat="server" Width="100%">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Tên menu
        </td>
        <td>
            <asp:TextBox ID="txtTenMenu" runat="server" Width="100%" CssClass="textbox"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Đường dẫn
        </td>
        <td>
            <asp:TextBox ID="txtPath" runat="server" Width="100%" CssClass="textbox"></asp:TextBox>
        </td>
    </tr>
    <tr id="trQuyen" runat="server">
        <td>Quyền
        </td>
        <td>
            <asp:TextBox ID="txtQuyen" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Thứ tự
        </td>
        <td>
            <asp:TextBox ID="txtThuTu" runat="server" CssClass="textbox" Width="54px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td></td>
        <td>
            <asp:CheckBox ID="chkHoatDong" runat="server" Font-Bold="True" Text="Hoạt động" Checked="True" /><br />
            <asp:CheckBox ID="chkCoBaoCao" runat="server" Font-Bold="True"
                Text="admin  có quyền" Checked="True"/>
            <asp:CheckBox ID="chkMacDinh" runat="server" Font-Bold="True" Text="Mặc định chạy trang chủ" Visible="False" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="padding-top: 20px; text-align: center">
            <asp:Button ID="btnThemMoi" runat="server" CssClass="button" Text="Thêm mới" OnClick="btnThemMoi_Click" />
            <asp:Button ID="btnCapNhat" runat="server" CssClass="button" Text="Cập nhật" OnClick="btnCapNhat_Click" />
            <asp:Button ID="btnXoa" runat="server" CssClass="button" OnClick="btnXoa_Click" Text="Xóa"
                Width="47px" />
        </td>
    </tr>
</table>
