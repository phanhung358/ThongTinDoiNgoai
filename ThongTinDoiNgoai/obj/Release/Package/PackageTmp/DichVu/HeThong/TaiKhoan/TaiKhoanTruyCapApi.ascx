<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaiKhoanTruyCapApi.ascx.cs" Inherits="PhanAnhKienNghi.DichVu.Reputa.TaiKhoanTruyCapApi" %>
<div class="Vien_Khung">
    <table border="0" style="width: 100%">
        <tr>
            <td style="text-align: right;width:100px">Tài khoản:<span style="color: Red;">*</span></td>
            <td>
                <asp:TextBox ID="txtTaiKhoan" runat="server" CssClass="textbox" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: right">Tên đơn vị:</td>
            <td>
                <asp:TextBox ID="txtTenDonVi" runat="server" CssClass="textbox" Width="300px"></asp:TextBox>
            </td>
        </tr>
         
        <tr>
            <td style="text-align: right">&nbsp;</td>
            <td style="padding-top:10px">
                <asp:Button ID="btnThemMoi" runat="server" CssClass="button" Text="Thêm mới" OnClick="btnThemMoi_Click" />
                <asp:Button ID="btnCapNhat" runat="server" CssClass="button" Text="Cập nhật" OnClick="btnCapNhat_Click"
                    Visible="false" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidID" runat="server" />
</div>
<div id="divDanhSach" runat="server"></div>
