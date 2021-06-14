<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChuyenMuc.ascx.cs" Inherits="QuanLyVanBan.DichVu.DuLieu.ChuyenMuc" %>

<style>
    .play, .pause {
        width: 100%;
        cursor: pointer;
        border: none;
        background: none;
        color: green;
        font-weight: 700;
        line-height: 20px;
        border-radius: 12px;
    }

    .play {
        color: green;
    }

    .play:hover {
        background-color: green;
        color: white;
    }

    .pause {
        color: orange;
    }

    .pause:hover {
        background-color: orange;
        color: white;
    }
</style>

<table style="width: 100%" class="Vien_Khung">
    <tr>
        <td style="width: 65px;">Trang web:</td>
        <td style="width:350px">
            <asp:DropDownList ID="drpWebID" CssClass="js-example-basic-single" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="drpWebID_SelectedIndexChanged"></asp:DropDownList>
        </td>
        <td style="text-align: right">
            <asp:ImageButton ID="imgThemMoi" runat="server" ImageUrl="~/Images/imgThemMoi.png" />
        </td>
    </tr>
</table>
<div id="divDanhSach" runat="server"></div>
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
<asp:Button ID="btnSuKien" runat="server" CssClass="button" Text="btnSuKien" />