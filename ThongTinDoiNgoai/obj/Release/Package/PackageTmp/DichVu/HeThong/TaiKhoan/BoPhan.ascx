<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BoPhan.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.TaiKhoan.BoPhan" %>
<div class="Vien_Khung">
    <table border="0" style="width:100%">
        <tr>
            <td style="width:80px">
                Tên bộ phận:<span style="color: Red;">*</span>
            </td>
            <td>
                <asp:TextBox ID="txtBoPhan" runat="server" CssClass="textbox" Width="99%" autocomplete="off"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div style="text-align: center;padding-top:10px">
        <asp:Button ID="btnThemMoi" runat="server" CssClass="button" Text="Lưu" OnClick="btnThemMoi_Click"
            Width="60px" />
        <asp:Button ID="btnCapNhat" runat="server" CssClass="button" Text="Lưu" OnClick="btnCapNhat_Click"
            Visible="false" Width="60px" />
        <asp:Button ID="btnHuyBo" runat="server" CssClass="button" Text="Mới" OnClick="btnHuyBo_Click"
            Visible="False" Width="60px" />
        <asp:Button ID="btnSuKien" runat="server" Style="display: none;" />
        <asp:HiddenField ID="hidID" runat="server" />
    </div>
</div>
<div id="divDanhSach" runat="server">
</div>
<table style="width: 100%" class="Vien_Khung" id="tblPhanTrang" runat="server" border="0">
    <tr>
        <td class="PhanTrang_Cot1">
            Số dòng trên 1 trang:
            <asp:DropDownList ID="drpSoTin" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSoTin_SelectedIndexChanged">
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem Selected="True">20</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            <asp:Label ID="lblPhanTrang" runat="server"></asp:Label>
        </td>
    </tr>
</table>
