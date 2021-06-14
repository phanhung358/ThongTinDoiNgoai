<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChuyenMuc_Tm.ascx.cs" Inherits="QuanLyVanBan.DichVu.DuLieu.ChuyenMuc_Tm" %>

<style type="text/css">
    .MT_DongTieuDe {
        border-bottom: 1px dashed #111;
        border-top: 0px !important;
        border-right: 0px;
        border-left: 0px;
        background-color: white;
        font-weight: bold;
    }

    .text_thue_chuanhap {
        border-bottom: 1px dashed #111;
        border-top: 0px !important;
        border-right: 0px;
        border-left: 0px;
        background-color: yellow;
        font-weight: bold;
    }

    input:focus {
        outline: 0px;
    }

    input[type=text] {
    }

    select {
        height: 20px !important;
    }

    .Dong_Table {
        display: table;
        margin: 3px 0;
    }

    .Dong_Cell {
        border: 0px solid black;
        vertical-align: middle;
        display: table-cell;
        line-height: 20px;
    }

    .Thue_TieuDe {
        line-height: 15px;
        font-weight: bold;
        padding: 10px 0px 5px 0px;
    }

    .BatBuoc {
        color: red;
    }

    sup {
        color: red;
        vertical-align: middle;
    }

    .resize_none {
        resize: none;
    }
</style>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">Trang web:<sup>*</sup></div>
    <div class="Dong_Cell">
        <asp:DropDownList ID="drpWebID" CssClass="js-example-basic-single" runat="server" Width="100%"></asp:DropDownList>
    </div>
</div>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">Tên chuyên mục:<sup>*</sup></div>
    <div class="Dong_Cell">
        <asp:TextBox ID="txtTenChuyenMuc" runat="server" Width="100%"></asp:TextBox>
    </div>
</div>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">URL chuyên mục:<sup>*</sup></div>
    <div class="Dong_Cell">
        <asp:TextBox ID="txtUrlChuyenMuc" runat="server" Width="100%"></asp:TextBox>
    </div>
</div>

<div style="text-align: center; padding-top: 10px; padding-bottom: 5px;">
    <asp:Button ID="btnThemMoi" runat="server" CssClass="button" Text="Cập nhật" OnClick="btnThemMoi_Click" />
</div>
