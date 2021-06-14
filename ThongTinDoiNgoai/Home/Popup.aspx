<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Popup.aspx.cs" Inherits="ThongTinDoiNgoai.Popup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
    <meta charset="utf-8" />
    <title>Untitled Page</title>
    <script type="text/javascript" src="../Js/Functions.js?v=1"></script>
    <script type="text/javascript" src="../Js/jquery-3.1.0.min.js"></script>
    <script type="text/javascript" src="../Js/jquery-ui-1.12.0.custom.min.js"></script>
    <script type="text/javascript" src="../Js/jquery.fancybox.pack.js"></script>
    <script type="text/javascript" src="../Js/NewCalendar.js"></script>
    <script type="text/javascript" src="../Js/Combox/select2.js"></script>
    <script type="text/javascript" src="../Js/contextMenu.js"></script>

    <link type="text/css" rel="Stylesheet" href="../Css/thickbox/jquery.fancybox.css" />
    <link type="text/css" rel="Stylesheet" href="../Css/jquery-ui-1.9.1.custom.css" />
    <link type="text/css" rel="Stylesheet" href="../Css/phananh.css?v=12" />
    <link type="text/css" rel="Stylesheet" href="../Css/Combox/select2.css?v=1" />
    <link type="text/css" rel="Stylesheet" href="../Css/ContextMenu.css" />
    <link type="text/css" rel="Stylesheet" href="../Css/reputa.css" />
</head>
<script type="text/javascript">

    function dongTrang(txt) {
        self.parent.document.getElementById(txt).click();
        self.parent.tb_remove();
        return false;
    }

    function heartBeat() {
        $.ajax
    ({
        type: 'POST',
        url: "home/yeucau.aspx",
        data: '',
        success: function (msg) {
        }
    });
    }
    $(function () {

        setInterval("heartBeat()", 1000 * 60); // 30s gửi request một lần  
    });
</script>
<body style="padding: 10px;" id="Popbody" runat="server" class="bodypop">
    <asp:Literal ID="lblMap" runat="server"></asp:Literal>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:PlaceHolder ID="phMain" runat="server"></asp:PlaceHolder>
    </form>
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {
        $(".js-example-basic-single").select2();
    });
</script>
<script type="text/javascript">
    $(".js-example-basic-multiple").select2();
</script>
