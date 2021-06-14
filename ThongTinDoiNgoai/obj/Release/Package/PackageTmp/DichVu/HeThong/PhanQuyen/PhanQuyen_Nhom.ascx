<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhanQuyen_Nhom.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.PhanQuyen.PhanQuyen_Nhom" %>
<table class="Vien_Khung" style="width: 100%; border-spacing: 1px" border="0">
    <tr class="">
        <td style="width: 300px" class="">
            <asp:LinkButton ID="lnkNhom" Visible="false" runat="server" Style="color: white">Tạo mới nhóm</asp:LinkButton>
        </td>
        <td style="text-align: right">
            <asp:Button ID="btnPhanQuyen" runat="server" CssClass="button"
                Text="Phân quyền" OnClick="btnPhanQuyen_Click" />
            <asp:Button ID="btnSuKien" runat="server" OnClick="btnSuKien_Click"
                Text="Sự kiện" />
        </td>
    </tr>
</table>
<table class="Vien_Bang" style="width: 100%; border-spacing: 1px" border="0">
    <tr class="Dong_Chan">
        <td style="vertical-align: top">
            <asp:RadioButtonList ID="optNhom" runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="optNhom_SelectedIndexChanged">
            </asp:RadioButtonList>
        </td>
        <td style="vertical-align: top">
            <div id="divDanhSach" runat="server">
            </div>
        </td>
    </tr>
</table>
