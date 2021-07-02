<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XpathChuyenMuc_Tm.ascx.cs" Inherits="QuanLyVanBan.DichVu.DuLieu.XpathChuyenMuc_Tm" %>
<table style="width:100%">
    <tr>
        <td style="text-align: right;width:125px">Trang web:<sup>*</sup></td>
        <td>
        <asp:DropDownList ID="drpWeb" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="drpWeb_SelectedIndexChanged" Enabled="False"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="text-align:right">Chuyên mục:<sup>*</sup></td>
        <td>
        <asp:DropDownList ID="drpChuyenMuc" runat="server" Width="100%" Enabled="False"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="text-align:right">Xpath danh sách:<sup>*</sup></td>
        <td>
        <asp:TextBox ID="txtDanhSach" runat="server" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align:right">Xpath URL bài viết 1:<sup>*</sup></td>
        <td>
        <asp:TextBox ID="txtBaiViet_Url1" runat="server" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align:right">Xpath URL bài viết 2:</td>
        <td>
        <asp:TextBox ID="txtBaiViet_Url2" runat="server" Width="100%"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align:right">Xpath URL bài viết 3:</td>
        <td>
        <asp:TextBox ID="txtBaiViet_Url3" runat="server" Width="100%"></asp:TextBox>
        </td>
    </tr>
</table>

<div style="text-align: center; padding-top: 10px; padding-bottom: 5px;">
    <asp:Button ID="btnSuKien" runat="server" CssClass="button" Text="Sự kiện" OnClick="btnSuKien_Click"  />
    <asp:Button ID="btnConnect" runat="server" CssClass="button" Text="Kiểm tra" OnClick="btnConnect_Click" />
    <asp:Button ID="btnThemMoi" runat="server" CssClass="button" Text="Cập nhật" OnClick="btnThemMoi_Click" />
    <asp:Button ID="btnLayTuTrangKhac" runat="server" Text="Lấy từ trang khác" CssClass="button" />
</div>
 