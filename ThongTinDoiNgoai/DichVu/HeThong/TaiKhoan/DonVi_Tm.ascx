<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DonVi_Tm.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.TaiKhoan.DonVi_Tm" %>
<table style="width: 100%">
    <tr>
        <td style="width: 110px; text-align: right">Đơn vị cấp trên<span style="color: red">*</span></td>
        <td>
            <asp:DropDownList ID="drpDonViCapTren" runat="server" Width="100%" CssClass="js-example-basic-single">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="width: 110px; text-align: right">Tên đơn vị<span style="color: red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtTenDonVi" CssClass="textbox" runat="server" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: right">Tên viết tắt</td>
        <td>
            <asp:TextBox ID="txtTenVietTat" CssClass="textbox" runat="server" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: right">Mã định danh<span style="color: red">*</span>
        </td>
        <td>
            <asp:TextBox ID="txtMaDinhDanh0" runat="server" CssClass="textbox" Width="50px"></asp:TextBox>
            .
            <asp:TextBox ID="txtMaDinhDanh1" runat="server" CssClass="textbox" Width="50px"></asp:TextBox>
            .
            <asp:TextBox ID="txtMaDinhDanh2" runat="server" CssClass="textbox" Width="50px"></asp:TextBox>
            .
            <asp:TextBox ID="txtMaDinhDanh3" runat="server" CssClass="textbox" Width="50px"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td style="text-align: right">Website
        </td>
        <td>
            <asp:TextBox ID="txtWebsite" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: right">Điện thoại
        </td>
        <td>
            <asp:TextBox ID="txtDienThoai" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: right">Fax
        </td>
        <td>
            <asp:TextBox ID="txtFax" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: right">Địa chỉ
        </td>
        <td>
            <asp:TextBox ID="txtDiaChi" runat="server" CssClass="textbox" Width="100%" Height="30px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: right">Email
        </td>
        <td>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: right">Cấp<span style="color: red">*</span></td>
        <td>
            <asp:DropDownList ID="drpCap" runat="server" Width="100%">
                <asp:ListItem Value="0">[Chon]</asp:ListItem>
                <asp:ListItem Selected="True" Value="1">Sở, ban, ngành</asp:ListItem>
                <asp:ListItem Value="2">Huyện, thị xã, thành phố</asp:ListItem>
                <asp:ListItem Value="3">Xã, phường</asp:ListItem>
                <asp:ListItem Value="4">Cơ quan TW đóng trên địa bàn</asp:ListItem>
                <asp:ListItem Value="5">Sự nghiệp</asp:ListItem>
                <asp:ListItem Value="6">Doanh nghiệp</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="text-align: right">Nhóm<span style="color: red">*</span></td>
        <td>
            <asp:DropDownList ID="drpNhom" runat="server" Width="100%">
                <asp:ListItem Value="0">[Chon]</asp:ListItem>
                <asp:ListItem Selected="True" Value="1">Cơ quan nhà nước</asp:ListItem>
                <asp:ListItem Value="2">Cơ quan TW đóng trên địa bàn</asp:ListItem>
                <asp:ListItem Value="3">Doanh nghiệp, đơn vị sự nghiệp</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
<div style="text-align: center; padding-top: 20px;">
    <asp:Button ID="btnThem" runat="server" CssClass="button" OnClick="btnThem_Click"
        Text="Thêm mới"   />
    <asp:Button ID="btnLuu" runat="server" CssClass="button" OnClick="btnLuu_Click" Text="Cập nhật"
        Visible="False"   />
    <asp:Button ID="btnHuy" runat="server" CssClass="button" OnClick="btnHuy_Click" Text="Hủy"
         />
</div>
<asp:Button ID="btnSuKien" runat="server" OnClick="btnSuKien_Click" Text="btnSuKien" />