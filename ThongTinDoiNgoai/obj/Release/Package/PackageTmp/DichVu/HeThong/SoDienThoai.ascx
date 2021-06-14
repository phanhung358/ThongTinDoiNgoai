<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SoDienThoai.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.SoDienThoai" %>
<table style="width: 100%;" class="Vien_Khung">
    <tr>
        <td style="width: 110px; vertical-align: middle; text-align: right">Danh sách số nhận:</td>
        <td>
            <asp:TextBox ID="txtSoDienThoai" runat="server" Height="100px" TextMode="MultiLine" Width="300px"></asp:TextBox></td>
    </tr>
    <tr>
        <td></td>
        <td style="padding-top:10px;">
            <asp:Button ID="btnCapNhat" runat="server" Text="Cập nhật" CssClass="button" OnClick="btnCapNhat_Click" />
        </td>
    </tr>
</table>
