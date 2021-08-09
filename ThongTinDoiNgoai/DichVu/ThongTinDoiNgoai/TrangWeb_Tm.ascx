<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrangWeb_Tm.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.ThongTinDoiNgoai.TrangWeb_Tm" %>

<table style="width: 100%">
    <tr>
        <td style="text-align: right; width: 115px">Nhóm thông tin:<sup>*</sup></td>
        <td>
                <asp:DropDownList ID="drpNhom" CssClass="js-example-basic-single" runat="server" Width="100%">
                    <asp:ListItem Value="0">[Chọn]</asp:ListItem>
                    <asp:ListItem Value="1">Thông tin đối ngoại</asp:ListItem>
                    <asp:ListItem Value="2">Thông tin báo chí</asp:ListItem>
                    <asp:ListItem Value="3">Sở, ban, ngành</asp:ListItem>
                </asp:DropDownList>
            </td>
    </tr>
    <tr>
        <td style="text-align: right">Tên trang web:<sup>*</sup></td>
        <td>
                <asp:TextBox ID="txtTenWeb" runat="server" CssClass="textbox" Width="100%" autocomplete="off"></asp:TextBox>
            </td>
    </tr>
    <tr>
        <td style="text-align: right">Địa chỉ(url):<sup>*</sup></td>
        <td>
                <asp:TextBox ID="txtDiaChiWeb" runat="server" CssClass="textbox" Width="100%" autocomplete="off"></asp:TextBox>
        </td>
    </tr>
</table>

<div style="text-align: center; padding-top: 10px; padding-bottom: 5px;">
    <asp:Button ID="btnThemMoi" runat="server" CssClass="button" Text="Cập nhật" OnClick="btnThemMoi_Click" />
</div>