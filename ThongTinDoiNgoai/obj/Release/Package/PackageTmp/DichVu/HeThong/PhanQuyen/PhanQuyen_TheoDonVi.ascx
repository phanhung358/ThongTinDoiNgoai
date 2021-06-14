<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhanQuyen_TheoDonVi.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.PhanQuyen.PhanQuyen_TheoDonVi" %>
<script type="text/javascript">
    function IsCheck(ckbId)//, sThongTin, sQuyen)
    {
        var f = document.forms[0];
        var e = document.getElementById(ckbId);
        if (e.type == "checkbox") {
            ckbId = ckbId + "_";
            if (e.checked)//Kiem tra check cha toan bo
            {
                for (j = 0; j < f.length; j++)//Đi checked con cua no
                {
                    ckbCha = f.elements[j];
                    var chaid = ckbCha.id + "_";

                    if (ckbCha.type == "checkbox") {
                        if (chaid.indexOf(ckbId) != -1 || ckbId.indexOf(chaid) != -1) {
                            ckbCha.checked = true;
                        }
                    }
                }
            }
            else//Bo check toan bo
            {
                var iTongCheck = 0
                var iTong = 0
                for (j = 0; j < f.length; j++)//
                {
                    ckbCha = f.elements[j];
                    if (ckbCha.type == "checkbox") {
                        if (ckbCha.id.indexOf(ckbId) != -1)
                            iTong = iTong + 1;
                        if (ckbCha.id.indexOf(ckbId) != -1 && ckbCha.checked) {
                            iTongCheck = iTongCheck + 1
                        }
                    }
                }
                if (iTong != iTongCheck)
                    return;
                for (j = 0; j < f.length; j++)//
                {
                    ckbCha = f.elements[j];
                    if (ckbCha.type == "checkbox") {
                        if (ckbCha.id.indexOf(ckbId) != -1) {
                            ckbCha.checked = false;
                        }
                    }
                }
            }
        }
    }
    function checkall(obj, id) {
        var frm = document.forms[0];
        for (j = 0; j < frm.length; j++) {
            e = frm.elements[j];
            if (e.type == 'checkbox' && e.id.indexOf(id) != -1) {
                e.checked = obj.checked;
            }
        }
    }
</script>

<div class="Vien_Khung" style="display: none;">
    <asp:RadioButtonList ID="optChucNang" runat="server" AutoPostBack="True" OnSelectedIndexChanged="optChucNang_SelectedIndexChanged"
        RepeatDirection="Horizontal" Font-Bold="True">
    </asp:RadioButtonList>
</div>

<table style="width: 100%" border="0" class="Vien_Khung">
    <tr>
        <td style="width: 50px; text-align: right">Đơn vị:</td>
        <td style="width: 200px">
            <asp:DropDownList ID="drpDonVi" CssClass="js-example-basic-single" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpNhomQuyen_SelectedIndexChanged"
                Width="100%">
            </asp:DropDownList>
        </td>
        <td style="width: 80px; text-align: right">Nhóm quyền:</td>
        <td style="width: 150px">
            <asp:DropDownList ID="drpNhomQuyen" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpNhomQuyen_SelectedIndexChanged"
                Width="100%">
            </asp:DropDownList>
        </td>
        <td style="text-align: right">
            <asp:Button ID="btnPhanQuyen" runat="server" CssClass="button" OnClick="btnPhanQuyen_Click"
                Text="Phân quyền" />
        </td>
    </tr>
</table>
<table style="width: 100%; border-spacing: 1px;" border="0" class="Vien_Bang">
    <tr class="Dong_Chan">
        <td style="padding-left: 20px; width: 200px; vertical-align: top">
            <div style="padding-left: 3px;">
                <asp:CheckBox ID="chkTatCa1" runat="server" AutoPostBack="True" OnCheckedChanged="chkTatCa1_CheckedChanged" Text="Tất cả" Style="font-weight: 700" />
            </div>
            <div style="height: 400px; overflow: auto">
                <asp:CheckBoxList ID="optQuyenTT" runat="server" AutoPostBack="True" OnSelectedIndexChanged="optQuyenTT_SelectedIndexChanged">
                </asp:CheckBoxList>
            </div>
        </td>
        <td style="vertical-align: top; padding-left: 3px;">
            <asp:CheckBox ID="chkTatCa" runat="server" Text="Tất cả" Font-Bold="True" />
            <div style="height: 400px; overflow: auto">
                <asp:Label ID="lblDs_QuyenTT" runat="server"></asp:Label>
            </div>
        </td>
    </tr>
</table>
