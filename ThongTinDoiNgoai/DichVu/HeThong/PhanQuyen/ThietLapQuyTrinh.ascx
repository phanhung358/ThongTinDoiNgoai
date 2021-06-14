<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ThietLapQuyTrinh.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.PhanQuyen.ThietLapQuyTrinh" %>
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

<div class="tabChuan_Dong">
    <asp:Button ID="btnTongHopPhanAnh" runat="server" Text="Phản ánh theo đơn vị" CssClass="tabChuan" OnClick="btnTongHopPhanAnh_Click" />
    <asp:Button ID="btnTongHopPhanAnhTheoLinhVuc" runat="server" Text="Phản ánh theo chuyên mục" CssClass="tabChuan" OnClick="btnTongHopPhanAnhTheoLinhVuc_Click" />
    <asp:Button ID="btnCanhBao" runat="server" Text="Chuyên mục cảnh báo" OnClick="btnCanhBao_Click" CssClass="tabChuan" />
    <asp:Button ID="btnXemQuetThe" runat="server" Text="Xem quẹt thẻ" CssClass="tabChuan" OnClick="btnXemQuetThe_Click"/>
</div>


<asp:PlaceHolder ID="pl" runat="server"></asp:PlaceHolder>



