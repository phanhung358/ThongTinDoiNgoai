<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ttdn.aspx.cs" Inherits="ThongTinDoiNgoai.ttdn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Thông tin đối ngoại</title>
    <meta content="user-scalable=no, initial-scale=1.0, maximum-scale=1.0, width=device-width" name="viewport" />
    <script type="text/javascript" src="/js/Functions.js"></script>
    <script type="text/javascript" src="/Js/jquery-3.1.0.min.js"></script>
    <script type="text/javascript" src="/js/jquery-ui-1.12.0.custom.min.js"></script>
    <script type="text/javascript" src="/js/jquery.fancybox.pack.js"></script>
    <script type="text/javascript" src="/js/NewCalendar.js"></script>
    <script type="text/javascript" src="/js/Combox/select2.js"></script>
    <link href="/Css/jquery-ui-1.9.1.custom.css" rel="stylesheet" type="text/css" />
   
    <link href="/Css/Thickbox/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <link href="/Css/Combox/select3.css" rel="stylesheet" type="text/css" />
    <asp:Literal ID="dangky" runat="server"></asp:Literal>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="vien">
            <div class="banner">
                <img src="/css/trangchinh/banner.png" alt="" style="width: 100%" />
            </div>
            <div class="main">
                <div class="menu-trai" id="divMenuTrai" runat="server">
                </div>
                <div class="vung-chinh" id="divMain" runat="server">
                </div>
            </div>
            <div class="footer">
                <img src="css/trangchinh/footer.png" alt="" style="width: 100%" />
            </div>
        </div>
    </form>
</body>
</html>
