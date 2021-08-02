<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BaiViet_Tm.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.ThongTinDoiNgoai.BaiViet_Tm" %>

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

    .loading {
        width: 100%;
        height: 100%;
        position: fixed;
        top: 0;
        left: 0;
        background-color: rgba(0,0,0,0.4);
    }

    .loading img {
        position: relative;
        top: 50%;
        left: 50%;
        transform: translate(-50%,-50%);
    }

    .d-none {
        display: none;
    }

    td {
        padding: 1.5px 0;
    }

    tr:nth-child(5n+2) td {
        padding-top: 10px;
    }
</style>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">Trang web:<sup>*</sup></div>
    <div class="Dong_Cell">
        <asp:DropDownList ID="drpWeb" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="drpWeb_SelectedIndexChanged"></asp:DropDownList>
    </div>
</div>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">Chuyên mục:</div>
    <div class="Dong_Cell">
        <asp:DropDownList ID="drpChuyenMuc" runat="server" Width="100%"></asp:DropDownList>
    </div>
</div>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">Tiêu đề:<sup>*</sup></div>
    <div class="Dong_Cell">
        <asp:TextBox ID="txtTieuDe" runat="server" Width="100%"></asp:TextBox>
    </div>
</div>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">Tóm tắt:<sup>*</sup></div>
    <div class="Dong_Cell">
        <asp:TextBox ID="txtTomTat" runat="server" Width="100%"></asp:TextBox>
    </div>
</div>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">Nội dung:<sup>*</sup></div>
    <div class="Dong_Cell">
        <asp:TextBox ID="txtNoiDung" runat="server" Width="100%" CssClass="resize_none" TextMode="MultiLine" Rows="25" Font-Names="Arial"></asp:TextBox>
    </div>
</div>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">Tác giả:</div>
    <div class="Dong_Cell">
        <asp:TextBox ID="txtTacGia" runat="server" Width="100%"></asp:TextBox>
    </div>
</div>

<div id="divDanhSach" runat="server"></div>

<div style="text-align: center; padding-top: 10px; padding-bottom: 5px;">
    <asp:Button ID="btnThemMoi" runat="server" CssClass="button" Text="Cập nhật" OnClick="btnThemMoi_Click" />
</div>