<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DangNhap.aspx.cs" Inherits="ThongTinDoiNgoai.DangNhap" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Đăng nhập hệ thống - Phản ánh, kiến nghị</title>
    <link href="../Css/home.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Js/fcm/firebase-app.js"></script>
    <script type="text/javascript" src="../Js/fcm/firebase-messaging.js"></script>
</head>
<script type="text/javascript">
    function CheckLogin(txt1) {
        if (document.getElementById(txt1).value == "") {
            alert("Chưa nhập tên đăng nhập");
            return false;
        }
        else
            return true;
    }
</script>
<body class="DangNhap_Body">
    <form id="form1" runat="server">
        <center>
            <div class="DangNhap_Vien">
                <div class="DangNhap_Nen">
                    <div style="width: 100%" id="divNguoiDung" runat="server">
                        <div class="DangNhap_DongNguoiDung">
                            Người dùng
                        </div>
                        <div class="DangNhap_DongTextBox">
                            <asp:TextBox ID="txtUser" runat="server" CssClass="DangNhap_TextBox"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 100%" id="divMatKhau" runat="server">
                        <div class="DangNhap_DongMatKhau">
                            Mật khẩu
                        </div>
                        <div class="DangNhap_DongTextBox">
                            <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="DangNhap_TextBox"></asp:TextBox>
                        </div>
                    </div>
                     <div style="width: 100%" id="divMaBaoVe" runat="server" visible="false">
                        <div class="DangNhap_DongMatKhau">
                         Mã xác nhận
                        </div>
                        <div class="DangNhap_DongTextBox">
                            <asp:TextBox ID="txtMaBaoVe" runat="server" CssClass="DangNhap_TextBaoVe" Width="70px"></asp:TextBox>
                            <asp:Image ID="imgCaptcha" runat="server" ImageUrl="../Captcha.ashx" CssClass="DangNhap_MaBaoVe"></asp:Image>
                                <asp:ImageButton
                                ID="imgRefresh" OnClick="imgRefresh_Click" runat="server" ImageUrl="~/Images/refresh.png" CssClass="DangNhap_refresh"></asp:ImageButton>
                        </div>
                    </div>
                    <div class="DangNhap_DongButton">
                        <div style="float: left;">
                            <asp:Button ID="btnDangNhap" runat="server" OnClick="btnDangNhap_Click" Text="Đăng nhập"
                                CssClass="DangNhap_Button" />
                        </div>
                    </div>
                </div>
            </div>
            <asp:Literal ID="lbl" runat="server"></asp:Literal>
        </center>
    </form>
</body>
</html>
