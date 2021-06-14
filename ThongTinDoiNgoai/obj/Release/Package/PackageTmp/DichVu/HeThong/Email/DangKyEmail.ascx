<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DangKyEmail.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.Email.DangKyEmail" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%" class="Vien_Khung">
            <tr>
                <td class="Tin_TieuDeChinh">
                    Đăng ký tài khoản email</td>
                <td align="right" style="width: 100px">
                    <asp:ImageButton runat="server" ID="imgThemMoi" ImageUrl="~/Images/ImgThemMoi.png" /></td>
            </tr>
        </table>
        <div id="divDanhSach" runat="server">
        </div>
        <asp:Button ID="btnSuKien" runat="server" Style="display: none;" />
    </ContentTemplate>
</asp:UpdatePanel>
