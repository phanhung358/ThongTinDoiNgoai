<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhanQuyen.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.PhanQuyen.PhanQuyen" %>
<div class="tabChuan_Dong">
    <asp:Button ID="btnTheoTaiKhoan" runat="server" Text="Theo tài khoản" CssClass="tabChuan" OnClick="btnTheoTaiKhoan_Click" />
    <asp:Button ID="btnTheoNhom" runat="server" Text="Theo nhóm" CssClass="tabChuan" OnClick="btnTheoNhom_Click" />
    <asp:Button ID="btnTongHopPhanAnh" runat="server" Text="Theo đơn vị" CssClass="tabChuan" OnClick="btnTongHopPhanAnh_Click" />
    <asp:Button ID="btnKiemSoatQuyen" runat="server" Text="Kiểm soát quyền" CssClass="tabChuan" OnClick="btnKiemSoatQuyen_Click" />
</div>
<asp:PlaceHolder ID="pl" runat="server"></asp:PlaceHolder>



