<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DangKyEmail_Tm.ascx.cs" Inherits="ThongTinDoiNgoai.DichVu.HeThong.Email.DangKyEmail_Tm" %>
<div>
    <div style="font-weight: bold;">
        Thông tin tài khoản:
    </div>
    <table style="width: 100%">
        <tr>
            <td style="width: 100px;">Địa chỉ email: <span style="color: Red;">(*)</span>
            </td>
            <td>
                <asp:TextBox ID="txtEmailAddress" runat="server" Width="100%" CssClass="email_diachi" ClientIDMode="Static"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Mật khẩu: <span style="color: Red;">(*)</span>
            </td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="100%" ClientIDMode="Static"></asp:TextBox>
                <asp:TextBox ID="txtHidPassword" runat="server" Style="display: none;" ClientIDMode="Static"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Tên hiển thị: <span style="color: Red;">(*)</span>
            </td>
            <td>
                <asp:TextBox ID="txtDisplayName" runat="server" Width="100%" ClientIDMode="Static"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none;">
            <td></td>
            <td>
                <asp:CheckBox ID="chkDefaultAccount" runat="server" />
                Tài khoản mặc định
                        <asp:CheckBox ID="chkDeleteFromServer" runat="server" />
                Xóa thư ở máy chủ sau khi tải về
            </td>
        </tr>
    </table>
    <br />
    <div style="font-weight: bold;">
        Thông tin máy chủ nhận Email:
    </div>
    <table style="width: 100%">
        <tr>
            <td style="width: 110px;">Giao thức: <span style="color: Red;">(*)</span>
            </td>
            <td>
                <asp:DropDownList ID="drpAccountType" runat="server" ClientIDMode="Static">
                    <asp:ListItem>POP3</asp:ListItem>
                    <asp:ListItem>IMAP4</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Máy chủ nhận: <span style="color: Red;">(*)</span>
            </td>
            <td>
                <asp:TextBox ID="txtIncomingServer" runat="server" Width="100%" ClientIDMode="Static"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:CheckBox ID="chkIncomingPort" runat="server" Text=" Cổng máy chủ:" ClientIDMode="Static" />
                <asp:TextBox ID="txtIncomingPort" runat="server" Width="50px" MaxLength="4" ClientIDMode="Static"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none">
            <td></td>
            <td>
                <asp:CheckBox ID="chkIncomingSecure" runat="server" Text=" Sử dụng bảo mật (SSL/TSL) nhận email" ClientIDMode="Static" />
            </td>
        </tr>
    </table>
    <br />
    <div style="font-weight: bold;">
        Thông tin máy chủ gởi Email:
    </div>
    <table style="width: 100%">
        <tr>
            <td style="width: 100px;">Máy chủ gởi: <span style="color: Red;">(*)</span>
            </td>
            <td>
                <asp:TextBox ID="txtOutgoingServer" runat="server" Width="100%" ClientIDMode="Static"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:CheckBox ID="chkOutgoingPort" runat="server" Text=" Cổng máy chủ:"  ClientIDMode="Static"/>
                <asp:TextBox ID="txtOutgoingPort" runat="server" Width="50px" MaxLength="4" ClientIDMode="Static"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none">
            <td></td>
            <td>
                <asp:CheckBox ID="chkOutgoingSecure" runat="server" Text=" Sử dụng bảo mật (SSL/TSL) gởi email"  ClientIDMode="Static"/>
                <asp:CheckBox ID="chkOutgoingAuthentication" runat="server" Text=" Sử dụng chứng thực gởi email" />
            </td>
        </tr>
    </table>
    <br />
    <div style="text-align: center;">
        <asp:Button ID="btnThemMoi" runat="server" Text="Thêm mới" Width="100px" CssClass="button"
            OnClick="btnThemMoi_Click" />
        <asp:Button ID="btnCapNhat" runat="server" Text="Cập nhật" Width="100px" CssClass="button"
            OnClick="btnCapNhat_Click" Visible="false" />
        <asp:Button ID="btnHuyBo" runat="server" Text="Hủy bỏ" Width="100px" CssClass="button"
            OnClick="btnHuyBo_Click" Visible="false" />
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#txtEmailAddress').keyup(function () {
            var email = $(this).val();
            email = email.toLowerCase();
            $(this).val(email);
            $('#txtDisplayName').val(email);
        });

        $('#txtEmailAddress').blur(function () {
            GetDefaultSettings();
        });

        $('#drpAccountType').change(function () {
            GetDefaultSettings();
        });

        $('#chkIncomingPort').click(function () {
            if ($(this).is(':checked'))
                $('#txtIncomingPort').attr('disabled', '');
            else
                $('#txtIncomingPort').attr('disabled', 'disabled');
        });

        $('#chkOutgoingPort').click(function () {
            if ($(this).is(':checked'))
                $('#txtOutgoingPort').attr('disabled', '');
            else
                $('#txtOutgoingPort').attr('disabled', 'disabled');
        });

        $('#txtPassword').val($('#txtHidPassword').val());

        function GetDefaultSettings() {
            var email = document.getElementById('txtEmailAddress');
            var inServer = "";
            var outServer = "";
            var ssl = false;
            var accType = document.getElementById('drpAccountType');
        if (email.value.lastIndexOf("yahoo") > 0) {
            inServer = "pop.mail.yahoo.com";
            outServer = "smtp.mail.yahoo.com";
            ssl = true;
        }
        else if (email.value.lastIndexOf("gmail") > 0) {
            if (accType.value == "POP3") inServer = "pop.gmail.com";
            else inServer = "imap.googlemail.com";
            outServer = "smtp.googlemail.com";
            ssl = true;
        }
        else {
            if (email.value != "") {
                inServer = "mail." + email.value.substring(email.value.lastIndexOf("@") + 1);
                outServer = inServer;
            }
        }
        $('#txtIncomingServer').val(inServer);
        $('#txtOutgoingServer').val(outServer);
            $('#chkIncomingSecure').attr('checked', ssl);
            $('#chkOutgoingSecure').attr('checked', ssl);
        }
    });
</script>
