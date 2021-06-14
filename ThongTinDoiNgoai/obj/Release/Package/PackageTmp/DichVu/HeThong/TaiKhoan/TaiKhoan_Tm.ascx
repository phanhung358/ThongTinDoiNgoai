<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaiKhoan_Tm.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.TaiKhoan.TaiKhoan_Tm" %>
<div class="Vien_Khung">
    <table style="width: 100%">
        <tr>
            <td style="width:95px">Đơn vị <sup>*</sup></td>
            <td>
                <asp:DropDownList ID="drpDonVi" runat="server" Width="100%" CssClass="js-example-basic-single">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Họ và tên <sup>*</sup>
            </td>
            <td>
                <asp:TextBox ID="txtTenTaiKhoan" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Chức vụ</td>
            <td>
                <asp:TextBox ID="txtChucVu" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Nhóm quyền <sup>*</sup>
            </td>
            <td>
                <asp:DropDownList ID="drpNhom" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="drpNhom_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Tên đăng nhập <sup>*</sup>
            </td>
            <td>
                <asp:TextBox ID="txtTenDangNhap" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Mật khẩu 
            </td>
            <td>
                <asp:TextBox ID="txtMatKhau" runat="server" CssClass="textbox" Width="100%" TextMode="Password"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>Số điện thoại</td>
            <td>
                <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="textbox" Width="100%"></asp:TextBox>
            </td>
        </tr>

    </table>
</div>
<div style="text-align: center; padding-top: 10px;">
    <asp:Button ID="btnThemMoi" runat="server" Text="Thêm mới" CssClass="button" Width="70px"
        OnClick="btnThemMoi_Click" />
    <asp:Button ID="btnCapNhat" runat="server" Text="Cập nhật" CssClass="button" Width="70px"
        OnClick="btnCapNhat_Click" />
    <asp:Button ID="btnSuKien" runat="server" OnClick="btnSuKien_Click" Text="Button" />
</div>
