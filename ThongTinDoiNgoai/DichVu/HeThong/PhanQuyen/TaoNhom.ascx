<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaoNhom.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.PhanQuyen.TaoNhom" %>
<style type="text/css">
    .style1
    {
        width: 100px;
    }
</style>
<div class="Vien_Khung">
    <table border="0">
        <tr>
            <td class="style1">
                Tên nhóm: <span style="color: Red;">(*)</span>
            </td>
            <td>
                <asp:TextBox ID="txtNhom" runat="server" CssClass="textbox" Width="350px"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div style="text-align: center; padding-top: 10px">
        <asp:Button ID="btnThemMoi" runat="server" CssClass="button" Text="Thêm mới" OnClick="btnThemMoi_Click" />
        <asp:Button ID="btnCapNhat" runat="server" CssClass="button" Text="Cập nhật" OnClick="btnCapNhat_Click"
            Visible="false" />
        <asp:Button ID="btnSuKien" runat="server" Style="display: none;" />
        <asp:HiddenField ID="hidID" runat="server" />
    </div>
</div>
<div id="divDanhSach" runat="server">
</div>
