<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhanQuyen_TaiKhoan.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.PhanQuyen.PhanQuyen_TaiKhoan" %>

<table style="width: 100%" class="Vien_Khung">
    <tr>
        <td style="width: 50px;text-align:right">Đơn vị:</td>
        <td style="width: 200px">
            <asp:DropDownList ID="drpDonVi" CssClass="js-example-basic-single" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpNhomQuyen_SelectedIndexChanged"
                Width="100%">
            </asp:DropDownList>
        </td>
        <td style="width: 80px;text-align:right">Nhóm quyền:</td>
        <td style="width: 150px">
            <asp:DropDownList ID="drpNhomQuyen" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpNhomQuyen_SelectedIndexChanged"
                Width="100%">
            </asp:DropDownList>
        </td>
        <td style="width: 50px;text-align:right">Từ khóa:</td>
        <td style="width: 150px">
            <asp:TextBox ID="txtTuKhoa" runat="server" CssClass="textbox" Width="98%"></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="btnTim" runat="server" Text="Tìm" CssClass="button" OnClick="btnTim_Click" />
        </td>
        <td style="text-align:right">
            <asp:Button ID="btnPhanQuyen" runat="server" Text="Phân quyền" CssClass="button" OnClick="btnPhanQuyen_Click" />
        </td>
    </tr>
</table>

<table class="Vien_Bang" style="width: 100%; border-spacing: 1px" border="0">
    <tr class="Dong_Chan">
        <td style="vertical-align: top;width:250px;">
            <asp:RadioButtonList ID="optNhanVien" runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="optNhanVien_SelectedIndexChanged">
            </asp:RadioButtonList>
        </td>
        <td style="vertical-align: top">
            <div id="divDanhSach" runat="server">
            </div>
        </td>
    </tr>
</table>
