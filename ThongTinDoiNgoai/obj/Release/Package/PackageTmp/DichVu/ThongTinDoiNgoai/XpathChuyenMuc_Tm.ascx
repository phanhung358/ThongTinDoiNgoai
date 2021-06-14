<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XpathChuyenMuc_Tm.ascx.cs" Inherits="QuanLyVanBan.DichVu.DuLieu.XpathChuyenMuc_Tm" %>

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
</style>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 125px;">Trang web:<sup>*</sup></div>
    <div class="Dong_Cell">
        <asp:DropDownList ID="drpWebID" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="drpWebID_SelectedIndexChanged" Enabled="False"></asp:DropDownList>
    </div>
</div>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 125px;">Chuyên mục:<sup>*</sup></div>
    <div class="Dong_Cell">
        <asp:DropDownList ID="drpChuyenMucID" runat="server" Width="100%" Enabled="False"></asp:DropDownList>
    </div>
</div>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 125px;">Xpath Danh sách:<sup>*</sup></div>
    <div class="Dong_Cell">
        <asp:TextBox ID="txtDanhSach" runat="server" Width="100%"></asp:TextBox>
    </div>
</div>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 125px;">Xpath URL bài viết 1:<sup>*</sup></div>
    <div class="Dong_Cell">
        <asp:TextBox ID="txtBaiViet_Url1" runat="server" Width="100%"></asp:TextBox>
    </div>
</div>
<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 125px;">Xpath URL bài viết 2:</div>
    <div class="Dong_Cell">
        <asp:TextBox ID="txtBaiViet_Url2" runat="server" Width="100%"></asp:TextBox>
    </div>
</div>
<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 125px;">Xpath URL bài viết 3:</div>
    <div class="Dong_Cell">
        <asp:TextBox ID="txtBaiViet_Url3" runat="server" Width="100%"></asp:TextBox>
    </div>
</div>

<div class="loading d-none" runat="server" id="loading">
    <img src="../../Css/Thickbox/fancybox_loading.gif" />
</div>

<div style="text-align: center; padding-top: 10px; padding-bottom: 5px;">
    <asp:Button ID="btnConnect" runat="server" CssClass="button" Text="Kiểm tra" OnClick="btnConnect_Click" />
    <asp:Button ID="btnThemMoi" runat="server" CssClass="button" Text="Cập nhật" OnClick="btnThemMoi_Click" />
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $("#ctl11_btnConnect").click(function () {
            $("#ctl11_loading").removeClass("d-none");
        });
    });
</script>