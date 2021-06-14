<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaHoa.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.MaHoa" %>
<table style="width: 100%" class="Vien_Khung">
    <tr>
        <td style="width:50px">Đầu vào:</td>
        <td>
            <asp:TextBox ID="txtDauVao" Width="100%" CssClass="button" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Kết quả:</td>
        <td>
            <asp:TextBox ID="txtKetQua" Width="100%" CssClass="button" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td> </td>
        <td style="padding-top:10px">
            <asp:Button ID="btnMaHoa" runat="server" Text="Mã hóa" CssClass="button" OnClick="btnMaHoa_Click" />
            <asp:Button ID="btnDichNguoc" runat="server" Text="Đọc mã hóa" CssClass="button" OnClick="btnDichNguoc_Click" />
        </td>
    </tr>
</table>
