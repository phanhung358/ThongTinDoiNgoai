<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChuyenMuc_Tm.ascx.cs" Inherits="QuanLyVanBan.DichVu.DuLieu.ChuyenMuc_Tm" %>

<table style="width: 100%">
    <tr>
        <td style="text-align: right; width: 115px">Trang web:<sup>*</sup></td>
        <td>
            <asp:DropDownList ID="drpWeb" CssClass="js-example-basic-single" runat="server" Width="100%"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="text-align: right">Chuyên mục:<sup>*</sup></td>
        <td>
            <asp:TextBox ID="txtTenChuyenMuc" runat="server" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: right">URL chuyên mục:<sup>*</sup></td>
        <td>
            <asp:TextBox ID="txtUrlChuyenMuc" runat="server" Width="100%"></asp:TextBox>
        </td>
    </tr>
</table>

<div style="text-align: center; padding-top: 10px; padding-bottom: 5px;">
    <asp:Button ID="btnThemMoi" runat="server" CssClass="button" Text="Cập nhật" OnClick="btnThemMoi_Click" />
</div>
