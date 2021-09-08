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

    .Dong_Cell img {
        width: 300px;
        aspect-ratio: 3/2;
        object-fit: cover;
        padding: 3px;
        border: solid 1px #00000038;
        border-radius: 5px;
    }

    .resize_none {
        resize: none !important;
    }

    .select2-selection__rendered {
        max-width: 100% !important;
    }
</style>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">Trang web:<sup>*</sup></div>
    <div class="Dong_Cell">
        <asp:DropDownList ID="drpWeb" runat="server" CssClass="js-example-basic-single" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="drpWeb_SelectedIndexChanged"></asp:DropDownList>
    </div>
</div>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">Chuyên mục:</div>
    <div class="Dong_Cell">
        <asp:DropDownList ID="drpChuyenMuc" CssClass="js-example-basic-single" runat="server" Width="100%"></asp:DropDownList>
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

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">Bài viết URL:</div>
    <div class="Dong_Cell">
        <asp:TextBox ID="txtBaiViet_Url" runat="server" Width="100%"></asp:TextBox>
    </div>
</div>

<div style="width: 100%; clear: both" class="Dong_Table">
    <div class="Dong_Cell" style="width: 115px;">Anh đại diện:</div>
    <div class="Dong_Cell" style="width: 300px;">
        <asp:FileUpload ID="UploadFile" runat="server" Width="100%"></asp:FileUpload>
    </div>
    <div class="Dong_Cell" style="text-align: right;">
        <label for="<%=UploadFile.ClientID%>">
            <img runat="server" id="AnhDaiDien" src="/Images/no_image.png" />
        </label>
    </div>
</div>

<script type="text/javascript">
    var oldsrc = $("#<%=AnhDaiDien.ClientID%>").attr("src");
    $("#<%=UploadFile.ClientID%>").change(function (e) {
        var src = oldsrc;
        if (e.target.files.length !== 0)
            src = URL.createObjectURL(e.target.files[0]);
        console.log(e.target.files[0]);
        $("#<%=AnhDaiDien.ClientID%>").attr("src", src);
    });
</script>

<div style="text-align: center; padding-top: 10px; padding-bottom: 5px;">
    <asp:Button ID="btnThemMoi" runat="server" CssClass="button" Text="Cập nhật" OnClick="btnThemMoi_Click" />
</div>