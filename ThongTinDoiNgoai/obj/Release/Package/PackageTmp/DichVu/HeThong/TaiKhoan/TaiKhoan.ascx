<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaiKhoan.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.TaiKhoan.TaiKhoan" %>
<table style="width: 100%" class="Vien_Khung">
    <tr>
        <td style="width: 60px; text-align: right">Đơn vị:</td>
        <td style="width: 300px">
            <asp:DropDownList ID="drpDonVi" runat="server" Width="100%" CssClass="js-example-basic-single" AutoPostBack="True" OnSelectedIndexChanged="drpDonVi_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
        <td style="width: 60px; text-align: right">Từ khóa:</td>
        <td style="width: 180px">
            <asp:TextBox ID="txtTuKhoa" runat="server" CssClass="textbox" Width="98%"></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="btnTim" runat="server" Text="Tìm" CssClass="button" OnClick="btnTim_Click" />
        </td>
        <td style="text-align: right; width: 100px">
            <asp:ImageButton ID="imgThemMoi" runat="server" ImageUrl="~/Images/ImgThemMoi.png" />
        </td>
    </tr>
</table>
<div id="divDanhSach" runat="server">
</div>
<asp:Button ID="btnSuKien" runat="server" Text="Nạp lại" Style="display: none;" OnClick="btnSuKien_Click" />