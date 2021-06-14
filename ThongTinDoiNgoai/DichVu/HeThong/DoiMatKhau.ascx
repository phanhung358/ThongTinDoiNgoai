<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DoiMatKhau.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.DoiMatKhau" %>

<table style="width: 100%" id="tblMatKhau" runat="server" class="Vien_Khung">
    <tr>
        <td style="width: 110px;">Mật khẩu cũ:
        </td>
        <td>
            <asp:TextBox ID="txtMatKhauCu" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Mật khẩu mới:
        </td>
        <td>
            <asp:TextBox ID="txtMatKhauMoi" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Xác nhận mật khẩu:
        </td>
        <td>
            <asp:TextBox ID="txtXacNhanMK" runat="server" CssClass="textbox" TextMode="Password"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td></td>
        <td style="padding-top: 10px">
            <asp:Button ID="btnLuuMatKhau" runat="server" Text="Lưu " Width="60px" OnClick="btnLuuMatKhau_Click"
                CssClass="button" />
            <asp:Button ID="btnQuayLai" runat="server" CssClass="button" Text="Quay lại"
                OnClick="btnQuayLai_Click" />
        </td>
    </tr>
</table>
