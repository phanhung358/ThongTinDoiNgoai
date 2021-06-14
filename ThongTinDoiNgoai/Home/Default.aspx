<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ThongTinDoiNgoai.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hệ thống thông tin hỏi đáp, tiếp nhận phản ánh kiến nghị dịch vụ đô thị thông minh</title>
    <script type="text/javascript" src="../Js/Functions.js?v=1"></script>
    <script type="text/javascript" src="../Js/jquery-3.1.0.min.js"></script>
    <script type="text/javascript" src="../Js/jquery-ui-1.12.0.custom.min.js"></script>
    <script type="text/javascript" src="../Js/jquery.fancybox.pack.js"></script>
    <script type="text/javascript" src="../Js/NewCalendar.js"></script>
    <script type="text/javascript" src="../Js/Combox/select2.js"></script>
   
    <script type="text/javascript" src="../Js/bieudo/highcharts.js"></script>
    <link href="../Css/Thickbox/jquery.fancybox.css" rel="stylesheet" type="text/css" />

    <link href="../Css/jquery-ui-1.9.1.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Css/phananh.css?v=61" rel="stylesheet" type="text/css" />
    <link href="../Css/Combox/select2.css?v=3" rel="stylesheet" type="text/css" />
    <link href="../Css/GiamSat.css" rel="stylesheet" type="text/css" />
    <link href="../Css/reputa.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Js/fcm/firebase-app.js"></script>
    <script type="text/javascript" src="../Js/fcm/firebase-messaging.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Banner">
            <asp:Literal ID="lblCanhBao" runat="server"></asp:Literal>
            <div id="divTenDonVi" runat="server" class="Menu_TenDonVi">
            </div>
        </div>
        <div class="Vung_Giua">
            <div class="Vung_Trai">
                <asp:DropDownList ID="drpMenuCha" CssClass="drp_Menu js-example-basic-single" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="drpMenuCha_SelectedIndexChanged">
                </asp:DropDownList>
                <div id="divMenuMoi" runat="server"></div>
            </div>
            <div class="Vung_Phai">
                <div class="Vung_Chinh" id="divMain" runat="server">
                </div>
            </div>
        </div>
        <div class="Footer">
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            $(document).ready(function () {
                $(".js-example-basic-single").select2();
            });
        </script>
        <script type="text/javascript">
            $(".js-example-basic-multiple").select2();
        </script>

        <asp:Button ID="btnSuKien" runat="server" Text="Button" OnClick="btnSuKien_Click" />

    </form>
</body>
</html>
<asp:Literal ID="lbl" runat="server"></asp:Literal>