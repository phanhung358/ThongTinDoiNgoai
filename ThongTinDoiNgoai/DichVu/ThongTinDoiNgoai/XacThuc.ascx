<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XacThuc.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.ThongTinDoiNgoai.XacThuc" %>

<style>
    .xacthuc, .dangtai {
        width: 100%;
        cursor: pointer;
        border: none;
        background: none;
        font-weight: 700;
        line-height: 20px;
        border-radius: 12px;
    }

    .xacthuc {
        color: deepskyblue;
    }

    .xacthuc:hover {
        background-color: deepskyblue;
        color: white;
    }

    .dangtai {
        color: green;
    }

    .dangtai:hover {
        background-color: green;
        color: white;
    }
</style>

<div id="divDanhSach" runat="server"></div>
<table style="width: 100%" class="Vien_Khung" id="tblPhanTrang" runat="server" border="0">
    <tr>
        <td class="PhanTrang_Cot1" style="width:200px">Số dòng trên 1 trang:
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