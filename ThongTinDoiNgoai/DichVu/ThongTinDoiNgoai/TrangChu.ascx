<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrangChu.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.ThongTinDoiNgoai.TrangChu" %>

<div class="ttdn-vien">
    <div class="ttdn-demuc">Thông tin đối ngoại</div>
    <div class="ttdn-control-vien" id="divControl" runat="server">
        <div class="ttdn-control-nen">
            <div class="ttdn-control1">
                <div class="ttdn-control-text">Trang web</div>
                <div class="ttdn-control-doituong">
                    <asp:DropDownList Width="100%" ID="drpTrangWeb" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpTrangWeb_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="ttdn-control2">
                <div class="ttdn-control-text">Chuyên mục</div>
                <div class="ttdn-control-doituong">
                    <asp:DropDownList Width="100%" ID="drpChuyenMuc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpChuyenMuc_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="divDanhSach" runat="server"></div>
<table style="width: 100%" class="Vien_Khung" id="tblPhanTrang" runat="server" border="0">
    <tr>
        <td class="PhanTrang_Cot1">Số dòng trên 1 trang:
            <asp:DropDownList ID="drpSoTin" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpSoTin_SelectedIndexChanged">
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem Selected="True">50</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            <asp:Label ID="lblPhanTrang" runat="server"></asp:Label>
        </td>
    </tr>
</table>