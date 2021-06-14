<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XpathChuyenMuc_Tm_Copy.ascx.cs" Inherits="QuanLyVanBan.DichVu.DuLieu.XpathChuyenMuc_Tm_Copy" %>
<table style="width:100%">
    <tr>
        <td style="text-align: right;width:90px">Trang web:<sup>*</sup></td>
        <td>
            <asp:DropDownList ID="drpWeb" CssClass="js-example-basic-single" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="drpWeb_SelectedIndexChanged"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="text-align:right">Chuyên mục:<sup>*</sup></td>
        <td>
            <asp:DropDownList ID="drpChuyenMuc" CssClass="js-example-basic-single" runat="server" Width="100%"></asp:DropDownList>
        </td>
    </tr>
</table>

<div style="text-align: center; padding-top: 10px; padding-bottom: 5px;">
    <asp:Button ID="btnThemMoi" runat="server" CssClass="button" Text="Cập nhật" OnClick="btnThemMoi_Click" />
</div>
 