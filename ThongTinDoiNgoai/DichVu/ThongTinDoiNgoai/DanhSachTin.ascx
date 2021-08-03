<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DanhSachTin.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.ThongTinDoiNgoai.DanhSachTin" %>

<div class="main-vien">
    <div class="demuc">Thông tin đối ngoại</div>
    <div id="divDanhSach" class="danhsachtin" runat="server"></div>
    <table style="width: 100%" class="Vien_Khung" id="tblPhanTrang" runat="server" border="0">
        <tr>
            <td class="PhanTrang_Cot1">Số dòng trên 1 trang:
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
</div>